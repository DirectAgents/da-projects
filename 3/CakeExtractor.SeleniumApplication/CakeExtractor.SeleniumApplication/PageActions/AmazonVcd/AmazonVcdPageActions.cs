using System;
using System.Linq;
using CakeExtracter;
using CakeExtractor.SeleniumApplication.Drivers;
using CakeExtractor.SeleniumApplication.Models.CommonHelperModels;
using DirectAgents.Domain.Entities.CPProg;
using OpenQA.Selenium;

namespace CakeExtractor.SeleniumApplication.PageActions.AmazonVcd
{
    /// <summary>
    /// Selenium VCD page action manager.
    /// </summary>
    /// <seealso cref="CakeExtractor.SeleniumApplication.PageActions.BaseAmazonPageActions" />
    internal class AmazonVcdPageActions : BaseAmazonPageActions
    {
        private const string SalesDiagnosticPageUrl = "https://ara.amazon.com/analytics/dashboard/salesDiagnostic";
        private const string TypeOfAccounts = "premium";

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonVcdPageActions"/> class.
        /// </summary>
        public AmazonVcdPageActions()
            : base(new ChromeWebDriver(string.Empty), Properties.Settings.Default.WaitPageTimeoutInMinuts)
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
            Logger.Info("Sales diagnostic page refreshing");
            NavigateToSalesDiagnosticPage();
            if (!AmazonVcdLoginHelper.NeedResetPassword(this))
            {
                return;
            }
            AmazonVcdLoginHelper.ResetPassword(this, authorizationModel);
            NavigateToSalesDiagnosticPage();
        }

        /// <summary>
        /// Selects the account on page from account dromdown on top right side of page.
        /// </summary>
        /// <param name="account">The account.</param>
        public void SelectAccountOnPage(ExtAccount account)
        {
            try
            {
                ClickElement(AmazonVcdPageObjects.AccountsDropdownButton);
                var accountItems = Driver.FindElements(AmazonVcdPageObjects.AccountsDropdownItem);
                var accountItem = accountItems.FirstOrDefault(x => x.Text == TypeOfAccounts + account.Name);
                accountItem.Click();
                WaitElementClickable(AmazonVcdPageObjects.AccountsDropdownButton, timeout);
            }
            catch (Exception e)
            {
                Logger.Warn(account.Id, $"Could not open a page for {account.Name} account: {e.Message}");
            }
        }
    }
}
