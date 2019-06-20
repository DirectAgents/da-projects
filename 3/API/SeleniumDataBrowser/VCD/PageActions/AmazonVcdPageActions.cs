using System;
using System.Linq;
using OpenQA.Selenium;
using SeleniumDataBrowser.PageActions;
using SeleniumDataBrowser.Drivers;
using SeleniumDataBrowser.Helpers;
using SeleniumDataBrowser.Models;
using SeleniumDataBrowser.VCD.Helpers;

namespace SeleniumDataBrowser.VCD.PageActions
{
    /// <summary>
    /// Selenium VCD page action manager.
    /// </summary>
    /// <seealso cref="BaseAmazonPageActions" />
    public class AmazonVcdPageActions : BaseAmazonPageActions
    {
        private const string SalesDiagnosticPageUrl = "https://ara.amazon.com/analytics/dashboard/salesDiagnostic";
        private const string TypeOfAccounts = "premium";

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonVcdPageActions"/> class.
        /// </summary>
        public AmazonVcdPageActions(int waitPageTimeoutInMinutes, SeleniumLogger logger, bool isHidingBrowserWindow)
            : base(new ChromeWebDriver(string.Empty, isHidingBrowserWindow), waitPageTimeoutInMinutes, logger)
        {
        }

        /// <summary>
        /// Gets the access token that used in vcd report request as part of auth process.
        /// </summary>
        /// <returns>Access token string.</returns>
        public string GetAccessToken()
        {
            IJavaScriptExecutor js = Driver as IJavaScriptExecutor;
            var token = js.ExecuteScript("return window.token") as string;
            return token;
        }

        /// <summary>
        /// Gets the user information json from page global variable.
        /// </summary>
        /// <returns>User information json string from page.</returns>
        public string GetUserInfoJson()
        {
            IJavaScriptExecutor js = Driver as IJavaScriptExecutor;
            var userInfoJson = js.ExecuteScript("return JSON.stringify(window.userInfo)") as string;
            return userInfoJson;
        }

        /// <summary>
        /// Navigates to sales diagnostic page on ara amazon portal.
        /// </summary>
        public void NavigateToSalesDiagnosticPage()
        {
            NavigateToUrl(SalesDiagnosticPageUrl);
        }

        /// <summary>
        /// Navigates to sales diagnostic page on ara amazon portal. Reset password if needed.
        /// </summary>
        /// <param name="authorizationModel">The authorization model.</param>
        public void RefreshSalesDiagnosticPage(AuthorizationModel authorizationModel)
        {
            Logger.LogInfo("Sales diagnostic page refreshing");
            NavigateToSalesDiagnosticPage();
            if (!AmazonVcdLoginHelper.NeedRepeatPassword(this))
            {
                return;
            }
            AmazonVcdLoginHelper.RepeatPassword(this, authorizationModel);
            NavigateToSalesDiagnosticPage();
        }

        /// <summary>
        /// Selects the account on page from account dromdown on top right side of page.
        /// </summary>
        /// <param name="account">The account.</param>
        public void SelectAccountOnPage(string accountName)
        {
            try
            {
                ClickElement(AmazonVcdPageObjects.AccountsDropdownButton);
                var accountItems = Driver.FindElements(AmazonVcdPageObjects.AccountsDropdownItem);
                var accountItem = accountItems.FirstOrDefault(x => x.Text == TypeOfAccounts + accountName);
                accountItem.Click();
                WaitElementClickable(AmazonVcdPageObjects.AccountsDropdownButton);
            }
            catch (Exception e)
            {
                Logger.LogWarning($"Could not open a page for {accountName} account: {e.Message}");
            }
        }
    }
}
