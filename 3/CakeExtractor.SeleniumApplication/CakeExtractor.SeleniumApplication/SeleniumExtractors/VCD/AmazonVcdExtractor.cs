using CakeExtracter;
using CakeExtractor.SeleniumApplication.Configuration.Models;
using CakeExtractor.SeleniumApplication.Configuration.Vcd;
using CakeExtractor.SeleniumApplication.Helpers;
using CakeExtractor.SeleniumApplication.Models.CommonHelperModels;
using CakeExtractor.SeleniumApplication.PageActions;
using CakeExtractor.SeleniumApplication.PageActions.AmazonVcd;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.ExtractionHelpers;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.Models;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.ReportDataComposer;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.ReportParsing;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.UserInfoExtracting;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.UserInfoExtracting.Models;
using System;
using System.Collections.Generic;
using System.Threading;

namespace CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD
{
    internal class AmazonVcdExtractor
    {
        private AuthorizationModel authorizationModel;

        private AmazonVcdPageActions pageActions;

        private VcdReportDownloader reportDownloader;

        private VcdReportCSVParser reportParser;

        private VcdReportComposer reportComposer;

        private UserInfoExtracter userInfoExtractor;

        private const int ReportDownloadingFailedDelayInSeconds = 30;

        readonly VcdCommandConfigurationManager configurationManager;

        private const int ReportDownloadingAttemptCount = 5;

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

        public VcdReportData ExtractDailyData(DateTime reportDay, AccountInfo accountInfo)
        {
            try
            {
                var shippedRevenueReportData = GetShippedRevenueReportData(reportDay, accountInfo);
                Thread.Sleep(TimeSpan.FromSeconds(ReportDownloadingFailedDelayInSeconds)); // wait between report downloading requests to prevent "Too many requests response."
                var shippedCogsReportData = GetShippingCogsReportData(reportDay, accountInfo);
                var composedData = reportComposer.ComposeReportData(shippedRevenueReportData, shippedCogsReportData);
                return composedData;
            }
            catch (Exception ex)
            {
                Logger.Warn("Error occured while extracting data for {0} account on {1} date", accountInfo.Account.Id, reportDay);
                Logger.Error(ex);
                throw ex;
            }
        }

        public UserInfo ExtractAccountsInfo()
        {
            return userInfoExtractor.ExtractUserInfo(pageActions);
        }

        private List<Product> GetShippedRevenueReportData(DateTime reportDay, AccountInfo accountInfo)
        {
            Logger.Info("Amazon VCD, Downloading shipped revenue report.");
            var reportTextContent = DownloadReport(reportDay, accountInfo,
                ()=> { return reportDownloader.DownloadShippedRevenueCsvReport(reportDay, accountInfo); });
            var reportProducts = reportParser.ParseShippedRevenueReportData(reportTextContent);
            Logger.Info("Amazon VCD, Shipped revenue report downloaded. {0} products", reportProducts.Count);
            return reportProducts;
        }

        private List<Product> GetShippingCogsReportData(DateTime reportDay, AccountInfo accountInfo)
        {
            Logger.Info("Amazon VCD, Downloading shipped cogs report.");
            var reportTextContent = DownloadReport(reportDay, accountInfo,
                () => { return reportDownloader.DownloadShippedCogsCsvReport(reportDay, accountInfo); });
            var reportProducts = reportParser.ParseShippedCogsReportData(reportTextContent);
            Logger.Info("Amazon VCD, Shipped cogs report downloaded. {0} products", reportProducts.Count);
            return reportProducts;
        }

        private string DownloadReport(DateTime reportDay, AccountInfo accountInfo, Func<string> downloadingAction )
        {
            try
            {
                var reportTextContent =
                    RetryHelper.Do(downloadingAction,
                    TimeSpan.FromSeconds(ReportDownloadingFailedDelayInSeconds), ReportDownloadingAttemptCount);
                return reportTextContent;
            }
            catch (Exception ex)
            {
                Logger.Info("Report downloading failed");
                Logger.Error(ex);
                throw ex;
            }
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
