using System;
using System.Collections.Generic;
using CakeExtracter.Common;
using CakeExtracter.Common.JobExecutionManagement;
using CakeExtracter.Etl.AmazonSelenium.VCD.Configuration;
using CakeExtracter.Etl.AmazonSelenium.VCD.Configuration.Models;
using CakeExtracter.Etl.AmazonSelenium.VCD.Extractors.VcdExtractionHelpers.ReportDataComposer;
using CakeExtracter.Etl.AmazonSelenium.VCD.Extractors.VcdExtractionHelpers.ReportParsing;
using CakeExtracter.Etl.AmazonSelenium.VCD.Models;
using Polly;
using SeleniumDataBrowser.VCD.Helpers.ReportDownloading;

namespace CakeExtracter.Etl.AmazonSelenium.VCD.Extractors
{
    /// <summary>
    /// Extractor for Amazon Vendor Central statistics.
    /// </summary>
    internal class AmazonVcdExtractor : Extracter<VcdReportData>
    {
        private readonly VcdReportDownloader reportDownloader;
        private readonly VcdReportCSVParser reportParser;
        private readonly VcdReportComposer reportComposer;

        private readonly AccountInfo accountInfo;
        private readonly DateRange dateRange;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonVcdExtractor"/> class.
        /// </summary>
        /// <param name="accountInfo">Information about the current account.</param>
        /// <param name="dateRange">Date range for extracting.</param>
        /// <param name="reportDownloader">Downloader of reports.</param>
        public AmazonVcdExtractor(AccountInfo accountInfo, DateRange dateRange, VcdReportDownloader reportDownloader)
        {
            this.accountInfo = accountInfo;
            this.dateRange = dateRange;
            this.reportDownloader = reportDownloader;

            reportParser = new VcdReportCSVParser();
            reportComposer = new VcdReportComposer();
        }

        /// <inheritdoc/>
        /// <summary>
        /// Extracts Vendor Central statistics.
        /// </summary>
        protected override void Extract()
        {
            foreach (var date in dateRange.Dates)
            {
                Extract(date);
            }
            End();
        }

        private void Extract(DateTime date)
        {
            try
            {
                CommandExecutionContext.Current.SetJobExecutionStateInHistory($"Date - {date.ToString()}", accountInfo.Account.Id);
                Logger.Info(accountInfo.Account.Id, $"Amazon VCD, ETL for {date} started. Account {accountInfo.Account.Name} - {accountInfo.Account.Id}");
                var data = TryExtractData(date, "daily reports", ExtractDailyData);
                Add(data);
            }
            catch (Exception e)
            {
                var exception = new Exception($"Error occurred while extracting data for {accountInfo.Account.Name} ({accountInfo.Account.Id}) account on {date} date.", e);
                Logger.Error(accountInfo.Account.Id, exception);
                throw exception;
            }
        }

        private VcdReportData ExtractDailyData(DateTime reportDay)
        {
            var shippedRevenueReportData = GetShippedRevenueReportData(reportDay);
            var shippedCogsReportData = GetShippingCogsReportData(reportDay);
            var orderedRevenueReportData = GetOrderedRevenueReportData(reportDay);
            var composedData = reportComposer.ComposeReportData(shippedRevenueReportData, shippedCogsReportData, orderedRevenueReportData);
            composedData.Date = reportDay;
            return composedData;
        }

        private List<Product> GetShippedRevenueReportData(DateTime reportDay)
        {
            return GetReportData(
                "Shipped Revenue",
                reportDay,
                () => reportDownloader.DownloadShippedRevenueCsvReport(reportDay),
                reportParser.ParseShippedRevenueReportData);
        }

        private List<Product> GetShippingCogsReportData(DateTime reportDay)
        {
            return GetReportData(
                "Shipped COGS",
                reportDay,
                () => reportDownloader.DownloadShippedCogsCsvReport(reportDay),
                reportParser.ParseShippedCogsReportData);
        }

        private List<Product> GetOrderedRevenueReportData(DateTime reportDay)
        {
            return GetReportData(
                "Ordered Revenue",
                reportDay,
                () => reportDownloader.DownloadOrderedRevenueCsvReport(reportDay),
                reportParser.ParseOrderedRevenueReportData);
        }

        private List<Product> GetReportData(
            string reportName,
            DateTime reportDay,
            Func<string> downloadReportFunc,
            Func<string, AccountInfo, DateTime, List<Product>> parseReportFunc)
        {
            Logger.Info(accountInfo.Account.Id, $"Amazon VCD, Downloading {reportName} report.");
            var reportProducts = TryExtractData(reportDay, reportName, date =>
            {
                var reportTextContent = DownloadReport(downloadReportFunc);
                return parseReportFunc(reportTextContent, accountInfo, reportDay);
            });
            Logger.Info(accountInfo.Account.Id, $"Amazon VCD, {reportName} report downloaded. {reportProducts.Count} products");
            return reportProducts;
        }

        private T TryExtractData<T>(DateTime date, string dataName, Func<DateTime, T> extractDataFunc)
        {
            var maxRetryAttempts = VcdExecutionProfileManger.Current.ProfileConfiguration.ExtractDailyDataAttemptCount;
            var data = Policy
                .Handle<Exception>()
                .Retry(
                    maxRetryAttempts,
                    (exception, retryCount, context) => LogFailedReports(date, retryCount, maxRetryAttempts, dataName))
                .Execute(() => extractDataFunc(date));
            return data;
        }

        private void LogFailedReports(DateTime date, int retryCount, int maxRetryAttempts, string dataName)
        {
            var message = $"Could not extract {dataName} for {date} started. " +
                          $"Account {accountInfo.Account.Name} - {accountInfo.Account.Id}. " +
                          $"Try extracting data again (number of retrying - {retryCount}, max attempts - {maxRetryAttempts})";
            Logger.Warn(accountInfo.Account.Id, message);
        }

        private string DownloadReport(Func<string> downloadingAction)
        {
            try
            {
                var reportTextContent = downloadingAction();
                return reportTextContent;
            }
            catch (Exception ex)
            {
                Logger.Info(accountInfo.Account.Id, "Report downloading failed");
                Logger.Error(accountInfo.Account.Id, ex);
                throw;
            }
        }
    }
}
