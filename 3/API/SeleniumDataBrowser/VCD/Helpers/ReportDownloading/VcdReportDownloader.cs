using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Newtonsoft.Json;
using Polly;
using RestSharp;
using SeleniumDataBrowser.Helpers;
using SeleniumDataBrowser.Models;
using SeleniumDataBrowser.VCD.Enums;
using SeleniumDataBrowser.VCD.Helpers.ReportDownloading.Configurations;
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
    internal class VcdReportDownloader
    {
        private const string AmazonBaseUrl = "https://vendorcentral.amazon.com";
        private const string AmazonCsvDownloadReportUrl = "/analytics/data/dashboard/{0}/report/{0}Detail";

        private static int delayEqualizer;
        private readonly VcdReportDownloaderSettings reportDownloaderSettings;
        private readonly AmazonVcdActionsWithPagesManager pageActions;
        private readonly AuthorizationModel authorizationModel;
        private readonly VcdAccountInfo accountInfo;
        private readonly SeleniumLogger logger;
        private readonly Dictionary<string, ReportType> reportTypesByNames;

        /// <summary>
        /// Initializes a new instance of the <see cref="VcdReportDownloader"/> class.
        /// </summary>
        /// <param name="accountInfo">Information about the current account.</param>
        /// <param name="pageActions">Manager of page actions.</param>
        /// <param name="authorizationModel">Authorization settings.</param>
        /// <param name="logger">Selenium data browser logger.</param>
        /// <param name="reportDownloaderSettings">Configuration settings.</param>
        public VcdReportDownloader(
            VcdAccountInfo accountInfo,
            AmazonVcdActionsWithPagesManager pageActions,
            AuthorizationModel authorizationModel,
            SeleniumLogger logger,
            VcdReportDownloaderSettings reportDownloaderSettings)
        {
            this.accountInfo = accountInfo;
            this.pageActions = pageActions;
            this.authorizationModel = authorizationModel;
            this.logger = logger;
            this.reportDownloaderSettings = reportDownloaderSettings;

            reportTypesByNames = GetNetPpmReportTypesDictionary();
        }

        /// <summary>
        /// Downloads the CSV Shipped Revenue report.
        /// </summary>
        /// <param name="reportDay">Day for report.</param>
        /// <returns>Text of report content.</returns>
        public string DownloadShippedRevenueCsvReport(DateTime reportDay)
        {
            logger.LogInfo("Amazon VCD, Attempt to download shipped revenue report.");
            var reportParameters = new VcdReportParameters()
            {
                PageIndex = 0,
                ReportDate = reportDay,
                ReportLevel = RequestBodyConstants.ShippedRevenueReportLevel,
                ReportId = RequestBodyConstants.ShippedRevenueColumnId,
                ReportType = ReportType.salesDiagnostic,
            };
            return DownloadCsvReportFromBackendApi(reportParameters);
        }

        /// <summary>
        /// Downloads the CSV Shipped COGS report.
        /// </summary>
        /// <param name="reportDay">Day for report.</param>
        /// <returns>Text of report content.</returns>
        public string DownloadShippedCogsCsvReport(DateTime reportDay)
        {
            logger.LogInfo("Amazon VCD, Attempt to download shipped cogs report.");
            var reportParameters = new VcdReportParameters()
            {
                PageIndex = 0,
                ReportDate = reportDay,
                ReportLevel = RequestBodyConstants.ShippedCogsLevel,
                ReportId = RequestBodyConstants.ShippedCogsColumnId,
                ReportType = ReportType.salesDiagnostic,
            };
            return DownloadCsvReportFromBackendApi(reportParameters);
        }

        /// <summary>
        /// Downloads the CSV Ordered Revenue report.
        /// </summary>
        /// <param name="reportDay">Day for report.</param>
        /// <returns>Text of report content.</returns>
        public string DownloadOrderedRevenueCsvReport(DateTime reportDay)
        {
            logger.LogInfo("Amazon VCD, Attempt to download ordered revenue report.");
            var reportParameters = new VcdReportParameters()
            {
                PageIndex = 0,
                ReportDate = reportDay,
                ReportLevel = RequestBodyConstants.OrderedRevenueLevel,
                ReportId = RequestBodyConstants.OrderedRevenueColumnId,
                ReportType = ReportType.salesDiagnostic,
            };
            return DownloadCsvReportFromBackendApi(reportParameters);
        }

        /// <summary>
        /// Downloads the CSV Inventory Health report.
        /// </summary>
        /// <param name="reportDay">Day for report.</param>
        /// <returns>Text of report content.</returns>
        public string DownloadInventiryHealthCsvReport(DateTime reportDay)
        {
            logger.LogInfo("Amazon VCD, Attempt to download Inventory Health report.");
            var reportParameters = new VcdReportParameters()
            {
                PageIndex = 0,
                ReportDate = reportDay,
                ReportLevel = string.Empty,
                ReportId = RequestBodyConstants.HealthInventoryColumnId,
                ReportType = ReportType.inventoryHealth,
            };
            return DownloadCsvReportFromBackendApi(reportParameters);
        }

        /// <summary>
        /// Downloads the CSV Customer Reviews report.
        /// </summary>
        /// <param name="reportDay">Day for report.</param>
        /// <returns>Text of report content.</returns>
        public string DownloadCustomerReviewsCsvReport(DateTime reportDay)
        {
            logger.LogInfo("Amazon VCD, Attempt to download Inventory Health report.");
            var reportParameters = new VcdReportParameters()
            {
                PageIndex = 0,
                ReportDate = reportDay,
                ReportLevel = string.Empty,
                ReportId = RequestBodyConstants.CustomerReviewsColumnId,
                ReportType = ReportType.customerReviews,
            };
            return DownloadCsvReportFromBackendApi(reportParameters);
        }

        /// <summary>
        /// Downloads the CSV  Geographic Sales Insights report.
        /// </summary>
        /// <param name="reportDay">Day for report.</param>
        /// <returns>Text of report content.</returns>
        public string DownloadGeographicSalesInsightsCsvReport(DateTime reportDay)
        {
            logger.LogInfo("Amazon VCD, Attempt to download Geographic Sales Insights report.");
            var reportParameters = new VcdReportParameters()
            {
                PageIndex = 0,
                ReportDate = reportDay,
                ReportLevel = string.Empty,
                ReportId = RequestBodyConstants.GeographicSalesInsightColumnId,
                ReportType = ReportType.geographicSalesInsights,
            };
            return DownloadCsvReportFromBackendApi(reportParameters);
        }

        /// <summary>
        /// Downloads the CSV Net PPM report.
        /// </summary>
        /// <param name="period">Period for report.</param>
        /// <returns>Text of report content.</returns>
        public string DownloadNetPpmCsvReport(string period)
        {
            logger.LogInfo($"Amazon VCD, Attempt to download Net Ppm {period} report.");
            ReportType reportType = ReportType.netPPMWeekly;
            reportType = GetNetPpmReportTypesDictionary()[period];
            var reportParameters = new VcdReportParametersWithPeriods()
            {
                PageIndex = 0,
                ReportLevel = string.Empty,
                ReportId = RequestBodyConstants.NetPPMColumnId,
                ReportType = reportType,
                Period = period,
            };
            return DownloadNonDefaultPeriodCsvReportFromBackendApi(reportParameters);
        }

        /// <summary>
        /// Downloads the CSV Repeat Purchase Behavior report.
        /// </summary>
        /// <param name="period">Period for report.</param>
        /// <returns>Text of report content.</returns>
        public string DownloadRepeatPurchaseBehaviorCsvReport(string period)
        {
            logger.LogInfo($"Amazon VCD, Attempt to download Repeat Purchase Behavior {period} report.");
            ReportType reportType;
            reportType = GetRepeatPurchaseBehaviorReportTypesDictionary()[period];

            var reportParameters = new VcdReportParametersWithPeriods()
            {
                PageIndex = 0,
                ReportLevel = string.Empty,
                ReportId = RequestBodyConstants.RepeatPurchaseBehaviorColumnId,
                ReportType = reportType,
                Period = period,
            };
            return DownloadNonDefaultPeriodCsvReportFromBackendApi(reportParameters);
        }

        private string DownloadCsvReportFromBackendApi(VcdReportParameters reportParameters)
        {
            var firstPartOfReportData = TryProcessRequest(reportParameters);
            return CreateCsvReportContent(reportParameters, firstPartOfReportData);
        }

        private string DownloadNonDefaultPeriodCsvReportFromBackendApi(VcdReportParametersWithPeriods reportParameters)
        {
            var firstPartOfReportData = TryProcessRequestWithPeriod(reportParameters);
            var parameters = new VcdReportParameters
            {
                ReportType = reportParameters.ReportType,
                RequestId = reportParameters.RequestId,
                ReportId = reportParameters.ReportId,
                ReportLevel = reportParameters.ReportLevel,
                PageIndex = reportParameters.PageIndex,
            };
            return CreateCsvReportContent(parameters, firstPartOfReportData);
        }

        private string CreateCsvReportContent(VcdReportParameters reportParameters, dynamic firstPartOfReportData)
        {
            var csvReportContent = new StringBuilder();
            SetCsvReportHeader(csvReportContent, firstPartOfReportData);
            SetCsvReportRows(reportParameters, csvReportContent, firstPartOfReportData);
            return csvReportContent.ToString();
        }

        private void SetCsvReportHeader(StringBuilder csvReportContent, dynamic firstPartOfReportData)
        {
            var reportHeader = VcdComposingReportDataHelper.CreateHeaderLineWithColumnNames(firstPartOfReportData);
            csvReportContent.AppendLine(reportHeader.ToString());
        }

        private void SetCsvReportRows(VcdReportParameters reportParameters, StringBuilder csvReportContent, dynamic firstPartOfReportData)
        {
            var productsRows = GetReportRows(reportParameters, firstPartOfReportData);
            foreach (var productRow in productsRows)
            {
                var newReportLine = VcdComposingReportDataHelper.CreateRowLineWithProductInfo(productRow);
                csvReportContent.AppendLine(newReportLine.ToString());
            }
        }

        private IEnumerable<dynamic> GetReportRows(VcdReportParameters reportParameters, dynamic firstPartOfReportData)
        {
            var firstPartOfProductsRows = VcdComposingReportDataHelper.GetReportProductsRows(firstPartOfReportData);
            var allProductsRows = JsonArrayToList(firstPartOfProductsRows);
            var totalReportRowCount = VcdComposingReportDataHelper.GetTotalReportRowCount(firstPartOfReportData);
            var downloadedRowCount = firstPartOfProductsRows.Count;

            reportParameters.PageIndex = 0;
            while (downloadedRowCount < totalReportRowCount)
            {
                reportParameters.PageIndex++;
                var nextPartOfProductsRows =
                    GetNextPartOfProductRows(reportParameters);
                if (!nextPartOfProductsRows.Any())
                {
                    logger.LogWarning($"GetReportRows: No records extracted from {downloadedRowCount} " +
                                      $"to {totalReportRowCount} for {reportParameters.ReportDate.ToShortDateString()}");
                    break;
                }

                allProductsRows.AddRange(nextPartOfProductsRows);
                downloadedRowCount += nextPartOfProductsRows.Count;
            }
            return allProductsRows;
        }

        private List<dynamic> GetNextPartOfProductRows(VcdReportParameters reportParameters)
        {
            var nextPartOfReportData = TryProcessRequest(reportParameters);
            var nextPartOfProductsRows = VcdComposingReportDataHelper.GetReportProductsRows(nextPartOfReportData);
            return JsonArrayToList(nextPartOfProductsRows);
        }

        private dynamic TryProcessRequest(VcdReportParameters reportParameters)
        {
            var failed = false;
            WaitBeforeReportGenerating(reportParameters);
            var response = Policy
                .Handle<Exception>()
                .OrResult<IRestResponse<dynamic>>(resp => !IsSuccessfulResponse(resp))
                .WaitAndRetry(
                    reportDownloaderSettings.ReportDownloadingAttemptCount,
                    retryCount => GetTimeSpanForWaiting(),
                    (exception, timeSpan, retryCount, context) =>
                    {
                        failed = true;
                        ProcessFailedResponse(exception.Result, reportParameters.PageIndex);
                        logger.LogWaiting(
                            $"Report (part {reportParameters.PageIndex}) generating for {reportParameters.ReportDate.ToShortDateString()}, " +
                            $"{reportParameters.ReportType}, {reportParameters.ReportLevel}, {accountInfo.AccountName}. "
                            + "Waiting {0} ...",
                            timeSpan,
                            retryCount);
                    })
                .Execute(() =>
                {
                    var resp = ProcessRequest(reportParameters);
                    EqualizeDelay(IsSuccessfulResponse(resp), failed);
                    return resp;
                });
            return ProcessResponse(response, reportParameters.PageIndex);
        }

        private dynamic TryProcessRequestWithPeriod(VcdReportParametersWithPeriods reportParameters)
        {
            var failed = false;
            WaitBeforeReportGeneratingWithPeriods(reportParameters);
            var response = Policy
                .Handle<Exception>()
                .OrResult<IRestResponse<dynamic>>(resp => !IsSuccessfulResponse(resp))
                .WaitAndRetry(
                    reportDownloaderSettings.ReportDownloadingAttemptCount,
                    retryCount => GetTimeSpanForWaiting(),
                    (exception, timeSpan, retryCount, context) =>
                    {
                        failed = true;
                        ProcessFailedResponse(exception.Result, reportParameters.PageIndex);
                        logger.LogWaiting(
                            $"Report (part {reportParameters.PageIndex}) generating for {reportParameters.Period} period, " +
                            $"{reportParameters.ReportType}, {reportParameters.ReportLevel}, {accountInfo.AccountName}. "
                            + "Waiting {0} ...",
                            timeSpan,
                            retryCount);
                    })
                .Execute(() =>
                {
                    var resp = ProcessRequestWithPeriods(reportParameters);
                    EqualizeDelay(IsSuccessfulResponse(resp), failed);
                    return resp;
                });
            return ProcessResponse(response, reportParameters.PageIndex);
        }

        private IRestResponse<dynamic> ProcessRequest(VcdReportParameters reportParameters)
        {
            var request = GenerateDownloadingReportRequest(reportParameters);
            var response = RestRequestHelper.SendPostRequest<dynamic>(AmazonBaseUrl, request);
            return response;
        }

        private IRestResponse<dynamic> ProcessRequestWithPeriods(VcdReportParametersWithPeriods reportParameters)
        {
            var request = GenerateDownloadingReportRequestWithPeriods(reportParameters);
            var response = RestRequestHelper.SendPostRequest<dynamic>(AmazonBaseUrl, request);
            return response;
        }

        private void WaitBeforeReportGenerating(VcdReportParameters reportParameters)
        {
            var timeSpan = GetTimeSpanForWaiting();
            logger.LogWaiting(
                $"Report (part {reportParameters.PageIndex}) generating for {reportParameters.ReportDate.ToShortDateString()}," +
                $" {reportParameters.ReportType}, {reportParameters.ReportLevel}, {accountInfo.AccountName}. "
                + "Waiting {0} ...",
                timeSpan,
                null);
            Thread.Sleep(timeSpan);
        }

        private void WaitBeforeReportGeneratingWithPeriods(VcdReportParametersWithPeriods reportParameters)
        {
            var timeSpan = GetTimeSpanForWaiting();
            logger.LogWaiting(
                $"Report (part {reportParameters.PageIndex}) generating for {reportParameters.Period} period," +
                $" {reportParameters.ReportType}, {reportParameters.ReportLevel}, {accountInfo.AccountName}. "
                + "Waiting {0} ...",
                timeSpan,
                null);
            Thread.Sleep(timeSpan);
        }
        private dynamic ProcessResponse(IRestResponse<dynamic> response, int pageIndex)
        {
            if (IsSuccessfulResponse(response))
            {
                return ProcessSuccessfulResponse(response, pageIndex);
            }
            ProcessFailedResponse(response, pageIndex);
            throw new Exception($"Report (part {pageIndex}) was not downloaded successfully. Status code {response.StatusCode}");
        }

        private dynamic ProcessSuccessfulResponse(IRestResponse<dynamic> response, int pageIndex)
        {
            var data = response.Data;
            logger.LogInfo($"Amazon VCD, Report (part {pageIndex}) downloading finished successfully.");
            return data;
        }

        private void ProcessFailedResponse(IRestResponse<dynamic> response, int pageIndex)
        {
            logger.LogWarning($"Report (part {pageIndex}) downloading attempt failed, Status code: {response.StatusCode}");
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

        private RestRequest GenerateDownloadingReportRequest(VcdReportParameters reportParameters)
        {
            var pageRequestData = GetPageDataForReportRequest();
            reportParameters.RequestId = Guid.NewGuid().ToString();
            var requestBodyObject = PrepareRequestBody(reportParameters);
            var requestHeaders = GetHeadersDictionary(reportParameters.RequestId);
            var requestBodyJson = JsonConvert.SerializeObject(requestBodyObject);
            var requestQueryParams = RequestQueryConstants.GetRequestQueryParameters(
                pageRequestData.Token,
                accountInfo.VendorGroupId.ToString(),
                accountInfo.McId.ToString());
            var resourceUrl = string.Format(AmazonCsvDownloadReportUrl, reportParameters.ReportType);
            var request = RestRequestHelper.CreateRestRequest(resourceUrl, pageRequestData.Cookies, requestQueryParams);
            request.AddParameter(RequestBodyConstants.RequestBodyFormat, requestBodyJson, ParameterType.RequestBody);
            requestHeaders.ForEach(x => request.AddHeader(x.Key, x.Value));
            return request;
        }

        private RestRequest GenerateDownloadingReportRequestWithPeriods(VcdReportParametersWithPeriods reportParameters)
        {
            reportParameters.RequestId = Guid.NewGuid().ToString();
            var parameters = new VcdReportParameters
            {
                ReportType = reportParameters.ReportType,
                RequestId = reportParameters.RequestId,
                ReportId = reportParameters.ReportId,
                ReportLevel = reportParameters.ReportLevel,
                PageIndex = reportParameters.PageIndex,
            };
            var pageRequestData = GetPageDataForReportRequest();
            reportParameters.RequestId = Guid.NewGuid().ToString();
            var requestBodyObject = PrepareRequestBody(parameters);
            var requestHeaders = GetHeadersDictionary(reportParameters.RequestId);
            var requestBodyJson = JsonConvert.SerializeObject(requestBodyObject);
            var requestQueryParams = RequestQueryConstants.GetRequestQueryParameters(
                pageRequestData.Token,
                accountInfo.VendorGroupId.ToString(),
                accountInfo.McId.ToString());
            var reportTypeForUrl = reportParameters.ReportType.ToString().Replace("Weekly", "").Replace("Monthly", "").Replace("Yearly", "").Replace("Quaterly", "");
            var resourceUrl = string.Format(AmazonCsvDownloadReportUrl, reportTypeForUrl);
            var request = RestRequestHelper.CreateRestRequest(resourceUrl, pageRequestData.Cookies, requestQueryParams);
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

        private Dictionary<string, ReportType> GetNetPpmReportTypesDictionary()
        {
            return new Dictionary<string, ReportType>
            {
                { "MONTHLY", ReportType.netPPMMonthly },
                { "WEEKLY", ReportType.netPPMWeekly },
                { "YEARLY", ReportType.netPPMYearly },
            };
        }

        private Dictionary<string, ReportType> GetRepeatPurchaseBehaviorReportTypesDictionary()
        {
            return new Dictionary<string, ReportType>
            {
                { "MONTHLY", ReportType.repeatPurchaseBehaviorMonthly },
                { "QUATERLY", ReportType.repeatPurchaseBehaviorQuaterly },
            };
        }

        private SalesDiagnosticDetail PrepareRequestBody(VcdReportParameters reportParameters)
        {
            var reportParams = GetReportParameters(reportParameters);
            var reportPaginationWithOrderParameter =
                GetReportPaginationWithOrderParameter(reportParameters.ReportId, reportParameters.PageIndex);
            return new SalesDiagnosticDetail
            {
                requestId = reportParameters.RequestId,
                reportParameters = reportParams,
                reportPaginationWithOrderParameter = reportPaginationWithOrderParameter,
            };
        }

        private List<ReportParameter> GetReportParameters(VcdReportParameters reportParameters)
        {
            var reportDay = GetReportParameterFilterDate(reportParameters.ReportDate);
            List<ReportParameter> reportParams = null;
            switch (reportParameters.ReportType)
            {
                case ReportType.salesDiagnostic:
                    reportParams =
                        RequestBodyConstants.GetSalesDiagnosticParameters(reportDay, reportParameters.ReportLevel);
                    break;
                case ReportType.inventoryHealth:
                    reportParams =
                        RequestBodyConstants.GetInventoryHealthParameters(reportDay);
                    break;
                case ReportType.customerReviews:
                    reportParams =
                        RequestBodyConstants.GetCustomerReviewsParameters(reportDay);
                    break;
                case ReportType.netPPMWeekly:
                    reportParams =
                        RequestBodyConstants.GetNetPpmParameters(reportDay, "WEEKLY");
                    break;
                case ReportType.netPPMMonthly:
                    reportParams =
                        RequestBodyConstants.GetNetPpmParameters(reportDay, "MONTHLY");
                    break;
                case ReportType.netPPMYearly:
                    reportParams =
                        RequestBodyConstants.GetNetPpmParameters(reportDay, "YEARLY");
                    break;
                case ReportType.geographicSalesInsights:
                    reportParams =
                        RequestBodyConstants.GetGeographicSalesInsightsParameters(reportDay);
                    break;
                case ReportType.repeatPurchaseBehaviorMonthly:
                    reportParams =
                        RequestBodyConstants.GetRepeatPurchaseBehaviorParameters(reportDay, "MONTHLY");
                    break;
                case ReportType.repeatPurchaseBehaviorQuaterly:
                    reportParams =
                        RequestBodyConstants.GetRepeatPurchaseBehaviorParameters(reportDay, "QUATERLY");
                    break;
            }
            return reportParams;
        }

        private ReportPaginationWithOrderParameter GetReportPaginationWithOrderParameter(string reportId, int pageIndex)
        {
            var reportPaginationWithOrderParameter = RequestBodyConstants.GetReportPaginationWithOrderParameter(
                reportId,
                pageIndex,
                reportDownloaderSettings.MaxPageSizeForReport);
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
            var expectedDelay = reportDownloaderSettings.MinDelayBetweenReportDownloadingInSeconds
                                + (reportDownloaderSettings.ReportDownloadingStartedDelayInSeconds * delayEqualizer);
            var waitTime = Math.Min(
                reportDownloaderSettings.MaxDelayBetweenReportDownloadingInSeconds,
                expectedDelay);
            return TimeSpan.FromSeconds(waitTime);
        }

        private static List<dynamic> JsonArrayToList(dynamic jsonArray)
        {
            return JsonConvert.DeserializeObject<List<dynamic>>(jsonArray.ToString());
        }
    }
}
