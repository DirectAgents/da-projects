using OpenQA.Selenium;
using SeleniumDataBrowser.PageActions;

namespace SeleniumDataBrowser.PDA.PageActions
{
    /// <inheritdoc/>
    /// <summary>
    /// Class for elements of Amazon portal pages for PDA.
    /// </summary>
    internal class AmazonPdaPageObjects : AmazonLoginPageObjects
    {
        /// <summary>
        /// Button "Filter by".
        /// </summary>
        public static By FilterByButton = By.CssSelector("button[data-e2e-id='topFilterDropdownButton']");

        /// <summary>
        /// Button for open Campaign profiles menu.
        /// </summary>
        public static By CurrentProfileButton = By.CssSelector("div[data-e2e-id='aac-account-dropdown'] > button");

        /// <summary>
        /// Campaign profiles menu.
        /// </summary>
        public static By ProfilesMenu = By.CssSelector("div#top-bar-utilities-acc-selector > ul");

        /// <summary>
        /// Item of Campaign profiles menu.
        /// </summary>
        public static By ProfilesMenuItem = By.CssSelector("li > a");
    }
}