﻿using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Etl.AmazonSelenium.VCD.Configuration.Models;
using DirectAgents.Domain.Concrete;
using DirectAgents.Domain.Entities.CPProg;
using SeleniumDataBrowser.VCD.Helpers.UserInfoExtracting.Models;

namespace CakeExtracter.Etl.AmazonSelenium.VCD.Configuration
{
    /// <summary>
    /// Provider of accounts information for VCD job.
    /// </summary>
    internal class VcdAccountsDataProvider
    {
        private readonly PlatformAccountRepository accountsRepository;
        private readonly PageUserInfo pageUserInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="VcdAccountsDataProvider"/> class.
        /// </summary>
        public VcdAccountsDataProvider(PageUserInfo pageUserInfo)
        {
            this.pageUserInfo = pageUserInfo;
            accountsRepository = new PlatformAccountRepository();
        }

        /// <summary>
        /// Gets the accounts data to process. Combined data from database and sales diagnostic page.
        /// </summary>
        /// <param name="extractor">The extractor.</param>
        /// <returns>Account info collection.</returns>
        public List<AccountInfo> GetAccountsDataToProcess()
        {
            try
            {
                var dbAccounts = GetDbAccountsToProcess();

                var accountsInfo = new List<AccountInfo>();
                dbAccounts.ForEach(dbAccount =>
                {
                    var pageAccountData = pageUserInfo.subAccounts.FirstOrDefault(pa => pa.name == dbAccount.Name);
                    if (pageAccountData != null)
                    {
                        accountsInfo.Add(new AccountInfo
                        {
                            Account = dbAccount,
                            McId = pageAccountData.mcId,
                            VendorGroupId = pageAccountData.vendorGroupId,
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
                Logger.Error(new Exception("Error occurred while fetching accounts information." ,ex));
                throw ex;
            }
        }

        private List<ExtAccount> GetDbAccountsToProcess()
        {
            return VcdExecutionProfileManger.Current.ProfileConfiguration.AccountIds.Count == 0
                ? accountsRepository.GetAccountsByPlatformCode(Platform.Code_AraAmazon).ToList()
                : accountsRepository.GetAccountsWithIds(VcdExecutionProfileManger.Current.ProfileConfiguration.AccountIds).ToList();
        }
    }
}
