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
using System;
using System.Collections.Generic;
using System.Linq;

namespace CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.ExtractionHelpers
{
    internal class VcdReportDownloader
    {
        private const string amazonBaseUrl = "https://ara.amazon.com";

        private const string amazonCsvDownloadReportUrl = "/analytics/download/csv/dashboard/salesDiagnostic";

        private AmazonVcdPageActions pageActions;

        private AuthorizationModel authorizationModel;

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

        private string DownloadReportAsCsvText(DateTime reportDay, string reportLevel, string salesViewName, AccountInfo accountInfo)
        {
            pageActions.RefreshSalesDiagnosticPage(authorizationModel);
            var request = GenerateDownloadingReportRequest(reportDay, reportLevel, salesViewName, accountInfo);
            var response = RestRequestHelper.SendPostRequest<object>(amazonBaseUrl, request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string textReport = System.Text.Encoding.UTF8.GetString(response.RawBytes, 0, response.RawBytes.Length);
                Logger.Info("Amazon VCD, Report downloading finished successfully. Size:{0} characters.",
                  textReport.Length);
                return textReport;
            }
            else
            {
                Logger.Warn("Report downloading attempt failed, Status code {0}", response.StatusDescription);
                throw new Exception(string.Format("Report was not downloaded successfully. Status code {0}", response.StatusDescription));
            }
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
            var requestHeaders = GetHeadersDictionary(requestId, accountInfo.VendorGroupId.ToString());
            var requestBodyJson = JsonConvert.SerializeObject(requestBodyObject);
            var requestQueryParams = RequestQueryConstants.GetRequestQueryParameters(pageRequestData.Token,
                accountInfo.VendorGroupId.ToString(), accountInfo.McId.ToString());
            var request = RestRequestHelper.CreateRestRequest(amazonCsvDownloadReportUrl, pageRequestData.Cookies, requestQueryParams, null);
            request.AddParameter(RequestBodyConstants.RequestBodyFormat, requestBodyJson, ParameterType.RequestBody);
            requestHeaders.ForEach(x => request.AddHeader(x.Key, x.Value));
            return request;
        }

        private Dictionary<string, string> GetHeadersDictionary(string requestId, string vendorGroupId)
        {
            var requestHeaders = RequestHeaderConstants.GetHeadersDictionary();
            requestHeaders[RequestHeaderConstants.RequestIdHeaderName] = requestId;
            return requestHeaders;
        }

        private DownloadReportRequestBody PrepareRequestBody(string requestId, DateTime reportDay, string reportLevel, string salesViewName)
        {
            var visibleFilters = RequestBodyConstants.GetInitialVisibleFilters(GetBodyVisibleFilterDateRange(reportDay), salesViewName);
            var reportParameters = GetReportParameters(reportDay, reportLevel);
            return new DownloadReportRequestBody()
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
