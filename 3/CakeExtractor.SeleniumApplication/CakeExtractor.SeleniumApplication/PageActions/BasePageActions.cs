using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace CakeExtractor.SeleniumApplication.PageActions
{
    internal class BasePageActions
    {
        protected readonly IWebDriver Driver;

        public BasePageActions(IWebDriver driver)
        {
            this.Driver = driver;
        }

        public void GoToUrl(string url)
        {
            Driver.Navigate().GoToUrl(url);
        }

        public void RefreshPage()
        {
            Driver.Navigate().Refresh();
        }

        public void ClickElement(By element)
        {
            Driver.FindElement(element).Click();
        }

        public void SendKeys(By element, string keys)
        {
            Driver.FindElement(element).SendKeys(keys);
        }

        public void WaitElement(By elem, TimeSpan waitCount)
        {
            var wait = new WebDriverWait(Driver, waitCount);
            var element = wait.Until(ExpectedConditions.ElementToBeClickable(elem));
        }
    }
}
