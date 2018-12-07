using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using CakeExtracter.Bootstrappers;
using CakeExtracter.Common;
using CakeExtracter.Etl.TradingDesk.Extracters;
using CakeExtracter.Etl.TradingDesk.Extracters.AmazonExtractors.AmazonApiExtractors;
using CakeExtracter.Etl.TradingDesk.LoadersDA;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class DASynchAmazonStats : ConsoleCommand
    {
        public static int RunStatic(int? accountId = null, DateTime? startDate = null, DateTime? endDate = null, string statsType = null, bool fromDatabase = false)
        {
            AutoMapperBootstrapper.CheckRunSetup();
            var cmd = new DASynchAmazonStats
            {
                AccountId = accountId,
                //CampaignId = campaignId,
                StartDate = startDate,
                EndDate = endDate,
                StatsType = statsType,
                FromDatabase = fromDatabase
            };
            return cmd.Run();
        }

        public int? AccountId { get; set; }
        //public int? CampaignId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? DaysAgoToStart { get; set; }
        public string StatsType { get; set; }
        public bool DisabledOnly { get; set; }
        public bool FromDatabase { get; set; }

        public override void ResetProperties()
        {
            AccountId = null;
            //CampaignId = null;
            StartDate = null;
            EndDate = null;
            DaysAgoToStart = null;
            StatsType = null;
            DisabledOnly = false;
            FromDatabase = false;
        }

        public DASynchAmazonStats()
        {
            IsCommand("daSynchAmazonStats", "Synch Amazon Stats");
            HasOption<int>("a|accountId=", "Account Id (default = all)", c => AccountId = c);
            //HasOption<int>("c|campaignId=", "Campaign Id ", c => CampaignId = c);
            HasOption("s|startDate=", "Start Date (default is 'daysAgo')", c => StartDate = DateTime.Parse(c));
            HasOption("e|endDate=", "End Date (default is yesterday)", c => EndDate = DateTime.Parse(c));
            HasOption<int>("d|daysAgo=", "Days Ago to start, if startDate not specified (default = 41)", c => DaysAgoToStart = c);
            HasOption<string>("t|statsType=", "Stats Type (default: all)", c => StatsType = c);
            HasOption<bool>("x|disabledOnly=", "Include only disabled accounts (default = false)", c => DisabledOnly = c);
            HasOption<bool>("z|fromDatabase=", "Retrieve from database instead of API (where implemented)", c => FromDatabase = c);
        }

        public override int Execute(string[] remainingArguments)
        {
            if (!DaysAgoToStart.HasValue)
                DaysAgoToStart = 41; // used if StartDate==null
            var today = DateTime.Today;
            var yesterday = today.AddDays(-1);
            var dateRange = new DateRange(StartDate ?? today.AddDays(-DaysAgoToStart.Value), EndDate ?? yesterday);
            Logger.Info("Amazon ETL. DateRange {0}.", dateRange);

            var statsType = new StatsTypeAgg(this.StatsType);

            var accounts = GetAccounts();
            AmazonUtility.TokenSets = GetTokens();

            Parallel.ForEach(accounts, account =>
            {
                Logger.Info(account.Id, "Commencing ETL for Amazon account ({0}) {1}", account.Id, account.Name);

                var amazonUtility = new AmazonUtility(m => Logger.Info(account.Id, m), m => Logger.Warn(account.Id, m));
                amazonUtility.SetWhichAlt(account.ExternalId);

                try
                {
                    if (statsType.Daily && !FromDatabase)
                    {
                        DoETL_Daily(dateRange, account, amazonUtility);
                    }
                    if (statsType.Strategy)
                        DoETL_Strategy(dateRange, account, amazonUtility);
                    if (statsType.Daily && FromDatabase)
                        DoETL_DailyFromStrategyInDatabase(dateRange, account); // need to update strat stats first

                    if (statsType.AdSet)
                        DoETL_AdSet(dateRange, account, amazonUtility);
                    if (statsType.Creative)
                        DoETL_Creative(dateRange, account, amazonUtility);
                    if (statsType.Keyword)
                        DoETL_Keyword(dateRange, account, amazonUtility);
                    if (statsType.SearchTerm)
                        DoETL_SearchTerm(dateRange, account, amazonUtility);
                }
                catch (Exception ex)
                {
                    Logger.Error(account.Id, ex);
                }
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

        private void DoETL_Daily(DateRange dateRange, ExtAccount account, AmazonUtility amazonUtility)
        {
            var extracter = new AmazonApiDailySummaryExtracter(amazonUtility, dateRange, account, campaignFilter: account.Filter);
            var loader = new AmazonDailySummaryLoader(account.Id);
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
        }

        private void DoETL_DailyFromStrategyInDatabase(DateRange dateRange, ExtAccount account)
        {
            var extracter = new DatabaseStrategyToDailySummaryExtracter(dateRange, account.Id);
            var loader = new TDDailySummaryLoader(account.Id);
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
        }

        private void DoETL_Strategy(DateRange dateRange, ExtAccount account, AmazonUtility amazonUtility)
        {
            var extracter = new AmazonApiCampaignSummaryExtracter(amazonUtility, dateRange, account, campaignFilter: account.Filter);
            var loader = new AmazonCampaignSummaryLoader(account.Id);
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
        }

        private void DoETL_AdSet(DateRange dateRange, ExtAccount account, AmazonUtility amazonUtility)
        {
            var extracter = new AmazonApiAdSetExtracter(amazonUtility, dateRange, account, campaignFilter: account.Filter);
            var loader = new AmazonAdSetSummaryLoader(account.Id);
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
        }

        private void DoETL_Creative(DateRange dateRange, ExtAccount account, AmazonUtility amazonUtility)
        {
            var extracter = new AmazonApiAdExtrater(amazonUtility, dateRange, account, campaignFilter: account.Filter);
            var loader = new AmazonAdSummaryLoader(account.Id);
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
        }

        private void DoETL_Keyword(DateRange dateRange, ExtAccount account, AmazonUtility amazonUtility)
        {
            var extracter = new AmazonApiKeywordExtracter(amazonUtility, dateRange, account, campaignFilter: account.Filter);
            var loader = new AmazonKeywordSummaryLoader(account.Id);
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
        }

        private void DoETL_SearchTerm(DateRange dateRange, ExtAccount account, AmazonUtility amazonUtility)
        {
            var extracter = new AmazonApiSearchTermExtracter(amazonUtility, dateRange, account, campaignFilter: account.Filter);
            var loader = new AmazonSearchTermSummaryLoader(account.Id);
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
        }

        private IEnumerable<ExtAccount> GetAccounts()
        {
            using (var db = new ClientPortalProgContext())
            {
                //var accounts = db.ExtAccounts.Include("Platform.PlatColMapping").Where(a => a.Platform.Code == Platform.Code_Amazon);
                var accounts = db.ExtAccounts.Where(a => a.Platform.Code == Platform.Code_Amazon);
                if (AccountId.HasValue)
                    accounts = accounts.Where(a => a.Id == AccountId.Value);
                else if (!DisabledOnly)
                    accounts = accounts.Where(a => !a.Disabled);

                if (DisabledOnly)
                    accounts = accounts.Where(a => a.Disabled);

                return accounts.ToList().Where(a => !string.IsNullOrWhiteSpace(a.ExternalId));
            }
        }
    }
}
