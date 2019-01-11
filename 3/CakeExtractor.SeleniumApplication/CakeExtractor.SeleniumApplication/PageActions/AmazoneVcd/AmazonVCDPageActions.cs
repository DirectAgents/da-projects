using OpenQA.Selenium;

namespace CakeExtractor.SeleniumApplication.PageActions.AmazoneVcd
{
    internal class AmazonVcdPageActions : BaseAmazonPageActions
    {
        public AmazonVcdPageActions(IWebDriver driver, int timeoutMinutes) : base(driver, timeoutMinutes)
        {
        }

        public void ClickDownloadReportButton()
        {
            WaitElementClickable(AmazonVcdPageObjects.DownloadButton, timeout);
            ClickElement(AmazonVcdPageObjects.DownloadButton);
        }

        public string GetAccessToken()
        {
            IJavaScriptExecutor js = Driver as IJavaScriptExecutor;
            var token = js.ExecuteScript("return window.token") as string;
            return token;
        }
    }
}
