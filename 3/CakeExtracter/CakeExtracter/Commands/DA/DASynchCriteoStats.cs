using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using CakeExtracter.Bootstrappers;
using CakeExtracter.Common;
using CakeExtracter.Etl.TradingDesk.Extracters;
using CakeExtracter.Etl.TradingDesk.LoadersDA;
using CakeExtracter.Helpers;
using Criteo;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class DASynchCriteoStats : ConsoleCommand
    {
        public static int RunStatic(int? accountId = null, DateTime? startDate = null, DateTime? endDate = null, string statsType = null)
        {
            AutoMapperBootstrapper.CheckRunSetup();
            var cmd = new DASynchCriteoStats
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
        public bool DisabledOnly { get; set; }
        public int TimeZoneOffset { get; set; }

        CriteoUtility criteoUtility { get; set; }

        public override void ResetProperties()
        {
            AccountId = null;
            StartDate = null;
            EndDate = null;
            DaysAgoToStart = null;
            StatsType = null;
            DisabledOnly = false;
            TimeZoneOffset = 0;
        }

        public DASynchCriteoStats()
        {
            IsCommand("daSynchCriteoStats", "synch Criteo Stats");
            HasOption<int>("a|accountId=", "Account Id (default = all)", c => AccountId = c);
            HasOption("s|startDate=", "Start Date (default is 'daysAgo')", c => StartDate = DateTime.Parse(c));
            HasOption("e|endDate=", "End Date (default is yesterday)", c => EndDate = DateTime.Parse(c));
            HasOption<int>("d|daysAgo=", "Days Ago to start, if startDate not specified (default = 41 if timeZoneOffset==0, otherwise 6)", c => DaysAgoToStart = c);
            HasOption<string>("t|statsType=", "Stats Type (default: all)", c => StatsType = c);
            HasOption<bool>("x|disabledOnly=", "Include only disabled accounts (default = false)", c => DisabledOnly = c);
            HasOption<int>("z|timeZoneOffset=", "TimeZoneOffset from GMT for stats (default = 0)", c => TimeZoneOffset = c); // -8 for Pacific Time
        }

        public override int Execute(string[] remainingArguments)
        {
            Logger.LogToOneFile = true;

            //if (TimeZoneOffset != 0) ...
            // (See SynchSearchDailySummariesCriteoCommand)

            if (!DaysAgoToStart.HasValue)
            {
                DaysAgoToStart = (TimeZoneOffset == 0 ? 41 : 6); // used if StartDate==null
            }

            var today = DateTime.Today;
            var yesterday = today.AddDays(-1);
            var dateRange = new DateRange(StartDate ?? today.AddDays(-DaysAgoToStart.Value), EndDate ?? yesterday);
            Logger.Info("Criteo ETL. DateRange {0}.", dateRange);

            var statsType = new StatsTypeAgg(this.StatsType);

            foreach (var account in GetAccounts())
            {
                criteoUtility = new CriteoUtility(m => Logger.Info(m), m => Logger.Warn(m));
                criteoUtility.SetCredentials(account.ExternalId);

                if (statsType.Strategy)
                {
                    DoETL_Strategy(dateRange, account);
                }

                if (statsType.Daily)
                {
                    DoETL_Daily(dateRange, account);
                }
            }

            return 0;
        }

        // Note: for daily stats, we just extract what's in the db for strategy stats

        private void DoETL_Daily(DateRange dateRange, ExtAccount account)
        {
            var extractor = new DatabaseStrategyToDailySummaryExtracter(dateRange, account.Id);
            var loader = new TDDailySummaryLoader(account.Id);
            CommandHelper.DoEtl(extractor, loader);
        }

        // Limited to a 7-day window and can go back to about 2 weeks from yesterday
        private void DoETL_Strategy(DateRange dateRange, ExtAccount account)
        {
            //Logger.Info("Criteo ETL - hourly. DateRange {0}.", dateRange); // account...
            var extractor = new CriteoStrategySummaryExtracter(criteoUtility, account.ExternalId, dateRange, TimeZoneOffset);
            var loader = new TDStrategySummaryLoader(account.Id);
            CommandHelper.DoEtl(extractor, loader);
        }

        private IEnumerable<ExtAccount> GetAccounts()
        {
            using (var db = new ClientPortalProgContext())
            {
                var accounts = db.ExtAccounts.Where(a => a.Platform.Code == Platform.Code_Criteo);
                if (AccountId.HasValue)
                {
                    accounts = accounts.Where(a => a.Id == AccountId.Value);
                }
                else
                {
                    if (!DisabledOnly)
                    {
                        accounts = accounts.Where(a => !a.Disabled);
                    }
                }

                if (DisabledOnly)
                {
                    accounts = accounts.Where(a => a.Disabled);
                }

                return accounts.ToList().Where(a => !string.IsNullOrWhiteSpace(a.ExternalId));
            }
        }
    }
}
