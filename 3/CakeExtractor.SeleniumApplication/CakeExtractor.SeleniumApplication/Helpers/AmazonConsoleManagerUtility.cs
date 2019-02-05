using CakeExtracter.Common;
using CakeExtractor.SeleniumApplication.Helpers;
using CakeExtractor.SeleniumApplication.Models.ConsoleManagerUtilityModels;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CakeExtractor.SeleniumApplication.Utilities
{
    internal class AmazonConsoleManagerUtility
    {
        private const int PageSize = 100;

        private readonly Dictionary<string, string> cookies;
        private readonly Action<string> logInfo;
        private readonly Action<string> logError;

        public AmazonConsoleManagerUtility(IEnumerable<Cookie> cookies, Action<string> logInfo, Action<string> logError)
        {
            var simpleCookies = cookies.ToDictionary(x => x.Name, x => x.Value);
            this.cookies = simpleCookies;
            this.logInfo = logInfo;
            this.logError = logError;
        }

        public IEnumerable<AmazonCmApiCampaignSummary> GetPdaCampaignsTruncatedSummaries(string accountEntityId, DateRange dateRange)
        {
            var queryParams = AmazonCmApiHelper.GetCampaignsApiQueryParams(accountEntityId);
            var parameters = AmazonCmApiHelper.GetBasePdaCampaignsApiParams(false);
            var resultData = GetCampaignsSummaries(dateRange, queryParams, parameters);
            return resultData;
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

        private IEnumerable<AmazonCmApiCampaignSummary> GetCampaignsSummaries(DateRange dateRange,
            Dictionary<string, string> queryParams, AmazonCmApiParams parameters)
        {
            var resultData = new List<AmazonCmApiCampaignSummary>();
            foreach (var date in dateRange.Dates)
            {
                var data = GetCampaignsSummaries(date, queryParams, parameters);
                resultData.AddRange(data);
            }

            return resultData;
        }

        private IEnumerable<AmazonCmApiCampaignSummary> GetCampaignsSummaries(DateTime date,
            Dictionary<string, string> queryParams, AmazonCmApiParams parameters)
        {
            LogInfo($"Retrieve campaigns info from API for {date} date.");
            AmazonCmApiHelper.SetCampaignApiSpecificInitParams(parameters, date, PageSize);
            var data = new List<AmazonCmApiCampaignSummary>();
            try
            {
                data = GetPdaCampaignsSummaries(queryParams, parameters);
                data.ForEach(x => x.Date = date);
            }
            catch (Exception e)
            {
                LogError(e.Message);
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
            var request = RestRequestHelper.CreateRestRequest(AmazonCmApiHelper.CampaignsApiRelativePath, cookies, queryParams, body);
            var response = RestRequestHelper.SendPostRequest<dynamic>(AmazonCmApiHelper.AmazonAdvertisingPortalUrl, request);
            return response.Data;
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
    }
}
