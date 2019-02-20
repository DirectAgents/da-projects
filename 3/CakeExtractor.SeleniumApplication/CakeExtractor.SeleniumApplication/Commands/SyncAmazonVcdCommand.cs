using System.Collections.Generic;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD;
using CakeExtractor.SeleniumApplication.Loaders.VCD;
using CakeExtracter;
using CakeExtracter.Common;
using CakeExtracter.Helpers;
using CakeExtractor.SeleniumApplication.Configuration.Vcd;
using CakeExtractor.SeleniumApplication.Configuration.Models;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;

namespace CakeExtractor.SeleniumApplication.Commands
{
    internal class SyncAmazonVcdCommand : BaseAmazonSeleniumCommand
    {
        private readonly AmazonVcdExtractor extractor;
        private readonly VcdCommandConfigurationManager configurationManager;
        private readonly VcdAccountsDataProvider accountsDataProvider;

        public SyncAmazonVcdCommand()
        {
            configurationManager = new VcdCommandConfigurationManager();
            extractor = new AmazonVcdExtractor(configurationManager);
            accountsDataProvider = new VcdAccountsDataProvider();
        }

        public override string CommandName => "SyncAmazonVcdCommand";

        public override void PrepareCommandEnvironment()
        {
            extractor.PrepareExtractor();
            AmazonVcdLoader.PrepareLoader();
        }

        public override void Run()
        {
            var dateRanges = configurationManager.GetDaysToProcess();
            var accountsData = accountsDataProvider.GetAccountsDataToProcess(extractor);
            dateRanges.ForEach(d => RunForDateRange(d, accountsData));
        }

        private void RunForDateRange(DateRange dateRange, List<AccountInfo> accountsData)
        {
            extractor.DateRange = dateRange;
            Logger.Info($"\nAmazon VCD ETL. DateRange {dateRange}.");
            foreach (var accountData in accountsData)
            {
                DoEtlForAccount(accountData);
            }
        }

        private void DoEtlForAccount(AccountInfo accountInfo)
        {
            Logger.Info($"Amazon VCD, ETL for account {accountInfo.Account.Name} ({accountInfo.Account.Id}) started.");
            extractor.AccountInfo = accountInfo;
            var loader = new AmazonVcdLoader(accountInfo.Account);
            CommandHelper.DoEtl(extractor, loader);
            Logger.Info($"Amazon VCD, ETL for account {accountInfo.Account.Name} ({accountInfo.Account.Id}) finished.");
        }
    }
}