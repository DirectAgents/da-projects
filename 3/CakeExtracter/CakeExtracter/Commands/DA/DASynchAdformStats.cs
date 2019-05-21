using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Adform.Utilities;
using CakeExtracter.Bootstrappers;
using CakeExtracter.Common;
using CakeExtracter.Etl.TradingDesk.Extracters.AdformExtractors;
using CakeExtracter.Etl.TradingDesk.LoadersDA;
using CakeExtracter.Etl.TradingDesk.LoadersDA.AdformLoaders;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class DASynchAdformStats : ConsoleCommand
    {
        private const int DefaultDaysAgo = 41;

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

        public bool DisabledOnly { get; set; }

        private List<Action> etlList;

        public override void ResetProperties()
        {
            AccountId = null;
            StartDate = null;
            EndDate = null;
            DaysAgoToStart = null;
            StatsType = null;
            DisabledOnly = false;
        }

        public DASynchAdformStats()
        {
            IsCommand("daSynchAdformStats", "synch Adform Stats");
            HasOption<int>("a|accountId=", "Account Id (default = all)", c => AccountId = c);
            HasOption("s|startDate=", "Start Date (default is 'daysAgo')", c => StartDate = DateTime.Parse(c));
            HasOption("e|endDate=", "End Date (default is yesterday)", c => EndDate = DateTime.Parse(c));
            HasOption<int>("d|daysAgo=", $"Days Ago to start, if startDate not specified (default = {DefaultDaysAgo})", c => DaysAgoToStart = c);
            HasOption<string>("t|statsType=", "Stats Type (default: all)", c => StatsType = c);
            HasOption<bool>("x|disabledOnly=", "Include only disabled accounts (default = false)", c => DisabledOnly = c);
        }

        //TODO: if synching all accounts, can we make one API call to get everything?

        public override int Execute(string[] remainingArguments)
        {
            var dateRange = CommandHelper.GetDateRange(StartDate, EndDate, DaysAgoToStart, DefaultDaysAgo);
            Logger.Info("Adform ETL. DateRange {0}.", dateRange);

            var statsType = new StatsTypeAgg(StatsType);
            var accountIdsForOrders = ConfigurationHelper.ExtractEnumerableFromConfig("Adform_OrderInsteadOfCampaign");
            var trackingIdsOfAccounts = ConfigurationHelper.ExtractDictionaryFromConfigValue("Adform_AccountsWithSpecificTracking", "Adform_AccountsTrackingIds");

            var accounts = GetAccounts();
            AdformUtility.TokenSets = GetTokens();

            foreach (var account in accounts)
            {
                etlList = new List<Action>();
                Logger.Info("Commencing ETL for Adform account ({0}) {1}", account.Id, account.Name);
                var orderInsteadOfCampaign = accountIdsForOrders.Contains(account.ExternalId);
                var adformUtility = CreateUtility(account, trackingIdsOfAccounts);

                AddEnabledEtl(statsType.Daily, account, () => DoETL_Daily(dateRange, account, adformUtility));
                AddEnabledEtl(statsType.Strategy, account, () => DoETL_Strategy(dateRange, account, orderInsteadOfCampaign, adformUtility));
                AddEnabledEtl(statsType.AdSet, account, () => DoETL_AdSet(dateRange, account, orderInsteadOfCampaign, adformUtility));
                AddEnabledEtl(statsType.Creative, account, () => DoETL_Creative(dateRange, account, adformUtility));

                Parallel.Invoke(etlList.ToArray());
            }

            SaveTokens(AdformUtility.TokenSets);
            return 0;
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

        private static AdformUtility CreateUtility(ExtAccount account, Dictionary<string, string> trackingIdsOfAccounts)
        {
            var adformUtility = new AdformUtility(
                message => Logger.Info(account.Id, message),
                exc => Logger.Error(account.Id, exc));
            adformUtility.SetWhichAlt(account.ExternalId);
            adformUtility.TrackingId = trackingIdsOfAccounts.ContainsKey(account.ExternalId)
                ? trackingIdsOfAccounts[account.ExternalId]
                : null;
            return adformUtility;
        }

        private static string[] GetTokens()
        {
            return Platform.GetPlatformTokens(Platform.Code_Adform);
        }

        private static void SaveTokens(string[] tokens)
        {
            Platform.SavePlatformTokens(Platform.Code_Adform, tokens);
        }

        // ---
        private static void DoETL_Daily(DateRange dateRange, ExtAccount account, AdformUtility adformUtility)
        {
            var extractor = new AdformDailySummaryExtractor(adformUtility, dateRange, account);
            var loader = new TDDailySummaryLoader(account.Id);
            CommandHelper.DoEtl(extractor, loader);
        }

        private static void DoETL_Strategy(DateRange dateRange, ExtAccount account, bool byOrder, AdformUtility adformUtility)
        {
            var extractor = new AdformStrategySummaryExtractor(adformUtility, dateRange, account, byOrder);
            var loader = new AdformCampaignSummaryLoader(account.Id);
            CommandHelper.DoEtl(extractor, loader);
        }

        private static void DoETL_AdSet(DateRange dateRange, ExtAccount account, bool byOrder, AdformUtility adformUtility)
        {
            var extractor = new AdformAdSetSummaryExtractor(adformUtility, dateRange, account, byOrder);
            var loader = new AdformLineItemSummaryLoader(account.Id);
            CommandHelper.DoEtl(extractor, loader);
        }

        private static void DoETL_Creative(DateRange dateRange, ExtAccount account, AdformUtility adformUtility)
        {
            var extractor = new AdformTDadSummaryExtractor(adformUtility, dateRange, account);
            var loader = new TDadSummaryLoader(account.Id);
            CommandHelper.DoEtl(extractor, loader);
        }

        private IEnumerable<ExtAccount> GetAccounts()
        {
            using (var db = new ClientPortalProgContext())
            {
                var accounts = db.ExtAccounts.Include("Campaign.BudgetInfos").Include("Campaign.PlatformBudgetInfos")
                    .Where(a => a.Platform.Code == Platform.Code_Adform);
                if (AccountId.HasValue)
                {
                    accounts = accounts.Where(a => a.Id == AccountId.Value);
                }
                else if (!DisabledOnly)
                {
                    accounts = accounts.Where(a => !a.Disabled);
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
