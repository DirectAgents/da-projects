﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using CakeExtracter.Common;
using CakeExtracter.Etl.AmazonSelenium.Helpers;
using CakeExtracter.Etl.AmazonSelenium.PDA.Helpers;
using CakeExtracter.Etl.AmazonSelenium.PDA.Models.ConsoleManagerUtilityModels;
using Polly;
using RestSharp;
using Cookie = OpenQA.Selenium.Cookie;

namespace CakeExtracter.Etl.AmazonSelenium.PDA.Utilities
{
    internal class AmazonConsoleManagerUtility
    {
        private const int PageSize = 100;

        private readonly Dictionary<string, string> cookies;
        private readonly Action<string> logInfo;
        private readonly Action<string> logError;
        private readonly Action<string> logWarning;
        private readonly int maxRetryAttempts;
        private readonly TimeSpan pauseBetweenAttempts;

        public AmazonConsoleManagerUtility(
            IEnumerable<Cookie> cookies,
            int maxRetryAttempts,
            TimeSpan pauseBetweenAttempts,
            Action<string> logInfo,
            Action<string> logError,
            Action<string> logWarning)
        {
            var simpleCookies = cookies.ToDictionary(x => x.Name, x => x.Value);
            this.cookies = simpleCookies;
            this.maxRetryAttempts = maxRetryAttempts;
            this.pauseBetweenAttempts = pauseBetweenAttempts;
            this.logInfo = logInfo;
            this.logError = logError;
            this.logWarning = logWarning;
        }

        public IEnumerable<AmazonCmApiCampaignSummary> GetPdaCampaignsSummaries(string accountEntityId, DateRange dateRange)
        {
            var queryParams = AmazonCmApiHelper.GetCampaignsApiQueryParams(accountEntityId);
            var parameters = AmazonCmApiHelper.GetBasePdaCampaignsApiParams(true);
            var resultData = GetCampaignsSummaries(dateRange, queryParams, parameters);
            return resultData;
        }

        private static void Log(string message, Action<string> logAction)
        {
            var updatedMessage = $"[AmazonConsoleManagerUtility]: {message}";
            logAction(updatedMessage);
        }

        private void LogError(string message)
        {
            Log(message, logError);
        }

        private void LogInfo(string message)
        {
            Log(message, logInfo);
        }

        private void LogWarning(string message)
        {
            Log(message, logWarning);
        }

        private IEnumerable<AmazonCmApiCampaignSummary> GetCampaignsSummaries(DateRange dateRange,
            Dictionary<string, string> queryParams, AmazonCmApiParams parameters)
        {
            var resultData = new List<AmazonCmApiCampaignSummary>();
            foreach (var date in dateRange.Dates)
            {
                try
                {
                    WaitBeforeRequest(date);
                    var data = GetCampaignsSummaries(date, queryParams, parameters);
                    resultData.AddRange(data);
                }
                catch (Exception e)
                {
                    LogError($"Failed: {e.Message}");
                }
            }

            return resultData;
        }

        private IEnumerable<AmazonCmApiCampaignSummary> GetCampaignsSummaries(DateTime date,
            Dictionary<string, string> queryParams, AmazonCmApiParams parameters)
        {
            LogInfo($"Retrieve campaigns info from API for {date.ToShortDateString()} date.");
            AmazonCmApiHelper.SetCampaignApiSpecificInitParams(parameters, date, PageSize);
            var data = new List<AmazonCmApiCampaignSummary>();
            try
            {
                data = GetPdaCampaignsSummaries(queryParams, parameters);
                data.ForEach(x => x.Date = date);
            }
            catch (Exception e)
            {
                throw new Exception($"Could not get campaign summaries for {date}", e);
            }
            return data;
        }

        private List<AmazonCmApiCampaignSummary> GetPdaCampaignsSummaries(Dictionary<string, string> queryParams, AmazonCmApiParams parameters)
        {
            var resultData = new List<AmazonCmApiCampaignSummary>();
            var data = GetCampaignSummaries(queryParams, parameters);
            AddDynamicCampaignDataToResultData(resultData, data);
            var numberOfRecords = AmazonCmApiHelper.GetNumberOfRecords(data);
            for (parameters.pageOffset = 1; numberOfRecords > parameters.pageSize * parameters.pageOffset; parameters.pageOffset++)
            {
                data = GetCampaignSummaries(queryParams, parameters);
                AddDynamicCampaignDataToResultData(resultData, data);
            }

            return resultData;
        }

        private dynamic GetCampaignSummaries(Dictionary<string, string> queryParams, AmazonCmApiParams body)
        {
            var response = Policy
                .Handle<Exception>()
                .OrResult<IRestResponse<dynamic>>(resp => resp.StatusCode != HttpStatusCode.OK)
                .WaitAndRetry(
                    maxRetryAttempts,
                    i => pauseBetweenAttempts,
                    (exception, timeSpan, retryCount, context) =>
                    {
                        var message = $"Failed to process request. Waiting {timeSpan}";
                        LoggerHelper.LogWaiting(message, retryCount, logInfo);
                    })
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
            LogWarning(message);
            return response;
        }

        private void AddDynamicCampaignDataToResultData(List<AmazonCmApiCampaignSummary> resultData, dynamic data)
        {
            var campaignsData = AmazonCmApiHelper.GetDynamicCampaigns(data);
            foreach (var campaignData in campaignsData)
            {
                var convertedData = AmazonCmApiHelper.ConvertDynamicCampaignInfoToModel(campaignData);
                resultData.Add(convertedData);
            }
        }

        private void WaitBeforeRequest(DateTime date)
        {
            var timeSpan = pauseBetweenAttempts;
            var message = $"Waiting {timeSpan} before requesting a campaign info (for {date.ToShortDateString()})";
            LoggerHelper.LogWaiting(message, null, logInfo);
            Thread.Sleep(timeSpan);
        }
    }
}
