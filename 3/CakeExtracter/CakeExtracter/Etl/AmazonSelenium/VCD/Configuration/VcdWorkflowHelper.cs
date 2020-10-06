using System;
using System.Collections.Generic;

using CakeExtracter.Common;
using CakeExtracter.Etl.AmazonSelenium.VCD.Loaders;
using CakeExtracter.Helpers;
using CakeExtracter.Logging.TimeWatchers.Amazon;

using DirectAgents.Domain.Entities.CPProg;

using SeleniumDataBrowser.VCD;
using SeleniumDataBrowser.VCD.Helpers.UserInfoExtracting;
using SeleniumDataBrowser.VCD.Helpers.UserInfoExtracting.Models;
using SeleniumDataBrowser.VCD.Models;

namespace CakeExtracter.Etl.AmazonSelenium.VCD.Configuration
{
    public class VcdWorkflowHelper
    {
        private readonly VcdDataProvider vcdDataProvider;

        public Dictionary<ExtAccount, VcdAccountInfo> VcdAccountsInfo { get; }

        public VcdWorkflowHelper(VcdDataProvider vcdDataProvider)
        {
            this.vcdDataProvider = vcdDataProvider;
            VcdAccountsInfo = GetAccountsData();
            InitializeLoader();
        }

        private Dictionary<ExtAccount, VcdAccountInfo> GetAccountsData()
        {
            try
            {
                var accountsDataProvider = GetAccountsManager();
                return accountsDataProvider.GetAccountsDataToProcess();
            }
            catch (Exception e)
            {
                throw new Exception("Failed to get accounts data.", e);
            }
        }

        private VcdAccountsManager GetAccountsManager()
        {
            var pageUserInfo = GetPageUserInfo();
            return new VcdAccountsManager(pageUserInfo);
        }

        private PageUserInfo GetPageUserInfo()
        {
            var userInfoExtractor = new UserInfoExtracter();
            return userInfoExtractor.ExtractUserInfo(vcdDataProvider);
        }

        private void InitializeLoader()
        {
            try
            {
                AmazonVcdLoader.PrepareLoader();
            }
            catch (Exception e)
            {
                throw new Exception("Failed to prepare VCD loader.", e);
            }
        }
    }
}