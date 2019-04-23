using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using CakeExtracter.Bootstrappers;
using CakeExtracter.Common;
using CakeExtracter.Etl.TradingDesk.LoadersDA;
using CakeExtracter.Etl.YAM.Extractors.ApiExtractors;
using CakeExtracter.Etl.YAM.Extractors.ConValExtractors;
using CakeExtracter.Etl.YAM.Helper;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using Yahoo;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class DASynchYAMStats : ConsoleCommand
    {
        private const int DefaultDaysAgo = 41;

        private List<string> extIdsUsePixelParams;
        private List<Action> etlList;

        public int? AccountId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? DaysAgoToStart { get; set; }
        public string StatsType { get; set; }
        public bool DisabledOnly { get; set; }

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

        public DASynchYAMStats()
        {
            IsCommand("daSynchYAMStats", "synch YAM Stats");
            HasOption<int>("a|accountId=", "Account Id (default = all)", c => AccountId = c);
            HasOption("s|startDate=", "Start Date (default is 'daysAgo')", c => StartDate = DateTime.Parse(c));
            HasOption("e|endDate=", "End Date (default is yesterday)", c => EndDate = DateTime.Parse(c));
            HasOption<int>("d|daysAgo=", $"Days Ago to start, if startDate not specified (default = {DefaultDaysAgo})", c => DaysAgoToStart = c);
            HasOption<string>("t|statsType=", "Stats Type (default: all)", c => StatsType = c);
            HasOption<bool>("x|disabledOnly=", "Include only disabled accounts (default = false)", c => DisabledOnly = c);
        }

        public override void ResetProperties()
        {
            AccountId = null;
            StartDate = null;
            EndDate = null;
            DaysAgoToStart = null;
            StatsType = null;
            DisabledOnly = false;
        }

        //TODO: if synching all accounts, can we make one API call to get everything?

        public override int Execute(string[] remainingArguments)
        {
            var dateRange = CommandHelper.GetDateRange(StartDate, EndDate, DaysAgoToStart, DefaultDaysAgo);
            Logger.Info("YAM ETL. DateRange {0}.", dateRange);

            var statsType = new YamStatsType(StatsType);
            extIdsUsePixelParams = ConfigurationHelper.ExtractEnumerableFromConfig("YAMids_UsePixelParm");
            var accounts = GetAccounts();
            DoEtlsParallel(dateRange, statsType, accounts);
            return 0;
        }

        private void DoEtlsParallel(DateRange dateRange, YamStatsType statsType, IEnumerable<ExtAccount> accounts)
        {
            YAMUtility.TokenSets = GetTokens();
            etlList = new List<Action>();
            foreach (var account in accounts)
            {
                DoEtls(dateRange, statsType, account);
            }

            Parallel.Invoke(etlList.ToArray());
            SaveTokens();
        }

        private string[] GetTokens()
        {
            // Get tokens, if any, from the database
            return Platform.GetPlatformTokens(Platform.Code_YAM);
        }

        private void SaveTokens()
        {
            Platform.SavePlatformTokens(Platform.Code_YAM, YAMUtility.TokenSets);
        }

        private void DoEtls(DateRange dateRange, YamStatsType statsType, ExtAccount account)
        {
            Logger.Info(account.Id, "Commencing ETL for YAM account ({0}) {1}", account.Id, account.Name);
            var yamUtility = CreateUtility(account);
            AddEnabledEtl(statsType.Daily, account, () => DoETL_Daily(dateRange, account, yamUtility));
            AddEnabledEtl(statsType.Campaign, account, () => DoETL_AdSet(dateRange, account, yamUtility));
            AddEnabledEtl(statsType.Line, account, () => DoETL_Strategy(dateRange, account, yamUtility));
            AddEnabledEtl(statsType.Creative, account, () => DoETL_Keyword(dateRange, account, yamUtility));
            AddEnabledEtl(statsType.Ad, account, () => DoETL_Creative(dateRange, account, yamUtility));
            AddEnabledEtl(statsType.Pixel, account, () => DoETL_SearchTerm(dateRange, account, yamUtility));
        }

        private YAMUtility CreateUtility(ExtAccount account)
        {
            var yamUtility = new YAMUtility(m => Logger.Info(account.Id, m), m => Logger.Error(account.Id, new Exception(m)));
            yamUtility.SetWhichAlt(account.ExternalId);
            return yamUtility;
        }

        private void AddEnabledEtl(bool etlEnabled, ExtAccount account, Action etlAction)
        {
            if (!etlEnabled)
            {
                return;
            }

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

        private void DoETL_Daily(DateRange dateRange, ExtAccount account, YAMUtility yamUtility)
        {
            var extractor = new YamDailySummaryExtractor(yamUtility, dateRange, account);
            var loader = new TDDailySummaryLoader(account.Id);
            CommandHelper.DoEtl(extractor, loader);

            if (!extIdsUsePixelParams.Contains(account.ExternalId))
            {
                return;
            } 
            
            // Get ConVals using the pixel parameter...
            var e = new YamDailyConValExtractor(yamUtility, dateRange, account);
            var l = new TDDailyConValLoader(account.Id);
            CommandHelper.DoEtl(e, l);
        }

        private void DoETL_Strategy(DateRange dateRange, ExtAccount account, YAMUtility yamUtility)
        {
            var extractor = new YamLineSummaryExtractor(yamUtility, dateRange, account);
            var loader = new TDStrategySummaryLoader(account.Id);
            CommandHelper.DoEtl(extractor, loader);

            if (!extIdsUsePixelParams.Contains(account.ExternalId))
            {
                return;
            } 
            
            // Get ConVals using the pixel parameter...
            var stratNames = GetExistingStrategyNames(dateRange, account.Id);
            var e = new YamStrategyConValExtractor(yamUtility, dateRange, account, existingStrategyNames: stratNames);
            var l = new TDStrategyConValLoader(account.Id);
            CommandHelper.DoEtl(e, l);
        }

        private void DoETL_AdSet(DateRange dateRange, ExtAccount account, YAMUtility yamUtility)
        {
            var extractor = new YamCampaignSummaryExtractor(yamUtility, dateRange, account);
            var loader = new TDAdSetSummaryLoader(account.Id);
            CommandHelper.DoEtl(extractor, loader);
        }

        private void DoETL_Creative(DateRange dateRange, ExtAccount account, YAMUtility yamUtility)
        {
            var extractor = new YamAdSummaryExtractor(yamUtility, dateRange, account);
            var loader = new TDadSummaryLoader(account.Id);
            CommandHelper.DoEtl(extractor, loader);
        }

        private void DoETL_Keyword(DateRange dateRange, ExtAccount account, YAMUtility yamUtility)
        {
            var extractor = new YamCreativeSummaryExtractor(yamUtility, dateRange, account);
            var loader = new KeywordSummaryLoader(account.Id);
            CommandHelper.DoEtl(extractor, loader);
        }

        private void DoETL_SearchTerm(DateRange dateRange, ExtAccount account, YAMUtility yamUtility)
        {
            var extractor = new YamBeaconSummaryExtractor(yamUtility, dateRange, account);
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
