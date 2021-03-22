using System;
using System.Collections.Generic;
using CakeExtracter.Common;
using CakeExtracter.Common.JobExecutionManagement;
using CakeExtracter.Etl.AmazonSelenium.VCD.Extractors.VcdExtractionHelpers.ReportDataComposer;
using CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Extractors.VcdCustomReportsExtractionHelpers.ReportParsing;
using CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Models;
using CakeExtracter.Logging.TimeWatchers.Amazon;
using DirectAgents.Domain.Entities.CPProg;
using Polly;
using SeleniumDataBrowser.VCD;
using SeleniumDataBrowser.VCD.Models;

namespace CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Extractors
{
    /// <summary>
    /// Extractor for GeographicSalesInsights statistics.
    /// </summary>
    internal class GeographicSalesInsightsExtractor : Extracter<VcdGeographicSalesInsightsReportData>
    {
        private readonly VcdDataProvider vcdDataProvider;
        private readonly GeographicSalesInsightsReportCsvParser reportParser;
        private readonly VcdReportComposer reportComposer;
        private readonly ExtAccount account;
        private readonly DateRange dateRange;
        private readonly VcdAccountInfo accountInfo;
        private readonly int maxRetryAttemptsForExtractData;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeographicSalesInsightsExtractor"/> class.
        /// </summary>
        /// <param name="account">Current account.</param>
        /// <param name="accountInfo">VCD info for a current account.</param>
        /// <param name="dateRange">Date range for extracting.</param>
        /// <param name="vcdDataProvider">VCD data provider instance.</param>
        /// <param name="maxRetryAttemptsForExtractData">Count of maximum attempts for extracting daily data.</param>
        public GeographicSalesInsightsExtractor(
            ExtAccount account,
            VcdAccountInfo accountInfo,
            DateRange dateRange,
            VcdDataProvider vcdDataProvider,
            int maxRetryAttemptsForExtractData)
        {
            this.account = account;
            this.dateRange = dateRange;
            this.vcdDataProvider = vcdDataProvider;
            this.maxRetryAttemptsForExtractData = maxRetryAttemptsForExtractData;
            this.accountInfo = accountInfo;
            reportParser = new GeographicSalesInsightsReportCsvParser(account.Id);
            reportComposer = new VcdReportComposer();
        }

        /// <inheritdoc/>
        /// <summary>
        /// Extracts Vendor Central statistics.
        /// </summary>
        protected override void Extract()
        {
            try
            {
                AmazonTimeTracker.Instance.ExecuteWithTimeTracking(
                    () =>
                    {
                        foreach (var date in dateRange.Dates)
                        {
                            Extract(date);
                        }
                    },
                    account.Id,
                    "VCD Data Extractor",
                    AmazonJobOperations.ReportExtracting);
            }
            finally
            {
                End();
            }
        }

        private void Extract(DateTime date)
        {
            try
            {
                CommandExecutionContext.Current.SetJobExecutionStateInHistory($"Date - {date}", account.Id);
                Logger.Info(account.Id, $"Amazon VCD, ETL for {date} started. Account {account.Name} - {account.Id}");
                var data = TryExtractData(date, "geographic sales insights reports", ExtractGeographicSalesInsightsData);
                data.Date = date;
                Add(data);
            }
            catch (Exception e)
            {
                var exception = new Exception($"Error occurred while extracting data for {account.Name} ({account.Id}) account on {date} date.", e);
                Logger.Error(account.Id, exception);
                throw exception;
            }
        }

        private VcdGeographicSalesInsightsReportData ExtractGeographicSalesInsightsData(DateTime reportDay)
        {
            var geographicSalesInsightsData = GetGeographicsInsightsData(reportDay);
            var composedData = reportComposer.ComposeGeographicSalesInsightsReportData(geographicSalesInsightsData);
            composedData.Date = reportDay;
            return composedData;
        }

        private List<GeographicSalesInsightsProduct> GetGeographicsInsightsData(DateTime reportDay)
        {
            return GetReportData(
                "GeographicSalesInsights",
                reportDay,
                () => vcdDataProvider.DownloadGeographicSalesInsightsCsvReport(accountInfo, reportDay),
                reportParser.ParseGeographicSalesInsightReportData);
        }

        private List<GeographicSalesInsightsProduct> GetReportData(
           string reportName,
           DateTime reportDay,
           Func<string> downloadReportFunc,
           Func<string, DateTime, List<GeographicSalesInsightsProduct>> parseReportFunc)
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
