using CakeExtractor.SeleniumApplication.Helpers;
using CakeExtractor.SeleniumApplication.Models.Vcd;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.ReportDownloading.Constants;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.ReportDownloading.Models;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Newtonsoft.Json;
using RestSharp;

namespace CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.ExtractionHelpers
{
    internal class VcdDownloadCsvReportRequestGenerator
    {
        private const string amazonCsvDownloadReportUrl = "/download/csv/dashboard/salesDiagnostic";

        private const string requestBodyFormat = "application/json";

        private const string reportId = "salesDiagnosticDetail";

        public static RestRequest GenerateDownloadingReportRequest(ReportDownloadingRequestPageData requestPageData)
        {
            var requestBodyObject = PrepareInitialRequestBody();
            var requestHeaders = RequestHeaderConstants.GetHeadersDictionary();
            var requestBodyJson = JsonConvert.SerializeObject(requestBodyObject);
            var requestQueryParams = RequestQueryConstants.GetRequiestQueryparameters(requestPageData.Token);
            var request = RestRequestHelper.CreateRestRequest(amazonCsvDownloadReportUrl, requestPageData.Cookies, requestQueryParams, null);
            request.AddParameter(requestBodyFormat, requestBodyJson, ParameterType.RequestBody);
            requestHeaders.ForEach(x => request.AddHeader(x.Key, x.Value));
            return request;
        }

        private static DownloadReportRequestBody PrepareInitialRequestBody()
        {
            return new DownloadReportRequestBody()
            {
                salesDiagnosticDetail = new SalesDiagnosticDetail
                {
                    requestId = "0c00cf37-0e17-45d0-9437-332b417733f4", //Parametrize reportId id
                    reportId = reportId, 
                    reportParameters = RequestBodyConstants.GetReportParameters(),
                    visibleFilters = RequestBodyConstants.GetInitialVisibleFilters()
                }
            };
        }
    }
}
