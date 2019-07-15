using System;
using System.Collections.Generic;
using System.Linq;
using DirectAgents.Domain.Concrete;
using DirectAgents.Domain.Entities.CPProg;
using SeleniumDataBrowser.VCD.Helpers.UserInfoExtracting.Models;
using SeleniumDataBrowser.VCD.Models;

namespace CakeExtracter.Etl.AmazonSelenium.VCD.Configuration
{
    /// <summary>
    /// Manager of accounts information for VCD job.
    /// </summary>
    internal class VcdAccountsManager
    {
        private readonly PlatformAccountRepository accountsRepository;
        private readonly PageUserInfo pageUserInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="VcdAccountsManager"/> class.
        /// </summary>
        /// <param name="pageUserInfo">Information about page user.</param>
        public VcdAccountsManager(PageUserInfo pageUserInfo)
        {
            this.pageUserInfo = pageUserInfo;
            accountsRepository = new PlatformAccountRepository();
        }

        /// <summary>
        /// Gets the accounts data to process. Combined data from database and sales diagnostic page.
        /// </summary>
        /// <returns>Account info collection.</returns>
        public Dictionary<ExtAccount, VcdAccountInfo> GetAccountsDataToProcess()
        {
            try
            {
                var dbAccounts = GetDbAccountsToProcess();
                var accountsInfo = new Dictionary<ExtAccount, VcdAccountInfo>();
                dbAccounts.ForEach(dbAccount =>
                {
                    var pageAccountData = pageUserInfo.subAccounts.FirstOrDefault(pa => pa.name == dbAccount.Name);
                    if (pageAccountData != null)
                    {
                        var accountInfo = new VcdAccountInfo
                        {
                            AccountName = dbAccount.Name,
                            McId = pageAccountData.mcId,
                            VendorGroupId = pageAccountData.vendorGroupId,
                        };
                        accountsInfo.Add(dbAccount, accountInfo);
                    }
                    else
                    {
                        Logger.Warn($"{dbAccount.Name} account was not found on page");
                    }
                });
                Logger.Info("{0} accounts will be processed", string.Join(",", accountsInfo.Keys.ToList().Select(a => a.Name)));
                return accountsInfo;
            }
            catch (Exception ex)
            {
                var exception = new Exception("Error occurred while fetching accounts information.", ex);
                Logger.Error(exception);
                throw exception;
            }
        }

        private List<ExtAccount> GetDbAccountsToProcess()
        {
            return VcdExecutionProfileManager.Current.ProfileConfiguration.AccountIds.Count == 0
                ? accountsRepository.GetAccountsByPlatformCode(Platform.Code_AraAmazon).ToList()
                : accountsRepository.GetAccountsWithIds(VcdExecutionProfileManager.Current.ProfileConfiguration.AccountIds).ToList();
        }
    }
}