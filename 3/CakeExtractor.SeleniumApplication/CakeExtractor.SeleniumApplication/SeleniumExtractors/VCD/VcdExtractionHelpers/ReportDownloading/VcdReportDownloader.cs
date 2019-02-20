using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter;
using CakeExtractor.SeleniumApplication.Configuration.Models;
using CakeExtractor.SeleniumApplication.Helpers;
using CakeExtractor.SeleniumApplication.Models.CommonHelperModels;
using CakeExtractor.SeleniumApplication.Models.Vcd;
using CakeExtractor.SeleniumApplication.PageActions.AmazonVcd;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.ReportDownloading.Constants;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.ReportDownloading.Models;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Newtonsoft.Json;
using RestSharp;

namespace CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.ReportDownloading
{
    internal class VcdReportDownloader
    {
        private const string AmazonBaseUrl = "https://ara.amazon.com";
        private const string AmazonCsvDownloadReportUrl = "/analytics/download/csv/dashboard/salesDiagnostic";

        private readonly AmazonVcdPageActions pageActions;
        private readonly AuthorizationModel authorizationModel;
        private readonly int refreshPageMinutesInterval;

        private DateTime lastRefreshTime;

        public VcdReportDownloader(AmazonVcdPageActions pageActions, AuthorizationModel authorizationModel, int refreshPageMinutesInterval)
        {
            this.pageActions = pageActions;
            this.authorizationModel = authorizationModel;
            this.refreshPageMinutesInterval = refreshPageMinutesInterval;
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
            TryToRefreshPage();
            var request = GenerateDownloadingReportRequest(reportDay, reportLevel, salesViewName, accountInfo);
            var response = RestRequestHelper.SendPostRequest<object>(AmazonBaseUrl, request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var textReport = System.Text.Encoding.UTF8.GetString(response.RawBytes, 0, response.RawBytes.Length);
                Logger.Info($"Amazon VCD, Report downloading finished successfully. Size: {textReport.Length} characters.");
                return textReport;
            }

            Logger.Warn("Report downloading attempt failed, Status code {0}", response.StatusDescription);
            throw new Exception($"Report was not downloaded successfully. Status code {response.StatusDescription}");
        }

        private void TryToRefreshPage()
        {
            var now = DateTime.Now;
            if (lastRefreshTime.AddMinutes(refreshPageMinutesInterval) > now)
            {
                return;
            }

            pageActions.RefreshSalesDiagnosticPage(authorizationModel);
            Logger.Info($"Amazon VCD, The portal page has been refreshed. Last refresh time: {lastRefreshTime}, current refresh time: {now}");
            lastRefreshTime = now;
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
    }
}
