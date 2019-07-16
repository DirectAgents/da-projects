using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

namespace SeleniumDataBrowser.VCD.Helpers.ReportDownloading
{
    /// <summary>
    /// Downloader of Vendor Central reports.
    /// </summary>
    public class VcdReportDownloader
    {
        private const string AmazonBaseUrl = "https://ara.amazon.com";
        private const string AmazonCsvDownloadReportUrl = "/analytics/download/csv/dashboard/salesDiagnostic";

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
            return DownloadReportAsCsvText(
                reportDay, RequestBodyConstants.ShippedRevenueReportLevel, RequestBodyConstants.ShippedRevenueSalesView);
        }

        /// <summary>
        /// Downloads the CSV Shipped COGS report.
        /// </summary>
        /// <param name="reportDay">Day for report.</param>
        /// <returns>Text of report content.</returns>
        public string DownloadShippedCogsCsvReport(DateTime reportDay)
        {
            logger.LogInfo("Amazon VCD, Attempt to download shipped cogs report.");
            return DownloadReportAsCsvText(
                reportDay, RequestBodyConstants.ShippedCogsLevel, RequestBodyConstants.ShippedCogsSalesView);
        }

        /// <summary>
        /// Downloads the CSV Ordered Revenue report.
        /// </summary>
        /// <param name="reportDay">Day for report.</param>
        /// <returns>Text of report content.</returns>
        public string DownloadOrderedRevenueCsvReport(DateTime reportDay)
        {
            logger.LogInfo("Amazon VCD, Attempt to download ordered revenue report.");
            return DownloadReportAsCsvText(
                reportDay, RequestBodyConstants.OrderedRevenueLevel, RequestBodyConstants.OrderedRevenueView);
        }

        private string DownloadReportAsCsvText(DateTime reportDay, string reportLevel, string salesViewName)
        {
            var failed = false;
            WaitBeforeReportGenerating(reportDay, reportLevel);
            var response = Policy
                .Handle<Exception>()
                .OrResult<IRestResponse>(resp => !IsSuccessfulResponse(resp))
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
                    var resp = DownloadReport(reportDay, reportLevel, salesViewName);
                    EqualizeDelay(IsSuccessfulResponse(resp), failed);
                    return resp;
                });
            return ProcessResponse(response);
        }

        private IRestResponse DownloadReport(DateTime reportDay, string reportLevel, string salesViewName)
        {
            var request = GenerateDownloadingReportRequest(reportDay, reportLevel, salesViewName);
            var response = RestRequestHelper.SendPostRequest<object>(AmazonBaseUrl, request);
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

        private string ProcessResponse(IRestResponse response)
        {
            if (IsSuccessfulResponse(response))
            {
                return ProcessSuccessfulResponse(response);
            }
            ProcessFailedResponse(response);
            throw new Exception($"Report was not downloaded successfully. Status code {response.StatusDescription}");
        }

        private string ProcessSuccessfulResponse(IRestResponse response)
        {
            var textReport = System.Text.Encoding.UTF8.GetString(response.RawBytes, 0, response.RawBytes.Length);
            logger.LogInfo($"Amazon VCD, Report downloading finished successfully. Size: {textReport.Length} characters.");
            return textReport;
        }

        private void ProcessFailedResponse(IRestResponse response)
        {
            logger.LogWarning($"Report downloading attempt failed, Status code: {response.StatusDescription}, content: {response.Content}");
            if (response.StatusCode == (HttpStatusCode)429)
            {
                return;
            }
            pageActions.RefreshSalesDiagnosticPage(authorizationModel);
            logger.LogInfo("Amazon VCD, The portal page has been refreshed.");
        }

        private bool IsSuccessfulResponse(IRestResponse response)
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

        private Dictionary<string, string> GetCookies()
        {
            var collectionOfCookies = pageActions.GetAllCookies();
            var cookies = collectionOfCookies.ToDictionary(x => x.Name, x => x.Value);
            return cookies;
        }

        private RestRequest GenerateDownloadingReportRequest(DateTime reportDay, string reportLevel, string salesViewName)
        {
            var pageRequestData = GetPageDataForReportRequest();
            var requestId = Guid.NewGuid().ToString();
            var requestBodyObject = PrepareRequestBody(requestId, reportDay, reportLevel, salesViewName);
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

        private DownloadReportRequestBody PrepareRequestBody(string requestId, DateTime reportDay, string reportLevel, string salesViewName)
        {
            var visibleFilters = RequestBodyConstants.GetInitialVisibleFilters(
                GetBodyVisibleFilterDateRange(reportDay), salesViewName);
            var reportParameters = GetReportParameters(reportDay, reportLevel);
            return new DownloadReportRequestBody
            {
                salesDiagnosticDetail = new SalesDiagnosticDetail
                {
                    requestId = requestId,
                    reportId = RequestBodyConstants.ReportId,
                    reportParameters = reportParameters,
                    visibleFilters = visibleFilters,
                },
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

        private string GetBodyVisibleFilterDateRange(DateTime reportDate)
        {
            const string filterDatePattern = "M'/'d'/'yy";
            return string.Format("{0} - {0}", reportDate.ToString(filterDatePattern));
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
