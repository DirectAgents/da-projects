using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Adform;
using CakeExtracter.Bootstrappers;
using CakeExtracter.Common;
using CakeExtracter.Etl.TradingDesk.Extracters;
using CakeExtracter.Etl.TradingDesk.Extracters.AdformExtractors;
using CakeExtracter.Etl.TradingDesk.LoadersDA;
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

        private AdformUtility adformUtility { get; set; }

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
            Logger.LogToOneFile = true;
            var dateRange = CommandHelper.GetDateRange(StartDate, EndDate, DaysAgoToStart, DefaultDaysAgo);
            Logger.Info("Adform ETL. DateRange {0}.", dateRange);

            var statsType = new StatsTypeAgg(StatsType);
            SetupAdformUtility();
            var accounts = GetAccounts();
            var accountIdsForOrders = ConfigurationHelper.ExtractEnumerableFromConfig("Adform_OrderInsteadOfCampaign");

            foreach (var account in accounts)
            {
                Logger.Info("Commencing ETL for Adform account ({0}) {1}", account.Id, account.Name);
                adformUtility.SetWhichAlt(account.ExternalId);
                try
                {
                    if (statsType.Daily)
                        DoETL_Daily(dateRange, account);
                    if (statsType.Strategy)
                        DoETL_Strategy(dateRange, account, accountIdsForOrders.Contains(account.ExternalId));
                    if (statsType.AdSet)
                        DoETL_AdSet(dateRange, account);

                    if (statsType.Creative && !statsType.All) // don't include when getting "all" statstypes
                        DoETL_Creative(dateRange, account);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }
            }
            SaveTokens();
            return 0;
        }

        private void SetupAdformUtility()
        {
            adformUtility = new AdformUtility(m => Logger.Info(m), m => Logger.Warn(m));
            GetTokens();
        }

        private void GetTokens()
        {
            // Get tokens, if any, from the database
            var tokens = Platform.GetPlatformTokens(Platform.Code_Adform);
            for (var i = 0; i < tokens.Length && i < AdformUtility.NumAlts; i++)
            {
                adformUtility.AccessTokens[i] = tokens[i];
            }
        }

        private void SaveTokens()
        {
            Platform.SavePlatformTokens(Platform.Code_Adform, adformUtility.AccessTokens);
        }
        // ---

        private void DoETL_Daily(DateRange dateRange, ExtAccount account)
        {
            var extractor = new AdformDailySummaryExtractor(adformUtility, dateRange, account);
            var loader = new TDDailySummaryLoader(account.Id);
            CommandHelper.DoEtl(extractor, loader);
        }

        private void DoETL_Strategy(DateRange dateRange, ExtAccount account, bool byOrder)
        {
            var extractor = new AdformStrategySummaryExtractor(adformUtility, dateRange, account, byOrder);
            var loader = new AdformCampaignSummaryLoader(account.Id);
            CommandHelper.DoEtl(extractor, loader);
        }

        private void DoETL_AdSet(DateRange dateRange, ExtAccount account)
        {
            var extractor = new AdformAdSetSummaryExtractor(adformUtility, dateRange, account);
            var loader = new TDAdSetSummaryLoader(account.Id);
            CommandHelper.DoEtl(extractor, loader);
        }

        private void DoETL_Creative(DateRange dateRange, ExtAccount account)
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
