using CakeExtractor.SeleniumApplication.Models.CommonHelperModels;
using OpenQA.Selenium;

namespace CakeExtractor.SeleniumApplication.PageActions.AmazonVcd
{
    internal class AmazonVcdPageActions : BaseAmazonPageActions
    {
        private const string salesDiagnosticPageUrl = "https://ara.amazon.com/analytics/dashboard/salesDiagnostic";

        public AmazonVcdPageActions(IWebDriver driver, int timeoutMinutes) : base(driver, timeoutMinutes)
        {
        }

        public string GetAccessToken()
        {
            IJavaScriptExecutor js = Driver as IJavaScriptExecutor;
            var token = js.ExecuteScript("return window.token") as string;
            return token;
        }

        public string GetUserInfoJson()
        {
            IJavaScriptExecutor js = Driver as IJavaScriptExecutor;
            var userInfoJson = js.ExecuteScript("return JSON.stringify(window.userInfo)") as string;
            return userInfoJson;
        }

        public void NavigateToSalesDiagnosticPage()
        {
            NavigateToUrl(salesDiagnosticPageUrl);
        }

        public void RefreshSalesDiagnosticPage(AuthorizationModel authorizationModel)
        {
            NavigateToSalesDiagnosticPage();
            if (AmazonVcdLoginHelper.NeedResetPassword(this))
            {
                AmazonVcdLoginHelper.ResetPassword(this, authorizationModel);
                NavigateToSalesDiagnosticPage();
            }
        }
    }
}
