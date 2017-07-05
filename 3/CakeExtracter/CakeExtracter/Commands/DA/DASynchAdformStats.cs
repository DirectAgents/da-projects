using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Adform;
using CakeExtracter.Bootstrappers;
using CakeExtracter.Common;
using CakeExtracter.Etl.TradingDesk.Extracters;
using CakeExtracter.Etl.TradingDesk.LoadersDA;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class DASynchAdformStats : ConsoleCommand
    {
        public static int RunStatic(int? accountId = null, DateTime? startDate = null, DateTime? endDate = null, string statsType = null)
        {
            AutoMapperBootstrapper.CheckRunSetup();
            var cmd = new DASynchAdformStats
            {
                AccountId = accountId,
                StartDate = startDate,
                EndDate = endDate,
                StatsType = statsType
            };
            return cmd.Run();
        }

        public int? AccountId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? DaysAgoToStart { get; set; }
        public string StatsType { get; set; }

        private AdformUtility adformUtility { get; set; }

        public override void ResetProperties()
        {
            AccountId = null;
            StartDate = null;
            EndDate = null;
            DaysAgoToStart = null;
            StatsType = null;
        }

        public DASynchAdformStats()
        {
            IsCommand("daSynchAdformStats", "synch Adform Stats");
            HasOption<int>("a|accountId=", "Account Id (default = all)", c => AccountId = c);
            HasOption("s|startDate=", "Start Date (default is 'daysAgo')", c => StartDate = DateTime.Parse(c));
            HasOption("e|endDate=", "End Date (default is yesterday)", c => EndDate = DateTime.Parse(c));
            HasOption<int>("d|daysAgo=", "Days Ago to start, if startDate not specified (default = 31)", c => DaysAgoToStart = c);
            HasOption<string>("t|statsType=", "Stats Type (default: all)", c => StatsType = c);
        }

        //TODO: if synching all accounts, can we make one API call to get everything?

        public override int Execute(string[] remainingArguments)
        {
            if (!DaysAgoToStart.HasValue)
                DaysAgoToStart = 31; // used if StartDate==null
            var today = DateTime.Today;
            var yesterday = today.AddDays(-1);
            var dateRange = new DateRange(StartDate ?? today.AddDays(-DaysAgoToStart.Value), EndDate ?? yesterday);
            Logger.Info("Adform ETL. DateRange {0}.", dateRange);

            var statsType = new StatsTypeAgg(this.StatsType);
            SetupAdformUtility();

            var accounts = GetAccounts();
            foreach (var account in accounts)
            {
                Logger.Info("Commencing ETL for Adform account ({0}) {1}", account.Id, account.Name);
                if (statsType.Daily)
                    DoETL_Daily(dateRange, account);
                if (statsType.Strategy)
                    DoETL_Strategy(dateRange, account);
                if (statsType.Creative)
                    DoETL_Creative(dateRange, account);
            }
            SaveTokens();
            return 0;
        }

        private void SetupAdformUtility()
        {
            this.adformUtility = new AdformUtility(m => Logger.Info(m), m => Logger.Warn(m));
            GetTokens();
        }
        private void GetTokens()
        {
            // Get tokens, if any, from the database
            string[] tokens = Platform.GetPlatformTokens(Platform.Code_Adform);
            if (tokens.Length > 0)
            {
                adformUtility.AccessToken = tokens[0];
            }
        }
        private void SaveTokens()
        {
            Platform.SavePlatformTokens(Platform.Code_Adform, adformUtility.AccessToken);
        }
        // ---

        private void DoETL_Daily(DateRange dateRange, ExtAccount account)
        {
            var extracter = new AdformDailySummaryExtracter(adformUtility, dateRange, account.ExternalId);
            var loader = new TDDailySummaryLoader(account.Id);
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
        }
        private void DoETL_Strategy(DateRange dateRange, ExtAccount account)
        {
            var extracter = new AdformStrategySummaryExtracter(adformUtility, dateRange, account.ExternalId);
            var loader = new TDStrategySummaryLoader(account.Id);
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
        }
        private void DoETL_Creative(DateRange dateRange, ExtAccount account)
        {
            var extracter = new AdformTDadSummaryExtracter(adformUtility, dateRange, account.ExternalId);
            var loader = new TDadSummaryLoader(account.Id);
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
        }

        private IEnumerable<ExtAccount> GetAccounts()
        {
            using (var db = new ClientPortalProgContext())
            {
                //var accounts = db.ExtAccounts.Include("Platform.PlatColMapping").Where(a => a.Platform.Code == Platform.Code_Adform);
                var accounts = db.ExtAccounts.Where(a => a.Platform.Code == Platform.Code_Adform);
                if (AccountId.HasValue)
                    accounts = accounts.Where(a => a.Id == AccountId.Value);

                return accounts.ToList().Where(a => !string.IsNullOrWhiteSpace(a.ExternalId));
            }
        }

    }
}
