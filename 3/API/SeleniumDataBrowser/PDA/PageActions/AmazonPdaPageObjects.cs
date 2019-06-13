using OpenQA.Selenium;
using SeleniumDataBrowser.PageActions;

namespace SeleniumDataBrowser.PDA.PageActions
{
    internal class AmazonPdaPageObjects : BaseAmazonPageObjects
    {
        public static By FilterByButton = By.XPath(".//button[contains(text(),'Filter by')]");
        // Campaign profiles menu
        public static By CurrentProfileButton = By.CssSelector("#brandDropDown");
        public static By ProfilesMenu = By.CssSelector("#dropDownBrandNameContainer");
        public static By ProfilesMenuItemContainer = By.CssSelector("div.dropDownBrandName");
        public static By ProfilesMenuItem = By.CssSelector("span > a");
    }
}
