using CakeExtracter.Commands;
using CakeExtracter.Common;
using CakeExtracter.Etl.TradingDesk.LoadersDA;
using CakeExtractor.SeleniumApplication.Jobs.ExtractAmazonPda;
using DirectAgents.Domain.Concrete;
using DirectAgents.Domain.Entities.CPProg;
using System;
using System.Collections.Generic;
using System.Threading;
using CakeExtracter;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.AmazonPdaExtractors;
using ConsoleCommand = ManyConsole.ConsoleCommand;
using Platform = DirectAgents.Domain.Entities.CPProg.Platform;

namespace CakeExtractor.SeleniumApplication.Commands
{
    internal class SyncAmazonPdaCommand : ConsoleCommand
    {
        public int? AccountId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? DaysAgoToStart { get; set; }
        public string StatsType { get; set; }
        public bool DisabledOnly { get; set; }

        private int executionNumber;

        public SyncAmazonPdaCommand()
        {
            IsCommand("SyncAmazonPdaCommand", "Synch Amazon PDA Stats");
            HasOption<int>("a|accountId=", "Account Id (default = all)", c => AccountId = c);
            HasOption("s|startDate=", "Start Date (default is from config or 'daysAgo')", c => StartDate = DateTime.Parse(c));
            HasOption("e|endDate=", "End Date (default is from config or yesterday)", c => EndDate = DateTime.Parse(c));
            HasOption<int>("d|daysAgo=", "Days Ago to start, if startDate not specified (default is from config or 41)", c => DaysAgoToStart = c);
            HasOption<string>("t|statsType=", "Stats Type (default: all)", c => StatsType = c);
            HasOption<bool>("x|disabledOnly=", "Include only disabled accounts (default = false)", c => DisabledOnly = c);
        }

        public override int Run(string[] remainingArguments)
        {
            AmazonPdaExtractor.PrepareExtractor();
            ExtractAmazonPdaScheduler.Start(this);
            AlwaysSleep();
            return 0;
        }

        public void Execute()
        {
            executionNumber++;
            var statsType = new StatsTypeAgg(StatsType);
            var dateRange = GetDateRange();
            Logger.Info("Amazon ETL (PDA Campaigns), execution number - {0}. DateRange: {1}.", executionNumber, dateRange);

            var accounts = GetAccounts();
            foreach (var account in accounts)
            {
                DoEtls(account, dateRange, statsType);
            }
        }

        private static void AlwaysSleep()
        {
            while (true)
            {
                Thread.Sleep(int.MaxValue);
            }
        }

        private static void DoEtls(ExtAccount account, DateRange dateRange, StatsTypeAgg statsType)
        {
            Logger.Info(account.Id, "Commencing ETL for Amazon account ({0}) {1}", account.Id, account.Name);

            try
            {
                if (statsType.Strategy)
                {
                    DoEtlStrategy(account, dateRange);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(account.Id, ex);
            }

            Logger.Info(account.Id, "Finished ETL for Amazon account ({0}) {1}", account.Id, account.Name);
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
            var daysAgo = GetDaysAgo();
            var startDate = GetStartDate(daysAgo);
            var endDate = GetEndDate();
            var dateRange = new DateRange(startDate, endDate);
            return dateRange;
        }

        private int GetDaysAgo()
        {
            try
            {
                return DaysAgoToStart ?? Properties.Settings.Default.DaysAgo;
            }
            catch (Exception)
            {
                return 41;
            }
        }

        private DateTime GetStartDate(int daysAgo)
        {
            if (StartDate.HasValue)
            {
                return StartDate.Value;
            }

            return Properties.Settings.Default.StartDate == default(DateTime)
                ? DateTime.Today.AddDays(-daysAgo)
                : Properties.Settings.Default.StartDate;
        }

        private DateTime GetEndDate()
        {
            if (EndDate.HasValue)
            {
                return EndDate.Value;
            }

            return Properties.Settings.Default.EndDate == default(DateTime)
                ? DateTime.Today.AddDays(-1)
                : Properties.Settings.Default.EndDate;
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
