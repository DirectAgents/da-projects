using System;
using System.Collections.Generic;
using System.Threading;
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
        
        public void NavigateToUrl(string url, By waitingElement, TimeSpan timeout)
        {
            try
            {
                Driver.Navigate().GoToUrl(url);
                WaitElement(waitingElement, timeout);
            }
            catch (Exception e)
            {
                throw new Exception($"Could not navigate to URL [{url}]: {e.Message}", e);
            }
        }

        public void RefreshPage()
        {
            Driver.Navigate().Refresh();
        }
        
        protected void ClickElement(By element)
        {
            try
            {
                Driver.FindElement(element).Click();
            }
            catch (Exception e)
            {
                throw new Exception($"Could not click on the element [{element}]: {e.Message}", e);
            }
        }
        
        public void SendKeys(By element, string keys)
        {
            try
            {
                Driver.FindElement(element).SendKeys(keys);
            }
            catch (Exception e)
            {
                throw new Exception($"Could not send keys on the element [{element}]: {e.Message}", e);
            }
        }

        protected void WaitElement(By element, TimeSpan waitCount)
        {
            try
            {
                var wait = new WebDriverWait(Driver, waitCount);
                wait.Until(ExpectedConditions.ElementToBeClickable(element));
            }
            catch (Exception e)
            {
                throw new Exception($"Could not wait the element [{element}]: {e.Message}", e);
            }
        }

        protected void Wait(TimeSpan timeoutThread)
        {
            Thread.Sleep(timeoutThread);
        }

        public bool IsElementEnabled(By element)
        {
            return Driver.FindElement(element).Enabled;
        }

        public bool IsElementDisplayed(By element)
        {
            return Driver.FindElement(element).Displayed && Driver.FindElement(element).Enabled;
        }

        public System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> GetChildrenElements(By parentElem, By childElem)
        {
            try
            {
                var parentElement = Driver.FindElement(parentElem);
                return parentElement.FindElements(childElem);
            }
            catch (Exception e)
            {
                throw new Exception($"Could not get children elements [{childElem}] from parent [{parentElem}]: {e.Message}", e);
            }
        }

        public IWebElement GetChildElement(IWebElement parentElem, By childElem)
        {
            try
            {
                return parentElem.FindElement(childElem);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IList<IWebElement> GetTableRows(By tableElem)
        {
            try
            {
                var tableElement = Driver.FindElement(tableElem);
                return tableElement.FindElements(By.TagName("tr"));
            }
            catch (Exception e)
            {
                throw new Exception($"Could not get table rows [{tableElem}]: {e.Message}", e);
            }
        }
    }
}
