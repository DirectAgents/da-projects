using CakeExtracter.Commands;
using CakeExtracter.Common;
using DirectAgents.Domain.Concrete;
using DirectAgents.Domain.Entities.CPProg;
using System;
using System.Collections.Generic;
using CakeExtracter;
using CakeExtracter.Etl.TradingDesk.Extracters;
using CakeExtracter.Etl.TradingDesk.LoadersDA.AmazonLoaders;
using CakeExtractor.SeleniumApplication.Configuration.Pda;
using CakeExtractor.SeleniumApplication.Models.CommonHelperModels;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.AmazonPdaExtractors;
using Platform = DirectAgents.Domain.Entities.CPProg.Platform;

namespace CakeExtractor.SeleniumApplication.Commands
{
    internal class SyncAmazonPdaCommand : BaseAmazonSeleniumCommand
    {
        public int? AccountId { get; set; }
        public string StatsType { get; set; }
        public bool DisabledOnly { get; set; }

        private int executionNumber;
        private JobScheduleModel scheduling;
        private readonly PdaCommandConfigurationManager configurationManager;

        public SyncAmazonPdaCommand()
        {
            configurationManager = new PdaCommandConfigurationManager();
        }

        public override string CommandName
        {
            get
            {
                return "SyncAmazonPdaCommand";
            }
        }

        public override void PrepareCommandEnvironment()
        {
            AmazonPdaExtractor.PrepareExtractor();
        }

        public override void Run()
        {
            
            executionNumber++;
            var statsType = new StatsTypeAgg(StatsType);
            var dateRange = GetDateRange();
            var fromDatabase = configurationManager.GetFromDatabase();

            Logger.Info("Amazon ETL (PDA Campaigns), execution number - {0}. DateRange: {1}.", executionNumber,
                dateRange);

            var accounts = GetAccounts();
            foreach (var account in accounts)
            {
                DoEtls(account, dateRange, statsType, fromDatabase);
            }

            Logger.Info("Amazon ETL (PDA Campaigns) has been finished.");
            //LogScheduledJobStartTime();
        }

        private static void DoEtls(ExtAccount account, DateRange dateRange, StatsTypeAgg statsType, bool fromDatabase)
        {
            Logger.Info(account.Id, "Commencing ETL for Amazon account ({0}) {1}", account.Id, account.Name);
            try
            {
                if (statsType.Daily && !fromDatabase)
                {
                    DoEtlDaily(account, dateRange);
                }

                if (statsType.Strategy)
                {
                    DoEtlStrategy(account, dateRange);
                }

                if (statsType.Daily && fromDatabase)
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
            var extractorThread = extractor.Start();
            var loaderThread = loader.Start(extractor);
            extractorThread.Join();
            loaderThread.Join();
        }

        private static void DoEtlDailyFromStrategyInDatabase(ExtAccount account, DateRange dateRange)
        {
            var extractor = new DatabaseStrategyToDailySummaryExtracter(dateRange, account.Id);
            var loader = new AmazonDailySummaryLoader(account.Id);
            var extractorThread = extractor.Start();
            var loaderThread = loader.Start(extractor);
            extractorThread.Join();
            loaderThread.Join();
        }

        private static void DoEtlStrategy(ExtAccount account, DateRange dateRange)
        {
            var extractor = new AmazonPdaCampaignExtractor(account, dateRange);
            var loader = new AmazonCampaignSummaryLoader(account.Id);
            var extractorThread = extractor.Start();
            var loaderThread = loader.Start(extractor);
            extractorThread.Join();
            loaderThread.Join();
        }

        private DateRange GetDateRange()
        {
            var daysAgo = configurationManager.GetDaysAgo();
            var startDate = configurationManager.GetStartDate(daysAgo);
            var endDate = configurationManager.GetEndDate();
            var dateRange = new DateRange(startDate, endDate);
            return dateRange;
        }

        private void LogScheduledJobStartTime()
        {
            var nextTime = scheduling.StartExtractionTime;
            while (nextTime < DateTime.Now)
            {
                nextTime = nextTime.AddDays(scheduling.DaysInterval);
            }

            Logger.Info("Waiting for the scheduled job to extract PDA statistics: next run time - {0} (UTC)...", nextTime.ToUniversalTime());
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
            return new[] {account};
        }
    }
}