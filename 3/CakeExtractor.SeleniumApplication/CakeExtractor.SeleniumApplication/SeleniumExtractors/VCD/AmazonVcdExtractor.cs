using CakeExtracter;
using CakeExtractor.SeleniumApplication.Configuration.Models;
using CakeExtractor.SeleniumApplication.Configuration.Vcd;
using CakeExtractor.SeleniumApplication.Drivers;
using CakeExtractor.SeleniumApplication.Helpers;
using CakeExtractor.SeleniumApplication.Models.CommonHelperModels;
using CakeExtractor.SeleniumApplication.PageActions;
using CakeExtractor.SeleniumApplication.PageActions.AmazonVcd;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.ExtractionHelpers;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.Models;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.ReportDataComposer;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.ReportParsing;
using System;
using System.Collections.Generic;

namespace CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD
{
    internal class AmazonVcdExtractor
    {
        private AuthorizationModel authorizationModel;

        private AmazonVcdPageActions pageActions;

        private VcdReportDownloader reportDownloader;

        private VcdReportCSVParser reportParser;

        private VcdReportComposer reportComposer;

        private const int ReportDownloadingDelayInSeconds = 30;
        
        readonly VcdCommandConfigurationManager configurationManager;

        private const int ReportDownloadingAttemptCount = 5;

        public AmazonVcdExtractor(VcdCommandConfigurationManager configurationManager)
        {
            this.configurationManager = configurationManager;
            InitializeAuthorizationModel();
            InitializePageManager(); // opens google chrome application
            reportDownloader = new VcdReportDownloader(pageActions, authorizationModel);
            reportParser = new VcdReportCSVParser();
            reportComposer = new VcdReportComposer();
        }

        public void PrepareExtractor()
        {
            CreateApplicationFolders();
            AmazonVcdLoginHelper.LoginToAmazonPortal(authorizationModel, pageActions);
        }

        public VcdReportData ExtractDailyData(DateTime reportDay, AccountInfo accountInfo)
        {
            var shippedRevenueReportData = GetShippedRevenueReportData(reportDay, accountInfo);
            var shippedCogsReportData = GetShippingCogsReportData(reportDay, accountInfo);
            var composedData = reportComposer.ComposeReportData(shippedRevenueReportData, shippedCogsReportData);
            return composedData;
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
                    TimeSpan.FromSeconds(ReportDownloadingDelayInSeconds), ReportDownloadingAttemptCount);
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

        private void InitializeAuthorizationModel() //ToDo: Findout way how to share settings
        {
            authorizationModel = new AuthorizationModel
            {
                Login = Properties.Settings.Default.EMail,
                Password = Properties.Settings.Default.EMailPassword,
                SignInUrl = Properties.Settings.Default.SignInPageUrl,
                CookiesDir = configurationManager.GetCookiesDirectoryPath()
            };
        }

        private void InitializePageManager()
        {
            var driver = new ChromeWebDriver(string.Empty);
            var waitPageTimeoutInMinutes = Properties.Settings.Default.WaitPageTimeoutInMinuts; // TODO: Figure out best solution for common settings
            pageActions = new AmazonVcdPageActions(driver, waitPageTimeoutInMinutes);
        }
    }
}
