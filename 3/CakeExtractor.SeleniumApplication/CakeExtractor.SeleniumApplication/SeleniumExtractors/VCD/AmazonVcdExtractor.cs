using CakeExtractor.SeleniumApplication.Drivers;
using CakeExtractor.SeleniumApplication.Helpers;
using CakeExtractor.SeleniumApplication.Models.CommonHelperModels;
using CakeExtractor.SeleniumApplication.PageActions;
using CakeExtractor.SeleniumApplication.PageActions.AmazonVcd;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.ExtractionHelpers;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.Models;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.ReportDownloading.Models;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.ReportParsing;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.UserInfoExtracting;
using CakeExtractor.SeleniumApplication.Settings;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using System;
using System.Linq;

namespace CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD
{
    internal class AmazonVcdExtractor
    {
        private AuthorizationModel authorizationModel;

        private AmazonVcdPageActions pageActions;

        public void PrepareExtractor()
        {
            InitializeAuthorizationModel();
            InitializePageManager(); // opens google chrome application
            CreateApplicationFolders();
            AmazonLoginHelper.LoginToAmazonPortal(authorizationModel, pageActions);
            NavigateToSalesDiagnosticPage();
        }

        public VcdReportData ExtractVendorCentralData(DateTime reportDay)
        {
            var pageRequestData = GetPageDataForReportRequest();
            var vendorGroupId = GetVendorGroupId();
            var reportTextContent = VcdReportDownloader.DownloadReportAsCSV(pageRequestData, reportDay, vendorGroupId);
            var reportParsedInfo = VcdReportCSVParser.GetReportData(reportTextContent);
            return reportParsedInfo;
        }

        private ReportDownloadingRequestPageData GetPageDataForReportRequest()
        {
            var token = pageActions.GetAccessToken();
            var cookies = pageActions.GetAllCookies().ToDictionary(x => x.Name, x => x.Value);
            return new ReportDownloadingRequestPageData
            {
                Token = token,
                Cookies = cookies
            };
        }

        private string GetVendorGroupId()
        {
            var userInfo = UserInfoExtracter.ExtractUserInfo(pageActions);
            return userInfo.activeVendorGroupId.ToString();
        }

        private void NavigateToSalesDiagnosticPage()
        {
            pageActions.NavigateToUrl("https://ara.amazon.com/dashboard/salesDiagnostic");
        }

        private void CreateApplicationFolders()
        {
            FileManager.CreateDirectoryIfNotExist(authorizationModel.CookiesDir);
        }

        private void InitializeAuthorizationModel() //ToDo: Findout way how to share settings
        {
            var cookieDir = Properties.Settings.Default.CookiesDirectory;
            authorizationModel = new AuthorizationModel
            {
                Login = Properties.Settings.Default.EMail,
                Password = Properties.Settings.Default.EMailPassword,
                SignInUrl = Properties.Settings.Default.SignInPageUrl,
                CookiesDir = VcdSettings.CookiesDirectory
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
