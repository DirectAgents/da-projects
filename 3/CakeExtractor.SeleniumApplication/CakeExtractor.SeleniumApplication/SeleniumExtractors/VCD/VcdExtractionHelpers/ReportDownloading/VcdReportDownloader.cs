using CakeExtracter;
using CakeExtractor.SeleniumApplication.Configuration.Models;
using CakeExtractor.SeleniumApplication.Helpers;
using CakeExtractor.SeleniumApplication.Models.Vcd;
using CakeExtractor.SeleniumApplication.PageActions.AmazonVcd;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.ReportDownloading.Constants;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.ReportDownloading.Models;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.UserInfoExtracting;
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

        private AccountInfo accountInfo;

        public VcdReportDownloader(AmazonVcdPageActions pageActions, AccountInfo accountInfo)
        {
            this.pageActions = pageActions;
            this.accountInfo = accountInfo;
        }

        public string DownloadReportAsCsvText(DateTime reportDay)
        {
            Logger.Info("Amazon VCD, Attemt to download report.");
            pageActions.NavigateToSalesDiagnosticPage();
            var request = GetDownloadingRequest(reportDay);
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
                Logger.Warn("Report downloading attempt failed,Status code {0}", response.StatusDescription);
                throw new Exception(string.Format("Report was not downloaded successfully. Status code {0}", response.StatusDescription));
            }
        }

        private RestRequest GetDownloadingRequest(DateTime reportDay)
        {
            var pageRequestData = GetPageDataForReportRequest();
            var request = GenerateDownloadingReportRequest(pageRequestData, reportDay, 
                accountInfo.VendorGroupId.ToString(), accountInfo.McId.ToString());
            return request;
        }

        private ReportDownloadingRequestPageData GetPageDataForReportRequest()
        {
            var token = pageActions.GetAccessToken();
            var cookies = pageActions.GetAllCookies().ToDictionary(x => x.Name, x => x.Value);
            var userInfo = UserInfoExtracter.ExtractUserInfo(pageActions);
            return new ReportDownloadingRequestPageData
            {
                Token = token,
                Cookies = cookies
            };
        }

       

        private RestRequest GenerateDownloadingReportRequest(ReportDownloadingRequestPageData requestPageData, DateTime reportDay,
            string vendorGroupId, string mcId)
        {
            var requestId = Guid.NewGuid().ToString();
            var requestBodyObject = PrepareRequestBody(requestId, reportDay);
            var requestHeaders = GetHeadersDictionary(requestId, vendorGroupId);
            var requestBodyJson = JsonConvert.SerializeObject(requestBodyObject);
            var requestQueryParams = RequestQueryConstants.GetRequestQueryParameters(requestPageData.Token, vendorGroupId, mcId);
            var request = RestRequestHelper.CreateRestRequest(amazonCsvDownloadReportUrl, requestPageData.Cookies, requestQueryParams, null);
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

        private DownloadReportRequestBody PrepareRequestBody(string requestId, DateTime reportDay)
        {
            var visibleFilters = RequestBodyConstants.GetInitialVisibleFilters();
            visibleFilters[RequestBodyConstants.ReportDatesVisibleFilterName] = new List<string> { GetBodyVisibleFilterDateRange(reportDay) };
            var reportParameters = getReportParameters(reportDay);
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

        private List<ReportParameter> getReportParameters(DateTime reportDay)
        {
            var reportParameters = RequestBodyConstants.GetReportParameters();
            var startDateReportParametr = reportParameters.Find(rp => rp.parameterId == RequestBodyConstants.StartDateReportParameter);
            startDateReportParametr.values = new List<Value>
            {
               new Value
               {
                   val = GetReportParameterFilterDate(reportDay)
               }
            };
            var endDateReportParameter = reportParameters.Find(rp => rp.parameterId == RequestBodyConstants.EndDateReportParameter);
            endDateReportParameter.values = new List<Value>
            {
               new Value
               {
                   val = GetReportParameterFilterDate(reportDay)
               }
            };
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
