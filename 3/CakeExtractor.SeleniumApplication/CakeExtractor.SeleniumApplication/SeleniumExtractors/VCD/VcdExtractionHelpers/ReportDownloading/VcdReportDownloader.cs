using CakeExtractor.SeleniumApplication.Helpers;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.ReportDownloading.Models;

namespace CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.ExtractionHelpers
{
    internal class VcdReportDownloader
    {
        private const string amazonBaseUrl = "https://ara.amazon.com";

        public static string DownloadReportAsCSV(ReportDownloadingRequestPageData requetsPageData)
        {
            var request = VcdDownloadCsvReportRequestGenerator.GenerateDownloadingReportRequest(requetsPageData);
            var response = RestRequestHelper.SendPostRequest<object>(amazonBaseUrl, request);
            string textReport = System.Text.Encoding.UTF8.GetString(response.RawBytes, 0, response.RawBytes.Length);
            return textReport;
        }
    }
}
