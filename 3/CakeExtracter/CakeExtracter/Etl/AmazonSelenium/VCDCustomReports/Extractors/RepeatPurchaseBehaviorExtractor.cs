using System;
using System.Collections.Generic;
using System.Threading;
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
    /// Extractor for RepeatPurchaseBehavior statistics.
    /// </summary>
    internal class RepeatPurchaseBehaviorExtractor : Extracter<RepeatPurchaseBehaviorReportData>
    {
        private readonly VcdDataProvider vcdDataProvider;
        private readonly RepeatPurchaseBehaviorReportCsvParser reportParser;
        private readonly VcdReportComposer reportComposer;
        private readonly ExtAccount account;
        private readonly VcdAccountInfo accountInfo;
        private readonly int maxRetryAttemptsForExtractData;
        private readonly string period;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepeatPurchaseBehaviorExtractor"/> class.
        /// </summary>
        /// <param name="account">Current account.</param>
        /// <param name="accountInfo">VCD info for a current account.</param>
        /// <param name="dateRange">Date range for extracting.</param>
        /// <param name="vcdDataProvider">VCD data provider instance.</param>
        /// <param name="maxRetryAttemptsForExtractData">Count of maximum attempts for extracting daily data.</param>
        /// <param name="period">Time period for extraction</param>
        public RepeatPurchaseBehaviorExtractor(
            ExtAccount account,
            VcdAccountInfo accountInfo,
            VcdDataProvider vcdDataProvider,
            int maxRetryAttemptsForExtractData,
            string period)
        {
            this.account = account;
            this.vcdDataProvider = vcdDataProvider;
            this.maxRetryAttemptsForExtractData = maxRetryAttemptsForExtractData;
            this.accountInfo = accountInfo;
            this.period = period;
            reportParser = new RepeatPurchaseBehaviorReportCsvParser(account.Id);
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
                        CommandExecutionContext.Current.SetJobExecutionStateInHistory($"{period} report ", account.Id);
                        Logger.Info(account.Id, $"Amazon VCD, ETL for {period} report started. Account {account.Name} - {account.Id}");
                        var data = TryExtractData("repeat purchase behavior reports", period, ExtractRepeatPurchaseBehaviorData);
                        Add(data);
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

        private RepeatPurchaseBehaviorReportData ExtractRepeatPurchaseBehaviorData(string reportPeriod)
        {
            var RepeatPurchaseBehaviorData = GetRepeatPurchaseBehaviorData(reportPeriod);
            var composedData = reportComposer.ComposeRepeatPurchaseBehaviorReportData(RepeatPurchaseBehaviorData);
            var dates = GetDatesByPeriod(reportPeriod);
            composedData.StartDate = dates.Item1;
            composedData.EndDate = dates.Item2;
            return composedData;
        }

        private Tuple<DateTime, DateTime> GetDatesByPeriod(string reportPeriod)
        {
            DateTime lastPeriodDate;
            DateTime periodStartDay;
            DateTime periodEndDay;
            var culture = Thread.CurrentThread.CurrentCulture;

            switch (period)
            {
                case "MONTHLY":
                    lastPeriodDate = DateTime.Today.AddMonths(-1);
                    periodStartDay = new DateTime(lastPeriodDate.Year, lastPeriodDate.Month, 1);
                    periodEndDay = periodStartDay.AddMonths(1).AddDays(-1);
                    break;
                case "QUATERLY":
                    lastPeriodDate = DateTime.Today.AddMonths(-4);
                    int quarterNumber = (lastPeriodDate.Month - 1) / 3 + 1;
                    periodStartDay = new DateTime(lastPeriodDate.Year, (quarterNumber - 1) * 3 + 1, 1);
                    periodEndDay = periodStartDay.AddMonths(3).AddDays(-1);
                    break;
                default:
                    periodStartDay = DateTime.Today;
                    periodEndDay = DateTime.Today;
                    break;
            }
            return Tuple.Create(periodStartDay, periodEndDay);
        }

        private List<RepeatPurchaseBehaviorProduct> GetRepeatPurchaseBehaviorData(string reportPeriod)
        {
            return GetReportData(
                reportPeriod,
                "repeatPurchaseBehavior",
                () => vcdDataProvider.DownloadRepeatPurchaseBehaviorCsvReport(accountInfo, reportPeriod),
                reportParser.ParseRepeatPurchaseBehaviorReportData);
        }

        private List<RepeatPurchaseBehaviorProduct> GetReportData(
           string reportPeriod,
           string reportName,
           Func<string> downloadReportFunc,
           Func<string, string, List<RepeatPurchaseBehaviorProduct>> parseReportFunc)
        {
            Logger.Info(account.Id, $"Amazon VCD, Downloading {reportName} report.");

            var reportProducts = TryExtractData(reportName, reportPeriod, period =>
            {
                var reportTextContent = DownloadReport(downloadReportFunc);
                return parseReportFunc(reportTextContent, reportPeriod);
            });
            return reportProducts;
        }

        private T TryExtractData<T>(string dataName, string reportPeriod, Func<string, T> extractDataFunc)
        {
            var data = Policy
                .Handle<Exception>()
                .Retry(maxRetryAttemptsForExtractData, (exception, retryCount, context) =>
                        LogFailedReports(retryCount, maxRetryAttemptsForExtractData, dataName, reportPeriod))
                .Execute(() => extractDataFunc(reportPeriod));
            return data;
        }

        private void LogFailedReports(int retryCount, int maxRetryAttempts, string dataName, string reportPeriod)
        {
            var message = $"Could not extract {dataName} for {reportPeriod} period started. " +
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
