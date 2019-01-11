using CakeExtractor.SeleniumApplication.Drivers;
using CakeExtractor.SeleniumApplication.Helpers;
using CakeExtractor.SeleniumApplication.Models.CommonHelperModels;
using CakeExtractor.SeleniumApplication.Models.Vcd;
using CakeExtractor.SeleniumApplication.PageActions;
using CakeExtractor.SeleniumApplication.PageActions.AmazoneVcd;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.ExtractionHelpers;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.ReportDownloading.Models;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.ReportParsing;
using CakeExtractor.SeleniumApplication.Settings;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Linq;

namespace CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD
{
    public class AmazonVcdExtractor
    {
        private AuthorizationModel authorizationModel;

        private AmazonVcdPageActions pageActions;

        private const string amazonBaseUrl = "https://ara.amazon.com";

        private const string amazonCsvDownloadReportUrl = "/download/csv/dashboard/salesDiagnostic";

        private string reportsDownloadDirectory;

        public void PrepareExtractor()
        {
            InitializeExtractionSettings();
            InitializeAuthorizationModel();
            InitializePageManager(); // opens google chrome application
            CreateApplicationFolders();
            AmazonLoginHelper.LoginToAmazonPortal(authorizationModel, pageActions);
            NavigateToSalesDiagnosticPage();
            ExecuteExtracting();
        }

        private void ExecuteExtracting()
        {
            var pageRequestData = GetPageDataForRequests();
            var reportTextContent = VcdReportDownloader.DownloadReportAsCSV(pageRequestData);
            var reportParsedInfo = VcdReportCSVParser.GetReportData(reportTextContent);
        }

        private ReportDownloadingRequestPageData GetPageDataForRequests()
        {
            var token = pageActions.GetAccessToken();
            var cookies = pageActions.GetAllCookies().ToDictionary(x => x.Name, x => x.Value);
            return new ReportDownloadingRequestPageData
            {
                Token = token,
                Cookies = cookies
            };
        }

        private void NavigateToSalesDiagnosticPage()
        {
            pageActions.NavigateToUrl("https://ara.amazon.com/dashboard/salesDiagnostic");
        }

        private void InitializeExtractionSettings()
        {
            reportsDownloadDirectory = VcdSettings.WebDriverDownloads;
        }

        private void CreateApplicationFolders()
        {
            FileManager.CreateDirectoryIfNotExist(reportsDownloadDirectory);
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
            var driver = new ChromeWebDriver(reportsDownloadDirectory);
            var waitPageTimeoutInMinutes = Properties.Settings.Default.WaitPageTimeoutInMinuts; // TODO: Figure out best solution for common settings
            pageActions = new AmazonVcdPageActions(driver, waitPageTimeoutInMinutes);
        }



        private const string requestbody = "{\"salesDiagnosticDetail\":{\"requestId\":\"d4897f65-d1f2-4ea4-9843-42e918d8db64\",\"reportId\":\"salesDiagnosticDetail\",\"reportParameters\":[{\"parameterId\":\"distributorView\",\"values\":[{\"val\":\"manufacturer\"}]},{\"parameterId\":\"viewFilter\",\"values\":[{\"val\":\"orderedRevenueLevel\"}]},{\"parameterId\":\"asin\",\"values\":[{\"val\":\"ALL\"}]},{\"parameterId\":\"aggregationFilter\",\"values\":[{\"val\":\"ASINLevel\"}]},{\"parameterId\":\"periodStartDay\",\"values\":[{\"val\":\"20181230\"}]},{\"parameterId\":\"periodEndDay\",\"values\":[{\"val\":\"20190105\"}]},{\"parameterId\":\"isPeriodToDate\",\"values\":[{\"val\":false}]},{\"parameterId\":\"isCustomDateRange\",\"values\":[{\"val\":\"false\"}]},{\"parameterId\":\"hideLastYear\",\"values\":[{\"val\":false}]},{\"parameterId\":\"period\",\"values\":[{\"val\":\"WEEKLY\"}]},{\"parameterId\":\"dataRefreshDate\",\"values\":[{\"val\":\"0000642909546\"}]},{\"parameterId\":\"categoryId\",\"values\":[{\"val\":\"ALL\"}]},{\"parameterId\":\"subcategoryId\",\"values\":[{\"val\":\"ALL\"}]},{\"parameterId\":\"brandId\",\"values\":[{\"val\":\"ALL\"}]},{\"parameterId\":\"parentASINVisibility\",\"values\":[{\"val\":false}]},{\"parameterId\":\"eanVisibility\",\"values\":[{\"val\":false}]},{\"parameterId\":\"isbn13Visibility\",\"values\":[{\"val\":false}]},{\"parameterId\":\"upcVisibility\",\"values\":[{\"val\":false}]},{\"parameterId\":\"janVisibility\",\"values\":[{\"val\":false}]},{\"parameterId\":\"brandVisibility\",\"values\":[{\"val\":false}]},{\"parameterId\":\"brandCodeVisibility\",\"values\":[{\"val\":false}]},{\"parameterId\":\"subcatVisibility\",\"values\":[{\"val\":false}]},{\"parameterId\":\"catVisibility\",\"values\":[{\"val\":false}]},{\"parameterId\":\"apparelSizeVisibility\",\"values\":[{\"val\":false}]},{\"parameterId\":\"apparelSizeWidthVisibility\",\"values\":[{\"val\":false}]},{\"parameterId\":\"authorVisibility\",\"values\":[{\"val\":false}]},{\"parameterId\":\"bindingVisibility\",\"values\":[{\"val\":false}]},{\"parameterId\":\"catalogNumberVisibility\",\"values\":[{\"val\":false}]},{\"parameterId\":\"colorVisibility\",\"values\":[{\"val\":false}]},{\"parameterId\":\"colorCountVisibility\",\"values\":[{\"val\":false}]},{\"parameterId\":\"manufactureOnDemandVisibility\",\"values\":[{\"val\":false}]},{\"parameterId\":\"listPriceVisibility\",\"values\":[{\"val\":false}]},{\"parameterId\":\"modelStyleVisibility\",\"values\":[{\"val\":false}]},{\"parameterId\":\"productGroupVisibility\",\"values\":[{\"val\":false}]},{\"parameterId\":\"releaseDateVisibility\",\"values\":[{\"val\":false}]}],\"visibleFilters\":{\"Program\":[\"Amazon Retail\"],\"Distributor View\":[\"Manufacturing\"],\"Sales View\":[\"Ordered Revenue\"],\"Category\":[\"ALL\"],\"Subcategory\":[\"ALL\"],\"Brand\":[\"ALL\"],\"Search for ASINs or Keywords\":[\"ALL\"],\"Reporting Range\":[\"Weekly\"],\"Viewing\":[\"12/30/18 - 1/5/19\"],\"View by\":[\"ASIN\"],\"Add\":[]}}}";
    }
}
