using CakeExtracter;
using CakeExtractor.SeleniumApplication.Configuration.Models;
using CakeExtractor.SeleniumApplication.PageActions.AmazonVcd;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.UserInfoExtracting;
using DirectAgents.Domain.Concrete;
using DirectAgents.Domain.Entities.CPProg;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CakeExtractor.SeleniumApplication.Configuration.Vcd
{
    /// <summary>
    /// Provider of accounts information for vcd job.
    /// </summary>
    internal class VcdAccountsDataProvider
    {
        private readonly VcdCommandConfigurationManager vcdConfigurationManager;
        private readonly PlatformAccountRepository accountsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="VcdAccountsDataProvider"/> class.
        /// </summary>
        public VcdAccountsDataProvider()
        {
            vcdConfigurationManager = new VcdCommandConfigurationManager();
            accountsRepository = new PlatformAccountRepository();
        }

        /// <summary>
        /// Gets the accounts data to process. Combined data from database and sales diagnostic page.
        /// </summary>
        /// <param name="extractor">The extractor.</param>
        /// <returns>Account info collection.</returns>
        public List<AccountInfo> GetAccountsDataToProcess(AmazonVcdPageActions pageActions)
        {
            try
            {
                var dbAccounts = GetDbAccountsToProcess();
                var userInfoExtractor = new UserInfoExtracter();
                var pageAccountsInfo = userInfoExtractor.ExtractUserInfo(pageActions);
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
