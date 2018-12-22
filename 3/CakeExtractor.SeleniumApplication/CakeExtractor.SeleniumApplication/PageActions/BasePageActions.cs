﻿using System;
using System.Collections.Generic;
using System.Threading;
using CakeExtracter;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace CakeExtractor.SeleniumApplication.PageActions
{
    public class BasePageActions
    {
        protected readonly IWebDriver Driver;

        public BasePageActions(IWebDriver driver)
        {
            Driver = driver;
        }

        public void NavigateToUrl(string url, By waitingElement, TimeSpan timeout)
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

        public void SendKeys(By byElement, string keys)
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

        public bool IsElementVisible(By byElement)
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

        public bool IsElementEnabledAndDisplayed(By byElement)
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

        public bool IsElementClickable(By byElement)
        {
            return IsElementEnabledAndDisplayed(byElement);
        }

        public System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> GetChildrenElements(By parentElem,
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
                var byElement = By.TagName("tr");
                return tableElement.FindElements(byElement);
            }
            catch (Exception e)
            {
                throw new Exception($"Could not get table rows [{tableElem}]: {e.Message}", e);
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
                Logger.Warn(e.Message);
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

        protected void WaitLoading(By loaderElement, TimeSpan waitCount)
        {
            try
            {

                var wait = new WebDriverWait(Driver, waitCount);
                wait.Until(driver => IsElementPresent(loaderElement));
                wait.Until(driver => !IsElementVisible(loaderElement));
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
    }
}