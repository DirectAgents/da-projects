using CakeExtracter;
using CakeExtractor.SeleniumApplication.Configuration.Models;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD;
using DirectAgents.Domain.Concrete;
using DirectAgents.Domain.Entities.CPProg;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CakeExtractor.SeleniumApplication.Configuration.Vcd
{
    internal class VcdAccountsDataProvider
    {
        private readonly VcdCommandConfigurationManager vcdConfigurationManager;
        private readonly PlatformAccountRepository accountsRepository;

        public VcdAccountsDataProvider()
        {
            vcdConfigurationManager = new VcdCommandConfigurationManager();
            accountsRepository = new PlatformAccountRepository();
        }

        public List<AccountInfo> GetAccountsDataToProcess(AmazonVcdExtractor extractor)
        {
            try
            {
                var dbAccounts = GetDbAccountsToProcess();
                var pageAccountsInfo = extractor.ExtractAccountsInfo();
                var accountsInfo = new List<AccountInfo>();
                dbAccounts.ForEach(dbAccount =>
                {
                    var pageAccountData = pageAccountsInfo.subAccounts.FirstOrDefault(pa => pa.name == dbAccount.Name);
                    if (pageAccountData != null)
                    {
                        accountsInfo.Add(new AccountInfo
                        {
                            Account = dbAccount,
                            McId = pageAccountData.mcId,
                            VendorGroupId = pageAccountData.vendorGroupId
                        });
                    }
                    else
                    {
                        Logger.Warn($"{dbAccount.Name} account was not found on page");
                    }
                });

                Logger.Info("{0} accounts will be processed", string.Join(",", accountsInfo.Select(a => a.Account.Name)));
                return accountsInfo;
            }
            catch (Exception ex)
            {
                Logger.Error(new Exception("Error occured while fetching accounts information." ,ex));
                throw ex;
            }
        }

        private List<ExtAccount> GetDbAccountsToProcess()
        {
            return (VcdExecutionProfileManger.Current.ProfileConfiguration.AccountIds.Count == 0) ?
                accountsRepository.GetAccountsByPlatformCode(Platform.Code_AraAmazon).ToList() :
                accountsRepository.GetAccountsWithIds(VcdExecutionProfileManger.Current.ProfileConfiguration.AccountIds).ToList();
        }
    }
}
