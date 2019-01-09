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
            WaitElementClickable(AmazonPdaPageObjects.FilterByButton, timeout);
            ClickElement(AmazonPdaPageObjects.FilterByButton);
        }
    }
}
