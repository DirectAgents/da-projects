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
        private VcdCommandConfigurationManager vcdConfigurationManager;

        private PlatformAccountRepository accountsRepository;

        public VcdAccountsDataProvider()
        {
            vcdConfigurationManager = new VcdCommandConfigurationManager();
            accountsRepository = new PlatformAccountRepository();
        }

        public List<AccountInfo> GetAccountsDataToProcess(AmazonVcdExtractor extractor)
        {
            var dbAccounts = GetDbAccountsToProcess();
            var pageAcountsInfo = extractor.ExtractAccountsInfo();
            var accountsInfo = new List<AccountInfo>();
            dbAccounts.ForEach(dbAccount =>
            {
                var pageAccountData = pageAcountsInfo.subAccounts.FirstOrDefault(pa => pa.name == dbAccount.Name);
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
                    Logger.Warn("{0} account was not found on page", dbAccount.Name);
                }
            });
            Logger.Info("{0} accounts will be processed", string.Join(",", accountsInfo.Select(a => a.Account.Name)));
            return accountsInfo;
        }

        private List<ExtAccount> GetDbAccountsToProcess()
        {
            if (vcdConfigurationManager.GetAccountId() != 0)
            {
                var account = accountsRepository.GetAccount(vcdConfigurationManager.GetAccountId());
                if (account != null)
                {
                    return new List<ExtAccount> { account };
                }
                else
                {
                    throw new Exception(string.Format("Account with id {0} was not found", vcdConfigurationManager.GetAccountId()));
                }
            }
            else
            {
                return accountsRepository.GetAccountsByPlatformCode(Platform.Code_AraAmazon).ToList();
            }
        }
    }
}
