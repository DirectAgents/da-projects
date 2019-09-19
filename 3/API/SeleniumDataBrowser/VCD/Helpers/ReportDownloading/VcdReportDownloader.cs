﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Newtonsoft.Json;
using Polly;
using RestSharp;
using SeleniumDataBrowser.Helpers;
using SeleniumDataBrowser.Models;
using SeleniumDataBrowser.VCD.Helpers.ReportDownloading.Constants;
using SeleniumDataBrowser.VCD.Helpers.ReportDownloading.Models;
using SeleniumDataBrowser.VCD.Models;
using SeleniumDataBrowser.VCD.PageActions;
using Cookie = OpenQA.Selenium.Cookie;

namespace SeleniumDataBrowser.VCD.Helpers.ReportDownloading
{
    /// <summary>
    /// Downloader of Vendor Central reports.
    /// </summary>
    public class VcdReportDownloader
    {
        private const string AmazonBaseUrl = "https://vendorcentral.amazon.com";
        private const string AmazonCsvDownloadReportUrl = "/analytics/data/dashboard/salesDiagnostic/report/salesDiagnosticDetail";

        private static int delayEqualizer;

        private readonly int reportDownloadingStartedDelayInSeconds;
        private readonly int minDelayBetweenReportDownloadingInSeconds;
        private readonly int maxDelayBetweenReportDownloadingInSeconds;
        private readonly int reportDownloadingAttemptCount;
        private readonly AmazonVcdActionsWithPagesManager pageActions;
        private readonly AuthorizationModel authorizationModel;
        private readonly VcdAccountInfo accountInfo;
        private readonly SeleniumLogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="VcdReportDownloader"/> class.
        /// </summary>
        /// <param name="accountInfo">Information about the current account.</param>
        /// <param name="pageActions">Manager of page actions.</param>
        /// <param name="authorizationModel">Authorization settings.</param>
        /// <param name="logger">Selenium data browser logger.</param>
        /// <param name="reportDownloadingStartedDelayInSeconds">Number of seconds delay before first attempt downloading reports.</param>
        /// <param name="minDelayBetweenReportDownloadingInSeconds">Number of seconds minimum delay between attempts downloading reports.</param>
        /// <param name="maxDelayBetweenReportDownloadingInSeconds">Number of seconds maximum delay between attempts downloading reports.</param>
        /// <param name="reportDownloadingAttemptCount">Count of attempts downloading reports.</param>
        public VcdReportDownloader(
            VcdAccountInfo accountInfo,
            AmazonVcdActionsWithPagesManager pageActions,
            AuthorizationModel authorizationModel,
            SeleniumLogger logger,
            int reportDownloadingStartedDelayInSeconds,
            int minDelayBetweenReportDownloadingInSeconds,
            int maxDelayBetweenReportDownloadingInSeconds,
            int reportDownloadingAttemptCount)
        {
            this.accountInfo = accountInfo;
            this.pageActions = pageActions;
            this.authorizationModel = authorizationModel;
            this.logger = logger;
            this.reportDownloadingStartedDelayInSeconds = reportDownloadingStartedDelayInSeconds;
            this.minDelayBetweenReportDownloadingInSeconds = minDelayBetweenReportDownloadingInSeconds;
            this.maxDelayBetweenReportDownloadingInSeconds = maxDelayBetweenReportDownloadingInSeconds;
            this.reportDownloadingAttemptCount = reportDownloadingAttemptCount;
        }

        /// <summary>
        /// Downloads the CSV Shipped Revenue report.
        /// </summary>
        /// <param name="reportDay">Day for report.</param>
        /// <returns>Text of report content.</returns>
        public string DownloadShippedRevenueCsvReport(DateTime reportDay)
        {
            logger.LogInfo("Amazon VCD, Attempt to download shipped revenue report.");
            return DownloadCsvReportFromBackendApi(
                reportDay, RequestBodyConstants.ShippedRevenueReportLevel, RequestBodyConstants.ShippedRevenueColumnId);
        }

        /// <summary>
        /// Downloads the CSV Shipped COGS report.
        /// </summary>
        /// <param name="reportDay">Day for report.</param>
        /// <returns>Text of report content.</returns>
        public string DownloadShippedCogsCsvReport(DateTime reportDay)
        {
            logger.LogInfo("Amazon VCD, Attempt to download shipped cogs report.");
            return DownloadCsvReportFromBackendApi(
                reportDay, RequestBodyConstants.ShippedCogsLevel, RequestBodyConstants.ShippedCogsColumnId);
        }

        /// <summary>
        /// Downloads the CSV Ordered Revenue report.
        /// </summary>
        /// <param name="reportDay">Day for report.</param>
        /// <returns>Text of report content.</returns>
        public string DownloadOrderedRevenueCsvReport(DateTime reportDay)
        {
            logger.LogInfo("Amazon VCD, Attempt to download ordered revenue report.");
            return DownloadCsvReportFromBackendApi(
                reportDay, RequestBodyConstants.OrderedRevenueLevel, RequestBodyConstants.OrderedRevenueColumnId);
        }

        private string DownloadCsvReportFromBackendApi(DateTime reportDay, string reportLevel, string reportId)
        {
            try
            {
                var firstPartOfReportData = TryProcessRequest(reportDay, reportLevel, reportId);
                var csvReportContent = new StringBuilder();
                SetCsvReportHeader(csvReportContent, firstPartOfReportData);
                SetCsvReportRows(reportDay, reportLevel, reportId, csvReportContent, firstPartOfReportData);
                return csvReportContent.ToString();
            }
            catch (Exception)
            {
                return "";
            }
        }

        private void SetCsvReportHeader(StringBuilder csvReportContent, dynamic firstPartOfReportData)
        {
            var reportHeader = VcdComposingOfReportHelper.GetLineOfReportColumns(firstPartOfReportData);
            csvReportContent.AppendLine(reportHeader.ToString());
        }

        private void SetCsvReportRows(
            DateTime reportDay,
            string reportLevel,
            string reportId,
            StringBuilder csvReportContent,
            dynamic firstPartOfReportData)
        {
            var productsRows = GetReportRows(reportDay, reportLevel, reportId, firstPartOfReportData);
            foreach (var productRow in productsRows)
            {
                var newReportLine = VcdComposingOfReportHelper.CreateRowLineOfReport(productRow);
                csvReportContent.AppendLine(newReportLine.ToString());
            }
        }

        private IEnumerable<dynamic> GetReportRows(
            DateTime reportDay,
            string reportLevel,
            string reportId,
            dynamic firstPartOfReportData)
        {
            var firstPartOfProductsRows = VcdComposingOfReportHelper.GetReportProductsRows(firstPartOfReportData);
            var allProductsRows = JsonConvert.DeserializeObject<List<dynamic>>(firstPartOfProductsRows.ToString());
            var totalReportRowCount = VcdComposingOfReportHelper.GetTotalReportRowCount(firstPartOfReportData);
            var downloadedRowCount = firstPartOfProductsRows.Count;

            var currentPageIndex = 0;
            //while (downloadedRowCount < totalReportRowCount)
            //{
            //    currentPageIndex++;
            //    var nextPartOfReportData = TryProcessRequest(reportDay, reportLevel, reportId, currentPageIndex);
            //    var nextPartOfProductsRows = VcdComposingOfReportHelper.GetReportProductsRows(nextPartOfReportData);
            //    allProductsRows.AddRange(JsonConvert.DeserializeObject<List<dynamic>>(nextPartOfProductsRows.ToString()));
            //    downloadedRowCount += nextPartOfProductsRows.Count;
            //}
            return allProductsRows;
        }

        private dynamic TryProcessRequest(DateTime reportDay, string reportLevel, string reportId, int pageIndex = 0)
        {
            var failed = false;
            WaitBeforeReportGenerating(reportDay, reportLevel);
            var response = Policy
                .Handle<Exception>()
                .OrResult<IRestResponse<dynamic>>(resp => !IsSuccessfulResponse(resp))
                .WaitAndRetry(
                    reportDownloadingAttemptCount,
                    retryCount => GetTimeSpanForWaiting(),
                    (exception, timeSpan, retryCount, context) =>
                    {
                        failed = true;
                        ProcessFailedResponse(exception.Result);
                        logger.LogWaiting(
                            $"Report generating for {reportDay.ToShortDateString()}, {reportLevel}, {accountInfo.AccountName}. "
                            + "Waiting {0} ...",
                            timeSpan,
                            retryCount);
                    })
                .Execute(() =>
                {
                    var resp = ProcessRequest(reportDay, reportLevel, reportId, pageIndex);
                    EqualizeDelay(IsSuccessfulResponse(resp), failed);
                    return resp;
                });
            return ProcessResponse(response);
        }

        private IRestResponse<dynamic> ProcessRequest(DateTime reportDay, string reportLevel, string reportId, int pageIndex)
        {
            var request = GenerateDownloadingReportRequest(reportDay, reportLevel, reportId, pageIndex);
            var response = RestRequestHelper.SendPostRequest<dynamic>(AmazonBaseUrl, request);
            return response;
        }

        private void WaitBeforeReportGenerating(DateTime reportDay, string reportLevel)
        {
            var timeSpan = GetTimeSpanForWaiting();
            logger.LogWaiting(
                $"Report generating for {reportDay.ToShortDateString()}, {reportLevel}, {accountInfo.AccountName}. "
                + "Waiting {0} ...",
                timeSpan,
                null);
            Thread.Sleep(timeSpan);
        }

        private dynamic ProcessResponse(IRestResponse<dynamic> response)
        {
            if (IsSuccessfulResponse(response))
            {
                return ProcessSuccessfulResponse(response);
            }
            ProcessFailedResponse(response);
            throw new Exception($"Report was not downloaded successfully. Status code {response.StatusDescription}");
        }

        private dynamic ProcessSuccessfulResponse(IRestResponse<dynamic> response)
        {
            var data = response.Data;
            logger.LogInfo($"Amazon VCD, Report downloading finished successfully.");
            return data;
        }

        private void ProcessFailedResponse(IRestResponse<dynamic> response)
        {
            logger.LogWarning($"Report downloading attempt failed, Status code: {response.StatusDescription}, content: {response.Content}");
            if (response.StatusCode == (HttpStatusCode)429)
            {
                return;
            }
            pageActions.RefreshSalesDiagnosticPage(authorizationModel);
            logger.LogInfo("Amazon VCD, The portal page has been refreshed.");
        }

        private bool IsSuccessfulResponse(IRestResponse<dynamic> response)
        {
            return response.StatusCode == HttpStatusCode.OK;
        }

        private ReportDownloadingRequestPageData GetPageDataForReportRequest()
        {
            var token = pageActions.GetAccessToken();
            var cookies = GetCookies();
            return new ReportDownloadingRequestPageData
            {
                Token = token,
                Cookies = cookies,
            };
        }

        private IEnumerable<Cookie> GetCookies()
        {
            var cookies = pageActions.GetAllCookies();
            return cookies;
        }

        private RestRequest GenerateDownloadingReportRequest(
            DateTime reportDay, string reportLevel, string reportId, int pageIndex)
        {
            var pageRequestData = GetPageDataForReportRequest();
            var requestId = Guid.NewGuid().ToString();
            var requestBodyObject = PrepareRequestBody(requestId, reportDay, reportLevel, reportId, pageIndex);
            var requestHeaders = GetHeadersDictionary(requestId);
            var requestBodyJson = JsonConvert.SerializeObject(requestBodyObject);
            var requestQueryParams = RequestQueryConstants.GetRequestQueryParameters(
                pageRequestData.Token,
                accountInfo.VendorGroupId.ToString(),
                accountInfo.McId.ToString());
            var request = RestRequestHelper.CreateRestRequest(
                AmazonCsvDownloadReportUrl, pageRequestData.Cookies, requestQueryParams);
            request.AddParameter(RequestBodyConstants.RequestBodyFormat, requestBodyJson, ParameterType.RequestBody);
            requestHeaders.ForEach(x => request.AddHeader(x.Key, x.Value));
            return request;
        }

        private Dictionary<string, string> GetHeadersDictionary(string requestId)
        {
            var requestHeaders = RequestHeaderConstants.GetHeadersDictionary();
            requestHeaders[RequestHeaderConstants.RequestIdHeaderName] = requestId;
            return requestHeaders;
        }

        private SalesDiagnosticDetail PrepareRequestBody(
            string requestId, DateTime reportDay, string reportLevel, string reportId, int pageIndex)
        {
            var reportParameters = GetReportParameters(reportDay, reportLevel);
            var reportPaginationWithOrderParameter = GetReportPaginationWithOrderParameter(reportId, pageIndex);
            return new SalesDiagnosticDetail
            {
                requestId = requestId,
                reportParameters = reportParameters,
                reportPaginationWithOrderParameter = reportPaginationWithOrderParameter,
            };
        }

        private List<ReportParameter> GetReportParameters(DateTime reportDay, string reportLevel)
        {
            var reportParameters = RequestBodyConstants.GetReportParameters(
                GetReportParameterFilterDate(reportDay),
                GetReportParameterFilterDate(reportDay),
                reportLevel);
            return reportParameters;
        }

        private ReportPaginationWithOrderParameter GetReportPaginationWithOrderParameter(string reportId, int pageIndex)
        {
            var reportPaginationWithOrderParameter = RequestBodyConstants.GetReportPaginationWithOrderParameter(reportId, pageIndex);
            return reportPaginationWithOrderParameter;
        }

        private string GetReportParameterFilterDate(DateTime reportDate)
        {
            const string parameterDatePattern = "yyyy''MM''dd";
            return reportDate.ToString(parameterDatePattern);
        }

        private void EqualizeDelay(bool isSuccessful, bool wasFailed)
        {
            delayEqualizer = isSuccessful
                ? wasFailed
                    ? delayEqualizer
                    : delayEqualizer == 0
                        ? 0
                        : delayEqualizer - 1
                : delayEqualizer + 1;
        }

        private TimeSpan GetTimeSpanForWaiting()
        {
            var waitTime = Math.Min(
                maxDelayBetweenReportDownloadingInSeconds,
                minDelayBetweenReportDownloadingInSeconds + reportDownloadingStartedDelayInSeconds * delayEqualizer);
            return TimeSpan.FromSeconds(waitTime);
        }
    }
}
