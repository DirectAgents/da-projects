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
using System.Threading;
using CakeExtracter.Common;
using CakeExtracter.Etl;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.ReportDownloading;

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
        readonly VcdCommandConfigurationManager configurationManager;

        public DateRange DateRange { get; set; }
        public AccountInfo AccountInfo { get; set; }

        public AmazonVcdExtractor(VcdCommandConfigurationManager configurationManager)
        {
            this.configurationManager = configurationManager;
        }

        public void PrepareExtractor()
        {
            InitializeAuthorizationModel();
            CreateApplicationFolders();
            pageActions = new AmazonVcdPageActions();
            reportDownloader = new VcdReportDownloader(pageActions, authorizationModel);
            reportParser = new VcdReportCSVParser();
            reportComposer = new VcdReportComposer();
            userInfoExtractor = new UserInfoExtracter();
            AmazonVcdLoginHelper.LoginToAmazonPortal(authorizationModel, pageActions);
        }

        public UserInfo ExtractAccountsInfo()
        {
            return userInfoExtractor.ExtractUserInfo(pageActions);
        }

        public void OpenAccountPage()
        {
            pageActions.SelectAccountOnPage(AccountInfo.Account.Name);
        }

        protected override void Extract()
        {
            var accId = AccountInfo.Account.Id;
            var accName = AccountInfo.Account.Name;
            foreach (var date in DateRange.Dates)
            {
                Extract(date, accId, accName);
            }
            End();
        }

        private void Extract(DateTime date, int accId, string accName)
        {
            try
            {
                Logger.Info($"Amazon VCD, ETL for {date} started. Account {accName} - {accId}");
                var data = ExtractDailyData(date, AccountInfo);
                Add(data);
            }
            catch (Exception e)
            {
                var exception = new Exception($"Error occured while extracting data for {accName} ({accId}) account on {date} date.", e);
                Logger.Error(exception);
            }
        }

        private VcdReportData ExtractDailyData(DateTime reportDay, AccountInfo accountInfo)
        {
            var shippedRevenueReportData = GetShippedRevenueReportData(reportDay, accountInfo);
            var shippedCogsReportData = GetShippingCogsReportData(reportDay, accountInfo);
            var orderedRevenueReportData = GetOrderedRevenueReportData(reportDay, accountInfo);
            var composedData = reportComposer.ComposeReportData(shippedRevenueReportData, shippedCogsReportData, orderedRevenueReportData);
            composedData.Date = reportDay;
            return composedData;
        }

        private static List<Product> GetReportData(string reportName, Func<string> downloadReportFunc, Func<string, List<Product>> parseReportFunc)
        {
            Logger.Info($"Amazon VCD, Downloading {reportName} report.");
            var reportTextContent = DownloadReport(downloadReportFunc);
            var reportProducts = parseReportFunc(reportTextContent);
            Logger.Info($"Amazon VCD, {reportName} report downloaded. {reportProducts.Count} products");
            return reportProducts;
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

        private List<Product> GetShippedRevenueReportData(DateTime reportDay, AccountInfo accountInfo)
        {
            return GetReportData("Shipped Revenue",
                () => reportDownloader.DownloadShippedRevenueCsvReport(reportDay, accountInfo),
                reportParser.ParseShippedRevenueReportData);
        }

        private List<Product> GetShippingCogsReportData(DateTime reportDay, AccountInfo accountInfo)
        {
            return GetReportData("Shipped COGS",
                () => reportDownloader.DownloadShippedCogsCsvReport(reportDay, accountInfo),
                reportParser.ParseShippedCogsReportData);
        }

        private List<Product> GetOrderedRevenueReportData(DateTime reportDay, AccountInfo accountInfo)
        {
            return GetReportData("Ordered Revenue",
                () => reportDownloader.DownloadOrderedRevenueCsvReport(reportDay, accountInfo),
                reportParser.ParseOrderedRevenueReportData);
        }

        private void CreateApplicationFolders()
        {
            FileManager.CreateDirectoryIfNotExist(authorizationModel.CookiesDir);
        }

        private void InitializeAuthorizationModel()
        {
            authorizationModel = new AuthorizationModel
            {
                Login = Properties.Settings.Default.EMail,
                Password = Properties.Settings.Default.EMailPassword,
                SignInUrl = Properties.Settings.Default.SignInPageUrl,
                CookiesDir = configurationManager.GetCookiesDirectoryPath()
            };
        }
    }
}
