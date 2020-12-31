using OpenQA.Selenium;
using SeleniumDataBrowser.PageActions;

namespace SeleniumDataBrowser.VCD.PageActions
{
    /// <inheritdoc/>
    /// <summary>
    /// Class for elements of Amazon portal pages for PDA.
    /// </summary>
    internal class AmazonVcdPageObjects : AmazonLoginPageObjects
    {
        /// <summary>
        /// "Sign In to Vendor Central" button.
        /// </summary>
        public static By SignInToVendorCentralButton = By.Id("signInSubmit");

        /// <summary>
        /// Data container of Detail View on main page.
        /// </summary>
        public static By DetailViewDataContainer = By.Id("salesDiagnosticDetail");

        /// <summary>
        /// Link to Sales Diagnostic page.
        /// </summary>
        public static By SalesDiagnosticLink = By.LinkText("Sales Diagnostic");

        /// <summary>
        /// Form of switch VCD accounts.
        /// </summary>
        public static By SwitchAccountForm = By.Id("vendor-group-switch-account-form");

        /// <summary>
        /// Element of list with VCD available accounts.
        /// </summary>
        public static By AccountList = By.CssSelector("form#vendor-group-switch-account-form > div > div > div > div");

        /// <summary>
        /// Element of VCD account.
        /// </summary>
        public static By AccountItem = By.CssSelector("div > label > span");

        /// <summary>
        /// Button for switch current account.
        /// </summary>
        public static By SwitchCurrentAccountButton = By.CssSelector("span#vendor-group-switch-confirm-button");
    }
}