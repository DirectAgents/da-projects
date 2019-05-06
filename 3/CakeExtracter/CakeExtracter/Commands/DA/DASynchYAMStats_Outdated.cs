using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using CakeExtracter.Bootstrappers;
using CakeExtracter.Common;
using CakeExtracter.Etl.TradingDesk.LoadersDA;
using CakeExtracter.Etl.YAM.Outdated;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using Yahoo;

namespace CakeExtracter.Commands.DA
{
    [Export(typeof(ConsoleCommand))]
    [Obsolete]
    public class DASynchYAMStats_Outdated : ConsoleCommand
    {
        private const int DefaultDaysAgo = 41;

        [Obsolete]
        public static int RunStatic(int? accountId = null, DateTime? startDate = null, DateTime? endDate = null, string statsType = null)
        {
            AutoMapperBootstrapper.CheckRunSetup();
            var cmd = new DASynchYAMStats
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

        private string[] extIds_UsePixelParm;
        private List<Action> etlList;

        [Obsolete]
        public override void ResetProperties()
        {
            AccountId = null;
            StartDate = null;
            EndDate = null;
            DaysAgoToStart = null;
            StatsType = null;
            DisabledOnly = false;
        }

        [Obsolete]
        public DASynchYAMStats_Outdated()
        {
            IsCommand("daSynchYAMStats_Outdated", "synch YAM Stats");
            HasOption<int>("a|accountId=", "Account Id (default = all)", c => AccountId = c);
            HasOption("s|startDate=", "Start Date (default is 'daysAgo')", c => StartDate = DateTime.Parse(c));
            HasOption("e|endDate=", "End Date (default is yesterday)", c => EndDate = DateTime.Parse(c));
            HasOption<int>("d|daysAgo=", $"Days Ago to start, if startDate not specified (default = {DefaultDaysAgo})", c => DaysAgoToStart = c);
            HasOption<string>("t|statsType=", "Stats Type (default: all)", c => StatsType = c);
            HasOption<bool>("x|disabledOnly=", "Include only disabled accounts (default = false)", c => DisabledOnly = c);
        }

        //TODO: if synching all accounts, can we make one API call to get everything?

        [Obsolete]
        public override int Execute(string[] remainingArguments)
        {
            var dateRange = CommandHelper.GetDateRange(StartDate, EndDate, DaysAgoToStart, DefaultDaysAgo);
            Logger.Info("YAM ETL. DateRange {0}.", dateRange);

            var statsType = new StatsTypeAgg(StatsType);
            var ppIds = ConfigurationManager.AppSettings["YAMids_UsePixelParm"];
            extIds_UsePixelParm = ppIds != null ? ppIds.Split(',') : new string[] { };

            var accounts = GetAccounts();
            YamUtility.TokenSets = GetTokens();

            etlList = new List<Action>();
            foreach (var account in accounts)
            {
                Logger.Info(account.Id, "Commencing ETL for YAM account ({0}) {1}", account.Id, account.Name);
                var yamUtility = new YamUtility(m => Logger.Info(account.Id, m), m => Logger.Warn(m),
                    exc => Logger.Error(account.Id, exc));
                yamUtility.SetWhichAlt(account.ExternalId);

                AddEnabledEtl(statsType.Daily, account, () => DoETL_Daily(dateRange, account, yamUtility));
                AddEnabledEtl(statsType.Strategy, account, () => DoETL_Strategy(dateRange, account, yamUtility));
                AddEnabledEtl(statsType.AdSet, account, () => DoETL_AdSet(dateRange, account, yamUtility));
                AddEnabledEtl(statsType.Keyword, account, () => DoETL_Keyword(dateRange, account, yamUtility));
                AddEnabledEtl(statsType.SearchTerm, account, () => DoETL_SearchTerm(dateRange, account, yamUtility));
                //don't include when getting "all" statstypes
                AddEnabledEtl(statsType.Creative && !statsType.All, account, () => DoETL_Creative(dateRange, account, yamUtility));
            }
            Parallel.Invoke(etlList.ToArray());

            SaveTokens();
            return 0;
        }

        private void AddEnabledEtl(bool etlEnabled, ExtAccount account, Action etlAction)
        {
            if (!etlEnabled) return;
            etlList.Add(() =>
            {
                try
                {
                    etlAction();
                }
                catch (Exception ex)
                {
                    Logger.Error(account.Id, ex);
                }
            });
        }

        private string[] GetTokens()
        {
            // Get tokens, if any, from the database
            return Platform.GetPlatformTokens(Platform.Code_YAM);
        }

        private void SaveTokens()
        {
            Platform.SavePlatformTokens(Platform.Code_YAM, YamUtility.TokenSets);
        }

        private void DoETL_Daily(DateRange dateRange, ExtAccount account, YamUtility yamUtility)
        {
            var extractor = new YAMDailySummaryExtracter(yamUtility, dateRange, account);
            var loader = new TDDailySummaryLoader(account.Id);
            CommandHelper.DoEtl(extractor, loader);

            if (!extIds_UsePixelParm.Contains(account.ExternalId))
            {
                return;
            }

            // Get ConVals using the pixel parameter...
            var e = new YAMDailyConValExtracter(yamUtility, dateRange, account);
            var l = new TDDailyConValLoader(account.Id);
            CommandHelper.DoEtl(e, l);
        }

        private void DoETL_Strategy(DateRange dateRange, ExtAccount account, YamUtility yamUtility)
        {
            var extractor = new YAMStrategySummaryExtracter(yamUtility, dateRange, account);
            var loader = new TDStrategySummaryLoader(account.Id);
            CommandHelper.DoEtl(extractor, loader);

            if (!extIds_UsePixelParm.Contains(account.ExternalId))
            {
                return;
            }

            // Get ConVals using the pixel parameter...
            var stratNames = GetExistingStrategyNames(dateRange, account.Id);
            var e = new YAMStrategyConValExtracter(yamUtility, dateRange, account, existingStrategyNames: stratNames);
            var l = new TDStrategyConValLoader(account.Id);
            CommandHelper.DoEtl(e, l);
        }

        private void DoETL_AdSet(DateRange dateRange, ExtAccount account, YamUtility yamUtility)
        {
            var extractor = new YAMAdSetSummaryExtracter(yamUtility, dateRange, account);
            var loader = new TDAdSetSummaryLoader(account.Id);
            CommandHelper.DoEtl(extractor, loader);
        }

        private void DoETL_Creative(DateRange dateRange, ExtAccount account, YamUtility yamUtility)
        {
            var extractor = new YAMTDadSummaryExtracter(yamUtility, dateRange, account);
            var loader = new TDadSummaryLoader(account.Id);
            CommandHelper.DoEtl(extractor, loader);
        }

        private void DoETL_Keyword(DateRange dateRange, ExtAccount account, YamUtility yamUtility)
        {
            var extractor = new YAMKeywordSummaryExtracter(yamUtility, dateRange, account);
            var loader = new KeywordSummaryLoader(account.Id);
            CommandHelper.DoEtl(extractor, loader);
        }

        private void DoETL_SearchTerm(DateRange dateRange, ExtAccount account, YamUtility yamUtility)
        {
            var extractor = new YAMSearchTermSummaryExtracter(yamUtility, dateRange, account);
            var loader = new SearchTermSummaryLoader(account.Id);
            CommandHelper.DoEtl(extractor, loader);
        }

        //Get the names of strategies that have stats for the specified dateRange and account (and whose stats' ConVals are not zero)
        private string[] GetExistingStrategyNames(DateRange dateRange, int accountId)
        {
            using (var db = new ClientPortalProgContext())
            {
                var stratSums = db.StrategySummaries.Where(s => s.Strategy.AccountId == accountId && s.Date >= dateRange.FromDate && s.Date <= dateRange.ToDate
                                                           && (s.PostClickRev != 0 || s.PostViewRev != 0));
                var strategies = stratSums.Select(s => s.Strategy).Distinct();
                var stratNames = strategies.Select(s => s.Name).Distinct().ToArray();
                return stratNames;
            }
        }

        private IEnumerable<ExtAccount> GetAccounts()
        {
            using (var db = new ClientPortalProgContext())
            {
                var accounts = db.ExtAccounts.Include("Platform.PlatColMapping").Where(a => a.Platform.Code == Platform.Code_YAM);
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
