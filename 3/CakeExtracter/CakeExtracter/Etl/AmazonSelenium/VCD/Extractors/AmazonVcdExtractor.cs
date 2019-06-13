using System;
using System.Collections.Generic;
using CakeExtracter.Common;
using CakeExtracter.Common.JobExecutionManagement;
using CakeExtracter.Etl.AmazonSelenium.VCD.Configuration;
using CakeExtracter.Etl.AmazonSelenium.VCD.Configuration.Models;
using CakeExtracter.Etl.AmazonSelenium.VCD.Models;
using CakeExtracter.Etl.AmazonSelenium.VCD.VcdExtractionHelpers.ReportDataComposer;
using CakeExtracter.Etl.AmazonSelenium.VCD.VcdExtractionHelpers.ReportParsing;
using Polly;
using SeleniumDataBrowser.VCD.Helpers.ReportDownloading;

namespace CakeExtracter.Etl.AmazonSelenium.VCD.Extractors
{
    internal class AmazonVcdExtractor : Extracter<VcdReportData>
    {
        private VcdReportDownloader reportDownloader;
        private VcdReportCSVParser reportParser;
        private VcdReportComposer reportComposer;

        private AccountInfo accountInfo;
        private DateRange dateRange;

        public AmazonVcdExtractor(AccountInfo accountInfo, DateRange dateRange, VcdReportDownloader reportDownloader)
        {
            this.accountInfo = accountInfo;
            this.dateRange = dateRange;
            this.reportDownloader = reportDownloader;

            reportParser = new VcdReportCSVParser();
            reportComposer = new VcdReportComposer();
        }

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
                var data = TryExtractDailyData(date);
                Add(data);
            }
            catch (Exception e)
            {
                var exception = new Exception($"Error occurred while extracting data for {accountInfo.Account.Name} ({accountInfo.Account.Id}) account on {date} date.", e);
                Logger.Error(accountInfo.Account.Id, exception);
            }
        }

        private VcdReportData TryExtractDailyData(DateTime date)
        {
            var maxRetryAttempts = VcdExecutionProfileManger.Current.ProfileConfiguration.ExtractDailyDataAttemptCount;
            var data = Policy
                .Handle<Exception>()
                .Retry(maxRetryAttempts, (exception, retryCount, context) =>
                    Logger.Warn(
                        accountInfo.Account.Id,
                        $"Could not extract for {date} started. Account {accountInfo.Account.Name} - {accountInfo.Account.Id}. Try extracting data again (number of retrying - {retryCount}, max attempts - {maxRetryAttempts})"))
                .Execute(() => ExtractDailyData(date));
            return data;
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

        private static string DownloadReport(Func<string> downloadingAction)
        {
            try
            {
                var reportTextContent = downloadingAction();
                return reportTextContent;
            }
            catch (Exception ex)
            {
                Logger.Info("Report downloading failed");
                Logger.Error(ex);
                throw ex;
            }
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

        private List<Product> GetReportData(string reportName, DateTime reportDay, Func<string> downloadReportFunc, Func<string, AccountInfo, DateTime, List<Product>> parseReportFunc)
        {
            Logger.Info($"Amazon VCD, Downloading {reportName} report.");
            var reportTextContent = DownloadReport(downloadReportFunc);
            var reportProducts = parseReportFunc(reportTextContent, accountInfo, reportDay);
            Logger.Info($"Amazon VCD, {reportName} report downloaded. {reportProducts.Count} products");
            return reportProducts;
        }
    }
}
