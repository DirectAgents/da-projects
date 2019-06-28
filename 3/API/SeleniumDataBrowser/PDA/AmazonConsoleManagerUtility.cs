using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using SeleniumDataBrowser.Helpers;
using SeleniumDataBrowser.Models;
using SeleniumDataBrowser.PDA.Helpers;
using SeleniumDataBrowser.PDA.Models;
using SeleniumDataBrowser.PDA.Exceptions;
using Polly;
using RestSharp;
using Cookie = OpenQA.Selenium.Cookie;

namespace SeleniumDataBrowser.PDA
{
    /// <summary>
    /// Product Display Ads Utility for sent requests to the portal backend.
    /// </summary>
    public class AmazonConsoleManagerUtility
    {
        private const int PageSize = 100;

        private readonly string accountName;
        private readonly AuthorizationModel authorizationModel;
        private readonly Dictionary<string, string> availableProfileUrls;
        private readonly SeleniumLogger logger;
        private readonly int maxRetryAttempts;
        private readonly TimeSpan pauseBetweenAttempts;

        private Dictionary<string, string> cookies;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonConsoleManagerUtility"/> class.
        /// </summary>
        /// <param name="accountName">Name of current account.</param>
        /// <param name="authorizationModel">Authorization settings.</param>
        /// <param name="availableProfileUrls">URLs of available campaign profiles.</param>
        /// <param name="maxRetryAttempts">Max number of retry attempts.</param>
        /// <param name="pauseBetweenAttempts">Time interval for pause between attempts.</param>
        /// <param name="logger">Selenium logger.</param>
        public AmazonConsoleManagerUtility(
            string accountName,
            AuthorizationModel authorizationModel,
            Dictionary<string, string> availableProfileUrls,
            int maxRetryAttempts,
            TimeSpan pauseBetweenAttempts,
            SeleniumLogger logger)
        {
            this.accountName = accountName;
            this.authorizationModel = authorizationModel;
            this.availableProfileUrls = availableProfileUrls;
            this.maxRetryAttempts = maxRetryAttempts;
            this.pauseBetweenAttempts = pauseBetweenAttempts;
            this.logger = logger;
        }

        /// <summary>
        /// Returns summaries of PDA campaigns for specified dates.
        /// </summary>
        /// <param name="extractionDates">Dates for which summaries will be extracted.</param>
        /// <returns>Summaries of PDA campaigns.</returns>
        public IEnumerable<AmazonCmApiCampaignSummary> GetPdaCampaignsSummaries(IEnumerable<DateTime> extractionDates)
        {
            var accountEntityId = GetCurrentProfileEntityId();
            var queryParams = AmazonCmApiHelper.GetCampaignsApiQueryParams(accountEntityId);
            var parameters = AmazonCmApiHelper.GetBasePdaCampaignsApiParams(true);
            var apiCampaignSummaries = GetCampaignsSummariesForAllDates(extractionDates, queryParams, parameters);
            return apiCampaignSummaries;
        }

        /// <summary>
        /// Sets the specified collection of cookies for the PDA utility.
        /// </summary>
        /// <param name="collectionOfCookies">Collection of cookies.</param>
        public void SetCookiesForUtility(IEnumerable<Cookie> collectionOfCookies)
        {
            cookies = collectionOfCookies.ToDictionary(x => x.Name, x => x.Value);
        }

        private static void AddDynamicCampaignDataToResultData(List<AmazonCmApiCampaignSummary> resultData, dynamic data)
        {
            var campaignsData = AmazonCmApiHelper.GetDynamicCampaigns(data);
            foreach (var campaignData in campaignsData)
            {
                var convertedData = AmazonCmApiHelper.ConvertDynamicCampaignInfoToModel(campaignData);
                resultData.Add(convertedData);
            }
        }

        private static string GetProfileEntityId(string url)
        {
            var uri = new Uri(url);
            var queryParams = HttpUtility.ParseQueryString(uri.Query);
            var entityId = queryParams.Get(AmazonCmApiHelper.EntityIdArgName);
            return entityId;
        }

        private string GetCurrentProfileEntityId()
        {
            var profileUrl = GetAvailableProfileUrl();
            var accountEntityId = GetProfileEntityId(profileUrl);
            return accountEntityId;
        }

        private string GetAvailableProfileUrl()
        {
            var profileName = accountName.Trim();
            var availableProfileUrl = availableProfileUrls.FirstOrDefault(x =>
                string.Equals(x.Key, profileName, StringComparison.OrdinalIgnoreCase));
            var profileUrl = availableProfileUrl.Value;
            if (string.IsNullOrEmpty(profileUrl))
            {
                // The current account does not have the following profile
                throw new AccountDoesNotHaveProfileException(authorizationModel.Login, accountName);
            }
            return profileUrl;
        }

        private IEnumerable<AmazonCmApiCampaignSummary> GetCampaignsSummariesForAllDates(
            IEnumerable<DateTime> extractionDates,
            Dictionary<string, string> queryParams,
            AmazonCmApiParams parameters)
        {
            var resultData = new List<AmazonCmApiCampaignSummary>();
            foreach (var date in extractionDates)
            {
                try
                {
                    WaitBeforeRequest(date);
                    var data = GetCampaignsSummariesForDate(date, queryParams, parameters);
                    resultData.AddRange(data);
                }
                catch (Exception exception)
                {
                    logger.LogError(exception);
                }
            }
            return resultData;
        }

        private IEnumerable<AmazonCmApiCampaignSummary> GetCampaignsSummariesForDate(
            DateTime date, Dictionary<string, string> queryParams, AmazonCmApiParams parameters)
        {
            logger.LogInfo($"Retrieve campaigns info from API for {date.ToShortDateString()} date.");
            AmazonCmApiHelper.SetCampaignApiSpecificInitParams(parameters, date, PageSize);
            try
            {
                var data = GetApiPdaCampaignsSummaries(queryParams, parameters);
                data.ForEach(x => x.Date = date);
                return data;
            }
            catch (Exception e)
            {
                throw new Exception($"Could not get campaign summaries for {date}", e);
            }
        }

        private List<AmazonCmApiCampaignSummary> GetApiPdaCampaignsSummaries(
            Dictionary<string, string> queryParams, AmazonCmApiParams parameters)
        {
            var resultData = new List<AmazonCmApiCampaignSummary>();
            var data = TryProcessRequest(queryParams, parameters);
            AddDynamicCampaignDataToResultData(resultData, data);
            var numberOfRecords = AmazonCmApiHelper.GetNumberOfRecords(data);
            for (parameters.pageOffset = 1; numberOfRecords > parameters.pageSize * parameters.pageOffset; parameters.pageOffset++)
            {
                data = TryProcessRequest(queryParams, parameters);
                AddDynamicCampaignDataToResultData(resultData, data);
            }
            return resultData;
        }

        private dynamic TryProcessRequest(Dictionary<string, string> queryParams, AmazonCmApiParams body)
        {
            var response = Policy
                .Handle<Exception>()
                .OrResult<IRestResponse<dynamic>>(resp => resp.StatusCode != HttpStatusCode.OK)
                .WaitAndRetry(
                    maxRetryAttempts,
                    i => pauseBetweenAttempts,
                    (exception, timeSpan, retryCount, context) =>
                        logger.LogWaiting("Failed to process request. Waiting {0} ...", timeSpan, retryCount))
                .Execute(() => ProcessRequest<dynamic>(queryParams, body));
            return response.Data;
        }

        private IRestResponse<T> ProcessRequest<T>(Dictionary<string, string> queryParams, AmazonCmApiParams body)
            where T : new()
        {
            var request = RestRequestHelper.CreateRestRequest(AmazonCmApiHelper.CampaignsApiRelativePath, cookies, queryParams, body);
            var response = RestRequestHelper.SendPostRequest<T>(AmazonCmApiHelper.AmazonAdvertisingPortalUrl, request);
            if (response.IsSuccessful)
            {
                return response;
            }
            var message = string.IsNullOrWhiteSpace(response.Content)
                ? response.ErrorMessage
                : response.Content;
            logger.LogWarning(message);
            return response;
        }

        private void WaitBeforeRequest(DateTime date)
        {
            logger.LogWaiting(
                $"Requesting a campaign info for {date.ToShortDateString()}. " + "Waiting {0} ...", pauseBetweenAttempts, null);
            Thread.Sleep(pauseBetweenAttempts);
        }
    }
}
