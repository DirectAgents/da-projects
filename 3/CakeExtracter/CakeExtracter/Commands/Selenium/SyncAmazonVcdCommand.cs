using System;
using System.ComponentModel.Composition;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using CakeExtracter.Common;
using CakeExtracter.Common.JobExecutionManagement;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Models;
using CakeExtracter.Etl.AmazonSelenium.Configuration;
using CakeExtracter.Etl.AmazonSelenium.VCD.Configuration;
using CakeExtracter.Etl.AmazonSelenium.VCD.Configuration.Models;
using CakeExtracter.Etl.AmazonSelenium.VCD.Extractors;
using CakeExtracter.Etl.AmazonSelenium.VCD.Loaders;
using CakeExtracter.Etl.AmazonSelenium.VCD.Synchers;
using CakeExtracter.Helpers;
using SeleniumDataBrowser.Helpers;
using SeleniumDataBrowser.VCD.PageActions;
using SeleniumDataBrowser.Models;
using SeleniumDataBrowser.VCD.Helpers;
using SeleniumDataBrowser.VCD.Helpers.ReportDownloading;
using SeleniumDataBrowser.VCD.Helpers.UserInfoExtracting.Models;
using SeleniumDataBrowser.VCD.Helpers.UserInfoExtracting;
using SeleniumDataBrowser.VCD.Models;

namespace CakeExtracter.Commands.Selenium
{
    /// <inheritdoc />
    /// <summary>
    /// The class represents a command that is used to retrieve statistics of Amazon Vendor Central statistics.
    /// </summary>
    [Export(typeof(ConsoleCommand))]
    public class SyncAmazonVcdCommand : ConsoleCommand
    {
        private const int DefaultDaysAgo = 60;

        private VcdAccountsDataProvider accountsDataProvider;
        private AuthorizationModel authorizationModel;
        private AmazonVcdActionsWithPagesManager pageActionsManager;
        private VcdLoginManager loginProcessManager;
        private SeleniumLogger loggerWithAccountId;
        private SeleniumLogger loggerWithoutAccountId;

        /// <summary>
        /// Gets or sets the command argument: a number of execution profile (default = 1).
        /// </summary>
        public int ProfileNumber { get; set; }

        /// <summary>
        /// Gets or sets the command argument: Start date
        /// from which statistics will be extracted (default is 'daysAgo').
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or sets the command argument: End date
        /// to which statistics will be extracted (default is yesterday).
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Gets or sets the command argument: The number of days ago to calculate the start date
        /// from which statistics will be retrieved, used if StartDate not specified (default = 31).
        /// </summary>
        public int? DaysAgoToStart { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to hide the browser window (default = false).
        /// </summary>
        public bool IsHidingBrowserWindow { get; set; }

        /// <inheritdoc cref="ConsoleCommand" />
        /// <summary>
        /// Initializes a new instance of the <see cref="SyncAmazonVcdCommand" /> class.
        /// </summary>
        public SyncAmazonVcdCommand()
        {
            IsCommand("SyncAmazonVcdCommand", "Sync Vendor Central Stats");
            HasOption<int>("p|profileNumber=", "Execution profile number", c => ProfileNumber = c);
            HasOption("s|startDate=", "Start Date (default is 'daysAgo')", c => StartDate = DateTime.Parse(c));
            HasOption("e|endDate=", "End Date (default is yesterday)", c => EndDate = DateTime.Parse(c));
            HasOption<int>("d|daysAgo=", $"Days Ago to start, if startDate not specified (default = {DefaultDaysAgo})", c => DaysAgoToStart = c);
            HasOption<bool>("h|hideWindow=", "Include hiding the browser window", c => IsHidingBrowserWindow = c);
        }

        /// <inheritdoc />
        /// <summary>
        /// The method resets command arguments to defaults.
        /// </summary>
        public override void ResetProperties()
        {
            ProfileNumber = 0;
            StartDate = null;
            EndDate = null;
            DaysAgoToStart = null;
            IsHidingBrowserWindow = false;
        }

        /// <inheritdoc />
        /// <summary>
        /// The method runs the current command and extract and save statistics of Amazon Vendor Central statistics.
        /// based on the command arguments.
        /// </summary>
        /// <param name="remainingArguments"></param>
        /// <returns>Execution code.</returns>
        public override int Execute(string[] remainingArguments)
        {
            InitializeCommand();
            RunEtls();
            return 0;
        }

        /// <inheritdoc />
        public override IEnumerable<CommandWithSchedule> GetUniqueBroadCommands(IEnumerable<CommandWithSchedule> commands)
        {
            var broadCommands = new List<CommandWithSchedule>();
            var commandsGroupedByProfile = commands.GroupBy(x => (x.Command as SyncAmazonVcdCommand)?.ProfileNumber);
            foreach (var commandsGroup in commandsGroupedByProfile)
            {
                broadCommands.AddRange(commandsGroup);
            }
            return broadCommands;
        }

        private void InitializeCommand()
        {
            try
            {
                InitializeFields();
                LoginProcess();
                InitializeLoader();
            }
            catch (Exception e)
            {
                throw new Exception("Failed to initialize VCD command.", e);
            }
        }

        private void RunEtls()
        {
            pageActionsManager.RefreshSalesDiagnosticPage(authorizationModel);
            var accountsData = GetAccountsData();
            RunForAccounts(accountsData);
        }

        private List<AccountInfo> GetAccountsData()
        {
            try
            {
                accountsDataProvider = GetAccountsDataProvider();
                var accountsData = accountsDataProvider.GetAccountsDataToProcess();
                accountsData.ForEach(accountData =>
                    CommandExecutionContext.Current.SetJobExecutionStateInHistory("Not started", accountData.Account.Id));
                return accountsData;
            }
            catch (Exception e)
            {
                throw new Exception("Failed to get accounts data.", e);
            }
        }

        private VcdAccountsDataProvider GetAccountsDataProvider()
        {
            var pageUserInfo = GetPageUserInfo();
            return new VcdAccountsDataProvider(pageUserInfo);
        }

        private PageUserInfo GetPageUserInfo()
        {
            var userInfoExtractor = new UserInfoExtracter();
            return userInfoExtractor.ExtractUserInfo(pageActionsManager);
        }

        private void RunForAccounts(IEnumerable<AccountInfo> accountsData)
        {
            var dateRange = CommandHelper.GetDateRange(StartDate, EndDate, DaysAgoToStart, DefaultDaysAgo);
            Logger.Info($"Amazon VCD ETL. DateRange {dateRange}.");
            foreach (var accountData in accountsData)
            {
                try
                {
                    DoEtlForAccount(accountData, dateRange);
                    SyncAccountDataToAnalyticTable(accountData.Account.Id);
                }
                catch (Exception ex)
                {
                    Logger.Error(accountData.Account.Id, ex);
                }
            }
        }

        private void DoEtlForAccount(AccountInfo accountInfo, DateRange dateRange)
        {
            Logger.Info(accountInfo.Account.Id, $"Amazon VCD, ETL for account {accountInfo.Account.Name} ({accountInfo.Account.Id}) started.");
            AccountPagePreparation(accountInfo);
            var reportDownloader = GetReportDownloader(accountInfo);
            var extractor = new AmazonVcdExtractor(accountInfo, dateRange, reportDownloader);
            var loader = new AmazonVcdLoader(accountInfo.Account);
            InitEtlEvents(extractor, loader);
            CommandHelper.DoEtl(extractor, loader);
            Logger.Info(accountInfo.Account.Id, $"Amazon VCD, ETL for account {accountInfo.Account.Name} ({accountInfo.Account.Id}) finished.");
        }

        private void AccountPagePreparation(AccountInfo accountInfo)
        {
            PreparePageActionsManagerLogger(accountInfo.Account.Id);
            pageActionsManager.SelectAccountOnPage(accountInfo.Account.Name);
        }

        private void PreparePageActionsManagerLogger(int accountId)
        {
            InitializeLogger(accountId);
            pageActionsManager.Logger = loggerWithAccountId;
        }

        private void InitEtlEvents(AmazonVcdExtractor extractor, AmazonVcdLoader loader)
        {
            extractor.ProcessEtlFailedWithoutInformation += exception =>
                ScheduleNewCommandLaunch<SyncAmazonVcdCommand>(command => { });
            loader.ProcessEtlFailedWithoutInformation += exception =>
                ScheduleNewCommandLaunch<SyncAmazonVcdCommand>(command => { });
        }

        private VcdReportDownloader GetReportDownloader(AccountInfo accountInfo)
        {
            var reportDownloadingStartedDelayInSeconds =
                VcdExecutionProfileManger.Current.ProfileConfiguration.ReportDownloadingStartedDelayInSeconds;
            var minDelayBetweenReportDownloadingInSeconds =
                VcdExecutionProfileManger.Current.ProfileConfiguration.MinDelayBetweenReportDownloadingInSeconds;
            var maxDelayBetweenReportDownloadingInSeconds =
                VcdExecutionProfileManger.Current.ProfileConfiguration.MaxDelayBetweenReportDownloadingInSeconds;
            var reportDownloadingAttemptCount =
                VcdExecutionProfileManger.Current.ProfileConfiguration.ReportDownloadingAttemptCount;
            var vcdAccountInfo = GetVcdAccountInformation(accountInfo);
            return new VcdReportDownloader(
                vcdAccountInfo,
                pageActionsManager,
                authorizationModel,
                loggerWithAccountId,
                reportDownloadingStartedDelayInSeconds,
                minDelayBetweenReportDownloadingInSeconds,
                maxDelayBetweenReportDownloadingInSeconds,
                reportDownloadingAttemptCount);
        }

        private VcdAccountInfo GetVcdAccountInformation(AccountInfo accountInfo)
        {
            return new VcdAccountInfo
            {
                AccountId = accountInfo.Account.Id,
                AccountName = accountInfo.Account.Name,
                McId = accountInfo.McId,
                VendorGroupId = accountInfo.VendorGroupId,
            };
        }

        private void SyncAccountDataToAnalyticTable(int accountId)
        {
            try
            {
                CommandExecutionContext.Current.SetJobExecutionStateInHistory("Sync analytic table data.", accountId);
                Logger.Info(accountId, "Sync analytic table data.");
                var syncScriptPath = VcdCommandConfigurationManager.GetSyncScriptPath();
                var vcdTablesSyncher = new VcdAnalyticTablesSyncher(syncScriptPath);
                vcdTablesSyncher.SyncData(accountId);
            }
            catch (Exception ex)
            {
                var exception = new Exception("Error occurred while sync VCD data to analytic table", ex);
                Logger.Error(accountId, exception);
                throw exception;
            }
        }

        private void InitializeFields()
        {
            SetFieldsFromConfig();
            SetExecutionProfile();
            InitializeLogger();
            InitializePageActionsManager();
            InitializeAuthorizationModel();
            InitializeLoginManager();
        }

        private void SetFieldsFromConfig()
        {
            IntervalBetweenUnsuccessfulAndNewRequestInMinutes =
                VcdCommandConfigurationManager.GetIntervalBetweenUnsuccessfulAndNewRequest();
        }

        private void LoginProcess()
        {
            try
            {
                loginProcessManager.LoginToAmazonPortal();
            }
            catch (Exception e)
            {
                throw new Exception("Failed to login to Amazon Advertiser Portal.", e);
            }
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

        private void SetExecutionProfile()
        {
            VcdExecutionProfileManger.Current.SetExecutionProfileNumber(ProfileNumber);
        }

        private void InitializePageActionsManager()
        {
            try
            {
                var waitPageTimeoutInMinutes = SeleniumCommandConfigurationManager.GetWaitPageTimeout();
                pageActionsManager = new AmazonVcdActionsWithPagesManager(
                    waitPageTimeoutInMinutes, IsHidingBrowserWindow, loggerWithoutAccountId);
            }
            catch (Exception e)
            {
                throw new Exception("Failed to initialize page actions manager.", e);
            }
        }

        private void InitializeAuthorizationModel()
        {
            try
            {
                authorizationModel = new AuthorizationModel
                {
                    Login = VcdExecutionProfileManger.Current.ProfileConfiguration.LoginEmail,
                    Password = VcdExecutionProfileManger.Current.ProfileConfiguration.LoginPassword,
                    SignInUrl = VcdExecutionProfileManger.Current.ProfileConfiguration.SignInUrl,
                    CookiesDir = VcdExecutionProfileManger.Current.ProfileConfiguration.CookiesDirectory,
                };
            }
            catch (Exception e)
            {
                throw new Exception("Failed to initialize authorization settings.", e);
            }
        }

        private void InitializeLoginManager()
        {
            loginProcessManager = new VcdLoginManager(
                authorizationModel, pageActionsManager, loggerWithoutAccountId);
        }

        private void InitializeLogger(int accountId = 0)
        {
            if (accountId == 0)
            {
                SetLoggerWithoutAccountId();
            }
            else
            {
                SetLoggerWithAccountId(accountId);
            }
        }

        private void SetLoggerWithoutAccountId()
        {
            loggerWithoutAccountId = new SeleniumLogger(
                x => Logger.Info(x), Logger.Error, x => Logger.Warn(x));
        }

        private void SetLoggerWithAccountId(int accountId)
        {
            loggerWithAccountId = new SeleniumLogger(
                x => Logger.Info(accountId, x),
                exc => Logger.Error(accountId, exc),
                x => Logger.Warn(accountId, x));
        }
    }
}