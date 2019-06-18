using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumDataBrowser.Helpers;

namespace SeleniumDataBrowser.PageActions
{
    /// <summary>
    /// Basic class for managing page actions.
    /// </summary>
    public class BasePageActions
    {
        public SeleniumLogger Logger;

        protected readonly TimeSpan Timeout;

        protected readonly IWebDriver Driver;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasePageActions"/> class.
        /// </summary>
        /// <param name="driver">Selenium web driver.</param>
        /// <param name="timeoutMinutes">Number of minutes for waiting of elements.</param>
        public BasePageActions(IWebDriver driver, int timeoutMinutes, SeleniumLogger logger)
        {
            Driver = driver;
            Timeout = TimeSpan.FromMinutes(timeoutMinutes);
            Logger = logger;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="BasePageActions"/> class.
        /// For closing the web driver.
        /// </summary>
        ~BasePageActions()
        {
            CloseWebDriver();
        }

        public IEnumerable<Cookie> GetAllCookies()
        {
            var options = Driver.Manage();
            return options.Cookies.AllCookies;
        }

        public bool SetCookie(Cookie cookie)
        {
            try
            {
                var options = Driver.Manage();
                options.Cookies.AddCookie(cookie);
                return true;
            }
            catch (Exception e)
            {
                Logger.LogWarning(e.Message);
                return false;
            }
        }

        public void NavigateToUrl(string url, By waitingElement)
        {
            Logger.LogInfo($"Go to URL [{url}]...");
            NavigateToUrl(url, waitingElement, Timeout);
        }

        public void NavigateToUrl(string url)
        {
            try
            {
                var navigation = Driver.Navigate();
                navigation.GoToUrl(url);
            }
            catch (Exception e)
            {
                throw new Exception($"Could not navigate to URL [{url}]: {e.Message}", e);
            }
        }

        public bool IsElementPresent(By byElement)
        {
            try
            {
                return Driver.FindElements(byElement).Count > 0;
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to check if element [{byElement}] is present: {e.Message}", e);
            }
        }

        public string GetCurrentWindowUrl()
        {
            return Driver.Url;
        }

        protected void SendKeys(By byElement, string keys)
        {
            try
            {
                var element = Driver.FindElement(byElement);
                element.SendKeys(keys);
            }
            catch (Exception e)
            {
                throw new Exception($"Could not send keys on the element [{byElement}]: {e.Message}", e);
            }
        }

        protected ReadOnlyCollection<IWebElement> GetChildrenElements(By parentElem, By childElem)
        {
            try
            {
                var parentElement = Driver.FindElement(parentElem);
                return parentElement.FindElements(childElem);
            }
            catch (Exception e)
            {
                throw new Exception(
                    $"Could not get children elements [{childElem}] from parent [{parentElem}]: {e.Message}", e);
            }
        }

        protected IWebElement GetChildElement(IWebElement parentElem, By childElem)
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

        protected void ClickElement(By byElement)
        {
            try
            {
                var element = Driver.FindElement(byElement);
                element.Click();
            }
            catch (Exception e)
            {
                throw new Exception($"Could not click on the element [{byElement}]: {e.Message}", e);
            }
        }

        protected void MoveToElementAndClick(By byElement)
        {
            try
            {
                var element = Driver.FindElement(byElement);
                var actions = new Actions(Driver);
                actions.MoveToElement(element).Click().Perform();
            }
            catch (Exception e)
            {
                throw new Exception($"Could not move to element and click [{byElement}]: {e.Message}", e);
            }
        }

        protected void WaitElementClickable(By element, TimeSpan waitCount)
        {
            try
            {
                var wait = new WebDriverWait(Driver, waitCount);
                wait.Until(driver => IsElementClickable(element));
            }
            catch (Exception e)
            {
                throw new Exception($"Could not wait the element [{element}]: {e.Message}", e);
            }
        }

        protected void WaitLoading(By loaderElement, TimeSpan waitCount, bool isElementExistInDOM = false)
        {
            try
            {
                var wait = new WebDriverWait(Driver, waitCount);
                wait.Until(driver => IsElementPresent(loaderElement));
                if (isElementExistInDOM)
                {
                    wait.Until(driver => !IsElementVisible(loaderElement));
                }
                else
                {
                    wait.Until(driver => !IsElementPresent(loaderElement));
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Could not wait the loader [{loaderElement}]: {e.Message}", e);
            }
        }

        private void CloseWebDriver()
        {
            Driver.Quit();
        }

        private void NavigateToUrl(string url, By waitingElement, TimeSpan timeout)
        {
            try
            {
                var navigation = Driver.Navigate();
                navigation.GoToUrl(url);
                WaitElementClickable(waitingElement, timeout);
            }
            catch (Exception e)
            {
                throw new Exception($"Could not navigate to URL [{url}]: {e.Message}", e);
            }
        }

        private bool IsElementVisible(By byElement)
        {
            try
            {
                var element = Driver.FindElement(byElement);
                return element.Displayed;
            }
            catch (NoSuchElementException exc)
            {
                return false;
            }
        }

        private bool IsElementClickable(By byElement)
        {
            return IsElementEnabledAndDisplayed(byElement);
        }

        private bool IsElementEnabledAndDisplayed(By byElement)
        {
            try
            {
                var element = Driver.FindElement(byElement);
                return element.Displayed && element.Enabled;
            }
            catch (NoSuchElementException exc)
            {
                return false;
            }
        }
    }
}