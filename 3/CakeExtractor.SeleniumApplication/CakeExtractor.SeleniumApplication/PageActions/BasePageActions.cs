using System;
using System.Collections.Generic;
using System.Threading;
using CakeExtractor.SeleniumApplication.Helpers;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace CakeExtractor.SeleniumApplication.PageActions
{
    public class BasePageActions
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
                WaitElementClickable(waitingElement, timeout);
            }
            catch (Exception e)
            {
                throw new Exception($"Could not navigate to URL [{url}]: {e.Message}", e);
            }
        }

        public void NavigateToUrl(string url)
        {
            try
            {
                Driver.Navigate().GoToUrl(url);                
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

        protected void WaitElementClickable(By element, TimeSpan waitCount)
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

        public bool IsElementPresent(By element)
        {
            try
            {
                Driver.FindElement(element);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        protected void WaitLoading(By loaderElement, TimeSpan waitCount)
        {
            try
            {
                var wait = new WebDriverWait(Driver, waitCount);
                wait.Until(ExpectedConditions.ElementExists(loaderElement));
                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(loaderElement));
            }
            catch (Exception e)
            {
                throw new Exception($"Could not wait the loader [{loaderElement}]: {e.Message}", e);
            }
        }

        protected void Wait(TimeSpan timeoutThread)
        {
            Thread.Sleep(timeoutThread);
        }
        
        public bool IsElementEnabledAndDisplayed(By element)
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

        public IEnumerable<Cookie> GetAllCookies()
        {
            return Driver.Manage().Cookies.AllCookies;
        }

        public Cookie GetCookie(string cookieName)
        {
            return Driver.Manage().Cookies.GetCookieNamed(cookieName);
        }

        public bool SetCookie(Cookie cookie)
        {
            try
            {
                Driver.Manage().Cookies.AddCookie(cookie);
                return true;
            }
            catch (Exception e)
            {
                FileManager.TmpConsoleLog($"Warning: {e.Message}");
                return false;
            }
        }
    }
}
