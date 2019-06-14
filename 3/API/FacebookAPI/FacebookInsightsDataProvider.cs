using System;
using System.Collections.Generic;
using System.Threading;
using FacebookAPI.Api;
using FacebookAPI.Constants;
using FacebookAPI.Converters;
using FacebookAPI.Entities;
using FacebookAPI.Enums;
using FacebookAPI.Utils;
using Polly;

namespace FacebookAPI
{
    /// <summary>
    /// Facebook Insights data provider.
    /// </summary>
    /// <seealso cref="FacebookAPI.BaseFacebookDataProvider" />
    public class FacebookInsightsDataProvider : BaseFacebookDataProvider
    {
        private const int RowsReturnedAtATime = 100;
        private const string Pattern_ParenNums = @"^\((\d+)\)\s*";
        private const int InitialWaitMillisecs = 1500;
        private const int MaxRetries = 20;
        private const int SecondsToWaitIfLimitReached = 61;

        private int asyncJobWaitingRetriesNumber = 25; // Will be updated from configuration. 25-default value.
        private int asyncJobFailureRetriesNumber = 5; // Will be updated from configuration. 25-default value.

        private static readonly Dictionary<PlatformFilter, string> PlatformFilterNames = new Dictionary<PlatformFilter, string>
        {
            { PlatformFilter.Facebook, "facebook" },
            { PlatformFilter.Instagram, "instagram" },
            { PlatformFilter.Audience, "audience_network" },
            { PlatformFilter.Messenger, "messenger" },
            { PlatformFilter.All, null },
        };

        private static readonly Dictionary<ConversionActionType, string> ActionTypeNames = new Dictionary<ConversionActionType, string>
        {
            {ConversionActionType.MobileAppInstall, "mobile_app_install"},
            {ConversionActionType.Purchase, "offsite_conversion.fb_pixel_purchase"},
            {ConversionActionType.Registration, "offsite_conversion.fb_pixel_complete_registration"},
            {ConversionActionType.VideoPlay, "video_play"},
            {ConversionActionType.Default, "offsite_conversion"},
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
            SetupConfigurableValues();
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
            return GetFBSummariesLoop(accountId, start, end, converter, byCampaign: true, byAdSet: true, byAd: true, getArchived: true);
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
            var clientParmsList = new List<FacebookJobRequest>();
            while (start <= end)
            {
                var tempEnd = start.AddDays(daysPerCall - 1);
                if (tempEnd > end)
                    tempEnd = end;

                var clientParms = PrepareSummaryExtractingRequest(accountId, start, tempEnd, byCampaign: byCampaign, byAdSet: byAdSet, byAd: byAd);
                clientParms.ResetAndGetRunId_withRetry(LogInfo, LogError);
                clientParmsList.Add(clientParms);
                if (getArchived)
                {
                    var clientParmsArchived = PrepareSummaryExtractingRequest(accountId, start, tempEnd, byCampaign: byCampaign, byAdSet: byAdSet, byAd: byAd, getArchived: true);
                    clientParmsArchived.ResetAndGetRunId_withRetry(LogInfo, LogError);
                    clientParmsList.Add(clientParmsArchived);
                }

                start = start.AddDays(daysPerCall);
            }
            Thread.Sleep(InitialWaitMillisecs);
            foreach (var clientParms in clientParmsList)
            {
                var fbSummaries = ProcessJobRequest(clientParms, converter);
                foreach (var fbSum in fbSummaries)
                {
                    yield return fbSum;
                }
            }
        }

        /// <summary>
        /// Prepares the facebook job request.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="byCampaign">if set to <c>true</c> [by campaign].</param>
        /// <param name="byAdSet">if set to <c>true</c> [by ad set].</param>
        /// <param name="byAd">if set to <c>true</c> [by ad].</param>
        /// <param name="getArchived">if set to <c>true</c> [get archived].</param>
        /// <returns>Async facebook insights data job request.</returns>
        private FacebookJobRequest PrepareSummaryExtractingRequest(string accountId, DateTime start, DateTime end, bool byCampaign = false, bool byAdSet = false, bool byAd = false, bool getArchived = false)
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
                filterList.Add(new Filter { field = $"{levelVal}.effective_status", @operator = "IN", value = new[] { EffectiveStatuses.Archived } });
            }
            var parameters = new
            {
                filtering = filterList.ToArray(),
                level = levelVal,
                fields = fieldsVal,
                action_breakdowns = "action_type",
                action_attribution_windows = clickAttribution + "," + viewAttribution,
                time_range = new { since = FacebookRequestUtils.GetDateString(start), until = FacebookRequestUtils.GetDateString(end) },
                time_increment = 1,
            };

            string by = byCampaign ? " by Campaign" : "";
            by += byAdSet ? " by AdSet" : "";
            by += byAd ? " by Ad" : "";
            string logMessage = string.Format("GetFBSummaries {0:d} - {1:d} ({2}{3})", start, end, accountId, by);

            return new FacebookJobRequest
            {
                fbClient = CreateFBClient(),
                path = accountId + "/insights",
                parms = parameters,
                logMessage = logMessage,
            };
        }

        /// <summary>
        /// Processes the job request.
        /// </summary>
        /// <param name="asyncJobRequest">Prepared job request data.</param>
        /// <param name="converter">The converter.</param>
        /// <returns>Enumerable collection of FbSummaries from asynchronous job.</returns>
        /// <exception cref="System.Exception">
        /// Job failed. Execution will be requested again.
        /// </exception>
        private IEnumerable<FBSummary> ProcessJobRequest(FacebookJobRequest asyncJobRequest, FacebookSummaryConverter converter)
        {
            LogInfo(asyncJobRequest.logMessage);
            var initialRunId = asyncJobRequest.GetRunId();
            var finalRunId = WaitAsyncJobRequestCompletionWithRetries(asyncJobRequest);
            return ReadJobRequestResults(asyncJobRequest, finalRunId, converter);
        }

        private IEnumerable<FBSummary> ReadJobRequestResults(FacebookJobRequest asyncJobRequest, string runId, FacebookSummaryConverter converter)
        {
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
                        retObj = asyncJobRequest.fbClient.Get(runId + "/insights", new { after = afterVal }); // get the actual data
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
            }
            while (moreData);
        }

        private string WaitAsyncJobRequestCompletionWithRetries(FacebookJobRequest asyncJobRequest)
        {
            Policy
                .Handle<Exception>()
                .WaitAndRetry(asyncJobFailureRetriesNumber, GetPauseBetweenFailureAttempts, (exception, timeSpan, retryCount, context) =>
                {
                    asyncJobRequest.ResetAndGetRunId_withRetry(LogInfo, LogError);
                    LogInfo($"Waiting job request completion failed. Trying again.");
                })
                .Execute(() => WaitForAsyncJobRequestCompletion(asyncJobRequest));
            return asyncJobRequest.GetRunId();
        }

        private void WaitForAsyncJobRequestCompletion(FacebookJobRequest asyncJobRequest)
        {
            var currentRunId = asyncJobRequest.GetRunId();
            dynamic retObj = asyncJobRequest.fbClient.Get(currentRunId); // check to see if job complete
            int numCalls = 0;
            int waitMillisecs = InitialWaitMillisecs;
            while (retObj.async_status != AsyncJobStatuses.Completed || retObj.async_percent_completion < 100)
            {
                numCalls++;
                LogInfo($"{numCalls} call(s) to check if job completed. {waitMillisecs} before another retry");
                if (numCalls > asyncJobWaitingRetriesNumber)
                {
                    throw new Exception($"Waited {numCalls} call(s) to check if job completed. Waiting time exceeded.");
                }
                if (retObj.async_status == AsyncJobStatuses.Failed)
                {
                    throw new Exception("Asynch job has failed status. Need retry.");
                }
                waitMillisecs += 500;
                Thread.Sleep(waitMillisecs);
                retObj = asyncJobRequest.fbClient.Get(currentRunId);
            }
        }

        private TimeSpan GetPauseBetweenFailureAttempts(int attemptNumber)
        {
            const int pauseBetweenFailureAttemptsInSeconds = 3;
            return new TimeSpan(0, 0, 0, pauseBetweenFailureAttemptsInSeconds);
        }

        private void SetupConfigurableValues()
        {
            asyncJobWaitingRetriesNumber = ConfigurationUtils.GetDefaultOrConfigIntValue("FB_AsyncJobWaitingRetriesNumber", asyncJobWaitingRetriesNumber);
            asyncJobFailureRetriesNumber = ConfigurationUtils.GetDefaultOrConfigIntValue("FB_AsyncJobFailureRetriesNumber", asyncJobFailureRetriesNumber);
        }
    }
}
