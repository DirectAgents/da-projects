using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using CakeExtracter.Bootstrappers;
using CakeExtracter.Common;
using CakeExtracter.Common.JobExecutionManagement;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Models;
using CakeExtracter.Etl;
using CakeExtracter.Etl.Facebook.Builders;
using CakeExtracter.Etl.Facebook.Exceptions;
using CakeExtracter.Etl.Facebook.Extractors;
using CakeExtracter.Etl.Facebook.Interfaces;
using CakeExtracter.Etl.Facebook.Loaders;
using CakeExtracter.Etl.Facebook.Repositories;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using FacebookAPI.Providers;

namespace CakeExtracter.Commands
{
    /// <summary>
    /// Facebook stats updating command(Included creatives expansion).
    /// </summary>
    /// <seealso cref="ConsoleCommand" />
    [Export(typeof(ConsoleCommand))]
    public class DASynchFacebookStats : ConsoleCommand
    {
        private const int DefaultDaysAgo = 41;

        private const int ProcessingChunkSize = 11;

        private readonly FacebookInsightsDataProviderBuilder insightsDataProviderBuilder;
        private readonly FacebookInsightsReachMetricProviderBuilder insightsReachMetricProviderBuilder;

        /// <summary>
        /// Gets or sets command argument: Account ID in the database for which the command will be executed (default = all).
        /// </summary>
        public int? AccountId { get; set; }

        /// <summary>
        /// Gets or sets command argument: Campaign ID in the database for which the command will be executed (default = all).
        /// </summary>
        public int? CampaignId { get; set; }

        /// <summary>
        /// Gets or sets command argument: Start date from which statistics will be extracted (default is 'daysAgo').
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or sets command argument: End date to which statistics will be extracted (default is yesterday).
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Gets or sets command argument: The number of days ago to calculate the start date from which statistics will be retrieved,
        /// used if StartDate not specified (default = 41).
        /// </summary>
        public int? DaysAgoToStart { get; set; }

        /// <summary>
        /// Gets or sets command argument: StatsType level for which the command will be executed (default = all).
        /// </summary>
        public string StatsType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether include only disabled accounts (default = all).
        /// </summary>
        public bool DisabledOnly { get; set; }

        /// <summary>
        /// Gets or sets command argument: Account ID in the database for which the command will be executed.
        /// </summary>
        public int? MinAccountId { get; set; }

        /// <summary>
        /// The static method used for sync from Admin Portal.
        /// </summary>
        /// <param name="campaignId">Id of search campaign.</param>
        /// <param name="accountId">Id of search account.</param>
        /// <param name="startDate">Start date.</param>
        /// <param name="endDate">End date.</param>
        /// <param name="statsType">StatsType level.</param>
        /// <returns>Execution code.</returns>
        public static int RunStatic(int? campaignId = null, int? accountId = null, DateTime? startDate = null, DateTime? endDate = null, string statsType = null)
        {
            AutoMapperBootstrapper.CheckRunSetup();
            var cmd = new DASynchFacebookStats
            {
                CampaignId = campaignId,
                AccountId = accountId,
                StartDate = startDate,
                EndDate = endDate,
                StatsType = statsType,
            };
            return cmd.Run();
        }

        /// <inheritdoc />
        /// <summary>
        /// The method resets command arguments to defaults.
        /// </summary>
        public override void ResetProperties()
        {
            AccountId = null;
            CampaignId = null;
            StartDate = null;
            EndDate = null;
            DaysAgoToStart = null;
            StatsType = null;
            DisabledOnly = false;
            MinAccountId = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DASynchFacebookStats"/> class.
        /// </summary>
        public DASynchFacebookStats()
        {
            IsCommand("daSynchFacebookStats", "synch Facebook stats");
            HasOption<int>("a|accountId=", "Account Id (default = all)", c => AccountId = c);
            HasOption<int>("c|campaignId=", "Campaign Id (optional)", c => CampaignId = c);
            HasOption("s|startDate=", "Start Date (default is 'daysAgo')", c => StartDate = DateTime.Parse(c));
            HasOption("e|endDate=", "End Date (default is yesterday)", c => EndDate = DateTime.Parse(c));
            HasOption<int>("d|daysAgo=", $"Days Ago to start, if startDate not specified (default = {DefaultDaysAgo})", c => DaysAgoToStart = c);
            HasOption<string>("t|statsType=", "Stats Type (default: all)", c => StatsType = c);
            HasOption<bool>("x|disabledOnly=", "Include only disabled accounts (default = false)", c => DisabledOnly = c);
            HasOption<int>("m|minAccountId=", "Include this and all higher accountIds (optional)", c => MinAccountId = c);
            insightsDataProviderBuilder = new FacebookInsightsDataProviderBuilder();
            insightsReachMetricProviderBuilder = new FacebookInsightsReachMetricProviderBuilder();
        }

        /// <inheritdoc/>
        public override int Execute(string[] remainingArguments)
        {
            var dateRange = CommandHelper.GetDateRange(StartDate, EndDate, DaysAgoToStart, DefaultDaysAgo);
            Logger.Info("Facebook ETL. DateRange {0}.", dateRange);

            var statsType = new FbStatsTypeAgg(StatsType);
            var accounts = GetAccounts();

            var fbAdMetadataProvider = new FacebookAdMetadataProvider(null, null);
            var metadataExtractor = new FacebookAdMetadataExtractor(fbAdMetadataProvider);
            Parallel.ForEach(accounts, (account) => { DoEtl(account, dateRange, statsType, metadataExtractor); });
            return 0;
        }

        /// <inheritdoc />
        public override IEnumerable<CommandWithSchedule> GetUniqueBroadCommands(
            IEnumerable<CommandWithSchedule> commands)
        {
            var broadCommands = new List<CommandWithSchedule>();
            var groupedCommands = commands.GroupBy(x =>
            {
                var command = x.Command as DASynchFacebookStats;
                return new { command?.AccountId, command?.CampaignId, command?.StatsType };
            });

            foreach (var commandsGroup in groupedCommands)
            {
                var accountBroadCommands = GetUniqueBroadAccountCommands(commandsGroup);
                broadCommands.AddRange(accountBroadCommands);
            }

            return broadCommands;
        }

        private void DoEtl(
            ExtAccount account,
            DateRange dateRange,
            FbStatsTypeAgg statsType,
            FacebookAdMetadataExtractor metadataExtractor)
        {
            CommandExecutionContext.Current.SetJobExecutionStateInHistory("Started", account.Id);
            var acctDateRange = GetAccountDateRange(dateRange, account);
            Logger.Info(account.Id, $"Facebook ETL. Account {account.Id} - {account.Name}. DateRange {acctDateRange}.");
            if (IsDataRangeEmpty(acctDateRange, account))
            {
                return;
            }

            var fbUtility = insightsDataProviderBuilder.BuildInsightsDataProvider(account);
            int? numDailyItems = null;
            if (statsType.Daily)
            {
                numDailyItems = DoETL_Daily(acctDateRange, account, fbUtility);
            }

            if (IsDailyOnlyAccountsMode(statsType, account, acctDateRange))
            {
                return;
            }

            // Skip strategy & adset stats if there were no dailies
            if (statsType.Strategy && (numDailyItems == null || numDailyItems.Value > 0))
            {
                DoETL_Strategy(acctDateRange, account, fbUtility);
            }

            if (statsType.AdSet && (numDailyItems == null || numDailyItems.Value > 0))
            {
                DoETL_AdSet(acctDateRange, account, fbUtility);
            }

            if (statsType.Creative && (numDailyItems == null || numDailyItems.Value > 0))
            {
                DoETL_Creative(acctDateRange, account, fbUtility, metadataExtractor);
            }

            if (statsType.Reach && (numDailyItems == null || numDailyItems.Value > 0))
            {
                var fbReachMetricUtility = insightsReachMetricProviderBuilder.BuildInsightsReachMetricProvider(account);
                DoETL_Reach(acctDateRange, account, fbReachMetricUtility);
            }

            Logger.Info(account.Id, $"Finished Facebook ETL. Account {account.Id} - {account.Name}. DateRange {acctDateRange}.");
            CommandExecutionContext.Current.SetJobExecutionStateInHistory("Finished", account.Id);
        }

        private static bool IsDataRangeEmpty(DateRange acctDateRange, ExtAccount account)
        {
            if (acctDateRange.ToDate < acctDateRange.FromDate)
            {
                Logger.Info(
                    account.Id,
                    "Finished Facebook ETL. Account {0} - {1}. DateRange {2}.",
                    account.Id,
                    account.Name,
                    acctDateRange);
                CommandExecutionContext.Current.SetJobExecutionStateInHistory("Finished", account.Id);
                return true;
            }

            return false;
        }

        private bool IsDailyOnlyAccountsMode(FbStatsTypeAgg statsType, ExtAccount account, DateRange acctDateRange)
        {
            var dailyOnlyAccounts = !AccountId.HasValue || statsType.All
                                        ? ConfigurationHelper.ExtractEnumerableFromConfig("FB_DailyStatsOnly").ToArray()
                                        : Array.Empty<string>();
            if (dailyOnlyAccounts.Contains(account.ExternalId))
            {
                Logger.Info(
                    account.Id,
                    "Finished Facebook ETL. Account {0} - {1}. DateRange {2}.",
                    account.Id,
                    account.Name,
                    acctDateRange);
                CommandExecutionContext.Current.SetJobExecutionStateInHistory("Finished", account.Id);
                return true;
            }

            return false;
        }

        private DateRange GetAccountDateRange(DateRange dateRange, ExtAccount account)
        {
            var acctDateRange = new DateRange(dateRange.FromDate, dateRange.ToDate);
            if (account.Campaign != null)
            {
                if (!StartDate.HasValue
                    && account.Campaign.Advertiser.StartDate.HasValue
                    && acctDateRange.FromDate < account.Campaign.Advertiser.StartDate.Value)
                {
                    acctDateRange.FromDate = account.Campaign.Advertiser.StartDate.Value;
                }

                if (!EndDate.HasValue
                    && account.Campaign.Advertiser.EndDate.HasValue
                    && acctDateRange.ToDate > account.Campaign.Advertiser.EndDate.Value)
                {
                    acctDateRange.ToDate = account.Campaign.Advertiser.EndDate.Value;
                }
            }

            return acctDateRange;
        }

        private int DoETL_Daily(DateRange dateRange, ExtAccount account, FacebookInsightsDataProvider fbUtility)
        {
            CommandExecutionContext.Current.SetJobExecutionStateInHistory("Daily level.", account.Id);
            var dateRangesToProcess = dateRange.GetDaysChunks(ProcessingChunkSize).ToList();
            int addedCount = 0;
            dateRangesToProcess.ForEach(rangeToProcess =>
            {
                var extractor = new FacebookDailySummaryExtractor(rangeToProcess, account, fbUtility);
                var loader = new FacebookDailySummaryLoader(account.Id, rangeToProcess);
                InitEtlEvents(extractor, loader);
                CommandHelper.DoEtl(extractor, loader);
                addedCount += extractor.Added;
            });
            return addedCount;
        }

        private void DoETL_Strategy(DateRange dateRange, ExtAccount account, FacebookInsightsDataProvider fbUtility)
        {
            CommandExecutionContext.Current.SetJobExecutionStateInHistory("Strategy level.", account.Id);
            var dateRangesToProcess = dateRange.GetDaysChunks(ProcessingChunkSize).ToList();
            dateRangesToProcess.ForEach(rangeToProcess =>
            {
                var extractor = new FacebookCampaignSummaryExtractor(rangeToProcess, account, fbUtility);
                var loader = new FacebookCampaignSummaryLoaderV2(account.Id, rangeToProcess);
                InitEtlEvents(extractor, loader);
                CommandHelper.DoEtl(extractor, loader);
            });
        }

        private void DoETL_AdSet(DateRange dateRange, ExtAccount account, FacebookInsightsDataProvider fbUtility)
        {
            CommandExecutionContext.Current.SetJobExecutionStateInHistory("Adset level.", account.Id);
            var dateRangesToProcess = dateRange.GetDaysChunks(ProcessingChunkSize).ToList();
            dateRangesToProcess.ForEach(rangeToProcess =>
            {
                var extractor = new FacebookAdSetSummaryExtractor(rangeToProcess, account, fbUtility);
                var loader = new FacebookAdSetSummaryLoaderV2(account.Id, rangeToProcess);
                InitEtlEvents(extractor, loader);
                CommandHelper.DoEtl(extractor, loader);
            });
        }

        private void DoETL_Creative(DateRange dateRange, ExtAccount account, FacebookInsightsDataProvider fbUtility, FacebookAdMetadataExtractor metadataExtractor)
        {
            CommandExecutionContext.Current.SetJobExecutionStateInHistory("Ad level.", account.Id);
            var dateRangesToProcess = dateRange.GetDaysChunks(ProcessingChunkSize).ToList();
            Logger.Info(account.Id, "Started reading ad's metadata");

            // It's not possible currently fetch facebook creatives data from the insights endpoint.
            var allAdsMetadata = metadataExtractor.GetAdCreativesData(account);
            Logger.Info(account.Id, "Finished reading ad's metadata");
            dateRangesToProcess.ForEach(rangeToProcess =>
            {
                var extractor = new FacebookAdSummaryExtractor(rangeToProcess, account, fbUtility, allAdsMetadata);
                var loader = new FacebookAdSummaryLoaderV2(account.Id, rangeToProcess);
                InitEtlEvents(extractor, loader);
                CommandHelper.DoEtl(extractor, loader);
            });
        }

        private void DoETL_Reach(DateRange dateRange, ExtAccount account, FacebookInsightsReachMetricProvider fbReachMetricUtility)
        {
            CommandExecutionContext.Current.SetJobExecutionStateInHistory("Reach Metric level.", account.Id);
            var metricRepository = new FacebookReachMetricDatabaseRepository();
            var extractor = new FacebookReachMetricExtractor(dateRange, account, fbReachMetricUtility);
            var loader = new FacebookReachMetricLoader(account.Id, metricRepository);
            InitEtlEvents(extractor, loader);
            CommandHelper.DoEtl(extractor, loader);
        }

        private IEnumerable<ExtAccount> GetAccounts()
        {
            using (var db = new ClientPortalProgContext())
            {
                var accounts = db.ExtAccounts.Include("Network").Include("Campaign.Advertiser").Where(a => a.Platform.Code == Platform.Code_FB);
                if (CampaignId.HasValue || AccountId.HasValue)
                {
                    if (CampaignId.HasValue)
                    {
                        accounts = accounts.Where(a => a.CampaignId == CampaignId.Value);
                    }
                    if (AccountId.HasValue)
                    {
                        accounts = accounts.Where(a => a.Id == AccountId.Value);
                    }
                }
                else if (!DisabledOnly)
                {
                    accounts = accounts.Where(a => !a.Disabled); // all accounts that aren't disabled
                }
                if (DisabledOnly)
                {
                    accounts = accounts.Where(a => a.Disabled);
                }
                if (MinAccountId.HasValue)
                {
                    accounts = accounts.Where(a => a.Id >= MinAccountId.Value);
                }

                return accounts.Where(a => a.ExternalId != null && a.ExternalId.Trim() != string.Empty)
                    .OrderBy(a => a.Id)
                    .ToList();
            }
        }

        private List<CommandWithSchedule> GetUniqueBroadAccountCommands(IEnumerable<CommandWithSchedule> commandsWithSchedule)
        {
            var accountCommands = new List<Tuple<DASynchFacebookStats, DateRange, CommandWithSchedule>>();
            foreach (var commandWithSchedule in commandsWithSchedule)
            {
                var command = (DASynchFacebookStats)commandWithSchedule.Command;
                var commandDateRange = CommandHelper.GetDateRange(command.StartDate, command.EndDate, command.DaysAgoToStart, 0);
                var crossCommands = accountCommands.Where(x => commandDateRange.IsCrossDateRange(x.Item2)).ToList();
                foreach (var crossCommand in crossCommands)
                {
                    commandDateRange = commandDateRange.MergeDateRange(crossCommand.Item2);
                    commandWithSchedule.ScheduledTime =
                        crossCommand.Item3.ScheduledTime > commandWithSchedule.ScheduledTime
                            ? crossCommand.Item3.ScheduledTime
                            : commandWithSchedule.ScheduledTime;
                    accountCommands.Remove(crossCommand);
                }

                accountCommands.Add(new Tuple<DASynchFacebookStats, DateRange, CommandWithSchedule>(command, commandDateRange, commandWithSchedule));
            }

            var broadCommands = accountCommands.Select(GetCommandWithCorrectDateRange).ToList();
            return broadCommands;
        }

        private CommandWithSchedule GetCommandWithCorrectDateRange(Tuple<DASynchFacebookStats, DateRange, CommandWithSchedule> setting)
        {
            setting.Item1.StartDate = setting.Item2.FromDate;
            setting.Item1.EndDate = setting.Item2.ToDate;
            setting.Item1.DaysAgoToStart = null;
            setting.Item3.Command = setting.Item1;
            return setting.Item3;
        }

        private void InitEtlEvents<TLoader, TSummary, TProvider>(FacebookApiExtractor<TSummary, TProvider> extractor, TLoader loader)
            where TLoader : Loader<TSummary>, IFacebookLoadingErrorHandler
            where TProvider : FacebookInsightsDataProvider
        {
            GeneralInitEtlEvents(extractor, loader);
            extractor.ProcessFailedExtraction += exception =>
                ScheduleNewCommandLaunch<DASynchFacebookStats>(command =>
                    UpdateCommandParameters(command, exception));
            loader.ProcessFailedLoading += exception =>
                ScheduleNewCommandLaunch<DASynchFacebookStats>(command =>
                    UpdateCommandParameters(command, exception));
        }

        private void GeneralInitEtlEvents<T>(Extracter<T> extractor, Loader<T> loader)
        {
            extractor.ProcessEtlFailedWithoutInformation += exception =>
                ScheduleNewCommandLaunch<DASynchFacebookStats>(command => { });
            loader.ProcessEtlFailedWithoutInformation += exception =>
                ScheduleNewCommandLaunch<DASynchFacebookStats>(command => { });
        }

        private void UpdateCommandParameters(DASynchFacebookStats command, FacebookFailedEtlException exception)
        {
            command.StartDate = exception.StartDate;
            command.EndDate = exception.EndDate;
            command.AccountId = exception.AccountId;
            command.StatsType = exception.StatsType;
        }
    }
}
