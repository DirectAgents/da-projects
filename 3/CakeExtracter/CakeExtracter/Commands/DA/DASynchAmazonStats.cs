using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using CakeExtracter.Bootstrappers;
using CakeExtracter.Common;
using CakeExtracter.Common.JobExecutionManagement;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Exceptions;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Models;
using CakeExtracter.Etl;
using CakeExtracter.Etl.Amazon.Exceptions;
using CakeExtracter.Etl.Amazon.Extractors.AmazonApiExtractors;
using CakeExtracter.Etl.Amazon.Loaders;
using CakeExtracter.Etl.Amazon.Loaders.AnalyticTablesSync;
using CakeExtracter.Etl.TradingDesk.Extracters.AmazonExtractors;
using CakeExtracter.Helpers;
using CakeExtracter.Logging.TimeWatchers.Amazon;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class DASynchAmazonStats : ConsoleCommand
    {
        public const int DefaultDaysAgo = 41;

        public virtual int? AccountId { get; set; }

        public virtual DateTime? StartDate { get; set; }

        public virtual DateTime? EndDate { get; set; }

        public virtual int? DaysAgoToStart { get; set; }

        public virtual string StatsType { get; set; }

        public virtual bool KeepAmazonReports { get; set; }

        public virtual bool SearchTermDateRangeToToday { get; set; }

        public DASynchAmazonStats()
        {
            IsCommand("daSynchAmazonStats", "Synch Amazon Stats");
            HasOption<int>("a|accountId=", "Account Id (default = all)", c => AccountId = c);
            HasOption("s|startDate=", "Start Date (default is 'daysAgo')", c => StartDate = DateTime.Parse(c));
            HasOption("e|endDate=", "End Date (default is yesterday)", c => EndDate = DateTime.Parse(c));
            HasOption<int>("d|daysAgo=", $"Days Ago to start, if startDate not specified (default = {DefaultDaysAgo})", c => DaysAgoToStart = c);
            HasOption<string>("t|statsType=", "Stats Type (default: all)", c => StatsType = c);
            HasOption<bool>("k|keepAmazonReports=", "Store received Amazon reports in a separate folder (default = false)", c => KeepAmazonReports = c);
            HasOption<bool>(
                "w|willSearchTermDateRangeToToday=",
                "End date of range for Search terms will be today, if 'End date' is not specified (default = false (yesterday))",
                c => SearchTermDateRangeToToday = c);
        }

        public static int RunStatic(int? accountId = null, DateTime? startDate = null, DateTime? endDate = null, string statsType = null)
        {
            AutoMapperBootstrapper.CheckRunSetup();
            var cmd = new DASynchAmazonStats
            {
                AccountId = accountId,
                StartDate = startDate,
                EndDate = endDate,
                StatsType = statsType,
            };
            return cmd.Run();
        }

        public override void ResetProperties()
        {
            AccountId = null;
            StartDate = null;
            EndDate = null;
            DaysAgoToStart = null;
            StatsType = null;
            KeepAmazonReports = false;
            SearchTermDateRangeToToday = false;
        }

        public override int Execute(string[] remainingArguments)
        {
            IntervalBetweenUnsuccessfulAndNewRequestInMinutes = ConfigurationHelper.GetIntConfigurationValue(
                "AmazonIntervalBetweenRequestsInMinutes", IntervalBetweenUnsuccessfulAndNewRequestInMinutes);
            var statsType = new StatsTypeAgg(StatsType);
            var dateRange = CommandHelper.GetDateRange(StartDate, EndDate, DaysAgoToStart, DefaultDaysAgo);
            Logger.Info("Amazon ETL. DateRange {0}.", dateRange);
            var accounts = GetAccounts();
            AmazonUtility.TokenSets = GetTokens();
            Parallel.ForEach(accounts, account =>
            {
                Logger.Info(account.Id, "Commencing ETL for Amazon account ({0}) {1}", account.Id, account.Name);
                var amazonUtility = CreateUtility(account);
                try
                {
                    if (statsType.Creative)
                    {
                        DoETL_Creative(dateRange, account, amazonUtility);
                    }

                    if (statsType.Keyword)
                    {
                        DoETL_Keyword(dateRange, account, amazonUtility);
                    }

                    if (statsType.Daily)
                    {
                        DoETL_DailyFromKeywordsDatabaseData(dateRange, account); // need to update keywords stats first
                    }

                    if (statsType.SearchTerm && !statsType.All)
                    {
                        DoETL_SearchTerm(dateRange, account, amazonUtility); // need to update keywords stats first
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(account.Id, ex);
                }

                AmazonTimeTracker.Instance.LogTrackingData(account.Id);
                Logger.Info(account.Id, "Finished ETL for Amazon account ({0}) {1}", account.Id, account.Name);
            });
            SaveTokens(AmazonUtility.TokenSets);
            return 0;
        }

        /// <inheritdoc />
        public override IEnumerable<CommandWithSchedule> GetUniqueBroadCommands(
            IEnumerable<CommandWithSchedule> commands)
        {
            var broadCommands = new List<CommandWithSchedule>();
            var commandsGroupedByAccountAndLevel = commands.GroupBy(x =>
            {
                var command = x.Command as DASynchAmazonStats;
                return new { command?.AccountId, command?.StatsType };
            });
            foreach (var commandsGroup in commandsGroupedByAccountAndLevel)
            {
                var accountLevelBroadCommands = GetUniqueBroadAccountLevelCommands(commandsGroup);
                broadCommands.AddRange(accountLevelBroadCommands);
            }

            return broadCommands;
        }

        public virtual IEnumerable<ExtAccount> GetAccounts()
        {
            var accountRepository = DIKernel.Get<IPlatformAccountRepository>();
            if (!AccountId.HasValue)
            {
                var accounts = accountRepository.GetAccountsWithFilledExternalIdByPlatformCode(Platform.Code_Amazon, false);
                return accounts;
            }

            var account = accountRepository.GetAccount(AccountId.Value);
            return new[] { account };
        }

        public virtual string[] GetTokens()
        {
            // Get tokens, if any, from the database
            return Platform.GetPlatformTokens(Platform.Code_Amazon);
        }

        public virtual void SaveTokens(string[] tokens)
        {
            Platform.SavePlatformTokens(Platform.Code_Amazon, tokens);
        }

        public virtual void SynchAsinAnalyticTables(int accountId)
        {
            try
            {
                CommandExecutionContext.Current?.SetJobExecutionStateInHistory("Sync analytic table.", accountId);
                AmazonTimeTracker.Instance.ExecuteWithTimeTracking(
                    () =>
                    {
                        var amazonAmsSyncher = new AmazonAmsAnalyticSyncher();
                        amazonAmsSyncher.SyncAsinLevelForAccount(accountId);
                    }, accountId, AmazonJobLevels.Creative, AmazonJobOperations.SyncToAnalyticTables);
            }
            catch (Exception ex)
            {
                Logger.Error(new Exception("Error occurred while Asin analytic table sync.", ex));
            }
        }

        public virtual AmazonUtility CreateUtility(ExtAccount account)
        {
            var amazonUtility = new AmazonUtility(m => Logger.Info(account.Id, m), m => Logger.Warn(account.Id, m));
            amazonUtility.SetWhichAlt(account.ExternalId);
            amazonUtility.SetApiEndpointUrl(account.Name);
            if (!KeepAmazonReports)
            {
                return amazonUtility;
            }

            amazonUtility.KeepReports = KeepAmazonReports;
            amazonUtility.ReportPrefix = account.Id.ToString();
            return amazonUtility;
        }

        private void DoETL_DailyFromKeywordsDatabaseData(DateRange dateRange, ExtAccount account)
        {
            AmazonTimeTracker.Instance.ExecuteWithTimeTracking(
                () =>
                {
                    var extractor = DIKernel.Get<AmazonDatabaseKeywordsToDailySummaryExtractor>(("dateRange", dateRange), ("accountId", account.Id));
                    var loader = DIKernel.Get<BaseAmazonLevelLoader<DailySummary, DailySummaryMetric>>(("accountId", account.Id));
                    extractor.ProcessFailedExtraction += exception =>
                        ScheduleNewCommandLaunch<DASynchAmazonStats>(command =>
                            UpdateCommandParameters(command, exception));
                    InitCommonEtlEvents<DailySummary, DailySummaryMetric, AmazonDatabaseKeywordsToDailySummaryExtractor,
                        BaseAmazonLevelLoader<DailySummary, DailySummaryMetric>>(extractor, loader);
                    CommandHelper.DoEtl(extractor, loader);
                },
                account.Id,
                AmazonJobLevels.Account,
                AmazonJobOperations.Total);
        }

        private void DoETL_Creative(DateRange dateRange, ExtAccount account, AmazonUtility amazonUtility)
        {
            AmazonTimeTracker.Instance.ExecuteWithTimeTracking(
                () =>
                {
                    var extractor = DIKernel.Get<AmazonApiAdExtractor>(
                        ("amazonUtility", amazonUtility),
                        ("dateRange", dateRange),
                        ("account", account),
                        ("campaignFilter", account.Filter),
                        ("campaignFilterOut", null));
                    var loader = DIKernel.Get<BaseAmazonLevelLoader<TDadSummary, TDadSummaryMetric>>(("accountId", account.Id));
                    InitEtlEvents<TDadSummary, TDadSummaryMetric, AmazonApiAdExtractor, BaseAmazonLevelLoader<TDadSummary, TDadSummaryMetric>>(
                        extractor, loader);
                    CommandHelper.DoEtl(extractor, loader);
                    SynchAsinAnalyticTables(account.Id);
                },
                account.Id,
                AmazonJobLevels.Creative,
                AmazonJobOperations.Total);
        }

        private void DoETL_Keyword(DateRange dateRange, ExtAccount account, AmazonUtility amazonUtility)
        {
            AmazonTimeTracker.Instance.ExecuteWithTimeTracking(
                () =>
                {
                    var extractor = DIKernel.Get<AmazonApiKeywordExtractor>(
                        ("amazonUtility", amazonUtility),
                        ("dateRange", dateRange),
                        ("account", account),
                        ("campaignFilter", account.Filter),
                        ("campaignFilterOut", null));
                    var loader = DIKernel.Get<BaseAmazonLevelLoader<KeywordSummary, KeywordSummaryMetric>>(("accountId", account.Id));
                    InitEtlEvents<KeywordSummary, KeywordSummaryMetric, AmazonApiKeywordExtractor, BaseAmazonLevelLoader<KeywordSummary, KeywordSummaryMetric>>(extractor, loader);
                    CommandHelper.DoEtl(extractor, loader);
                },
                account.Id,
                AmazonJobLevels.Keyword,
                AmazonJobOperations.Total);
        }

        private void DoETL_SearchTerm(DateRange dateRange, ExtAccount account, AmazonUtility amazonUtility)
        {
            var searchTermDateRange = GetDateRangeForSearchTerms(dateRange);
            AmazonTimeTracker.Instance.ExecuteWithTimeTracking(
                () =>
                {
                    var extractor = DIKernel.Get<AmazonApiSearchTermExtractor>(
                        ("amazonUtility", amazonUtility),
                        ("dateRange", searchTermDateRange),
                        ("account", account),
                        ("campaignFilter", account.Filter),
                        ("campaignFilterOut", null));
                    var loader = DIKernel.Get<BaseAmazonLevelLoader<SearchTermSummary, SearchTermSummaryMetric>>(("accountId", account.Id));
                    InitEtlEvents<SearchTermSummary, SearchTermSummaryMetric, AmazonApiSearchTermExtractor,
                        BaseAmazonLevelLoader<SearchTermSummary, SearchTermSummaryMetric>>(extractor, loader);
                    CommandHelper.DoEtl(extractor, loader);
                },
                account.Id,
                AmazonJobLevels.SearchTerm,
                AmazonJobOperations.Total);
        }

        private void InitEtlEvents<TSummary, TSummaryMetric, TExtractor, TLoader>(TExtractor extractor, TLoader loader)
            where TSummary : DatedStatsSummary, new()
            where TSummaryMetric : SummaryMetric, new()
            where TExtractor : BaseAmazonExtractor<TSummary>
            where TLoader : BaseAmazonLevelLoader<TSummary, TSummaryMetric>
        {
            extractor.ProcessFailedExtraction += exception =>
                ScheduleNewCommandLaunch<DASynchAmazonStats>(command =>
                    UpdateCommandParameters(command, exception));
            InitCommonEtlEvents<TSummary, TSummaryMetric, TExtractor, TLoader>(extractor, loader);
        }

        private void InitCommonEtlEvents<TSummary, TSummaryMetric, TExtractor, TLoader>(TExtractor extractor, TLoader loader)
            where TSummary : DatedStatsSummary, new()
            where TSummaryMetric : SummaryMetric, new()
            where TExtractor : Extracter<TSummary>
            where TLoader : BaseAmazonLevelLoader<TSummary, TSummaryMetric>
        {
            loader.ProcessFailedExtraction += exception =>
                ScheduleNewCommandLaunch<DASynchAmazonStats>(command =>
                    UpdateCommandParameters(command, exception));
            extractor.ProcessEtlFailedWithoutInformation += exception =>
                ScheduleNewCommandLaunch<DASynchAmazonStats>(command =>
                    UpdateCommandParameters(command, exception));
            loader.ProcessEtlFailedWithoutInformation += exception =>
                ScheduleNewCommandLaunch<DASynchAmazonStats>(command =>
                    UpdateCommandParameters(command, exception));
        }

        private void UpdateCommandParameters(DASynchAmazonStats command, FailedStatsLoadingException exception)
        {
            command.StartDate = exception.StartDate ?? command.StartDate;
            command.EndDate = exception.EndDate ?? command.EndDate;
            command.AccountId = exception.AccountId ?? command.AccountId;
            var statsType = new StatsTypeAgg
            {
                Creative = exception.ByAd,
                Keyword = exception.ByKeyword,
                Daily = exception.ByDaily,
                Strategy = exception.ByCampaign,
                SearchTerm = exception.BySearchTerm,
            };
            command.StatsType = statsType.GetStatsTypeString();
        }

        private void UpdateCommandParameters(DASynchAmazonStats command, FailedEtlException exception)
        {
            command.AccountId = exception.AccountId;
        }

        private IEnumerable<CommandWithSchedule> GetUniqueBroadAccountLevelCommands(IEnumerable<CommandWithSchedule> commandsWithSchedule)
        {
            var accountLevelCommands = new List<Tuple<DASynchAmazonStats, DateRange, CommandWithSchedule>>();
            foreach (var commandWithSchedule in commandsWithSchedule)
            {
                var command = (DASynchAmazonStats)commandWithSchedule.Command;
                var commandDateRange = CommandHelper.GetDateRange(command.StartDate, command.EndDate, command.DaysAgoToStart, DefaultDaysAgo);
                var crossCommands = accountLevelCommands.Where(x => commandDateRange.IsCrossDateRange(x.Item2)).ToList();
                foreach (var crossCommand in crossCommands)
                {
                    commandDateRange = commandDateRange.MergeDateRange(crossCommand.Item2);
                    commandWithSchedule.ScheduledTime =
                        crossCommand.Item3.ScheduledTime > commandWithSchedule.ScheduledTime
                            ? crossCommand.Item3.ScheduledTime
                            : commandWithSchedule.ScheduledTime;
                    accountLevelCommands.Remove(crossCommand);
                }

                accountLevelCommands.Add(new Tuple<DASynchAmazonStats, DateRange, CommandWithSchedule>(command, commandDateRange, commandWithSchedule));
            }

            var broadCommands = accountLevelCommands.Select(GetCommandWithCorrectDateRange).ToList();
            return broadCommands;
        }

        private CommandWithSchedule GetCommandWithCorrectDateRange(Tuple<DASynchAmazonStats, DateRange, CommandWithSchedule> setting)
        {
            setting.Item1.StartDate = setting.Item2.FromDate;
            setting.Item1.EndDate = setting.Item2.ToDate;
            setting.Item1.DaysAgoToStart = null;
            setting.Item3.Command = setting.Item1;
            return setting.Item3;
        }

        private DateRange GetDateRangeForSearchTerms(DateRange dateRange)
        {
            var searchTermDateRange = dateRange;
            if (!SearchTermDateRangeToToday)
            {
                return searchTermDateRange;
            }
            searchTermDateRange = CommandHelper.GetDateRange(StartDate, EndDate, DaysAgoToStart, DefaultDaysAgo, true);
            Logger.Info("Search Term Date Range - {0}.", searchTermDateRange);
            return searchTermDateRange;
        }
    }
}
