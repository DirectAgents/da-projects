using OpenQA.Selenium;

namespace SeleniumDataBrowser.GenerationReportsTrigger.PageActions
{
    public class GenerationReportsPageObjects
    {
        public static By ReportsDashboard = By.CssSelector("div[data-e2e-id='reportsDashboard']");

        public static By ReportTypeDropdown = By.CssSelector("span[data-e2e-id='reportTypeDropdown']");

        public static By ReportTypeItemList = By.CssSelector("div#a-popover-1 > div > div > ul");

        public static By ReportNameInput = By.CssSelector("input[data-e2e-id='reportNameInput']");

        public static By TimePeriodDropdown = By.CssSelector("span[data-e2e-id='timePeriodDropdown']");

        public static By TimePeriodItemList = By.CssSelector("div#a-popover-2 > div > div > ul");

        public static By DataUnitDropdown = By.CssSelector("span[data-e2e-id='dataUnitDropdown']");

        public static By DataUnitItemList = By.CssSelector("div#a-popover-3 > div > div > ul");

        public static By DropdownItem = By.CssSelector("li > a");

        public static By CreateReportButton = By.CssSelector("span[data-e2e-id='createReportButton']");

        public static By ChooseEntityText = By.XPath("//h1[contains(text(),'Choose an entity')]");

        public static By EntitiesPageMainContainer = By.CssSelector("section#main-container");

        public static By EntitiesPageRow = By.CssSelector("div.a-row > div.a-column > a");

        public static By OtherMarketplace = By.CssSelector("li[data-e2e-id='other-marketplace']");
    }
}
