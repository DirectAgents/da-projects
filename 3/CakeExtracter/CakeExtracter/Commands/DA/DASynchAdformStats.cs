using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Adform.Utilities;
using CakeExtracter.Bootstrappers;
using CakeExtracter.Common;
using CakeExtracter.Common.JobExecutionManagement;
using CakeExtracter.Etl.Adform.Loaders;
using CakeExtracter.Etl.Adform.Repositories;
using CakeExtracter.Etl.Adform.Repositories.Summaries;
using CakeExtracter.Etl.Adform.Extractors;
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
                StatsType = statsType,
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
                CommandExecutionContext.Current.SetJobExecutionStateInHistory("Started", account.Id);
                DoETLs(account, dateRange, statsType);
                CommandExecutionContext.Current.SetJobExecutionStateInHistory("Finished", account.Id);
            }
            SaveTokens(AdformUtility.TokenSets);
            return 0;
        }

        private static void SetConfigurableVariables()
        {
            accountIdsForOrders = ConfigurationHelper.ExtractEnumerableFromConfig("Adform_OrderInsteadOfCampaign");
            trackingIdsOfAccounts = ConfigurationHelper.ExtractDictionaryFromConfigValue("Adform_AccountsWithSpecificTracking", "Adform_AccountsTrackingIds");
        }

        private static void DoETLs(ExtAccount account, DateRange dateRange, StatsTypeAgg statsType)
        {
            var orderInsteadOfCampaign = accountIdsForOrders.Contains(account.ExternalId);
            var adformUtility = CreateUtility(account);
            int? numberOfAddedDailyItems = null;
            try
            {
                if (statsType.Daily)
                {
                    numberOfAddedDailyItems = DoETL_Daily(dateRange, account, adformUtility);
                }
                if (statsType.Strategy && (numberOfAddedDailyItems == null || numberOfAddedDailyItems.Value > 0))
                {
                    DoETL_Strategy(dateRange, account, orderInsteadOfCampaign, adformUtility);
                }
                if (statsType.AdSet && (numberOfAddedDailyItems == null || numberOfAddedDailyItems.Value > 0))
                {
                    DoETL_AdSet(dateRange, account, orderInsteadOfCampaign, adformUtility);
                }
                if (statsType.Creative && (numberOfAddedDailyItems == null || numberOfAddedDailyItems.Value > 0))
                {
                    DoETL_Creative(dateRange, account, adformUtility);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(account.Id, ex);
            }
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
        private static int DoETL_Daily(DateRange dateRange, ExtAccount account, AdformUtility adformUtility)
        {
            CommandExecutionContext.Current.SetJobExecutionStateInHistory("Daily level.", account.Id);
            var extractor = new AdformDailySummaryExtractor(adformUtility, dateRange, account);
            var loader = CreateDailyLoader(account.Id);
            CommandHelper.DoEtl(extractor, loader);
            return extractor.Added;
        }

        private static void DoETL_Strategy(DateRange dateRange, ExtAccount account, bool byOrder, AdformUtility adformUtility)
        {
            CommandExecutionContext.Current.SetJobExecutionStateInHistory("Campaign level.", account.Id);
            var extractor = new AdformStrategySummaryExtractor(adformUtility, dateRange, account, byOrder);
            var loader = CreateCampaignLoader(account.Id);
            CommandHelper.DoEtl(extractor, loader);
        }

        private static void DoETL_AdSet(DateRange dateRange, ExtAccount account, bool byOrder, AdformUtility adformUtility)
        {
            CommandExecutionContext.Current.SetJobExecutionStateInHistory("LineItem level.", account.Id);
            var extractor = new AdformAdSetSummaryExtractor(adformUtility, dateRange, account, byOrder);
            var loader = CreateLineItemLoader(account.Id);
            CommandHelper.DoEtl(extractor, loader);
        }

        private static void DoETL_Creative(DateRange dateRange, ExtAccount account, AdformUtility adformUtility)
        {
            CommandExecutionContext.Current.SetJobExecutionStateInHistory("Banner level.", account.Id);
            var extractor = new AdformTDadSummaryExtractor(adformUtility, dateRange, account);
            var loader = CreateBannerLoader(account.Id);
            CommandHelper.DoEtl(extractor, loader);
        }

        private static AdfMediaTypeLoader CreateMediaTypeLoader(int accountId)
        {
            var mediaTypeRepository = new AdfMediaTypeDatabaseRepository();
            return new AdfMediaTypeLoader(accountId, mediaTypeRepository);
        }

        private static AdfDailySummaryLoader CreateDailyLoader(int accountId)
        {
            var summaryRepository = new AdfDailySummaryDatabaseRepository();
            var mediaTypeLoader = CreateMediaTypeLoader(accountId);
            return new AdfDailySummaryLoader(accountId, summaryRepository, mediaTypeLoader);
        }

        private static AdfCampaignSummaryLoader CreateCampaignLoader(int accountId)
        {
            var entityRepository = new AdfCampaignDatabaseRepository();
            var summaryRepository = new AdfCampaignSummaryDatabaseRepository();
            var mediaTypeLoader = CreateMediaTypeLoader(accountId);
            return new AdfCampaignSummaryLoader(accountId, entityRepository, summaryRepository, mediaTypeLoader);
        }

        private static AdfLineItemSummaryLoader CreateLineItemLoader(int accountId)
        {
            var entityRepository = new AdfLineItemDatabaseRepository();
            var summaryRepository = new AdfLineItemSummaryDatabaseRepository();
            var campaignLoader = CreateCampaignLoader(accountId);
            var mediaTypeLoader = CreateMediaTypeLoader(accountId);
            return new AdfLineItemSummaryLoader(accountId, entityRepository, summaryRepository, campaignLoader, mediaTypeLoader);
        }

        private static AdfBannerSummaryLoader CreateBannerLoader(int accountId)
        {
            var entityRepository = new AdfBannerDatabaseRepository();
            var summaryRepository = new AdfBannerSummaryDatabaseRepository();
            var mediaTypeLoader = CreateMediaTypeLoader(accountId);
            return new AdfBannerSummaryLoader(accountId, entityRepository, summaryRepository, mediaTypeLoader);
        }

        private IEnumerable<ExtAccount> GetAccounts()
        {
            using (var db = new ClientPortalProgContext())
            {
                var accounts = db.ExtAccounts
                    .Include("Campaign.BudgetInfos")
                    .Include("Campaign.PlatformBudgetInfos")
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