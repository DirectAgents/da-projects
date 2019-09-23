using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumDataBrowser.Helpers;

namespace SeleniumDataBrowser.PageActions
{
    /// <inheritdoc />
    /// <summary>
    /// Basic class for managing actions with web-pages.
    /// </summary>
    public class ActionsWithPagesManager : IDisposable
    {
        // Timeout that web driver will wait for web-elements (configurable).
        private readonly TimeSpan timeout;

        /// <summary>
        /// Gets or sets logger for logging actions. Can be modified externally.
        /// </summary>
        public SeleniumLogger Logger { get; set; }

        /// <summary>
        /// Gets or sets Selenium web-driver.
        /// </summary>
        protected IWebDriver Driver { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionsWithPagesManager"/> class.
        /// </summary>
        /// <param name="driver">Selenium web driver.</param>
        /// <param name="timeoutMinutes">Number of minutes for waiting of elements.</param>
        /// <param name="logger">Selenium logger.</param>
        public ActionsWithPagesManager(IWebDriver driver, int timeoutMinutes, SeleniumLogger logger)
        {
            Driver = driver;
            timeout = TimeSpan.FromMinutes(timeoutMinutes);
            Logger = logger;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="ActionsWithPagesManager"/> class.
        /// For closing the web driver.
        /// </summary>
        ~ActionsWithPagesManager()
        {
            Dispose(false);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Gets the collection of cookies that web-driver uses.
        /// </summary>
        /// <returns>Collection of current cookies.</returns>
        public IEnumerable<Cookie> GetAllCookies()
        {
            var options = Driver.Manage();
            return options.Cookies.AllCookies;
        }

        /// <summary>
        /// Sets cookies to web-driver.
        /// </summary>
        /// <param name="cookie">Cookies that needs to set.</param>
        public void SetCookie(Cookie cookie)
        {
            try
            {
                var options = Driver.Manage();
                options.Cookies.AddCookie(cookie);
            }
            catch (Exception e)
            {
                Logger.LogWarning(e.Message);
            }
        }

        /// <summary>
        /// Navigates to a web-page by the specified URL
        /// and waits for the specified web-element until it becomes clickable.
        /// </summary>
        /// <param name="url">URL of the web-page.</param>
        /// <param name="waitingElement">Web-element.</param>
        public void NavigateToUrl(string url, By waitingElement)
        {
            Logger.LogInfo($"Go to URL [{url}]...");
            try
            {
                var navigation = Driver.Navigate();
                navigation.GoToUrl(url);
                WaitElementClickable(waitingElement);
            }
            catch (Exception e)
            {
                throw new Exception($"Could not navigate to URL [{url}]: {e.Message}", e);
            }
        }

        /// <summary>
        /// Navigates to a web-page by specified URL.
        /// </summary>
        /// <param name="url">URL of the web-page.</param>
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

        /// <summary>
        /// Indicates whether specified web-element is present.
        /// </summary>
        /// <param name="byElement">Web-element.</param>
        /// <returns>True / False.</returns>
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

        /// <summary>
        /// Gets the current URL from web-driver.
        /// </summary>
        /// <returns>URL.</returns>
        public string GetCurrentWindowUrl()
        {
            return Driver.Url;
        }

        /// <summary>
        /// Enters the specified characters in the specified web-element.
        /// </summary>
        /// <param name="byElement">Web-element.</param>
        /// <param name="keys">Characters.</param>
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

        /// <summary>
        /// Gets a collection of children web-element from the parent web-element.
        /// </summary>
        /// <param name="parentElem">Way to find the parent web-element.</param>
        /// <param name="childElem">Way to find children web-elements.</param>
        /// <returns>Child web-element.</returns>
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

        /// <summary>
        /// Gets the child web-element from the parent web-element.
        /// </summary>
        /// <param name="parentElem">Parent web-element.</param>
        /// <param name="childElem">Way to find a child web-element.</param>
        /// <returns>Child web-element.</returns>
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

        /// <summary>
        /// Clicks on the specified web-element.
        /// </summary>
        /// <param name="byElement">Web-element.</param>
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

        /// <summary>
        /// Moves to the specified web-element and clicks on it.
        /// </summary>
        /// <param name="byElement">Web-element.</param>
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

        /// <summary>
        /// Waits for the specified web-element until it becomes clickable.
        /// </summary>
        /// <param name="element">Web-element.</param>
        protected void WaitElementClickable(By element)
        {
            try
            {
                var wait = new WebDriverWait(Driver, timeout);
                wait.Until(driver => IsElementClickable(element));
            }
            catch (Exception e)
            {
                throw new Exception($"Could not wait the element [{element}]: {e.Message}", e);
            }
        }

        /// <summary>
        /// Waits until the specified loader web-element becomes present or visible.
        /// </summary>
        /// <param name="loaderElement">Loader web-element.</param>
        /// <param name="isElementExistInDOM">Indicates whether the web-element will be present or visible.</param>
        protected void WaitLoading(By loaderElement, bool isElementExistInDOM = false)
        {
            try
            {
                var wait = new WebDriverWait(Driver, timeout);
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

        private bool IsElementVisible(By byElement)
        {
            try
            {
                var element = Driver.FindElement(byElement);
                return element.Displayed;
            }
            catch (NoSuchElementException)
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
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        private void Dispose(bool disposing)
        {
            ReleaseUnmanagedResources();
            if (disposing)
            {
                Driver?.Dispose();
            }
        }

        private void ReleaseUnmanagedResources()
        {
            Driver?.Quit();
        }
    }
}