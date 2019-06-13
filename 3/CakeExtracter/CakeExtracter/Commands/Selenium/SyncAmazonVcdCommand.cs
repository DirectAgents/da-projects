using System;
using System.ComponentModel.Composition;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using CakeExtracter.Common;
using CakeExtracter.Common.JobExecutionManagement;
using CakeExtracter.Etl.AmazonSelenium.VCD.Configuration;
using CakeExtracter.Etl.AmazonSelenium.VCD.Configuration.Models;
using CakeExtracter.Etl.AmazonSelenium.VCD.Extractors;
using CakeExtracter.Etl.AmazonSelenium.VCD.Loaders;
using CakeExtracter.Etl.AmazonSelenium.VCD.Synchers;
using CakeExtracter.Helpers;
using SeleniumDataBrowser.VCD.PageActions;
using SeleniumDataBrowser.Models;
using SeleniumDataBrowser.VCD.Helpers;
using SeleniumDataBrowser.VCD.Helpers.ReportDownloading;
using SeleniumDataBrowser.VCD.Helpers.UserInfoExtracting;
using SeleniumDataBrowser.VCD.Models;

namespace CakeExtracter.Commands.Selenium
{
    [Export(typeof(ConsoleCommand))]
    public class SyncAmazonVcdCommand : ConsoleCommand
    {
        private VcdCommandConfigurationManager configurationManager;
        private VcdAccountsDataProvider accountsDataProvider;
        private AuthorizationModel authorizationModel;
        private AmazonVcdPageActions pageActionsManager;

        public int ProfileNumber { get; set; }

        public SyncAmazonVcdCommand()
        {
            NoNeedToCreateRepeatRequests = true;
            IsCommand("SyncAmazonVcdCommand", "Sync VCD Stats");
            HasOption<int>("p|profileNumber=", "Profile Number", c => ProfileNumber = c);
        }

        public override void ResetProperties()
        {
            ProfileNumber = 0;
        }

        public override int Execute(string[] remainingArguments)
        {
            VcdExecutionProfileManger.Current.SetExecutionProfileNumber(ProfileNumber);

            const string WaitPageTimeoutConfigurationKey = "SeleniumWaitPageTimeoutInMinutes";
            var waitPageTimeoutInMinutes = ConfigurationHelper.GetIntConfigurationValue(WaitPageTimeoutConfigurationKey);
            pageActionsManager = new AmazonVcdPageActions(waitPageTimeoutInMinutes);
            SetLogActionsForPageActionsManager();

            configurationManager = new VcdCommandConfigurationManager();
            InitializeAuthorizationModel();

            LoginProcess();

            AmazonVcdLoader.PrepareLoader();

            RunEtls();

            return 0;
        }

        private void SetLogActionsForPageActionsManager(int accountId = 0)
        {
            if (accountId == 0)
            {
                pageActionsManager.LogInfo = x => Logger.Info(x);
                pageActionsManager.LogError = x => Logger.Error(new Exception(x));
                pageActionsManager.LogWarning = x => Logger.Warn(x);
            }
            else
            {
                pageActionsManager.LogInfo = x => Logger.Info(accountId, x);
                pageActionsManager.LogError = x => Logger.Error(accountId, new Exception(x));
                pageActionsManager.LogWarning = x => Logger.Warn(accountId, x);
            }
        }

        private void LoginProcess()
        {
            var loginProcessManager = new AmazonVcdLoginHelper(authorizationModel, pageActionsManager, x => Logger.Info(x));
            loginProcessManager.LoginToAmazonPortal();
        }

        private void RunEtls()
        {
            pageActionsManager.RefreshSalesDiagnosticPage(authorizationModel);
            var dateRanges = configurationManager.GetDateRangesToProcess();

            var userInfoExtractor = new UserInfoExtracter();
            var pageUserInfo = userInfoExtractor.ExtractUserInfo(pageActionsManager);

            accountsDataProvider = new VcdAccountsDataProvider(pageUserInfo);

            var accountsData = accountsDataProvider.GetAccountsDataToProcess();
            dateRanges.ForEach(d => RunForDateRange(d, accountsData));
        }

        private void RunForDateRange(DateRange dateRange, List<AccountInfo> accountsData)
        {
            Logger.Info($"\nAmazon VCD ETL. DateRange {dateRange}.");
            foreach (var accountData in accountsData)
            {
                DoEtlForAccount(accountData, dateRange);
                SyncAccountDataToAnalyticTable(accountData.Account.Id);
            }
        }

        private void DoEtlForAccount(AccountInfo accountInfo, DateRange dateRange)
        {
            Logger.Info(accountInfo.Account.Id, $"Amazon VCD, ETL for account {accountInfo.Account.Name} ({accountInfo.Account.Id}) started.");

            SetLogActionsForPageActionsManager(accountInfo.Account.Id);

            pageActionsManager.SelectAccountOnPage(accountInfo.Account.Name);

            var vcdAccountInfo = new VcdAccountInfo
            {
                AccountId = accountInfo.Account.Id,
                AccountName = accountInfo.Account.Name,
                McId = accountInfo.McId,
                VendorGroupId = accountInfo.VendorGroupId,
            };

            var reportDownloadingStartedDelayInSeconds = VcdExecutionProfileManger.Current.ProfileConfiguration.ReportDownloadingStartedDelayInSeconds;
            var minDelayBetweenReportDownloadingInSeconds = VcdExecutionProfileManger.Current.ProfileConfiguration.MinDelayBetweenReportDownloadingInSeconds;
            var maxDelayBetweenReportDownloadingInSeconds = VcdExecutionProfileManger.Current.ProfileConfiguration.MaxDelayBetweenReportDownloadingInSeconds;
            var reportDownloadingAttemptCount = VcdExecutionProfileManger.Current.ProfileConfiguration.ReportDownloadingAttemptCount;

            var reportDownloader = new VcdReportDownloader(
                vcdAccountInfo,
                pageActionsManager,
                authorizationModel,
                x => Logger.Info(vcdAccountInfo.AccountId, x),
                x => Logger.Warn(vcdAccountInfo.AccountId, x),
                reportDownloadingStartedDelayInSeconds,
                minDelayBetweenReportDownloadingInSeconds,
                maxDelayBetweenReportDownloadingInSeconds,
                reportDownloadingAttemptCount);

            var extractor = new AmazonVcdExtractor(accountInfo, dateRange, reportDownloader);
            var loader = new AmazonVcdLoader(accountInfo.Account);
            CommandHelper.DoEtl(extractor, loader);
            Logger.Info(accountInfo.Account.Id, $"Amazon VCD, ETL for account {accountInfo.Account.Name} ({accountInfo.Account.Id}) finished.");
        }

        private void SyncAccountDataToAnalyticTable(int accountId)
        {
            try
            {
                CommandExecutionContext.Current.SetJobExecutionStateInHistory($"Sync analytic table data.", accountId);
                Logger.Info(accountId, "Sync analytic table data.");
                var vcdTablesSyncher = new VcdAnalyticTablesSyncher();
                vcdTablesSyncher.SyncData(accountId);
            }
            catch (Exception ex)
            {
                Logger.Error(accountId, new Exception("Error occurred while sync VCD data to analytic table", ex));
            }
        }

        private void InitializeAuthorizationModel()
        {
            authorizationModel = new AuthorizationModel
            {
                Login = VcdExecutionProfileManger.Current.ProfileConfiguration.LoginEmail,
                Password = VcdExecutionProfileManger.Current.ProfileConfiguration.LoginPassword,
                SignInUrl = VcdExecutionProfileManger.Current.ProfileConfiguration.SignInUrl,
                CookiesDir = VcdExecutionProfileManger.Current.ProfileConfiguration.CookiesDirectory,
            };
        }
    }
}