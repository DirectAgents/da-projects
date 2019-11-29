using System;
using System.Collections.Generic;
using System.Threading;
using FacebookAPI.Api;
using FacebookAPI.Converters;
using FacebookAPI.Entities;
using FacebookAPI.Helpers;

namespace FacebookAPI.Providers
{
    public class FacebookInsightsReachMetricProvider : FacebookInsightsDataProvider
    {
        public FacebookInsightsReachMetricProvider(Action<string> logInfo, Action<string> logError)
            : base(logInfo, logError)
        {
        }

        public IEnumerable<FbReachRow> GetReachMetricStats(string accountId, DateTime start, DateTime end)
        {
            var converter = new FacebookReachMetricConverter();
            return GetReachMetricsLoop(accountId, start, end, converter);
        }

        private IEnumerable<FbReachRow> GetReachMetricsLoop(
            string accountId, DateTime start, DateTime end, FacebookReachMetricConverter converter)
        {
            var clientParametersList = new List<FacebookJobRequest>();
            var clientParameters = PrepareReachMetricExtractingRequest(accountId, start, end);
            clientParameters.ResetAndGetRunId_withRetry(LogInfo, LogError);
            clientParametersList.Add(clientParameters);

            Thread.Sleep(InitialWaitMillisecs);
            foreach (var clientParms in clientParametersList)
            {
                var fbReachMetrics = ProcessJobRequest(clientParms, converter);
                foreach (var fbReachMetric in fbReachMetrics)
                {
                    yield return fbReachMetric;
                }
            }
        }

        private FacebookJobRequest PrepareReachMetricExtractingRequest(string accountId, DateTime start, DateTime end)
        {
            const string fieldsVal = "reach";
            var filterList = new List<Filter>();
            if (!string.IsNullOrWhiteSpace(CurrentPlatformFilter))
            {
                filterList.Add(new Filter { field = "publisher_platform", @operator = "IN", value = new[] { CurrentPlatformFilter } });
            }
            if (!string.IsNullOrEmpty(CampaignFilterValue))
            {
                filterList.Add(new Filter { field = "campaign.name", @operator = CampaignFilterOperator, value = CampaignFilterValue });
            }
            var timeRanges = FacebookReachPeriodHelper.GetPeriodsForRequest();
            var parameters = new
            {
                filtering = filterList.ToArray(),
                fields = fieldsVal,
                time_ranges = timeRanges,
            };
            var logMessage = $"Get FB Reach metrics {start:d} - {end:d} ({accountId})";
            var facebookClient = CreateFBClient();
            var path = accountId + "/insights";
            return CreateFacebookJobRequest(facebookClient, path, parameters, logMessage);
        }

        private IEnumerable<FbReachRow> ProcessJobRequest(
            FacebookJobRequest asyncJobRequest, FacebookReachMetricConverter converter)
        {
            LogInfo(asyncJobRequest.logMessage);
            var initialRunId = asyncJobRequest.GetRunId();
            var finalRunId = WaitAsyncJobRequestCompletionWithRetries(asyncJobRequest);
            return ReadJobRequestReachMetricResults(asyncJobRequest, finalRunId, converter);
        }

        private IEnumerable<FbReachRow> ReadJobRequestReachMetricResults(
            FacebookJobRequest asyncJobRequest, string runId, FacebookReachMetricConverter converter)
        {
            return ReadJobRequestResults(asyncJobRequest, runId, converter);
        }
    }
}
