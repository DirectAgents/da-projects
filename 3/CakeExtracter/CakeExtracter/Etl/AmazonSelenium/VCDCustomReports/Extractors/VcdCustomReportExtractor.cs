using System;
using System.Collections.Generic;
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
        where TProduct : VcdCustomProduct, ISumMetrics<VcdCustomProduct>
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

        private VcdCustomReportData<TProduct> TryExtractData()
        {
            var data = Policy<VcdCustomReportData<TProduct>>
                .Handle<Exception>()
                .Retry(maxRetryAttemptsForExtractData, (exception, retryCount, context) =>
                    LogFailedReports(retryCount, maxRetryAttemptsForExtractData))
                .Execute(ExtractReportData);
            return data;
        }

        private VcdCustomReportData<TProduct> ExtractReportData()
        {
            var reportData = GetReportData();
            var composedData = reportComposer.ComposeCustomReportData(reportData, dateRange, reportType, period);
            composedData.ReportDateRange = dateRange;
            return composedData;
        }

        private List<TProduct> GetReportData()
        {
            Logger.Info(account.Id, $"Amazon VCD, Downloading {reportType} report.");

            var csvReport = vcdDataProvider.DownloadVcdCustomReport(accountInfo, reportType, period, dateRange.FromDate, dateRange.ToDate);
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
    }
}