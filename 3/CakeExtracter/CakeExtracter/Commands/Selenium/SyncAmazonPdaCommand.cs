using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using CakeExtracter.Common;
using CakeExtracter.Etl.AmazonSelenium.PDA.Configuration;
using CakeExtracter.Etl.AmazonSelenium.PDA.Extractors;
using CakeExtracter.Etl.TradingDesk.Extracters;
using CakeExtracter.Etl.TradingDesk.LoadersDA.AmazonLoaders;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Concrete;
using DirectAgents.Domain.Entities.CPProg;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using SeleniumDataBrowser.PDA;
using SeleniumDataBrowser.PDA.Helpers;
using SeleniumDataBrowser.PDA.Models;
using SeleniumDataBrowser.PDA.PageActions;
using Platform = DirectAgents.Domain.Entities.CPProg.Platform;

namespace CakeExtracter.Commands.Selenium
{
    /// <inheritdoc />
    /// <summary>
    /// The class represents a command that is used to retrieve statistics
    /// of Product Display Ads type from the Amazon Advertising Portal.
    /// </summary>
    [Export(typeof(ConsoleCommand))]
    public class SyncAmazonPdaCommand : ConsoleCommand
    {
        private const int DefaultDaysAgo = 41;

        /// <summary>
        /// Gets or sets the command argument: Account ID in the database
        /// for which the command will be executed (default = all).
        /// </summary>
        public int? AccountId { get; set; }

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
        /// Gets or sets the command argument: Type of statistics will be extracted (default = all).
        /// </summary>
        public string StatsType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to enable extracting stats
        /// for only disabled accounts (default = false).
        /// </summary>
        public bool DisabledOnly { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to enable extracting stats of the Daily level
        /// from the Strategy level (default = false).
        /// </summary>
        public bool FromDatabase { get; set; }

        private AuthorizationModel authorizationModel;
        private AmazonPdaPageActions pageActionsManager;

        /// <inheritdoc cref="ConsoleCommand"/>/>
        /// <summary>
        /// Initializes a new instance of the <see cref="SyncAmazonPdaCommand" /> class.
        /// </summary>
        public SyncAmazonPdaCommand()
        {
            NoNeedToCreateRepeatRequests = true;

            IsCommand("SyncAmazonPdaCommand", "Sync Amazon Product Display Ads Stats");
            HasOption<int>("a|accountId=", "Account Id (default = all)", c => AccountId = c);
            HasOption("s|startDate=", "Start Date (default is 'daysAgo')", c => StartDate = DateTime.Parse(c));
            HasOption("e|endDate=", "End Date (default is yesterday)", c => EndDate = DateTime.Parse(c));
            HasOption<int>("d|daysAgo=", $"Days Ago to start, if startDate not specified (default = {DefaultDaysAgo})", c => DaysAgoToStart = c);
            HasOption<string>("t|statsType=", "Stats Type (default: all)", c => StatsType = c);
            HasOption<bool>("x|disabledOnly=", "Include only disabled accounts (default = false)", c => DisabledOnly = c);
            HasOption<bool>("f|fromDatabase=", "To enable extracting stats of the Daily level from the Strategy level (default = false)", c => FromDatabase = c);
        }

        /// <inheritdoc />
        /// <summary>
        /// The method resets command arguments to defaults.
        /// </summary>
        public override void ResetProperties()
        {
            AccountId = null;
            StartDate = null;
            EndDate = null;
            DaysAgoToStart = null;
            StatsType = null;
            DisabledOnly = false;
            FromDatabase = false;
        }

        /// <inheritdoc />
        /// <summary>
        /// The method runs the current command and extract and save statistics
        /// of Product Display Ads type from the Amazon Advertising Portal
        /// based on the command arguments.
        /// </summary>
        /// <param name="remainingArguments"></param>
        /// <returns>Execution code.</returns>
        public override int Execute(string[] remainingArguments)
        {
            PreparationForWork();

            RunEtl();

            return 0;
        }

        private void PreparationForWork()
        {
            InitializeAuthorizationModel();
            InitializePageActionsManager();

            LoginToPortal();
            SetAvailableProfileUrls();
        }

        private void LoginToPortal()
        {
            var loginManager = new PdaLoginHelper(
                authorizationModel,
                pageActionsManager,
                x => Logger.Info(x),
                x => Logger.Warn(x));
            loginManager.LoginToPortal();
        }

        private void SetAvailableProfileUrls()
        {
            var availableProfileUrls = PdaProfileUrlManager.GetAvailableProfileUrls(authorizationModel, pageActionsManager);
            Logger.Info("The following profiles were found for the current account:");
            availableProfileUrls.ForEach(x => Logger.Info($"{x.Key} - {x.Value}"));

            AmazonConsoleManagerUtility.AvailableProfileUrls = availableProfileUrls;
        }

        private void RunEtl()
        {
            var statsType = new StatsTypeAgg(StatsType);
            var dateRange = CommandHelper.GetDateRange(StartDate, EndDate, DaysAgoToStart, DefaultDaysAgo);
            Logger.Info("Amazon ETL (PDA Campaigns). DateRange: {0}.", dateRange);
            var accounts = GetAccounts();
            foreach (var account in accounts)
            {
                DoEtls(account, dateRange, statsType);
            }
            Logger.Info("Amazon ETL (PDA Campaigns) has been finished.");
        }

        private void InitializeAuthorizationModel()
        {
            var cookieDirectoryName = PdaConfigurationHelper.GetCookiesDirectoryName();
            authorizationModel = new AuthorizationModel
            {
                Login = PdaConfigurationHelper.GetEMail(),
                Password = PdaConfigurationHelper.GetEMailPassword(),
                CookiesDir = cookieDirectoryName,
            };
        }

        private void InitializePageActionsManager()
        {
            var timeoutInMinutes = PdaConfigurationHelper.GetWaitPageTimeout();
            pageActionsManager = new AmazonPdaPageActions(timeoutInMinutes);
            SetLogActionsForPageActionsManager();
        }

        private void SetLogActionsForPageActionsManager()
        {
            pageActionsManager.LogInfo = x => Logger.Info(x);
            pageActionsManager.LogError = x => Logger.Error(new Exception(x));
            pageActionsManager.LogWarning = x => Logger.Warn(x);
        }

        private void DoEtls(ExtAccount account, DateRange dateRange, StatsTypeAgg statsType)
        {
            Logger.Info(account.Id, "Commencing ETL for Amazon account ({0}) {1}", account.Id, account.Name);

            var amazonPdaUtility = CreateAmazonPdaUtility(account);
            try
            {
                if (statsType.Daily && !FromDatabase)
                {
                    DoEtlDailyFromRequests(account, dateRange, amazonPdaUtility);
                }

                if (statsType.Strategy)
                {
                    DoEtlStrategyFromRequests(account, dateRange, amazonPdaUtility);
                }

                if (statsType.Daily && FromDatabase)
                {
                    DoEtlDailyFromStrategyInDatabase(account, dateRange);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(account.Id, ex);
            }

            Logger.Info(account.Id, "Finished ETL for Amazon account ({0}) {1}", account.Id, account.Name);
        }

        private AmazonConsoleManagerUtility CreateAmazonPdaUtility(ExtAccount account)
        {
            var amazonPdaUtility = new AmazonConsoleManagerUtility(
                account.Name,
                authorizationModel,
                pageActionsManager,
                PdaConfigurationHelper.GetMaxRetryAttempts(),
                PdaConfigurationHelper.GetPauseBetweenAttempts(),
                x => Logger.Info(account.Id, x),
                x => Logger.Error(account.Id, new Exception(x)),
                x => Logger.Warn(account.Id, x));

            return amazonPdaUtility;
        }

        private static void DoEtlDailyFromRequests(ExtAccount account, DateRange dateRange, AmazonConsoleManagerUtility amazonPdaUtility)
        {
            var extractor = new AmazonPdaDailyRequestExtractor(account, dateRange, amazonPdaUtility);
            var loader = new AmazonPdaDailySummaryLoader(account.Id);
            CommandHelper.DoEtl(extractor, loader);
        }

        private static void DoEtlDailyFromStrategyInDatabase(ExtAccount account, DateRange dateRange)
        {
            var extractor = new DatabaseStrategyToDailySummaryExtractor(dateRange, account.Id);
            var loader = new AmazonDailySummaryLoader(account.Id);
            CommandHelper.DoEtl(extractor, loader);
        }

        private static void DoEtlStrategyFromRequests(ExtAccount account, DateRange dateRange, AmazonConsoleManagerUtility amazonPdaUtility)
        {
            var extractor = new AmazonPdaCampaignRequestExtractor(account, dateRange, amazonPdaUtility);
            var loader = new AmazonCampaignSummaryLoader(account.Id);
            CommandHelper.DoEtl(extractor, loader);
        }

        private IEnumerable<ExtAccount> GetAccounts()
        {
            var repository = new PlatformAccountRepository();
            if (!AccountId.HasValue)
            {
                var accounts = repository.GetAccountsWithFilledExternalIdByPlatformCode(Platform.Code_Amazon, DisabledOnly);
                return accounts;
            }
            var account = repository.GetAccount(AccountId.Value);
            return new[] { account };
        }
    }
}