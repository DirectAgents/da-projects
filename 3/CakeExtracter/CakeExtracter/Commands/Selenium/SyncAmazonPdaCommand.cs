using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using CakeExtracter.Common;
using DirectAgents.Domain.Concrete;
using DirectAgents.Domain.Entities.CPProg;
using CakeExtracter.Etl;
using CakeExtracter.Etl.AmazonSelenium.PDA.Extractors;
using CakeExtracter.Etl.AmazonSelenium.PDA.Extractors.RequestExtractors;
using CakeExtracter.Etl.TradingDesk.Extracters;
using CakeExtracter.Etl.TradingDesk.LoadersDA.AmazonLoaders;
using CakeExtracter.Helpers;
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
        /// Command argument: Account ID in the database for which the command will be executed (default = all)
        /// </summary>
        public int? AccountId { get; set; }

        /// <summary>
        /// Command argument: Start date from which statistics will be extracted (default is 'daysAgo')
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Command argument: End date to which statistics will be extracted (default is yesterday)
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Command argument: The number of days ago to calculate the start date from which statistics will be retrieved,
        /// used if StartDate not specified (default = 31)
        /// </summary>
        public int? DaysAgoToStart { get; set; }

        public string StatsType { get; set; }

        public bool DisabledOnly { get; set; }

        public bool FromDatabase { get; set; }

        public bool FromRequests { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SyncAmazonPdaCommand"/> class.
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
            HasOption<bool>("f|fromDatabase=", "??? maybe to remove ???", c => FromDatabase = c);
            HasOption<bool>("r|fromRequest=", "??? maybe to remove ???", c => FromRequests = c);
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
            FromRequests = false;
        }

        public override int Execute(string[] remainingArguments)
        {
            if (!AmazonPdaExtractor.IsInitialised)
            {
                AmazonPdaExtractor.PrepareExtractor();
            }
            RunEtl();
            return 0;
        }

        private void RunEtl()
        {
            var statsType = new StatsTypeAgg(StatsType);
            var dateRange = CommandHelper.GetDateRange(StartDate, EndDate, DaysAgoToStart, DefaultDaysAgo);
            Logger.Info("Amazon ETL (PDA Campaigns). DateRange: {0}.", dateRange);
            AmazonPdaExtractor.SetAvailableProfileUrls();
            var accounts = GetAccounts();
            foreach (var account in accounts)
            {
                DoEtls(account, dateRange, statsType);
            }
            Logger.Info("Amazon ETL (PDA Campaigns) has been finished.");
        }

        private void DoEtls(ExtAccount account, DateRange dateRange, StatsTypeAgg statsType)
        {
            Logger.Info(account.Id, "Commencing ETL for Amazon account ({0}) {1}", account.Id, account.Name);
            try
            {
                if (statsType.Daily && !FromDatabase)
                {
                    if (FromRequests)
                    {
                        DoEtlDailyFromRequests(account, dateRange);
                    }
                    else
                    {
                        DoEtlDaily(account, dateRange);
                    }
                }

                if (statsType.Strategy)
                {
                    if (FromRequests)
                    {
                        DoEtlStrategyFromRequests(account, dateRange);
                    }
                    else
                    {
                        DoEtlStrategy(account, dateRange);
                    }
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

        private static void DoEtlDaily(ExtAccount account, DateRange dateRange)
        {
            var extractor = new AmazonPdaDailyExtractor(account, dateRange);
            var loader = new AmazonPdaDailySummaryLoader(account.Id);
            StartEtl(extractor, loader);
        }

        private static void DoEtlDailyFromRequests(ExtAccount account, DateRange dateRange)
        {
            var extractor = new AmazonPdaDailyRequestExtractor(account, dateRange);
            var loader = new AmazonPdaDailySummaryLoader(account.Id);
            StartEtl(extractor, loader);
        }

        private static void DoEtlDailyFromStrategyInDatabase(ExtAccount account, DateRange dateRange)
        {
            var extractor = new DatabaseStrategyToDailySummaryExtractor(dateRange, account.Id);
            var loader = new AmazonDailySummaryLoader(account.Id);
            StartEtl(extractor, loader);
        }

        private static void DoEtlStrategy(ExtAccount account, DateRange dateRange)
        {
            var extractor = new AmazonPdaCampaignExtractor(account, dateRange);
            var loader = new AmazonCampaignSummaryLoader(account.Id);
            StartEtl(extractor, loader);
        }

        private static void DoEtlStrategyFromRequests(ExtAccount account, DateRange dateRange)
        {
            var extractor = new AmazonPdaCampaignRequestExtractor(account, dateRange);
            var loader = new AmazonCampaignSummaryLoader(account.Id);
            StartEtl(extractor, loader);
        }

        private static void StartEtl<T>(Extracter<T> extractor, Loader<T> loader)
        {
            var extractorThread = extractor.Start();
            var loaderThread = loader.Start(extractor);
            extractorThread.Join();
            loaderThread.Join();
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