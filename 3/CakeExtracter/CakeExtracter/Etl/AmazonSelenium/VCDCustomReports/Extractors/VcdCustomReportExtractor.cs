using System;
using System.Collections.Generic;
using System.Threading;
using CakeExtracter.Common;
using CakeExtracter.Common.JobExecutionManagement;
using CakeExtracter.Etl.AmazonSelenium.VCD.Configuration;
using CakeExtracter.Etl.AmazonSelenium.VCD.Extractors.VcdExtractionHelpers.ReportDataComposer;
using CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Extractors.VcdCustomReportsExtractionHelpers;
using CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Extractors.VcdCustomReportsExtractionHelpers.ReportParsing;
using CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Extractors.VcdCustomReportsExtractionHelpers.ReportParsing.RowMaps;
using CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Models;
using CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Models.Base;
using CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Models.Interface;
using CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Models.Products;
using CakeExtracter.Logging.TimeWatchers.Amazon;
using DirectAgents.Domain.Entities.CPProg;
using Polly;
using SeleniumDataBrowser.VCD;
using SeleniumDataBrowser.VCD.Enums;
using SeleniumDataBrowser.VCD.Models;

namespace CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Extractors
{
    internal class VcdCustomReportExtractor<TProduct, TProductRowMap> : Extracter<VcdCustomReportData<TProduct>>
        where TProduct : VcdCustomProduct, ISumMetrics<TProduct>
        where TProductRowMap : BaseCustomReportRowMap<TProduct>
    {
        private readonly VcdDataProvider vcdDataProvider;
        private readonly VcdCustomReportParser<TProduct> reportParser;
        private readonly VcdReportComposer reportComposer;
        private readonly ExtAccount account;
        private readonly VcdAccountInfo accountInfo;
        private readonly int maxRetryAttemptsForExtractData;
        private readonly PeriodType period;
        private readonly ReportType reportType;
        private readonly DateRange dateRange;

        private readonly Dictionary<PeriodType, Func<ReportType, DateRange, List<DateRange>>> dateRangesDictionary
           = new Dictionary<PeriodType, Func<ReportType, DateRange, List<DateRange>>>
       {
           { PeriodType.DAILY, GetDailyDateRanges },
           { PeriodType.WEEKLY, GetWeeklyDateRanges },
           { PeriodType.MONTHLY, GetMonthlyDateRanges },
           { PeriodType.QUARTERLY, GetQuarterlyDateRanges },
           { PeriodType.YEARLY, GetYearlyDateRanges },
       };

        /// <summary>
        /// Initializes a new instance of the <see cref="VcdCustomReportEx1tractor"/> class.
        /// </summary>
        public VcdCustomReportExtractor(VcdCustomReportExtractorParameters<TProduct> reportExtractorParameters)
        {
            account = reportExtractorParameters.Account;
            vcdDataProvider = reportExtractorParameters.DataProvider;
            accountInfo = reportExtractorParameters.AccountInfo;
            period = reportExtractorParameters.Period;
            reportType = reportExtractorParameters.ReportType;
            dateRange = reportExtractorParameters.DateRange;
            maxRetryAttemptsForExtractData = VcdCommandConfigurationManager.GetExtractDailyDataAttemptCount();
            reportParser = new VcdCustomReportParser<TProduct>(account.Id);
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
                        var data = TryExtractData();
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

        private List<VcdCustomReportData<TProduct>> TryExtractData()
        {
            var data = Policy<List<VcdCustomReportData<TProduct>>>
                .Handle<Exception>()
                .Retry(maxRetryAttemptsForExtractData, (exception, retryCount, context) =>
                    LogFailedReports(retryCount, maxRetryAttemptsForExtractData))
                .Execute(ExtractReportData);
            return data;
        }

        private List<VcdCustomReportData<TProduct>> ExtractReportData()
        {
            var dateRanges = dateRangesDictionary[period](reportType, dateRange);
            var composedDataList = new List<VcdCustomReportData<TProduct>>();
            foreach (var dateRangeForReport in dateRanges)
            {
                var reportData = GetReportData(dateRangeForReport);
                composedDataList.Add(reportComposer.ComposeCustomReportData(reportData, dateRangeForReport, reportType, period));
            }
            return composedDataList;
        }

        private List<TProduct> GetReportData(DateRange dateRangeForReport)
        {
            Logger.Info(account.Id, $"Amazon VCD, Downloading {reportType} report.");

            var csvReport = vcdDataProvider.DownloadVcdCustomReport(accountInfo, reportType, period, dateRangeForReport.FromDate, dateRangeForReport.ToDate);
            var parsedDate = reportParser.ParseReportData<TProductRowMap>(csvReport, period.ToString(), reportType.ToString());
            return parsedDate;
        }

        private void LogFailedReports(int retryCount, int maxRetryAttempts)
        {
            var message = $"Could not extract {reportType} for {period} period started. " +
                          $"Account {account.Name} - {account.Id}. " +
                          $"Try extracting data again (number of retrying - {retryCount}, max attempts - {maxRetryAttempts})";
            Logger.Warn(account.Id, message);
        }

        private static List<DateRange> GetDailyDateRanges(ReportType reportType, DateRange dateRange)
        {
            var dateRanges = new List<DateRange>();
            var currentDate = dateRange.FromDate;
            while (currentDate <= dateRange.ToDate)
            {
                dateRanges.Add(new DateRange(currentDate, currentDate));
                currentDate = currentDate.AddDays(1);
            }
            return dateRanges;
        }

        private static List<DateRange> GetWeeklyDateRanges(ReportType reportType, DateRange dateRange)
        {
            var dateRanges = new List<DateRange>();

            var lastPeriodDate = DateTime.Today.AddDays(-7);
            var culture = Thread.CurrentThread.CurrentCulture;

            var diff = lastPeriodDate.DayOfWeek - culture.DateTimeFormat.FirstDayOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }
            var startDate = lastPeriodDate.AddDays(-diff).Date;
            var endDate = lastPeriodDate.AddDays(-diff + 6).Date;

            dateRanges.Add(new DateRange(startDate, endDate));
            return dateRanges;
        }

        private static List<DateRange> GetMonthlyDateRanges(ReportType reportType, DateRange dateRange)
        {
            var dateRanges = new List<DateRange>();
            if (reportType == ReportType.netPPM)
            {
                var lastPeriodDate = DateTime.Today.AddMonths(-1);
                var startDate = new DateTime(lastPeriodDate.Year, lastPeriodDate.Month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);
                dateRanges.Add(new DateRange(startDate, endDate));
            }
            else
            {
                var currentDate = dateRange.ToDate;
                while (currentDate >= dateRange.FromDate)
                {
                    currentDate = currentDate.AddMonths(-1);
                    var startDate = new DateTime(currentDate.Year, currentDate.Month, 1);
                    var endDate = startDate.AddMonths(1).AddDays(-1).Date;
                    if (startDate >= dateRange.FromDate)
                    {
                        dateRanges.Add(new DateRange(startDate, endDate));
                    }
                    currentDate = startDate;
                }
            }
            return dateRanges;
        }

        private static List<DateRange> GetQuarterlyDateRanges(ReportType reportType, DateRange dateRange)
        {
            var dateRanges = new List<DateRange>();
            var currentDate = dateRange.ToDate;
            while (currentDate >= dateRange.FromDate)
            {
                currentDate = currentDate.AddMonths(-4);
                var quarterNumber = (currentDate.Month - 1) / 3 + 1;
                var startDate = new DateTime(currentDate.Year, (quarterNumber - 1) * 3 + 1, 1);
                var endDate = startDate.AddMonths(3).AddDays(-1);
                if (startDate >= dateRange.FromDate)
                {
                    dateRanges.Add(new DateRange(startDate, endDate));
                }
            }
            return dateRanges;
        }

        private static List<DateRange> GetYearlyDateRanges(ReportType reportType, DateRange dateRange)
        {
            var dateRanges = new List<DateRange>();
            var lastPeriodDate = DateTime.Today.AddYears(-1);
            var startDate = new DateTime(lastPeriodDate.Year, 1, 1);
            var endDate = new DateTime(lastPeriodDate.Year, 12, 31);
            dateRanges.Add(new DateRange(startDate, endDate));
            return dateRanges;
        }
    }
}
