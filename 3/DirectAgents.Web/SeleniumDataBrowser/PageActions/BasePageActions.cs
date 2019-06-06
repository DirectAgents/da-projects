using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace SeleniumDataBrowser.PageActions
{
    public class BasePageActions
    {
        protected const string HrefAttribute = "href";

        protected readonly IWebDriver Driver;
        protected readonly TimeSpan Timeout;

        public static Action<string> LogInfo;

        public static Action<string> LogError;

        public static Action<string> LogWarning;

        public BasePageActions(IWebDriver driver, int timeoutMinutes)
        {
            Driver = driver;
            Timeout = TimeSpan.FromMinutes(timeoutMinutes);
        }

        ~BasePageActions()
        {
            CloseWebDriver();
        }

        public void NavigateToUrl(string url, By waitingElement)
        {
            LogInfo($"Go to URL [{url}]...");
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

        protected System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> GetChildrenElements(By parentElem,
            By childElem)
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
                LogWarning(e.Message);
                return false;
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