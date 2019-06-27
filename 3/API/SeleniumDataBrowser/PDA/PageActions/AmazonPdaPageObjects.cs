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
        public static By FilterByButton = By.XPath(".//button[contains(text(),'Filter by')]");

        /// <summary>
        /// Button for open Campaign profiles menu.
        /// </summary>
        public static By CurrentProfileButton = By.Id("brandDropDown");

        /// <summary>
        /// Campaign profiles menu.
        /// </summary>
        public static By ProfilesMenu = By.Id("dropDownBrandNameContainer");

        /// <summary>
        /// Container for elements of Campaign profiles menu.
        /// </summary>
        public static By ProfilesMenuItemContainer = By.CssSelector("div.dropDownBrandName");

        /// <summary>
        /// Item of Campaign profiles menu.
        /// </summary>
        public static By ProfilesMenuItem = By.CssSelector("span > a");
    }
}