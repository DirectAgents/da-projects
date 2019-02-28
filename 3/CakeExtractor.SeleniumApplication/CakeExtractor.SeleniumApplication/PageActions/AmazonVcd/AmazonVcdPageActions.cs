using System;
using System.Linq;
using CakeExtracter;
using CakeExtractor.SeleniumApplication.Drivers;
using CakeExtractor.SeleniumApplication.Models.CommonHelperModels;
using DirectAgents.Domain.Entities.CPProg;
using OpenQA.Selenium;

namespace CakeExtractor.SeleniumApplication.PageActions.AmazonVcd
{
    internal class AmazonVcdPageActions : BaseAmazonPageActions
    {
        private const string SalesDiagnosticPageUrl = "https://ara.amazon.com/analytics/dashboard/salesDiagnostic";
        private const string TypeOfAccounts = "premium";

        public AmazonVcdPageActions()
            : base(new ChromeWebDriver(string.Empty), Properties.Settings.Default.WaitPageTimeoutInMinuts)
        {
        }

        public string GetAccessToken()
        {
            IJavaScriptExecutor js = Driver as IJavaScriptExecutor;
            var token = js.ExecuteScript("return window.token") as string;
            return token;
        }

        public string GetUserInfoJson()
        {
            IJavaScriptExecutor js = Driver as IJavaScriptExecutor;
            var userInfoJson = js.ExecuteScript("return JSON.stringify(window.userInfo)") as string;
            return userInfoJson;
        }

        public void NavigateToSalesDiagnosticPage()
        {
            NavigateToUrl(SalesDiagnosticPageUrl);
        }

        public void RefreshSalesDiagnosticPage(AuthorizationModel authorizationModel)
        {
            NavigateToSalesDiagnosticPage();
            if (!AmazonVcdLoginHelper.NeedResetPassword(this))
            {
                return;
            }

            AmazonVcdLoginHelper.ResetPassword(this, authorizationModel);
            NavigateToSalesDiagnosticPage();
        }

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
