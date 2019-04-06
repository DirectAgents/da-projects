using CakeExtracter;
using CakeExtractor.SeleniumApplication.Configuration.Models;
using CakeExtractor.SeleniumApplication.Configuration.Vcd;
using CakeExtractor.SeleniumApplication.Helpers;
using CakeExtractor.SeleniumApplication.Models.CommonHelperModels;
using CakeExtractor.SeleniumApplication.PageActions;
using CakeExtractor.SeleniumApplication.PageActions.AmazonVcd;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.Models;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.ReportDataComposer;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.ReportParsing;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.UserInfoExtracting;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.UserInfoExtracting.Models;
using System;
using System.Collections.Generic;
using CakeExtracter.Common;
using CakeExtracter.Etl;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.ReportDownloading;
using Polly;

namespace CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD
{
    internal class AmazonVcdExtractor : Extracter<VcdReportData>
    {
        private AuthorizationModel authorizationModel;
        private AmazonVcdPageActions pageActions;
        private VcdReportDownloader reportDownloader;
        private VcdReportCSVParser reportParser;
        private VcdReportComposer reportComposer;
        private UserInfoExtracter userInfoExtractor;
        
        public DateRange DateRange { get; set; }
        public AccountInfo AccountInfo { get; set; }

        public AmazonVcdExtractor(AmazonVcdPageActions pageActions, AuthorizationModel authorizationModel)
        {
            this.pageActions = pageActions;
            this.authorizationModel = authorizationModel;
            CreateApplicationFolders();
            reportDownloader = new VcdReportDownloader(pageActions, authorizationModel);
            reportParser = new VcdReportCSVParser();
            reportComposer = new VcdReportComposer();
            userInfoExtractor = new UserInfoExtracter();
        }

        public static bool IsInitialised
        {
            get;
            private set;
        }

        public void PrepareExtractor()
        {
            AmazonVcdLoginHelper.LoginToAmazonPortal(authorizationModel, pageActions);
            IsInitialised = true;
        }

        public UserInfo ExtractAccountsInfo()
        {
            return userInfoExtractor.ExtractUserInfo(pageActions);
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

        private void Extract(DateTime date)
        {
            try
            {
                Logger.Info(AccountInfo.Account.Id, $"Amazon VCD, ETL for {date} started. Account {AccountInfo.Account.Name} - {AccountInfo.Account.Id}");
                var data = TryExtractDailyData(date);
                Add(data);
            }
            catch (Exception e)
            {
                var exception = new Exception($"Error occured while extracting data for {AccountInfo.Account.Name} ({AccountInfo.Account.Id}) account on {date} date.", e);
                Logger.Error(AccountInfo.Account.Id, exception);
            }
        }

        private VcdReportData TryExtractDailyData(DateTime date)
        {
            var maxRetryAttempts = VcdExecutionProfileManger.Current.ProfileConfiguration.ExtractDailyDataAttemptCount;
            var data = Policy
                .Handle<Exception>()
                .Retry(maxRetryAttempts, (exception, retryCount, context) => Logger.Warn(AccountInfo.Account.Id,
                    $"Could not extract for {date} started. Account {AccountInfo.Account.Name} - {AccountInfo.Account.Id}. " +
                    $"Try extracting data again (number of retrying - {retryCount}, max attempts - {maxRetryAttempts})"))
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
            return GetReportData("Shipped Revenue", AccountInfo, reportDay,
                () => reportDownloader.DownloadShippedRevenueCsvReport(reportDay, AccountInfo),
                reportParser.ParseShippedRevenueReportData);
        }

        private List<Product> GetShippingCogsReportData(DateTime reportDay)
        {
            return GetReportData("Shipped COGS", AccountInfo, reportDay,
                () => reportDownloader.DownloadShippedCogsCsvReport(reportDay, AccountInfo),
                reportParser.ParseShippedCogsReportData);
        }

        private List<Product> GetOrderedRevenueReportData(DateTime reportDay)
        {
            return GetReportData("Ordered Revenue", AccountInfo, reportDay,
                () => reportDownloader.DownloadOrderedRevenueCsvReport(reportDay, AccountInfo),
                reportParser.ParseOrderedRevenueReportData);
        }

        private void CreateApplicationFolders()
        {
            FileManager.CreateDirectoryIfNotExist(authorizationModel.CookiesDir);
        }

        private static List<Product> GetReportData(string reportName, AccountInfo accountInfo, DateTime reportDay,
            Func<string> downloadReportFunc, Func<string, AccountInfo, DateTime, List<Product>> parseReportFunc)
        {
            Logger.Info($"Amazon VCD, Downloading {reportName} report.");
            var reportTextContent = DownloadReport(downloadReportFunc);
            var reportProducts = parseReportFunc(reportTextContent, accountInfo, reportDay);
            Logger.Info($"Amazon VCD, {reportName} report downloaded. {reportProducts.Count} products");
            return reportProducts;
        }
    }
}
