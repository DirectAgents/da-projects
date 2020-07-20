using System;
using System.Collections.Generic;
using System.Threading;
using FacebookAPI.Api;
using FacebookAPI.Converters;
using FacebookAPI.Entities;
using FacebookAPI.Helpers;

namespace FacebookAPI.Providers
{
    /// <inheritdoc />
    /// <summary>
    /// Facebook insights data provider for Reach metrics.
    /// </summary>
    public class FacebookInsightsReachMetricProvider : FacebookInsightsDataProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookInsightsReachMetricProvider"/> class.
        /// </summary>
        /// <param name="logInfo">Action for logging (the info level).</param>
        /// <param name="logWarn">Action for logging (the warn level).</param>
        public FacebookInsightsReachMetricProvider(Action<string> logInfo, Action<string> logWarn)
            : base(logInfo, logWarn)
        {
        }

        /// <summary>
        /// Gets list of Reach metrics from API.
        /// </summary>
        /// <param name="accountId">External identifier of account.</param>
        /// <returns>List of Reach metrics.</returns>
        public IEnumerable<FbReachRow> GetReachMetricStats(string accountId)
        {
            var converter = new FacebookReachMetricConverter(LogWarn);
            return GetReachMetricsLoop(accountId, converter);
        }

        /// <summary>
        /// Gets list of Campaign Reach metrics from API.
        /// </summary>
        /// <param name="accountId">External identifier of account.</param>
        /// <returns>List of Reach metrics.</returns>
        public IEnumerable<FbCampaignReachRow> GetCampaignReachMetricStats(string accountId)
        {
            var converter = new FacebookCampaignReachMetricConverter(LogWarn);
            return GetCampaignReachMetricsLoop(accountId, converter);
        }

        private IEnumerable<FbReachRow> GetReachMetricsLoop(string accountId, FacebookReachMetricConverter converter)
        {
            var parameters = CreateRequestParameters();
            foreach (var fbCampaignReachRow in GetMetricsLoop(converter, parameters, accountId))
            {
                yield return fbCampaignReachRow;
            }
        }

        private IEnumerable<FbCampaignReachRow> GetCampaignReachMetricsLoop(string accountId, FacebookCampaignReachMetricConverter converter)
        {
            var parameters = CreateCampaignReachRequestParameters();
            foreach (var fbCampaignReachRow in GetMetricsLoop(converter, parameters, accountId))
            {
                yield return fbCampaignReachRow;
            }
        }

        private IEnumerable<T> GetMetricsLoop<T>(IFacebookConverter<T> converter, object parameters, string accountId)
        {
            var clientParametersList = new List<FacebookJobRequest>();
            var clientParameters = PrepareExtractingRequestBase(parameters, accountId);
            clientParameters.ResetAndGetRunId_withRetry(LogInfo, LogWarn);
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


        private FacebookJobRequest PrepareExtractingRequestBase(object parameters, string accountId)
        {
            var facebookClient = CreateFBClient();
            var path = accountId + "/insights";
            var logMessage = $"Get FB Reach metrics ({accountId})";
            return CreateFacebookJobRequest(facebookClient, path, parameters, logMessage);
        }

        private object CreateRequestParameters()
        {
            const string fieldsVal = "reach";
            var filters = GetRequestFilters();
            var timeRanges = FacebookReachPeriodHelper.GetPeriodsForApiRequest();
            return new
            {
                filtering = filters,
                fields = fieldsVal,
                time_ranges = timeRanges,
            };
        }

        private object CreateCampaignReachRequestParameters()
        {
            var levelVal = "campaign";
            var fieldsVal = "campaign_id,campaign_name,reach,frequency";
            var timeRanges = FacebookReachPeriodHelper.GetMonthlyPeriodsForApiRequest();
            return new
            {
                level = levelVal,
                fields = fieldsVal,
                time_ranges = timeRanges,
            };
        }

        private Filter[] GetRequestFilters()
        {
            var filterList = new List<Filter>();
            if (!string.IsNullOrWhiteSpace(CurrentPlatformFilter))
            {
                filterList.Add(new Filter { field = "publisher_platform", @operator = "IN", value = new[] { CurrentPlatformFilter } });
            }
            if (!string.IsNullOrEmpty(CampaignFilterValue))
            {
                filterList.Add(new Filter { field = "campaign.name", @operator = CampaignFilterOperator, value = CampaignFilterValue });
            }
            return filterList.ToArray();
        }
    }
}
