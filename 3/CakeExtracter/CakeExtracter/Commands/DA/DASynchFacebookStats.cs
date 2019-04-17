using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CakeExtracter.Bootstrappers;
using CakeExtracter.Common;
using CakeExtracter.Etl.SocialMarketing.Extracters;
using CakeExtracter.Etl.SocialMarketing.LoadersDA;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using FacebookAPI;
using FacebookAPI.Enums;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class DASynchFacebookStats : ConsoleCommand
    {
        private const int DefaultDaysAgo = 41;

        private readonly Dictionary<string, PlatformFilter> networkFilters = new Dictionary<string, PlatformFilter>
        {
            {"FACEBOOK", PlatformFilter.Facebook},
            {"INSTAGRAM", PlatformFilter.Instagram},
            {"AUDIENCE", PlatformFilter.Audience},
            {"MESSENGER", PlatformFilter.Messenger}
        };

        private readonly Dictionary<string, ConversionActionType> configNamesForAccountsOfActionType = new Dictionary<string, ConversionActionType>
        {
            {"FB_ConversionsAsMobileAppInstalls", ConversionActionType.MobileAppInstall},
            {"FB_ConversionsAsPurchases", ConversionActionType.Purchase},
            {"FB_ConversionsAsRegistrations", ConversionActionType.Registration},
            {"FB_ConversionsAsVideoPlays", ConversionActionType.VideoPlay}
        };

        private readonly Dictionary<Attribution, Dictionary<string, AttributionWindow>> configNamesForAccountsOfWindow =
            new Dictionary<Attribution, Dictionary<string, AttributionWindow>>
        {
            {
                Attribution.Click, new Dictionary<string, AttributionWindow>
                {
                    {"FB_7d_click", AttributionWindow.Days7},
                    {"FB_28d_click", AttributionWindow.Days28}
                }
            },
            {
                Attribution.View, new Dictionary<string, AttributionWindow>
                {
                    {"FB_7d_view", AttributionWindow.Days7},
                    {"FB_28d_view", AttributionWindow.Days28}
                }
            }
        };

        public static int RunStatic(int? campaignId = null, int? accountId = null, DateTime? startDate = null, DateTime? endDate = null, string statsType = null)
        {
            AutoMapperBootstrapper.CheckRunSetup();
            var cmd = new DASynchFacebookStats
            {
                CampaignId = campaignId,
                AccountId = accountId,
                StartDate = startDate,
                EndDate = endDate,
                StatsType = statsType
            };
            return cmd.Run();
        }

        public int? AccountId { get; set; }
        public int? CampaignId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? DaysAgoToStart { get; set; }
        public string StatsType { get; set; }
        public bool DisabledOnly { get; set; }
        public int? MinAccountId { get; set; }
        public int? DaysPerCall { get; set; }
        public int? ClickWindow { get; set; }
        public int? ViewWindow { get; set; }

        public override void ResetProperties()
        {
            AccountId = null;
            CampaignId = null;
            StartDate = null;
            EndDate = null;
            DaysAgoToStart = null;
            StatsType = null;
            DisabledOnly = false;
            MinAccountId = null;
            DaysPerCall = null;
            ClickWindow = null;
            ViewWindow = null;
        }

        public DASynchFacebookStats()
        {
            IsCommand("daSynchFacebookStats", "synch Facebook stats");
            HasOption<int>("a|accountId=", "Account Id (default = all)", c => AccountId = c);
            HasOption<int>("c|campaignId=", "Campaign Id (optional)", c => CampaignId = c);
            HasOption("s|startDate=", "Start Date (default is 'daysAgo')", c => StartDate = DateTime.Parse(c));
            HasOption("e|endDate=", "End Date (default is yesterday)", c => EndDate = DateTime.Parse(c));
            HasOption<int>("d|daysAgo=", $"Days Ago to start, if startDate not specified (default = {DefaultDaysAgo})", c => DaysAgoToStart = c);
            HasOption<string>("t|statsType=", "Stats Type (default: all)", c => StatsType = c);
            HasOption<bool>("x|disabledOnly=", "Include only disabled accounts (default = false)", c => DisabledOnly = c);
            HasOption<int>("m|minAccountId=", "Include this and all higher accountIds (optional)", c => MinAccountId = c);
            HasOption<int>("p|daysPerCall=", "Days Per API call (default: varies per stats type)", c => DaysPerCall = c);
            HasOption<int>("u|clickWindow=", "Click attribution window (can set to 7 or 28, otherwise will be default or from config)", c => ClickWindow = c);
            HasOption<int>("v|viewWindow=", "View attribution window (can set to 7 or 1 or 28, otherwise will be default or from config)", c => ViewWindow = c);
        }

        public override int Execute(string[] remainingArguments)
        {
            var dateRange = CommandHelper.GetDateRange(StartDate, EndDate, DaysAgoToStart, DefaultDaysAgo);
            Logger.Info("Facebook ETL. DateRange {0}.", dateRange);

            var statsType = new StatsTypeAgg(StatsType);

            var accounts = GetAccounts().Take(1);
            Parallel.ForEach(accounts, (account) =>
            {
                //var adCreativeProvider = new FacebookAdMetadataProvider();
               
                //var adsInfo = adCreativeProvider.GetAdsCreativesForAccount(account.ExternalId, dateRange.FromDate, dateRange.ToDate);
                //return;

                var acctDateRange = new DateRange(dateRange.FromDate, dateRange.ToDate);
                if (account.Campaign != null) // check/adjust daterange - if acct assigned to a campaign/advertiser
                {
                    //if FromDate came from the default value, check Advertiser's start date
                    if (!StartDate.HasValue && account.Campaign.Advertiser.StartDate.HasValue
                        && acctDateRange.FromDate < account.Campaign.Advertiser.StartDate.Value)
                        acctDateRange.FromDate = account.Campaign.Advertiser.StartDate.Value;
                    //likewise for ToDate / Advertiser's end date
                    if (!EndDate.HasValue && account.Campaign.Advertiser.EndDate.HasValue
                        && acctDateRange.ToDate > account.Campaign.Advertiser.EndDate.Value)
                        acctDateRange.ToDate = account.Campaign.Advertiser.EndDate.Value;
                }
                Logger.Info(account.Id, "Facebook ETL. Account {0} - {1}. DateRange {2}.", account.Id, account.Name, acctDateRange);
                if (acctDateRange.ToDate < acctDateRange.FromDate)
                    return;

                var fbUtility = CreateUtility(account);

                int? numDailyItems = null;
                if (statsType.Daily)
                    numDailyItems = DoETL_Daily(acctDateRange, account, fbUtility);

                var Accts_DailyOnly = !AccountId.HasValue || statsType.All
                    ? ConfigurationHelper.ExtractEnumerableFromConfig("FB_DailyStatsOnly").ToArray()
                    : new string[] { };

                // Used when synching all accounts AND/OR all stats types...
                // So if an account is marked as "daily only", you can only load other stats by specifying the accountId and statsType
                // TODO? remove this since we now handle exceptions in the extracter?
                if (Accts_DailyOnly.Contains(account.ExternalId))
                    return;

                // Skip strategy & adset stats if there were no dailies
                if (statsType.Strategy && (numDailyItems == null || numDailyItems.Value > 0))
                    DoETL_Strategy(acctDateRange, account, fbUtility);
                if (statsType.AdSet && (numDailyItems == null || numDailyItems.Value > 0))
                    DoETL_AdSet(acctDateRange, account, fbUtility);

                if (statsType.Creative && !statsType.All) // don't include when getting "all" statstypes
                    DoETL_Creative(acctDateRange, account, fbUtility);
                //if (statsType.Site)
                //    DoETL_Site(acctDateRange, acct, fbUtility);
            });

            return 0;
        }

        private int DoETL_Daily(DateRange dateRange, ExtAccount account, FacebookInsightsDataProvider fbUtility)
        {
            var extractor = new FacebookDailySummaryExtracter(dateRange, account, fbUtility);
            var loader = new FacebookDailySummaryLoader(account.Id);
            CommandHelper.DoEtl(extractor, loader);
            return extractor.Added;
        }

        private void DoETL_Strategy(DateRange dateRange, ExtAccount account, FacebookInsightsDataProvider fbUtility)
        {
            var extractor = new FacebookCampaignSummaryExtracter(dateRange, account, fbUtility);
            var loader = new FacebookCampaignSummaryLoader(account.Id);
            CommandHelper.DoEtl(extractor, loader);
        }

        private void DoETL_AdSet(DateRange dateRange, ExtAccount account, FacebookInsightsDataProvider fbUtility)
        {
            var extractor = new FacebookAdSetSummaryExtracter(dateRange, account, fbUtility);
            var loader = new FacebookAdSetSummaryLoader(account.Id, loadActions: true);
            CommandHelper.DoEtl(extractor, loader);
        }

        private void DoETL_Creative(DateRange dateRange, ExtAccount account, FacebookInsightsDataProvider fbUtility)
        {
            var fbAdMetadataProvider = new FacebookAdMetadataProvider(m => Logger.Info(account.Id, m), m => Logger.Warn(account.Id, m));
            var extractor = new FacebookAdSummaryExtracter(dateRange, account, fbUtility, fbAdMetadataProvider);
            var loader = new FacebookAdSummaryLoader(account.Id, dateRange);
            CommandHelper.DoEtl(extractor, loader);
        }

        private IEnumerable<ExtAccount> GetAccounts()
        {
            using (var db = new ClientPortalProgContext())
            {
                var accounts = db.ExtAccounts.Include("Network").Include("Campaign.Advertiser").Where(a => a.Platform.Code == Platform.Code_FB);
                if (CampaignId.HasValue || AccountId.HasValue)
                {
                    if (CampaignId.HasValue)
                        accounts = accounts.Where(a => a.CampaignId == CampaignId.Value);
                    if (AccountId.HasValue)
                        accounts = accounts.Where(a => a.Id == AccountId.Value);
                }
                else if (!DisabledOnly)
                    accounts = accounts.Where(a => !a.Disabled); //all accounts that aren't disabled

                if (DisabledOnly)
                    accounts = accounts.Where(a => a.Disabled);
                if (MinAccountId.HasValue)
                    accounts = accounts.Where(a => a.Id >= MinAccountId.Value);

                return accounts.OrderBy(a => a.Id).ToList().Where(a => !string.IsNullOrWhiteSpace(a.ExternalId));
            }
        }

        private FacebookInsightsDataProvider CreateUtility(ExtAccount account)
        {
            var accountId = account.ExternalId;
            var fbUtility = new FacebookInsightsDataProvider(m => Logger.Info(account.Id, m), m => Logger.Warn(account.Id, m))
            {
                DaysPerCall_Override = DaysPerCall
            };
            SetUtilityFilters(fbUtility, account);
            SetUtilityConversionType(fbUtility, accountId);
            SetUtilityAttributionWindows(fbUtility, accountId);
            return fbUtility;
        }

        private void SetUtilityFilters(FacebookInsightsDataProvider fbUtility, ExtAccount account)
        {
            if (account.Network != null)
            {
                SetUtilityPlatformFilters(fbUtility, account.Network.Name);
            }

            fbUtility.SetCampaignFilter(account.Filter);
        }

        private void SetUtilityPlatformFilters(FacebookInsightsDataProvider fbUtility, string networkName)
        {
            var network = Regex.Replace(networkName, @"\s+", "").ToUpper();
            foreach (var filter in networkFilters)
            {
                if (network.StartsWith(filter.Key))
                {
                    fbUtility.SetPlatformFilter(filter.Value);
                }
            }
        }

        private void SetUtilityConversionType(FacebookInsightsDataProvider fbUtility, string accountId)
        {
            foreach (var configName in configNamesForAccountsOfActionType)
            {
                var accounts = ConfigurationHelper.ExtractEnumerableFromConfig(configName.Key);
                if (accounts.Contains(accountId))
                {
                    fbUtility.SetConversionActionType(configName.Value);
                }
            }
        }

        private void SetUtilityAttributionWindows(FacebookInsightsDataProvider fbUtility, string accountId)
        {
            SetUtilityClickAttributionWindows(fbUtility, accountId);
            SetUtilityViewAttributionWindows(fbUtility, accountId);
        }

        private void SetUtilityClickAttributionWindows(FacebookInsightsDataProvider fbUtility, string accountId)
        {
            var window = GetWindow(accountId, Attribution.Click, ClickWindow);
            if (window != 0)
            {
                fbUtility.SetClickAttributionWindow(window);
            }
        }

        private void SetUtilityViewAttributionWindows(FacebookInsightsDataProvider fbUtility, string accountId)
        {
            var window = GetWindow(accountId, Attribution.View, ViewWindow);
            if (window != 0)
            {
                fbUtility.SetViewAttributionWindow(window);
            }
        }

        private AttributionWindow GetWindow(string accountId, Attribution attribution, int? sourceWindow)
        {
            AttributionWindow window = 0;
            try
            {
                window = (AttributionWindow)Enum.ToObject(typeof(AttributionWindow), sourceWindow);
            }
            catch (Exception)
            {
                foreach (var configName in configNamesForAccountsOfWindow[attribution])
                {
                    var accounts = ConfigurationHelper.ExtractEnumerableFromConfig(configName.Key);
                    if (accounts.Contains(accountId))
                    {
                        window = configName.Value;
                    }
                }
            }
            return window;
        }
    }
}
