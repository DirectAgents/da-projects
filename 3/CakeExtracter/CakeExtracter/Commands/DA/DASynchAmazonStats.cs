using Amazon;
using CakeExtracter.Bootstrappers;
using CakeExtracter.Common;
using CakeExtracter.Common.JobExecutionManagement;
using CakeExtracter.Etl.TradingDesk.Extracters.AmazonExtractors.AmazonApiExtractors;
using CakeExtracter.Etl.TradingDesk.LoadersDA.AmazonLoaders;
using CakeExtracter.Etl.TradingDesk.LoadersDA.AmazonLoaders.AnalyticTablesSync;
using CakeExtracter.Helpers;
using CakeExtracter.Logging.TimeWatchers;
using CakeExtracter.Logging.TimeWatchers.Amazon;
using DirectAgents.Domain.Concrete;
using DirectAgents.Domain.Entities.CPProg;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class DASynchAmazonStats : ConsoleCommand
    {
        private const int DefaultDaysAgo = 41;

        public static int RunStatic(int? accountId = null, DateTime? startDate = null, DateTime? endDate = null, string statsType = null, bool fromDatabase = false)
        {
            AutoMapperBootstrapper.CheckRunSetup();
            var cmd = new DASynchAmazonStats
            {
                AccountId = accountId,
                StartDate = startDate,
                EndDate = endDate,
                StatsType = statsType,
                FromDatabase = fromDatabase
            };
            return cmd.Run();
        }

        public int? AccountId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? DaysAgoToStart { get; set; }
        public string StatsType { get; set; }
        public bool DisabledOnly { get; set; }
        public bool FromDatabase { get; set; }
        public bool ClearBeforeLoad { get; set; }
        public bool KeepAmazonReports { get; set; }

        public override void ResetProperties()
        {
            AccountId = null;
            StartDate = null;
            EndDate = null;
            DaysAgoToStart = null;
            StatsType = null;
            DisabledOnly = false;
            FromDatabase = false;
            ClearBeforeLoad = false;
            KeepAmazonReports = false;
        }

        public DASynchAmazonStats()
        {
            IsCommand("daSynchAmazonStats", "Synch Amazon Stats");
            HasOption<int>("a|accountId=", "Account Id (default = all)", c => AccountId = c);
            HasOption("s|startDate=", "Start Date (default is 'daysAgo')", c => StartDate = DateTime.Parse(c));
            HasOption("e|endDate=", "End Date (default is yesterday)", c => EndDate = DateTime.Parse(c));
            HasOption<int>("d|daysAgo=", $"Days Ago to start, if startDate not specified (default = {DefaultDaysAgo})", c => DaysAgoToStart = c);
            HasOption<string>("t|statsType=", "Stats Type (default: all)", c => StatsType = c);
            HasOption<bool>("x|disabledOnly=", "Include only disabled accounts (default = false)", c => DisabledOnly = c);
            HasOption<bool>("z|fromDatabase=", "Retrieve from database instead of API (where implemented)", c => FromDatabase = c);
            HasOption<bool>("c|clearBeforeLoad=", "Remove data from the database on a specific date before loading new extracted data (default = false)", c => ClearBeforeLoad = c);
            HasOption<bool>("k|keepAmazonReports=", "Store received Amazon reports in a separate folder (default = false)", c => KeepAmazonReports = c);
        }

        public override int Execute(string[] remainingArguments)
        {
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
                    if (statsType.Daily && !FromDatabase)
                    {
                        DoETL_Daily(dateRange, account, amazonUtility);
                    }
                    if (statsType.Strategy && !statsType.All)
                    {
                        DoETL_Strategy(dateRange, account, amazonUtility);
                    }
                    if (statsType.AdSet && !statsType.All)
                    {
                        DoETL_AdSet(dateRange, account, amazonUtility);
                    }
                    if (statsType.Creative)
                    {
                        DoETL_Creative(dateRange, account, amazonUtility);
                    }
                    if (statsType.Keyword)
                    {
                        DoETL_Keyword(dateRange, account, amazonUtility);
                    }
                    if (statsType.Daily && FromDatabase)
                    {
                        DoETL_DailyFromKeywordsDatabaseData(dateRange, account); // need to update keywords stats first
                    }
                    if (statsType.SearchTerm && !statsType.All)
                    {
                        DoETL_SearchTerm(dateRange, account, amazonUtility);
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

        private string[] GetTokens()
        {
            // Get tokens, if any, from the database
            return Platform.GetPlatformTokens(Platform.Code_Amazon);
        }

        private void SaveTokens(string[] tokens)
        {
            Platform.SavePlatformTokens(Platform.Code_Amazon, tokens);
        }

        private AmazonUtility CreateUtility(ExtAccount account)
        {
            var amazonUtility = new AmazonUtility(m => Logger.Info(account.Id, m), m => Logger.Warn(account.Id, m));
            amazonUtility.SetWhichAlt(account.ExternalId);
            if (!KeepAmazonReports)
            {
                return amazonUtility;
            }
            amazonUtility.KeepReports = KeepAmazonReports;
            amazonUtility.ReportPrefix = account.Id.ToString();
            return amazonUtility;
        }

        private void DoETL_Daily(DateRange dateRange, ExtAccount account, AmazonUtility amazonUtility)
        {
            AmazonTimeTracker.Instance.ExecuteWithTimeTracking(() =>
            {
                var extractor = new AmazonApiDailySummaryExtractor(amazonUtility, dateRange, account, ClearBeforeLoad, campaignFilter: account.Filter);
                var loader = new AmazonDailySummaryLoader(account.Id);
                CommandHelper.DoEtl(extractor, loader);
            }, account.Id, AmazonJobLevels.account, AmazonJobOperations.total);
        }

        private void DoETL_DailyFromKeywordsDatabaseData(DateRange dateRange, ExtAccount account)
        {
            AmazonTimeTracker.Instance.ExecuteWithTimeTracking(() =>
            {
                var extractor = new AmazonDatabaseKeywordsToDailySummaryExtracter(dateRange, account.Id);
                var loader = new AmazonDailySummaryLoader(account.Id);
                CommandHelper.DoEtl(extractor, loader);
            }, account.Id, AmazonJobLevels.account, AmazonJobOperations.total);
        }

        private void DoETL_Strategy(DateRange dateRange, ExtAccount account, AmazonUtility amazonUtility)
        {
            AmazonTimeTracker.Instance.ExecuteWithTimeTracking(() =>
            {
                var extractor = new AmazonApiCampaignSummaryExtractor(amazonUtility, dateRange, account, ClearBeforeLoad, campaignFilter: account.Filter);
                var loader = new AmazonCampaignSummaryLoader(account.Id);
                CommandHelper.DoEtl(extractor, loader);
            }, account.Id, AmazonJobLevels.strategy, AmazonJobOperations.total);
        }

        private void DoETL_AdSet(DateRange dateRange, ExtAccount account, AmazonUtility amazonUtility)
        {
            AmazonTimeTracker.Instance.ExecuteWithTimeTracking(() =>
            {
                var extractor = new AmazonApiAdSetExtractor(amazonUtility, dateRange, account, ClearBeforeLoad, campaignFilter: account.Filter);
                var loader = new AmazonAdSetSummaryLoader(account.Id);
                CommandHelper.DoEtl(extractor, loader);
            }, account.Id, AmazonJobLevels.adSet, AmazonJobOperations.total);
        }

        private void DoETL_Creative(DateRange dateRange, ExtAccount account, AmazonUtility amazonUtility)
        {
            AmazonTimeTracker.Instance.ExecuteWithTimeTracking(() =>
            {
                var extractor = new AmazonApiAdExtrator(amazonUtility, dateRange, account, ClearBeforeLoad, campaignFilter: account.Filter);
                var loader = new AmazonAdSummaryLoader(account.Id);
                CommandHelper.DoEtl(extractor, loader);
                SynchAsinAnalyticTables(account.Id);
            }, account.Id, AmazonJobLevels.creative, AmazonJobOperations.total);
        }

        private void DoETL_Keyword(DateRange dateRange, ExtAccount account, AmazonUtility amazonUtility)
        {
            AmazonTimeTracker.Instance.ExecuteWithTimeTracking(() =>
            {
                var extractor = new AmazonApiKeywordExtractor(amazonUtility, dateRange, account, ClearBeforeLoad, campaignFilter: account.Filter);
                var loader = new AmazonKeywordSummaryLoader(account.Id);
                CommandHelper.DoEtl(extractor, loader);
            }, account.Id, AmazonJobLevels.keyword, AmazonJobOperations.total);
        }

        private void DoETL_SearchTerm(DateRange dateRange, ExtAccount account, AmazonUtility amazonUtility)
        {
            AmazonTimeTracker.Instance.ExecuteWithTimeTracking(() =>
            {
                var extractor = new AmazonApiSearchTermExtractor(amazonUtility, dateRange, account, ClearBeforeLoad, campaignFilter: account.Filter);
                var loader = new AmazonSearchTermSummaryLoader(account.Id);
                CommandHelper.DoEtl(extractor, loader);
            }, account.Id, AmazonJobLevels.searchTerm, AmazonJobOperations.total);
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

        private void SynchAsinAnalyticTables(int accountId)
        {
            try
            {
                CommandExecutionContext.Current.JobDataWriter.SetStateInHistory("Synch analytic table." ,accountId);
                AmazonTimeTracker.Instance.ExecuteWithTimeTracking(() =>
                {
                    var amazonAmsSyncher = new AmazonAmsAnalyticSyncher();
                    amazonAmsSyncher.SyncAsinLevelForAccount(accountId);
                }, accountId, AmazonJobLevels.creative, AmazonJobOperations.syncToAnalyticTables);
            }
            catch (Exception ex)
            {
                Logger.Error(new Exception("Error occured while synching asin analytic table", ex));
            }
        }
    }
}
