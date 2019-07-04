using System;
using System.Collections.Generic;
using CakeExtracter.Common;
using CakeExtracter.Common.JobExecutionManagement;
using CakeExtracter.Etl.AmazonSelenium.VCD.Extractors.VcdExtractionHelpers.ReportDataComposer;
using CakeExtracter.Etl.AmazonSelenium.VCD.Extractors.VcdExtractionHelpers.ReportParsing;
using CakeExtracter.Etl.AmazonSelenium.VCD.Models;
using DirectAgents.Domain.Entities.CPProg;
using Polly;
using SeleniumDataBrowser.VCD;

namespace CakeExtracter.Etl.AmazonSelenium.VCD.Extractors
{
    /// <summary>
    /// Extractor for Amazon Vendor Central statistics.
    /// </summary>
    internal class AmazonVcdExtractor : Extracter<VcdReportData>
    {
        private readonly VcdDataProvider vcdDataProvider;
        private readonly VcdReportCSVParser reportParser;
        private readonly VcdReportComposer reportComposer;
        private readonly ExtAccount account;
        private readonly DateRange dateRange;
        private readonly int maxRetryAttemptsForExtractData;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonVcdExtractor"/> class.
        /// </summary>
        /// <param name="account">Current account.</param>
        /// <param name="dateRange">Date range for extracting.</param>
        /// <param name="vcdDataProvider">VCD data provider instance.</param>
        /// <param name="maxRetryAttemptsForExtractData">Count of maximum attempts for extracting daily data.</param>
        public AmazonVcdExtractor(
            ExtAccount account,
            DateRange dateRange,
            VcdDataProvider vcdDataProvider,
            int maxRetryAttemptsForExtractData)
        {
            this.account = account;
            this.dateRange = dateRange;
            this.vcdDataProvider = vcdDataProvider;
            this.maxRetryAttemptsForExtractData = maxRetryAttemptsForExtractData;
            reportParser = new VcdReportCSVParser(account.Id);
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
                CommandExecutionContext.Current.SetJobExecutionStateInHistory($"Date - {date.ToString()}", account.Id);
                Logger.Info(account.Id, $"Amazon VCD, ETL for {date} started. Account {account.Name} - {account.Id}");
                var data = TryExtractData(date, "daily reports", ExtractDailyData);
                Add(data);
            }
            catch (Exception e)
            {
                var exception = new Exception($"Error occurred while extracting data for {account.Name} ({account.Id}) account on {date} date.", e);
                Logger.Error(account.Id, exception);
                throw exception;
            }
        }

        private VcdReportData ExtractDailyData(DateTime reportDay)
        {
            var shippedRevenueReportData = GetShippedRevenueReportData(reportDay);
            var shippedCogsReportData = GetShippingCogsReportData(reportDay);
            var orderedRevenueReportData = GetOrderedRevenueReportData(reportDay);
            var composedData = reportComposer.ComposeReportData(
                shippedRevenueReportData, shippedCogsReportData, orderedRevenueReportData);
            composedData.Date = reportDay;
            return composedData;
        }

        private List<Product> GetShippedRevenueReportData(DateTime reportDay)
        {
            return GetReportData(
                "Shipped Revenue",
                reportDay,
                () => vcdDataProvider.DownloadShippedRevenueCsvReport(reportDay),
                reportParser.ParseShippedRevenueReportData);
        }

        private List<Product> GetShippingCogsReportData(DateTime reportDay)
        {
            return GetReportData(
                "Shipped COGS",
                reportDay,
                () => vcdDataProvider.DownloadShippedCogsCsvReport(reportDay),
                reportParser.ParseShippedCogsReportData);
        }

        private List<Product> GetOrderedRevenueReportData(DateTime reportDay)
        {
            return GetReportData(
                "Ordered Revenue",
                reportDay,
                () => vcdDataProvider.DownloadOrderedRevenueCsvReport(reportDay),
                reportParser.ParseOrderedRevenueReportData);
        }

        private List<Product> GetReportData(
            string reportName,
            DateTime reportDay,
            Func<string> downloadReportFunc,
            Func<string, DateTime, List<Product>> parseReportFunc)
        {
            Logger.Info(account.Id, $"Amazon VCD, Downloading {reportName} report.");
            var reportProducts = TryExtractData(reportDay, reportName, date =>
            {
                var reportTextContent = DownloadReport(downloadReportFunc);
                return parseReportFunc(reportTextContent, reportDay);
            });
            Logger.Info(account.Id, $"Amazon VCD, {reportName} report downloaded. {reportProducts.Count} products");
            return reportProducts;
        }

        private T TryExtractData<T>(DateTime date, string dataName, Func<DateTime, T> extractDataFunc)
        {
            var data = Policy
                .Handle<Exception>()
                .Retry(maxRetryAttemptsForExtractData, (exception, retryCount, context) =>
                        LogFailedReports(date, retryCount, maxRetryAttemptsForExtractData, dataName))
                .Execute(() => extractDataFunc(date));
            return data;
        }

        private void LogFailedReports(DateTime date, int retryCount, int maxRetryAttempts, string dataName)
        {
            var message = $"Could not extract {dataName} for {date} started. " +
                          $"Account {account.Name} - {account.Id}. " +
                          $"Try extracting data again (number of retrying - {retryCount}, max attempts - {maxRetryAttempts})";
            Logger.Warn(account.Id, message);
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
                Logger.Info(account.Id, "Report downloading failed");
                Logger.Error(account.Id, ex);
                throw;
            }
        }
    }
}