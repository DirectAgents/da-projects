using CakeExtractor.SeleniumApplication.Helpers;
using CakeExtractor.SeleniumApplication.Models.Vcd;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.ReportDownloading.Constants;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.ReportDownloading.Models;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;

namespace CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.ExtractionHelpers
{
    internal class VcdDownloadCsvReportRequestGenerator
    {
        private const string amazonCsvDownloadReportUrl = "/download/csv/dashboard/salesDiagnostic";

        private const string requestBodyFormat = "application/json";

        private const string reportId = "salesDiagnosticDetail";

        public static RestRequest GenerateDownloadingReportRequest(ReportDownloadingRequestPageData requestPageData, DateTime reportDay, string vendorGroupId)
        {
            var requestId = Guid.NewGuid().ToString();
            var requestBodyObject = PrepareRequestBody(requestId, reportDay);
            var requestHeaders = GetHeadersDictionary(requestId, vendorGroupId);
            var requestBodyJson = JsonConvert.SerializeObject(requestBodyObject);
            var requestQueryParams = RequestQueryConstants.GetRequiestQueryparameters(requestPageData.Token);
            var request = RestRequestHelper.CreateRestRequest(amazonCsvDownloadReportUrl, requestPageData.Cookies, requestQueryParams, null);
            request.AddParameter(requestBodyFormat, requestBodyJson, ParameterType.RequestBody);
            requestHeaders.ForEach(x => request.AddHeader(x.Key, x.Value));
            return request;
        }

        private static Dictionary<string, string> GetHeadersDictionary(string requestId, string vendorGroupId)
        {
            var requestHeaders = RequestHeaderConstants.GetHeadersDictionary();
            requestHeaders[RequestHeaderConstants.RequestIdHeaderName] = requestId;
            requestHeaders[RequestHeaderConstants.VendorGroupHeaderName] = vendorGroupId;
            return requestHeaders;
        }

        private static DownloadReportRequestBody PrepareRequestBody(string requestId, DateTime reportDay)
        {
            var visibleFilters = RequestBodyConstants.GetInitialVisibleFilters();
            visibleFilters[RequestBodyConstants.ReportDatesVisibleFilterName] = new List<string> { GetBodyVisibleFilterDateRange(reportDay) };
            var reportParameters = getReportParameters(reportDay);
            return new DownloadReportRequestBody()
            {
                salesDiagnosticDetail = new SalesDiagnosticDetail
                {
                    requestId = requestId,
                    reportId = reportId,
                    reportParameters = reportParameters,
                    visibleFilters = visibleFilters
                }
            };
        }

        private static List<ReportParameter> getReportParameters(DateTime reportDay)
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

        private static string GetBodyVisibleFilterDateRange(DateTime reportDate)
        {
            const string filterDatePattern = "M'/'d'/'yy";
            return string.Format("{0} - {0}", reportDate.ToString(filterDatePattern));
        }

        public static string GetReportParameterFilterDate(DateTime reportDate)
        {
            const string parameterDatePattern = "yyyy''MM''dd";
            return reportDate.ToString(parameterDatePattern);
        }
    }
}
