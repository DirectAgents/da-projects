using OpenQA.Selenium;

namespace CakeExtractor.SeleniumApplication.PageActions.AmazonVcd
{
    internal class AmazonVcdPageActions : BaseAmazonPageActions
    {
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
    }
}
