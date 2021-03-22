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

namespace CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Extractors.VcdCustomReportsExtractionHelpers
{
    /// <summary>
    /// Extractor for NetPpm statistics.
    /// </summary>
    internal class NetPpmExtractor : Extracter<NetPpmReportData>
    {
        private readonly VcdDataProvider vcdDataProvider;
        private readonly NetPpmReportCsvParser reportParser;
        private readonly VcdReportComposer reportComposer;
        private readonly ExtAccount account;
        private readonly VcdAccountInfo accountInfo;
        private readonly int maxRetryAttemptsForExtractData;
        private readonly string period;

        /// <summary>
        /// Initializes a new instance of the <see cref="NetPpmExtractor"/> class.
        /// </summary>
        /// <param name="account">Current account.</param>
        /// <param name="accountInfo">VCD info for a current account.</param>
        /// <param name="dateRange">Date range for extracting.</param>
        /// <param name="vcdDataProvider">VCD data provider instance.</param>
        /// <param name="maxRetryAttemptsForExtractData">Count of maximum attempts for extracting daily data.</param>
        /// <param name="period">Time period for extraction</param>
        public NetPpmExtractor(
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
            reportParser = new NetPpmReportCsvParser(account.Id);
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
                        var data = TryExtractData("net ppm reports", period, ExtractNetPpmData);
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

        private NetPpmReportData ExtractNetPpmData(string reportPeriod)
        {
            var NetPpmData = GetNetPpmData(reportPeriod);
            var composedData = reportComposer.ComposeNetPpmReportData(NetPpmData);
            var dates = getDatesByPeriod();
            composedData.StartDate = dates.Item1;
            composedData.EndDate = dates.Item2;
            return composedData;
        }

        private Tuple<DateTime, DateTime> getDatesByPeriod()
        {
            DateTime lastPeriodDate;
            DateTime periodStartDay;
            DateTime periodEndDay;
            var culture = Thread.CurrentThread.CurrentCulture;
            switch (period)
            {
                case "WEEKLY":
                    lastPeriodDate = DateTime.Today.AddDays(-7);
                    var diff = lastPeriodDate.DayOfWeek - culture.DateTimeFormat.FirstDayOfWeek;
                    if (diff < 0)
                    {
                        diff += 7;
                    }
                    periodStartDay = lastPeriodDate.AddDays(-diff).Date;
                    periodEndDay = lastPeriodDate.AddDays(-diff + 6).Date;
                    break;
                case "MONTHLY":
                    lastPeriodDate = DateTime.Today.AddMonths(-1);
                    periodStartDay = new DateTime(lastPeriodDate.Year, lastPeriodDate.Month, 1);
                    periodEndDay = periodStartDay.AddMonths(1).AddDays(-1);
                    break;
                case "YEARLY":
                    lastPeriodDate = DateTime.Today.AddYears(-1);
                    periodStartDay = new DateTime(lastPeriodDate.Year, 1, 1);
                    periodEndDay = new DateTime(lastPeriodDate.Year, 12, 31);
                    break;
                default:
                    periodStartDay = DateTime.Today;
                    periodEndDay = DateTime.Today;
                    break;
            }
            return Tuple.Create(periodStartDay, periodEndDay);
        }

        private List<NetPpmProduct> GetNetPpmData(string reportPeriod)
        {
            return GetReportData(
                reportPeriod,
                "netPPM",
                () => vcdDataProvider.DownloadNetPpmCsvReport(accountInfo, reportPeriod),
                reportParser.ParseNetPpmReportData);
        }

        private List<NetPpmProduct> GetReportData(
           string reportPeriod,
           string reportName,
           Func<string> downloadReportFunc,
           Func<string, string, List<NetPpmProduct>> parseReportFunc)
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
