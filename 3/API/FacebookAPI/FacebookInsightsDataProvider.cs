using System;
using System.Collections.Generic;
using System.Threading;
using FacebookAPI.Api;
using FacebookAPI.Converters;
using FacebookAPI.Entities;
using FacebookAPI.Enums;
using FacebookAPI.Utils;

namespace FacebookAPI
{
    /// <summary>
    /// Facebook Insights data provider.
    /// </summary>
    /// <seealso cref="FacebookAPI.BaseFacebookDataProvider" />
    public class FacebookInsightsDataProvider : BaseFacebookDataProvider
    {
        public const int RowsReturnedAtATime = 100;
        public const string Pattern_ParenNums = @"^\((\d+)\)\s*";
        public const int InitialWaitMillisecs = 1500;
        public const int MaxRetries = 20; //??reduce??
        private const int SecondsToWaitIfLimitReached = 61;

        private static readonly Dictionary<PlatformFilter, string> PlatformFilterNames = new Dictionary<PlatformFilter, string>
        {
            {PlatformFilter.Facebook, "facebook"},
            {PlatformFilter.Instagram, "instagram"},
            {PlatformFilter.Audience, "audience_network"},
            {PlatformFilter.Messenger, "messenger"},
            {PlatformFilter.All, null}
        };

        private static readonly Dictionary<ConversionActionType, string> ActionTypeNames = new Dictionary<ConversionActionType, string>
        {
            {ConversionActionType.MobileAppInstall, "mobile_app_install"},
            {ConversionActionType.Purchase, "offsite_conversion.fb_pixel_purchase"},
            {ConversionActionType.Registration, "offsite_conversion.fb_pixel_complete_registration"},
            {ConversionActionType.VideoPlay, "video_play"},
            {ConversionActionType.Default, "offsite_conversion"}
        };

        private static readonly Dictionary<Attribution, string> AttributionPostfixNames = new Dictionary<Attribution, string>
        {
            {Attribution.Click, "click"},
            {Attribution.View, "view"}
        };

        public int? DaysPerCall_Override = null;
        public int DaysPerCall_Campaign = 15;
        public int DaysPerCall_AdSet = 7;
        public int DaysPerCall_Ad = 3;

        private string campaignFilterOperator;
        private string campaignFilterValue;
        private string conversionActionType = ActionTypeNames[ConversionActionType.Default];
        private string platformFilter = PlatformFilterNames[PlatformFilter.All];
        private string clickAttribution = GetAttributionName(Attribution.Click, AttributionWindow.Days28);
        private string viewAttribution = GetAttributionName(Attribution.View, AttributionWindow.Day1);

        public FacebookInsightsDataProvider(Action<string> logInfo, Action<string> logError)
            : base(logInfo, logError)
        {
        }

        public void SetConversionActionType(ConversionActionType actionType)
        {
            conversionActionType = ActionTypeNames[actionType];
        }

        public void SetPlatformFilter(PlatformFilter filter)
        {
            platformFilter = PlatformFilterNames[filter];
        }

        public void SetCampaignFilter(string filter)
        {
            if (string.IsNullOrEmpty(filter))
            {
                campaignFilterValue = null;
            }
            else
            {
                campaignFilterValue = filter;
                campaignFilterOperator = "CONTAIN";
            }
        }

        public void SetClickAttributionWindow(AttributionWindow window)
        {
            clickAttribution = GetAttributionName(Attribution.Click, window);
        }

        public void SetViewAttributionWindow(AttributionWindow window)
        {
            viewAttribution = GetAttributionName(Attribution.View, window);
        }

        private static string GetAttributionName(Attribution attribution, AttributionWindow window)
        {
            return $"{(int)window}d_{AttributionPostfixNames[attribution]}";
        }

        public IEnumerable<FBSummary> GetDailyStats(string accountId, DateTime start, DateTime end)
        {
            var converter = new DailyInsigthsFacebookSummaryConverter(conversionActionType, clickAttribution, viewAttribution);
            return GetFBSummariesLoop(accountId, start, end, converter);
        }

        public IEnumerable<FBSummary> GetDailyCampaignStats(string accountId, DateTime start, DateTime end)
        {
            var converter = new StrategyInsigthsFacebookSummaryConverter(conversionActionType, clickAttribution, viewAttribution);
            return GetFBSummariesLoop(accountId, start, end, converter, byCampaign: true);
        }

        public IEnumerable<FBSummary> GetDailyAdSetStats(string accountId, DateTime start, DateTime end)
        {
            var converter = new AdSetInsigthsFacebookSummaryConverter(conversionActionType, clickAttribution, viewAttribution);
            return GetFBSummariesLoop(accountId, start, end, converter, byCampaign: true, byAdSet: true, getArchived: true);
        }

        public IEnumerable<FBSummary> GetDailyAdStats(string accountId, DateTime start, DateTime end)
        {
            var converter = new AdInsigthsFacebookSummaryConverter(conversionActionType, clickAttribution, viewAttribution);
            return GetFBSummariesLoop(accountId, start, end, converter, byCampaign: true, byAdSet: true, byAd: true, getArchived: false);
        }

        private IEnumerable<FBSummary> GetFBSummariesLoop(string accountId, DateTime start, DateTime end, FacebookSummaryConverter converter,
            bool byCampaign = false, bool byAdSet = false, bool byAd = false, bool getArchived = false)
        {
            int daysPerCall = 365; // default
            if (DaysPerCall_Override.HasValue)
                daysPerCall = DaysPerCall_Override.Value;
            else
            {
                if (byCampaign)
                    daysPerCall = DaysPerCall_Campaign;
                if (byAdSet)
                    daysPerCall = DaysPerCall_AdSet;
                if (byAd)
                    daysPerCall = DaysPerCall_Ad;
            }
            var clientParmsList = new List<ClientAndParams>();
            while (start <= end)
            {
                var tempEnd = start.AddDays(daysPerCall - 1);
                if (tempEnd > end)
                    tempEnd = end;

                var clientParms = GetClientAndParms(accountId, start, tempEnd, byCampaign: byCampaign, byAdSet: byAdSet, byAd: byAd);
                clientParms.GetRunId_withRetry(LogInfo, LogError);
                clientParmsList.Add(clientParms);
                if (getArchived)
                {
                    var clientParmsArchived = GetClientAndParms(accountId, start, tempEnd, byCampaign: byCampaign, byAdSet: byAdSet, byAd: byAd, getArchived: true);
                    clientParmsArchived.GetRunId_withRetry(LogInfo, LogError);
                    clientParmsList.Add(clientParmsArchived);
                }

                start = start.AddDays(daysPerCall);
            }
            Thread.Sleep(InitialWaitMillisecs);
            foreach (var clientParms in clientParmsList)
            {
                var fbSummaries = GetFBSummaries(clientParms, converter);
                foreach (var fbSum in fbSummaries)
                {
                    yield return fbSum;
                }
            }
        }

        private ClientAndParams GetClientAndParms(string accountId, DateTime start, DateTime end, bool byCampaign = false, bool byAdSet = false, bool byAd = false, bool getArchived = false)
        {
            var levelVal = "";
            var fieldsVal = "spend,impressions,inline_link_clicks,clicks,actions,action_values";
            if (byCampaign)
            {
                levelVal = "campaign";
                fieldsVal += ",campaign_id,campaign_name";
            }
            if (byAdSet)
            {
                levelVal = "adset";
                fieldsVal += ",adset_id,adset_name";
                fieldsVal += ",video_10_sec_watched_actions,video_p100_watched_actions";
            }
            if (byAd)
            {
                levelVal = "ad";
                fieldsVal += ",ad_id,ad_name";
            }
            var filterList = new List<Filter>();
            if (!String.IsNullOrWhiteSpace(platformFilter))
                filterList.Add(new Filter { field = "publisher_platform", @operator = "IN", value = new[] { platformFilter } });
            if (!String.IsNullOrEmpty(campaignFilterValue))
                filterList.Add(new Filter { field = "campaign.name", @operator = campaignFilterOperator, value = campaignFilterValue });
            if (getArchived)
            {
                filterList.Add(new Filter { field = $"{levelVal}.effective_status", @operator = "IN", value = new[] { "ARCHIVED" } });
            }
            var parameters = new
            {
                filtering = filterList.ToArray(),
                level = levelVal,
                fields = fieldsVal,
                action_breakdowns = "action_type", //,action_reaction
                action_attribution_windows = clickAttribution + "," + viewAttribution, // e.g. "28d_click,1d_view",
                time_range = new { since = FacebookRequestUtils.GetDateString(start), until = FacebookRequestUtils.GetDateString(end) },
                time_increment = 1,
            };

            string by = byCampaign ? " by Campaign" : "";
            by += byAdSet ? " by AdSet" : "";
            by += byAd ? " by Ad" : "";
            string logMessage = string.Format("GetFBSummaries {0:d} - {1:d} ({2}{3})", start, end, accountId, by);

            return new ClientAndParams
            {
                fbClient = CreateFBClient(),
                path = accountId + "/insights",
                parms = parameters,
                logMessage = logMessage
            };
        }

        private IEnumerable<FBSummary> GetFBSummaries(ClientAndParams clientParms, FacebookSummaryConverter converter)
        {
            LogInfo(clientParms.logMessage);
            bool moreData = false;
            var afterVal = ""; // later, used for paging
            do
            {
                dynamic retObj = null;
                int tryNumber = 0;
                do
                {
                    try
                    {
                        var runId = clientParms.GetRunId();
                        if (!moreData) // skip this when getting page 2+
                        {
                            retObj = clientParms.fbClient.Get(runId); // check to see if job complete
                            int numCalls = 1;
                            int waitMillisecs = InitialWaitMillisecs;
                            while (retObj.async_status != "Job Completed" || retObj.async_percent_completion < 100)
                            {
                                if (retObj.async_status == "Job Failed")
                                {
                                    clientParms.GetRunId_withRetry(LogInfo, LogError); // resets existing run  id
                                    throw new Exception("Job failed. Execution will be requested again.");
                                }
                                waitMillisecs += 500;
                                Thread.Sleep(waitMillisecs);
                                retObj = clientParms.fbClient.Get(runId);
                                numCalls++;
                            }
                            LogInfo(String.Format("{0} call(s) to check if job completed", numCalls));
                        }
                        retObj = clientParms.fbClient.Get(runId + "/insights", new { after = afterVal }); // get the actual data
                        tryNumber = 0; // mark as call succeeded (no exception)
                    }
                    catch (Exception ex)
                    {
                        LogError(ex.Message);
                        int secondsToWait = SecondsToWaitIfLimitReached;
                        tryNumber++;
                        if (tryNumber < MaxRetries)
                        {
                            LogInfo(String.Format("Waiting {0} seconds before trying again.", secondsToWait));
                            Thread.Sleep(secondsToWait * 1000);
                        }
                    }
                } while (tryNumber > 0 && tryNumber < MaxRetries);
                if (tryNumber >= MaxRetries)
                    throw new Exception(String.Format("Tried {0} times. Aborting GetFBSummaries.", tryNumber));

                if (retObj == null)
                    continue;
                if (retObj.data != null)
                {
                    foreach (var row in retObj.data)
                    {
                        var fbSum = converter.ParseSummaryRow(row);
                        yield return fbSum;
                    }
                }
                moreData = (retObj.paging != null && retObj.paging.next != null);
                if (moreData)
                    afterVal = retObj.paging.cursors.after;
            } while (moreData);
        }
    }
}
