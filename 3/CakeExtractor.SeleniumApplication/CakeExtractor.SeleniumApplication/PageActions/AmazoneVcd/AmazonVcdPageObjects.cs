using OpenQA.Selenium;

namespace CakeExtractor.SeleniumApplication.PageActions.AmazoneVcd
{
    internal class AmazonVcdPageObjects
    {
        public static By DownloadButton = By.XPath("//button[@type='button' and contains(., 'Download')]");

        public static By DownloadDetailsViewCsvOption = By.XPath("//a[contains(.,'As CSV')]");
    }
}
