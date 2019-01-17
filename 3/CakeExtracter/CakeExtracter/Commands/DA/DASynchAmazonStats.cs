﻿using Amazon;
using CakeExtracter.Bootstrappers;
using CakeExtracter.Common;
using CakeExtracter.Etl.TradingDesk.Extracters;
using CakeExtracter.Etl.TradingDesk.Extracters.AmazonExtractors.AmazonApiExtractors;
using CakeExtracter.Etl.TradingDesk.LoadersDA;
using CakeExtracter.Etl.TradingDesk.LoadersDA.AmazonLoaders;
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
            HasOption<int>("d|daysAgo=", "Days Ago to start, if startDate not specified (default = 41)", c => DaysAgoToStart = c);
            HasOption<string>("t|statsType=", "Stats Type (default: all)", c => StatsType = c);
            HasOption<bool>("x|disabledOnly=", "Include only disabled accounts (default = false)", c => DisabledOnly = c);
            HasOption<bool>("z|fromDatabase=", "Retrieve from database instead of API (where implemented)", c => FromDatabase = c);
            HasOption<bool>("c|clearBeforeLoad=", "Remove data from the database on a specific date before loading new extracted data (default = false)", c => ClearBeforeLoad = c);
            HasOption<bool>("k|keepAmazonReports=", "Store received Amazon reports in a separate folder (default = false)", c => KeepAmazonReports = c);
        }

        public override int Execute(string[] remainingArguments)
        {
            if (!DaysAgoToStart.HasValue)
            {
                DaysAgoToStart = 41; // used if StartDate==null
            }

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
                var amazonUtility = CreateUtility(account);

                try
                {
                    if (statsType.Daily && !FromDatabase)
                    {
                        DoETL_Daily(dateRange, account, amazonUtility);
                    }

                    if (statsType.Strategy)
                    {
                        DoETL_Strategy(dateRange, account, amazonUtility);
                    }

                    if (statsType.Daily && FromDatabase)
                    {
                        DoETL_DailyFromStrategyInDatabase(dateRange, account); // need to update strat stats first
                    }

                    if (statsType.AdSet)
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

                    if (statsType.SearchTerm && !statsType.All)
                    {
                        DoETL_SearchTerm(dateRange, account, amazonUtility);
                    }
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
            var extracter = new AmazonApiDailySummaryExtractor(amazonUtility, dateRange, account, ClearBeforeLoad, campaignFilter: account.Filter);
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
            var extracter = new AmazonApiCampaignSummaryExtractor(amazonUtility, dateRange, account, ClearBeforeLoad, campaignFilter: account.Filter);
            var loader = new AmazonCampaignSummaryLoader(account.Id);
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
        }

        private void DoETL_AdSet(DateRange dateRange, ExtAccount account, AmazonUtility amazonUtility)
        {
            var extracter = new AmazonApiAdSetExtractor(amazonUtility, dateRange, account, ClearBeforeLoad, campaignFilter: account.Filter);
            var loader = new AmazonAdSetSummaryLoader(account.Id);
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
        }

        private void DoETL_Creative(DateRange dateRange, ExtAccount account, AmazonUtility amazonUtility)
        {
            var extracter = new AmazonApiAdExtrator(amazonUtility, dateRange, account, ClearBeforeLoad, campaignFilter: account.Filter);
            var loader = new AmazonAdSummaryLoader(account.Id);
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
        }

        private void DoETL_Keyword(DateRange dateRange, ExtAccount account, AmazonUtility amazonUtility)
        {
            var extracter = new AmazonApiKeywordExtractor(amazonUtility, dateRange, account, ClearBeforeLoad, campaignFilter: account.Filter);
            var loader = new AmazonKeywordSummaryLoader(account.Id);
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
        }

        private void DoETL_SearchTerm(DateRange dateRange, ExtAccount account, AmazonUtility amazonUtility)
        {
            var extracter = new AmazonApiSearchTermExtractor(amazonUtility, dateRange, account, ClearBeforeLoad, campaignFilter: account.Filter);
            var loader = new AmazonSearchTermSummaryLoader(account.Id);
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
        }

        private IEnumerable<ExtAccount> GetAccounts()
        {
            var repository = new PlatformAccountRepository();
            if (!AccountId.HasValue)
            {
                var accounts = repository.GetAccounts(Platform.Code_Amazon, DisabledOnly);
                return accounts;
            }

            var account = repository.GetAccount(AccountId.Value);
            return new[] { account };
        }
    }
}
