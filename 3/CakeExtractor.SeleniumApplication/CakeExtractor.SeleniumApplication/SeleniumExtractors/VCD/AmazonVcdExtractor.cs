using System;
using System.Collections.Generic;
using CakeExtracter;
using CakeExtracter.Common;
using CakeExtracter.Common.JobExecutionManagement;
using CakeExtracter.Etl;
using CakeExtractor.SeleniumApplication.Configuration.Models;
using CakeExtractor.SeleniumApplication.Configuration.Vcd;
using CakeExtractor.SeleniumApplication.Helpers;
using CakeExtractor.SeleniumApplication.Models.CommonHelperModels;
using CakeExtractor.SeleniumApplication.PageActions;
using CakeExtractor.SeleniumApplication.PageActions.AmazonVcd;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.Models;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.ReportDataComposer;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.ReportDownloading;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.ReportParsing;
using Polly;

namespace CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD
{
    internal class AmazonVcdExtractor : Extracter<VcdReportData>
    {
        private readonly AmazonVcdPageActions pageActions;
        private readonly VcdReportDownloader reportDownloader;
        private readonly VcdReportCSVParser reportParser;
        private readonly VcdReportComposer reportComposer;

        public static bool IsInitialised
        {
            get;
            private set;
        }

        public DateRange DateRange { get; set; }

        public AccountInfo AccountInfo { get; set; }

        public AmazonVcdExtractor(AmazonVcdPageActions pageActions, AuthorizationModel authorizationModel)
        {
            this.pageActions = pageActions;
            reportDownloader = new VcdReportDownloader(pageActions, authorizationModel);
            reportParser = new VcdReportCSVParser();
            reportComposer = new VcdReportComposer();
        }

        public static void PrepareExtractor(AmazonVcdPageActions pageActions, AuthorizationModel authorizationModel)
        {
            CreateApplicationFolders(authorizationModel);
            AmazonVcdLoginHelper.LoginToAmazonPortal(authorizationModel, pageActions);
            IsInitialised = true;
        }

        public void OpenAccountPage()
        {
            pageActions.SelectAccountOnPage(AccountInfo.Account);
        }

        protected override void Extract()
        {
            foreach (var date in DateRange.Dates)
            {
                Extract(date);
            }
            End();
        }

        private static void CreateApplicationFolders(AuthorizationModel authorizationModel)
        {
            FileManager.CreateDirectoryIfNotExist(authorizationModel.CookiesDir);
        }

        private void Extract(DateTime date)
        {
            try
            {
                CommandExecutionContext.Current.SetJobExecutionStateInHistory($"Date - {date.ToString()}", AccountInfo.Account.Id);
                Logger.Info(AccountInfo.Account.Id, $"Amazon VCD, ETL for {date} started. Account {AccountInfo.Account.Name} - {AccountInfo.Account.Id}");
                var data = TryExtractData(date, "daily reports", ExtractDailyData);
                Add(data);
            }
            catch (Exception e)
            {
                var exception = new Exception($"Error occurred while extracting data for {AccountInfo.Account.Name} ({AccountInfo.Account.Id}) account on {date} date.", e);
                Logger.Error(AccountInfo.Account.Id, exception);
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
                AccountInfo,
                reportDay,
                () => reportDownloader.DownloadShippedRevenueCsvReport(reportDay, AccountInfo),
                reportParser.ParseShippedRevenueReportData);
        }

        private List<Product> GetShippingCogsReportData(DateTime reportDay)
        {
            return GetReportData(
                "Shipped COGS",
                AccountInfo,
                reportDay,
                () => reportDownloader.DownloadShippedCogsCsvReport(reportDay, AccountInfo),
                reportParser.ParseShippedCogsReportData);
        }

        private List<Product> GetOrderedRevenueReportData(DateTime reportDay)
        {
            return GetReportData(
                "Ordered Revenue",
                AccountInfo,
                reportDay,
                () => reportDownloader.DownloadOrderedRevenueCsvReport(reportDay, AccountInfo),
                reportParser.ParseOrderedRevenueReportData);
        }

        private List<Product> GetReportData(
            string reportName,
            AccountInfo accountInfo,
            DateTime reportDay,
            Func<string> downloadReportFunc,
            Func<string, AccountInfo, DateTime, List<Product>> parseReportFunc)
        {
            Logger.Info($"Amazon VCD, Downloading {reportName} report.");
            var reportProducts = TryExtractData(reportDay, reportName, date =>
            {
                var reportTextContent = DownloadReport(downloadReportFunc);
                return parseReportFunc(reportTextContent, accountInfo, reportDay);
            });
            Logger.Info($"Amazon VCD, {reportName} report downloaded. {reportProducts.Count} products");
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
                          $"Account {AccountInfo.Account.Name} - {AccountInfo.Account.Id}. " +
                          $"Try extracting data again (number of retrying - {retryCount}, max attempts - {maxRetryAttempts})";
            Logger.Warn(AccountInfo.Account.Id, message);
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
                Logger.Info("Report downloading failed");
                Logger.Error(ex);
                throw;
            }
        }
    }
}
