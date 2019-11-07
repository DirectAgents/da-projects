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

        private static List<string> accountIdsForOrders;
        private static Dictionary<string, string> trackingIdsOfAccounts;
        private static List<string> accountsWithMultipleMedia;
        private static List<string> accountsWithAllMediaUniqueImpressions;

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
            SetConfigurableVariables();
            var accounts = GetAccounts();
            AdformUtility.TokenSets = GetTokens();
            foreach (var account in accounts)
            {
                Logger.Info("Commencing ETL for Adform account ({0}) {1}", account.Id, account.Name);
                var etlLevelActions = GetEtlLevelActions(account, dateRange, statsType);
                Parallel.Invoke(etlLevelActions.ToArray());
            }
            SaveTokens(AdformUtility.TokenSets);
            return 0;
        }

        private static void SetConfigurableVariables()
        {
            accountIdsForOrders = ConfigurationHelper.ExtractEnumerableFromConfig("Adform_OrderInsteadOfCampaign");
            trackingIdsOfAccounts = ConfigurationHelper.ExtractDictionaryFromConfigValue("Adform_AccountsWithSpecificTracking", "Adform_AccountsTrackingIds");
            accountsWithMultipleMedia = ConfigurationHelper.ExtractEnumerableFromConfig("Adform_AccountsWithMultipleMedia");
            accountsWithAllMediaUniqueImpressions = ConfigurationHelper.ExtractEnumerableFromConfig("Adform_AccountsWithAllMediaUniqueImpressions");
        }

        private static List<Action> GetEtlLevelActions(ExtAccount account, DateRange dateRange, StatsTypeAgg statsType)
        {
            var etlLevelActions = new List<Action>();
            var orderInsteadOfCampaign = accountIdsForOrders.Contains(account.ExternalId);
            var rtbMediaOnly = !accountsWithMultipleMedia.Contains(account.ExternalId);
            var adformUtility = CreateUtility(account);
            if (statsType.Daily)
            {
                var etlLevelAction = GetEtlLevelAction(
                    account,
                    () => DoETL_Daily(dateRange, account, adformUtility, rtbMediaOnly));
                etlLevelActions.Add(etlLevelAction);
            }
            if (statsType.Strategy)
            {
                var etlLevelAction = GetEtlLevelAction(
                    account,
                    () => DoETL_Strategy(dateRange, account, orderInsteadOfCampaign, adformUtility, rtbMediaOnly));
                etlLevelActions.Add(etlLevelAction);
            }
            if (statsType.AdSet)
            {
                var etlLevelAction = GetEtlLevelAction(
                    account,
                    () => DoETL_AdSet(dateRange, account, orderInsteadOfCampaign, adformUtility, rtbMediaOnly));
                etlLevelActions.Add(etlLevelAction);
            }
            if (statsType.Creative)
            {
                var etlLevelAction = GetEtlLevelAction(
                    account,
                    () => DoETL_Creative(dateRange, account, adformUtility, rtbMediaOnly));
                etlLevelActions.Add(etlLevelAction);
            }
            return etlLevelActions;
        }

        private static Action GetEtlLevelAction(ExtAccount account, Action etlAction)
        {
            return () =>
            {
                try
                {
                    etlAction();
                }
                catch (Exception ex)
                {
                    Logger.Error(account.Id, ex);
                }
            };
        }

        private static AdformUtility CreateUtility(ExtAccount account)
        {
            var adformUtility = new AdformUtility(
                message => Logger.Info(account.Id, message),
                message => Logger.Warn(account.Id, message),
                exc => Logger.Error(account.Id, exc));
            adformUtility.SetWhichAlt(account.ExternalId);
            adformUtility.TrackingIds = trackingIdsOfAccounts.ContainsKey(account.ExternalId)
                ? new[] { trackingIdsOfAccounts[account.ExternalId] }
                : Array.Empty<string>();
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
        private static void DoETL_Daily(DateRange dateRange, ExtAccount account, AdformUtility adformUtility, bool rtbMediaOnly)
        {
            var uniqueImpressionsForAllMediaTypes = accountsWithAllMediaUniqueImpressions.Contains(account.ExternalId);
            var extractor = new AdformDailySummaryExtractor(adformUtility, dateRange, account, rtbMediaOnly, uniqueImpressionsForAllMediaTypes);
            var loader = new TDDailySummaryLoader(account.Id);
            CommandHelper.DoEtl(extractor, loader);
        }

        private static void DoETL_Strategy(DateRange dateRange, ExtAccount account, bool byOrder, AdformUtility adformUtility, bool rtbMediaOnly)
        {
            var extractor = new AdformStrategySummaryExtractor(adformUtility, dateRange, account, rtbMediaOnly, byOrder);
            var loader = new AdformCampaignSummaryLoader(account.Id);
            CommandHelper.DoEtl(extractor, loader);
        }

        private static void DoETL_AdSet(DateRange dateRange, ExtAccount account, bool byOrder, AdformUtility adformUtility, bool rtbMediaOnly)
        {
            var extractor = new AdformAdSetSummaryExtractor(adformUtility, dateRange, account, rtbMediaOnly, byOrder);
            var loader = new AdformLineItemSummaryLoader(account.Id);
            CommandHelper.DoEtl(extractor, loader);
        }

        private static void DoETL_Creative(DateRange dateRange, ExtAccount account, AdformUtility adformUtility, bool rtbMediaOnly)
        {
            var extractor = new AdformTDadSummaryExtractor(adformUtility, dateRange, account, rtbMediaOnly);
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