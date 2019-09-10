using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Etl.AmazonSelenium.Configuration;
using CakeExtracter.Etl.AmazonSelenium.VCD.Configuration;
using DirectAgents.Domain.Entities.CPProg;
using SeleniumDataBrowser.Helpers;
using SeleniumDataBrowser.Models;
using SeleniumDataBrowser.VCD.DataProviders;
using SeleniumDataBrowser.VCD.Helpers;
using SeleniumDataBrowser.VCD.Helpers.ReportDownloading;
using SeleniumDataBrowser.VCD.Helpers.UserInfoExtracting;
using SeleniumDataBrowser.VCD.Helpers.UserInfoExtracting.Models;
using SeleniumDataBrowser.VCD.Models;
using SeleniumDataBrowser.VCD.PageActions;

namespace CakeExtracter.Etl.AmazonSelenium.VCD.DataProviderBuilders
{
    /// <summary>
    /// Builder of the VCD Data Provider.
    /// </summary>
    internal class VcdDataProviderBuilder : IVcdDataProviderBuilder
    {
        private readonly bool isHidingBrowserWindow;
        private AuthorizationModel authorizationModel;
        private AmazonVcdActionsWithPagesManager pageActionsManager;
        private Dictionary<ExtAccount, VcdAccountInfo> vcdAccountsInfo;
        private VcdDataProvider vcdDataProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="VcdDataProviderBuilder"/> class.
        /// </summary>
        /// <param name="currentProfileNumber">Number of the current execution profile.</param>
        /// <param name="isHidingBrowserWindow">Include hiding the browser window.</param>
        public VcdDataProviderBuilder(int? currentProfileNumber, bool isHidingBrowserWindow)
        {
            vcdAccountsInfo = new Dictionary<ExtAccount, VcdAccountInfo>();
            this.isHidingBrowserWindow = isHidingBrowserWindow;
            SetExecutionProfile(currentProfileNumber);
        }

        /// <summary>
        /// Builds and gets ready instance of the VCD Data Provider.
        /// </summary>
        /// <param name="loggerWithoutAccountId">Logger without info about account ID.</param>
        /// <returns>Instance of the VCD Data Provider.</returns>
        public IVcdDataProvider BuildDataProvider(SeleniumLogger loggerWithoutAccountId)
        {
            authorizationModel = GetAuthorizationModel();
            pageActionsManager = GetPageActionsManager(loggerWithoutAccountId);
            var loginProcessManager = GetLoginProcessManager(loggerWithoutAccountId);
            vcdDataProvider = VcdDataProvider.GetVcdDataProviderInstance(loginProcessManager);
            vcdDataProvider.LoginToPortal();
            vcdAccountsInfo = GetAccountsData();
            return vcdDataProvider;
        }

        /// <summary>
        /// Creates the report downloader for the current account and sets it for the VCD data provider instance.
        /// </summary>
        /// <param name="currentAccount">Current account.</param>
        /// <param name="loggerWithAccountId">Logger with info about account ID.</param>
        public void InitializeReportDownloader(ExtAccount currentAccount, SeleniumLogger loggerWithAccountId)
        {
            var currentAccountInfo = vcdAccountsInfo[currentAccount];
            pageActionsManager.SelectAccountOnPage(currentAccount.Name);
            var reportDownloader = GetReportDownloader(currentAccountInfo, loggerWithAccountId);
            vcdDataProvider.SetReportDownloaderCurrentForDataProvider(reportDownloader);
        }

        /// <summary>
        /// Gets a list of external accounts for processing.
        /// </summary>
        /// <returns>a list of external accounts.</returns>
        public List<ExtAccount> GetAccounts()
        {
            return vcdAccountsInfo.Keys.ToList();
        }

        private void SetExecutionProfile(int? currentProfileNumber)
        {
            VcdExecutionProfileManager.Current.SetExecutionProfileNumber(currentProfileNumber);
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

        private VcdReportDownloader GetReportDownloader(VcdAccountInfo accountInfo, SeleniumLogger loggerWithAccountId)
        {
            var reportDownloadingStartedDelayInSeconds =
                VcdCommandConfigurationManager.GetReportDownloadingStartedDelay();
            var minDelayBetweenReportDownloadingInSeconds =
                VcdCommandConfigurationManager.GetMinDelayBetweenReportDownloading();
            var maxDelayBetweenReportDownloadingInSeconds =
                VcdCommandConfigurationManager.GetMaxDelayBetweenReportDownloading();
            var reportDownloadingAttemptCount =
                VcdCommandConfigurationManager.GetReportDownloadingAttemptCount();
            return new VcdReportDownloader(
                accountInfo,
                pageActionsManager,
                authorizationModel,
                loggerWithAccountId,
                reportDownloadingStartedDelayInSeconds,
                minDelayBetweenReportDownloadingInSeconds,
                maxDelayBetweenReportDownloadingInSeconds,
                reportDownloadingAttemptCount);
        }

        private AuthorizationModel GetAuthorizationModel()
        {
            try
            {
                return new AuthorizationModel
                {
                    Login = VcdExecutionProfileManager.Current.ProfileConfiguration.LoginEmail,
                    Password = VcdExecutionProfileManager.Current.ProfileConfiguration.LoginPassword,
                    SignInUrl = VcdExecutionProfileManager.Current.ProfileConfiguration.SignInUrl,
                    CookiesDir = VcdExecutionProfileManager.Current.ProfileConfiguration.CookiesDirectory,
                };
            }
            catch (Exception e)
            {
                throw new Exception("Failed to initialize authorization settings.", e);
            }
        }

        private AmazonVcdActionsWithPagesManager GetPageActionsManager(SeleniumLogger loggerWithoutAccountId)
        {
            try
            {
                var waitPageTimeoutInMinutes = SeleniumCommandConfigurationManager.GetWaitPageTimeout();
                return new AmazonVcdActionsWithPagesManager(
                    waitPageTimeoutInMinutes, isHidingBrowserWindow, loggerWithoutAccountId);
            }
            catch (Exception e)
            {
                throw new Exception("Failed to initialize page actions manager.", e);
            }
        }

        private VcdLoginManager GetLoginProcessManager(SeleniumLogger loggerWithoutAccountId)
        {
            return new VcdLoginManager(authorizationModel, pageActionsManager, loggerWithoutAccountId);
        }

        private VcdAccountsManager GetAccountsManager()
        {
            var pageUserInfo = GetPageUserInfo();
            return new VcdAccountsManager(pageUserInfo);
        }

        private PageUserInfo GetPageUserInfo()
        {
            var userInfoExtractor = new UserInfoExtracter();
            return userInfoExtractor.ExtractUserInfo(pageActionsManager);
        }
    }
}