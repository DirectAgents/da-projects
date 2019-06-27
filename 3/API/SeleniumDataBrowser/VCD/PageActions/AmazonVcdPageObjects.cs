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
        /// Button for open accounts menu.
        /// </summary>
        public static By AccountsDropdownButton = By.CssSelector("a.i90-accounts-dropdown-button");

        /// <summary>
        /// Container for elements of accounts menu.
        /// </summary>
        public static By AccountsDropdownContainer = By.CssSelector("div.i90-accounts-dropdown-content");

        /// <summary>
        /// Item of accounts menu.
        /// </summary>
        public static By AccountsDropdownItem = By.CssSelector("div.i90-accounts-dropdown-item");
    }
}