using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using CakeExtracter;
using CakeExtractor.SeleniumApplication.Configuration.Models;
using CakeExtractor.SeleniumApplication.Helpers;
using CakeExtractor.SeleniumApplication.Models.CommonHelperModels;
using CakeExtractor.SeleniumApplication.Models.Vcd;
using CakeExtractor.SeleniumApplication.PageActions.AmazonVcd;
using CakeExtractor.SeleniumApplication.Properties;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.ReportDownloading.Constants;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.ReportDownloading.Models;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Newtonsoft.Json;
using Polly;
using RestSharp;

namespace CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.ReportDownloading
{
    internal class VcdReportDownloader
    {
        private const string AmazonBaseUrl = "https://ara.amazon.com";
        private const string AmazonCsvDownloadReportUrl = "/analytics/download/csv/dashboard/salesDiagnostic";

        private static int delayEqualizer;

        private readonly int reportDownloadingFailedDelayInSeconds = VcdSettings.Default.ReportDownloadingFailedDelayInSeconds;
        private readonly int reportDownloadingMinFailedDelayInSeconds = VcdSettings.Default.ReportDownloadingMinFailedDelayInSeconds;
        private readonly int reportDownloadingMaxFailedDelayInSeconds = VcdSettings.Default.ReportDownloadingMaxFailedDelayInSeconds;
        private readonly int reportDownloadingAttemptCount = VcdSettings.Default.ReportDownloadingAttemptCount;

        private readonly AmazonVcdPageActions pageActions;
        private readonly AuthorizationModel authorizationModel;

        public VcdReportDownloader(AmazonVcdPageActions pageActions, AuthorizationModel authorizationModel)
        {
            this.pageActions = pageActions;
            this.authorizationModel = authorizationModel;
        }

        public string DownloadShippedRevenueCsvReport(DateTime reportDay, AccountInfo accountInfo)
        {
            Logger.Info("Amazon VCD, Attempt to download shipped revenue report.");
            return DownloadReportAsCsvText(reportDay, RequestBodyConstants.ShippedRevenueReportLevel, 
                RequestBodyConstants.ShippedRevenueSalesView, accountInfo);
        }

        public string DownloadShippedCogsCsvReport(DateTime reportDay, AccountInfo accountInfo)
        {
            Logger.Info("Amazon VCD, Attempt to download shipped cogs report.");
            return DownloadReportAsCsvText(reportDay, RequestBodyConstants.ShippedCogsLevel, RequestBodyConstants.ShippedCogsSalesView, accountInfo);
        }

        public string DownloadOrderedRevenueCsvReport(DateTime reportDay, AccountInfo accountInfo)
        {
            Logger.Info("Amazon VCD, Attempt to download ordered revenue report.");
            return DownloadReportAsCsvText(reportDay, RequestBodyConstants.OrderedRevenueLevel, RequestBodyConstants.OrderedRevenueView, accountInfo);
        }

        private string DownloadReportAsCsvText(DateTime reportDay, string reportLevel, string salesViewName, AccountInfo accountInfo)
        {
            var failed = false;
            WaitBeforeReportGenerating(reportDay, reportLevel, accountInfo);
            var response = Policy
                .Handle<Exception>()
                .OrResult<IRestResponse>(resp => !IsSuccessfulResponse(resp))
                .WaitAndRetry(reportDownloadingAttemptCount, retryCount => GetTimeSpanForWaiting(),
                    (exception, timeSpan, retryCount, context) =>
                    {
                        failed = true;
                        ProcessFailedResponse(exception.Result);
                        LogWaiting(timeSpan, retryCount, reportDay, reportLevel, accountInfo);
                    })
                .Execute(() =>
                {
                    var resp = DownloadReport(reportDay, reportLevel, salesViewName, accountInfo);
                    EqualizeDelay(IsSuccessfulResponse(resp), failed);
                    return resp;
                });
            return ProcessResponse(response);
        }

        private IRestResponse DownloadReport(DateTime reportDay, string reportLevel, string salesViewName, AccountInfo accountInfo)
        {
            var request = GenerateDownloadingReportRequest(reportDay, reportLevel, salesViewName, accountInfo);
            var response = RestRequestHelper.SendPostRequest<object>(AmazonBaseUrl, request);
            return response;
        }

        private void WaitBeforeReportGenerating(DateTime reportDay, string reportLevel, AccountInfo accountInfo)
        {
            var timeSpan = GetTimeSpanForWaiting();
            LogWaiting(timeSpan, null, reportDay, reportLevel, accountInfo);
            Thread.Sleep(timeSpan);
        }

        private void LogWaiting(TimeSpan timeSpan, int? retryCount, DateTime reportDay, string reportLevel,
            AccountInfo accountInfo)
        {
            var message = $"Waiting {timeSpan} for ({reportDay}, {reportLevel}, {accountInfo.Account.Name}) before report generating";
            if (retryCount.HasValue)
            {
                message += $" (number of retrying - {retryCount})";
            }

            Logger.Info(message);
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
            Logger.Info($"Amazon VCD, Report downloading finished successfully. Size: {textReport.Length} characters.");
            return textReport;
        }

        private void ProcessFailedResponse(IRestResponse response)
        {
            Logger.Warn($"Report downloading attempt failed, Status code: {response.StatusDescription}, content: {response.Content}");
            if (response.StatusCode == (HttpStatusCode)429)
            {
                return;
            }

            pageActions.RefreshSalesDiagnosticPage(authorizationModel);
            Logger.Info("Amazon VCD, The portal page has been refreshed.");
        }

        private bool IsSuccessfulResponse(IRestResponse response)
        {
            return response.StatusCode == HttpStatusCode.OK;
        }

        private ReportDownloadingRequestPageData GetPageDataForReportRequest()
        {
            var token = pageActions.GetAccessToken();
            var cookies = pageActions.GetAllCookies().ToDictionary(x => x.Name, x => x.Value);
            return new ReportDownloadingRequestPageData
            {
                Token = token,
                Cookies = cookies
            };
        }

        private RestRequest GenerateDownloadingReportRequest(DateTime reportDay, string reportLevel, string salesViewName, AccountInfo accountInfo)
        {
            var pageRequestData = GetPageDataForReportRequest();
            var requestId = Guid.NewGuid().ToString();
            var requestBodyObject = PrepareRequestBody(requestId, reportDay, reportLevel, salesViewName);
            var requestHeaders = GetHeadersDictionary(requestId);
            var requestBodyJson = JsonConvert.SerializeObject(requestBodyObject);
            var requestQueryParams = RequestQueryConstants.GetRequestQueryParameters(pageRequestData.Token,
                accountInfo.VendorGroupId.ToString(), accountInfo.McId.ToString());
            var request = RestRequestHelper.CreateRestRequest(AmazonCsvDownloadReportUrl, pageRequestData.Cookies, requestQueryParams);
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
            var visibleFilters = RequestBodyConstants.GetInitialVisibleFilters(GetBodyVisibleFilterDateRange(reportDay), salesViewName);
            var reportParameters = GetReportParameters(reportDay, reportLevel);
            return new DownloadReportRequestBody
            {
                salesDiagnosticDetail = new SalesDiagnosticDetail
                {
                    requestId = requestId,
                    reportId = RequestBodyConstants.ReportId,
                    reportParameters = reportParameters,
                    visibleFilters = visibleFilters
                }
            };
        }

        private List<ReportParameter> GetReportParameters(DateTime reportDay, string reportLevel)
        {
            var reportParameters = RequestBodyConstants.GetReportParameters(GetReportParameterFilterDate(reportDay),
                GetReportParameterFilterDate(reportDay), reportLevel);
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
            var waitTime = Math.Min(reportDownloadingMaxFailedDelayInSeconds,
                reportDownloadingMinFailedDelayInSeconds + reportDownloadingFailedDelayInSeconds * delayEqualizer);
            return TimeSpan.FromSeconds(waitTime);
        }
    }
}
