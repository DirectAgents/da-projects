using CakeExtracter;
using CakeExtractor.SeleniumApplication.Drivers;
using CakeExtractor.SeleniumApplication.Helpers;
using CakeExtractor.SeleniumApplication.Models.CommonHelperModels;
using CakeExtractor.SeleniumApplication.PageActions;
using CakeExtractor.SeleniumApplication.PageActions.AmazoneVcd;
using CakeExtractor.SeleniumApplication.Settings;

namespace CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD
{
    public class AmazonVcdExtractor
    {
        private AuthorizationModel authorizationModel;

        private AmazonVcdPageActions pageActions;

        private string reportsDownloadDirectory;

        public void PrepareExtractor()
        {
            InitializeExtractionSettings();
            InitializeAuthorizationModel();
            InitializePageManager(); // opens google chrome application
            CreateApplicationFolders();
            AmazonLoginHelper.LoginToAmazonPortal(authorizationModel, pageActions);
            NavigateToSalesDiagnosticPage();
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
    }
}
