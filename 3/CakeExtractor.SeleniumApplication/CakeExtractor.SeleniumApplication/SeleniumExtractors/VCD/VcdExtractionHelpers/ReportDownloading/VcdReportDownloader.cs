using CakeExtractor.SeleniumApplication.Helpers;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.ReportDownloading.Models;
using System;

namespace CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.ExtractionHelpers
{
    internal class VcdReportDownloader
    {
        private const string amazonBaseUrl = "https://ara.amazon.com";

        public static string DownloadReportAsCSV(ReportDownloadingRequestPageData requetsPageData, DateTime reportDay, string vendorGroupId)
        {
            var request = VcdDownloadCsvReportRequestGenerator.GenerateDownloadingReportRequest(requetsPageData, reportDay, vendorGroupId);
            var response = RestRequestHelper.SendPostRequest<object>(amazonBaseUrl, request);
            string textReport = System.Text.Encoding.UTF8.GetString(response.RawBytes, 0, response.RawBytes.Length);
            return textReport;
        }
    }
}
