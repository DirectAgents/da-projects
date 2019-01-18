﻿using CakeExtracter;
using CakeExtractor.SeleniumApplication.Configuration.Models;
using CakeExtractor.SeleniumApplication.Configuration.Vcd;
using CakeExtractor.SeleniumApplication.Drivers;
using CakeExtractor.SeleniumApplication.Helpers;
using CakeExtractor.SeleniumApplication.Models.CommonHelperModels;
using CakeExtractor.SeleniumApplication.PageActions;
using CakeExtractor.SeleniumApplication.PageActions.AmazonVcd;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.ExtractionHelpers;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.Models;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.ReportParsing;
using System;

namespace CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD
{
    internal class AmazonVcdExtractor
    {
        private AuthorizationModel authorizationModel;

        private AmazonVcdPageActions pageActions;

        private const int ReportDownloadingDelayInSeconds = 30;
        
        readonly VcdCommandConfigurationManager configurationManager;

        private const int ReportDownloadingAttemptCount = 5;
        public AmazonVcdExtractor(VcdCommandConfigurationManager configurationManager)
        {
            this.configurationManager = configurationManager;
        }

        public void PrepareExtractor()
        {
            InitializeAuthorizationModel();
            InitializePageManager(); // opens google chrome application
            CreateApplicationFolders();
            AmazonVcdLoginHelper.LoginToAmazonPortal(authorizationModel, pageActions);
            pageActions.NavigateToSalesDiagnosticPage();
        }

        public VcdReportData ExtractDailyData(DateTime reportDay, AccountInfo accountInfo)
        {
            var reportTextContent = DownloadReport(reportDay, accountInfo);
            var reportData = ParseReport(reportTextContent);
            return reportData;
        }

        private string DownloadReport(DateTime reportDay, AccountInfo accountInfo)
        {
            try
            {
                var reportDownloader = new VcdReportDownloader(pageActions, authorizationModel, accountInfo);
                var reportTextContent =
                    RetryHelper.Do(() => { return reportDownloader.DownloadReportAsCsvText(reportDay); },
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

        private VcdReportData ParseReport(string reportTextContent)
        {
            try
            {
                Logger.Info("Amazon VCD, Report parsing started.");
                var parser = new VcdReportCSVParser();
                var reportParsedInfo = parser.ParseReportData(reportTextContent);
                Logger.Info("Amazon VCD, Report parsed successfully. Prod:{0}, Cat:{1}, Subcat:{2}",
                    reportParsedInfo.Products.Count, reportParsedInfo.Categories.Count, reportParsedInfo.Subcategories.Count);
                return reportParsedInfo;
            }
            catch (Exception ex)
            {
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
