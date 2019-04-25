using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CakeExtracter.Bootstrappers;
using CakeExtracter.Common;
using CakeExtracter.Etl.SocialMarketing.Extracters.V2;
using CakeExtracter.Etl.SocialMarketing.LoadersDAV2;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using FacebookAPI;
using FacebookAPI.Enums;

namespace CakeExtracter.Commands
{
    /// <summary>
    /// Facebook stats updating command(Included creatives expansion)
    /// </summary>
    /// <seealso cref="CakeExtracter.Common.ConsoleCommand" />
    [Export(typeof(ConsoleCommand))]
    public class DASynchFacebookStatsV2 : ConsoleCommand
    {
        private const int DefaultDaysAgo = 41;

        private const int processingChunkSize = 11;

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
            var cmd = new DASynchFacebookStatsV2
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

        public DASynchFacebookStatsV2()
        {
            IsCommand("daSynchFacebookStatsV2", "synch Facebook stats");
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
            var accounts = GetAccounts();

            var fbAdMetadataProvider = new FacebookAdMetadataProvider(null, null);
            var metadataExtractor = new FacebookAdMetadataExtractorV2(fbAdMetadataProvider);
            Parallel.ForEach(accounts, (account) =>
            {
                var acctDateRange = new DateRange(dateRange.FromDate, dateRange.ToDate);
                if (account.Campaign != null) // check/adjust daterange - if acct assigned to a campaign/advertiser
                {
                    if (!StartDate.HasValue && account.Campaign.Advertiser.StartDate.HasValue
                        && acctDateRange.FromDate < account.Campaign.Advertiser.StartDate.Value)
                        acctDateRange.FromDate = account.Campaign.Advertiser.StartDate.Value;
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
                if (Accts_DailyOnly.Contains(account.ExternalId))
                    return;
                // Skip strategy & adset stats if there were no dailies
                if (statsType.Strategy && (numDailyItems == null || numDailyItems.Value > 0))
                    DoETL_Strategy(acctDateRange, account, fbUtility);
                if (statsType.AdSet && (numDailyItems == null || numDailyItems.Value > 0))
                    DoETL_AdSet(acctDateRange, account, fbUtility);
                if (statsType.Creative && (numDailyItems == null || numDailyItems.Value > 0))
                    DoETL_Creative(acctDateRange, account, fbUtility, metadataExtractor);
                Logger.Info(account.Id, "Finished Facebook ETL. Account {0} - {1}. DateRange {2}.", account.Id, account.Name, acctDateRange);
            });
            return 0;
        }

        private int DoETL_Daily(DateRange dateRange, ExtAccount account, FacebookInsightsDataProvider fbUtility)
        {
            var dateRangesToProcess = dateRange.GetDaysChunks(processingChunkSize).ToList();
            int addedCount = 0;
            dateRangesToProcess.ForEach(rangeToProcess =>
            {
                var extractor = new FacebookDailySummaryExtracterV2(rangeToProcess, account, fbUtility);
                var loader = new FacebookDailySummaryLoaderV2(account.Id, rangeToProcess);
                CommandHelper.DoEtl(extractor, loader);
                addedCount += extractor.Added;
            });
            return addedCount;
        }

        private void DoETL_Strategy(DateRange dateRange, ExtAccount account, FacebookInsightsDataProvider fbUtility)
        {
            var dateRangesToProcess = dateRange.GetDaysChunks(processingChunkSize).ToList();
            dateRangesToProcess.ForEach(rangeToProcess =>
            {
                var extractor = new FacebookCampaignSummaryExtracterV2(rangeToProcess, account, fbUtility);
                var loader = new FacebookCampaignSummaryLoaderV2(account.Id, rangeToProcess);
                CommandHelper.DoEtl(extractor, loader);
            });
        }

        private void DoETL_AdSet(DateRange dateRange, ExtAccount account, FacebookInsightsDataProvider fbUtility)
        {
            var dateRangesToProcess = dateRange.GetDaysChunks(processingChunkSize).ToList();
            dateRangesToProcess.ForEach(rangeToProcess =>
            {
                var extractor = new FacebookAdSetSummaryExtracterV2(rangeToProcess, account, fbUtility);
                var loader = new FacebookAdSetSummaryLoaderV2(account.Id, rangeToProcess);
                CommandHelper.DoEtl(extractor, loader);
            });
        }

        private void DoETL_Creative(DateRange dateRange, ExtAccount account, FacebookInsightsDataProvider fbUtility, FacebookAdMetadataExtractorV2 metadataExtractor)
        {
            var dateRangesToProcess = dateRange.GetDaysChunks(processingChunkSize).ToList();
           
            Logger.Info(account.Id, "Started reading ad's metadata");
            //It's not possible currently fetch facebook creatives data from the insights endpoint.
            var allAdsMetadata = metadataExtractor.GetAdCreativesData(account.ExternalId);
            Logger.Info(account.Id, "Finished reading ad's metadata");
            dateRangesToProcess.ForEach(rangeToProcess =>
            {
                var extractor = new FacebookAdSummaryExtracterV2(rangeToProcess, account, fbUtility, allAdsMetadata);
                var loader = new FacebookAdSummaryLoaderV2(account.Id, rangeToProcess);
                CommandHelper.DoEtl(extractor, loader);
            });
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
            var window = GetAttributionWindow(accountId, Attribution.Click, ClickWindow);
            if (window != 0)
            {
                fbUtility.SetClickAttributionWindow(window);
            }
        }

        private void SetUtilityViewAttributionWindows(FacebookInsightsDataProvider fbUtility, string accountId)
        {
            var window = GetAttributionWindow(accountId, Attribution.View, ViewWindow);
            if (window != 0)
            {
                fbUtility.SetViewAttributionWindow(window);
            }
        }

        private AttributionWindow GetAttributionWindow(string accountId, Attribution attribution, int? sourceWindow)
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
