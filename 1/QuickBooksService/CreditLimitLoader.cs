using System;
using System.Linq;
using DAgents.Common;
using AccountingBackupWeb.Models.AccountingBackup;
using System.Collections.Generic;

namespace QuickBooksService
{
    public class CreditLimitLoader : ProgramAction
    {
        public interface IConvertCreditLimitTextToCreditLimitAmount
        {
            bool Convert(string text, out decimal converted);
        }

        readonly IConvertCreditLimitTextToCreditLimitAmount[] _converters;
        readonly AccountingBackupWeb.Models.Staging.StagingEntities.IFactory _stagingModelFactory;
        readonly AccountingBackupEntities.IFactory _modelFactory;

        public CreditLimitLoader(
            IConvertCreditLimitTextToCreditLimitAmount[] converters,
            AccountingBackupWeb.Models.Staging.StagingEntities.IFactory stagingModelFactory,
            AccountingBackupEntities.IFactory modelFactory)
        {
            _converters = converters;
            _stagingModelFactory = stagingModelFactory;
            _modelFactory = modelFactory;
        }

        public override void Execute()
        {
            ConvertFromStaging();
            Load();
        }

        void ConvertFromStaging()
        {
            using (var model = _stagingModelFactory.Create())
            {
                foreach (var item in model.CustomerCreditLimits.ToList())
                {
                    string text = item.CreditLimitText;
                    decimal result = 0;
                    bool success = false;
                    foreach (var converter in _converters)
                    {
                        success = converter.Convert(text, out result);
                        if (success)
                        {
                            break;
                        }
                    }
                    if (success)
                    {
                        Logger.Log("converted " + text + " to " + result);
                    }
                    else
                    {
                        Logger.Log("did not convert [" + text + "]");
                    }
                }
            }
        }

        void Load()
        {
            using (var model = _modelFactory.Create())
            {
                foreach (var stagedCreditLimit in StagedCreditLimits)
                {
                    var advertiser = GetAdvertiser(model, stagedCreditLimit);
                    if (advertiser != null)
                    {
                        advertiser.CreditLimit = stagedCreditLimit.CreditLimitAmount.Value;

                        Logger.Log("set credit limit for advertiser " + advertiser.Name + " to " + advertiser.CreditLimit);
                    }
                    else
                    {
                        Logger.Log("no advertiser named " + stagedCreditLimit.CustomerName);
                    }
                }

                model.SaveChanges();
            }
        }

        Advertiser GetAdvertiser(AccountingBackupEntities model, AccountingBackupWeb.Models.Staging.CustomerCreditLimit stagedCreditLimit)
        {
            string customerName = stagedCreditLimit.CustomerName;
            if (customerName.Contains("("))
            {
                Console.WriteLine("1> " + customerName);
                var s = customerName.Split(new[] { '(', ')' }).First().Trim();
                customerName = s;
                Console.WriteLine("2> " + customerName);
            }

            var advertiser = model.Advertisers.FirstOrDefault(c => c.Name == customerName);
            return advertiser;
        }

        List<AccountingBackupWeb.Models.Staging.CustomerCreditLimit> StagedCreditLimits
        {
            get
            {
                using (var model = _stagingModelFactory.Create())
                {
                    return model.CustomerCreditLimits.Where(c => c.CreditLimitAmount != null).ToList();
                }
            }
        }
    }

    public class ConvertCreditLimitTextToCreditLimitAmount1 : CreditLimitLoader.IConvertCreditLimitTextToCreditLimitAmount
    {
        //[ReturnFalseIfArgumentIsNullAspect(0)]
        public bool Convert(string text, out decimal converted)
        {
            converted = 0;
            return decimal.TryParse(text.Trim(), out converted); ;
        }
    }

    public class ConvertCreditLimitTextToCreditLimitAmount2 : CreditLimitLoader.IConvertCreditLimitTextToCreditLimitAmount
    {
        public bool Convert(string text, out decimal converted)
        {
            converted = 0;
            return
                text == "No credit" ||
                text == "ask Josh" ||
                text == "Ask Josh" ||
                text == "No Credit" ||
                text == "ask Dinesh" ||
                text == "Ask Dinesh"
            ;
        }
    }

    //[Serializable]
    //public class ReturnFalseIfArgumentIsNullAspect : PostSharp.Aspects.OnMethodBoundaryAspect
    //{
    //    readonly int _argument;

    //    public ReturnFalseIfArgumentIsNullAspect(int argument)
    //    {
    //        _argument = argument;
    //    }

    //    public override void OnEntry(PostSharp.Aspects.MethodExecutionArgs args)
    //    {
    //        if (args.Arguments[_argument] == null)
    //        {
    //            args.FlowBehavior = PostSharp.Aspects.FlowBehavior.Return;
    //            args.ReturnValue = false;
    //        }
    //    }
    //}
}
