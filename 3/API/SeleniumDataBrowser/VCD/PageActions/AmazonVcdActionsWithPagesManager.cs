using System;
using System.Linq;
using System.Text.RegularExpressions;
using OpenQA.Selenium;
using SeleniumDataBrowser.PageActions;
using SeleniumDataBrowser.Drivers;
using SeleniumDataBrowser.Helpers;
using SeleniumDataBrowser.Models;
using SeleniumDataBrowser.VCD.Helpers;

namespace SeleniumDataBrowser.VCD.PageActions
{
    /// <inheritdoc cref="AmazonLoginActionsWithPagesManager"/>
    /// <summary>
    /// Class for managing actions with web-pages of Amazon Vendor Central Portal.
    /// </summary>
    public class AmazonVcdActionsWithPagesManager : AmazonLoginActionsWithPagesManager
    {
        private const string SalesDiagnosticPageUrl = "https://vendorcentral.amazon.com/analytics/dashboard/salesDiagnostic";
        private const string SwitchAccountPageUrl = "https://vendorcentral.amazon.com/account/choose";
        private const string SnapshotPageUrl = "https://vendorcentral.amazon.com/analytics/dashboard/snapshot";

        /// <inheritdoc cref="AmazonLoginActionsWithPagesManager"/>
        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonVcdActionsWithPagesManager" /> class.
        /// </summary>
        /// <param name="waitPageTimeoutInMinutes">Number of minutes the web-driver will wait for web-elements.</param>
        /// <param name="isHidingBrowserWindow">Indicates whether to hide the browser window.</param>
        /// <param name="logger">Selenium data browser logger.</param>
        public AmazonVcdActionsWithPagesManager(
            int waitPageTimeoutInMinutes, bool isHidingBrowserWindow, SeleniumLogger logger)
            : base(new ChromeWebDriver(string.Empty, isHidingBrowserWindow), waitPageTimeoutInMinutes, logger)
        {
        }

        /// <summary>
        /// Gets the access token that used in vcd report request as part of auth process.
        /// </summary>
        /// <returns>Access token string.</returns>
        public string GetAccessToken()
        {
            var js = Driver as IJavaScriptExecutor;
            var token = js?.ExecuteScript("return window.token") as string;
            return token;
        }

        /// <summary>
        /// Gets the user information json from page global variable.
        /// </summary>
        /// <returns>User information json string from page.</returns>
        public string GetUserInfoJson()
        {
            var js = Driver as IJavaScriptExecutor;
            var userInfoJson = js?.ExecuteScript("return JSON.stringify(window.userInfo)") as string;
            return userInfoJson;
        }

        /// <summary>
        /// Navigates to sales diagnostic page on ara amazon portal.
        /// </summary>
        /// <param name="waitingElement">Element for waiting if needed.</param>
        public void NavigateToSalesDiagnosticPage(By waitingElement = null)
        {
            NavigateToUrl(SalesDiagnosticPageUrl);
            AvoidRedirectToSnapshotPage();
            if (waitingElement != null)
            {
                WaitElementClickable(waitingElement);
            }
        }

        /// <summary>
        /// Navigates to sales diagnostic page on ara amazon portal. Reset password if needed.
        /// </summary>
        /// <param name="authorizationModel">The authorization model.</param>
        public void RefreshSalesDiagnosticPage(AuthorizationModel authorizationModel)
        {
            Logger.LogInfo("Sales diagnostic page refreshing");
            NavigateToSalesDiagnosticPage();
            if (!VcdLoginManager.NeedRepeatPassword(this))
            {
                return;
            }
            VcdLoginManager.RepeatPassword(this, authorizationModel);
            NavigateToSalesDiagnosticPage(AmazonVcdPageObjects.DetailViewDataContainer);
        }

        /// <summary>
        /// Selects the current account from account list on the separate page for choose current account.
        /// </summary>
        /// <param name="accountName">Name of the current account.</param>
        /// <returns>True if successfully select.</returns>
        public bool SelectAccountOnPage(string accountName)
        {
            try
            {
                NavigateToUrl(SwitchAccountPageUrl, AmazonVcdPageObjects.SwitchAccountForm);
                var accountItem = GetAccountElementForClicking(accountName);
                accountItem?.Click();
                ClickElement(AmazonVcdPageObjects.SwitchCurrentAccountButton);
                NavigateToSalesDiagnosticPage(AmazonVcdPageObjects.DetailViewDataContainer);
                return true;
            }
            catch (Exception e)
            {
                Logger.LogWarning($"Could not open a page for {accountName} account: {e.Message}");
                return false;
            }
        }

        /// <summary>
        /// Navigates to sign in to Vendor Central page and clicks on the Sign In button.
        /// </summary>
        /// <param name="signInPageUrl">URL to the Sign In page.</param>
        public void NavigateToSignInPage(string signInPageUrl)
        {
            NavigateToUrl(signInPageUrl, AmazonVcdPageObjects.SignInToVendorCentralButton);
            WaitElementClickable(AmazonVcdPageObjects.SignInToVendorCentralButton);
            ClickElement(AmazonVcdPageObjects.SignInToVendorCentralButton);
        }

        private IWebElement GetAccountElementForClicking(string accountName)
        {
            var accountItems = GetChildrenElements(AmazonVcdPageObjects.AccountList, AmazonVcdPageObjects.AccountItem);
            var accountItem = accountItems.FirstOrDefault(x => IsMatchAccountItemToName(x.Text, accountName));
            return accountItem;
        }

        private bool IsMatchAccountItemToName(string accountElementText, string accountName)
        {
            var pattern = $@"\s{Regex.Escape(accountName)}";
            var regex = new Regex(pattern);
            return regex.IsMatch(accountElementText);
        }

        private void AvoidRedirectToSnapshotPage()
        {
            var isRedirectedToSnapshotPage = string.Equals(GetCurrentWindowUrl(), SnapshotPageUrl, StringComparison.OrdinalIgnoreCase);
            if (isRedirectedToSnapshotPage)
            {
                WaitElementClickable(AmazonVcdPageObjects.SalesDiagnosticLink);
                ClickElement(AmazonVcdPageObjects.SalesDiagnosticLink);
            }
        }
    }
}