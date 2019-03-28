using OpenQA.Selenium;

namespace CakeExtractor.SeleniumApplication.PageActions.AmazonPda
{
    internal class AmazonPdaPageObjects
    {
        // Login step 1 screen objects
        public static By LoginEmailInput = By.XPath(".//input[@id='ap_email']");
        public static By LoginPassInput = By.XPath(".//input[@id='ap_password']");
        public static By LoginButton = By.Id("signInSubmit");
        public static By ForgotPassLink = By.LinkText("Forgot your password?");
        public static By RememberMeCheckBox = By.XPath(".//input[@name='rememberMe']");
        
        // Login step 2 screen objects
        public static By CodeInput = By.Id("auth-mfa-otpcode");
        public static By IncorrectPasswordSpan = By.XPath(".//span[contains(text(),'Your password is incorrect')]");
        public static By DontAskCodeCheckBox = By.CssSelector("input#auth-mfa-remember-device");

        // Campaigns page objects. Filter        
        public static By FilterResetButton = By.CssSelector("button[data-e2e-id=resetButton]");
        public static By FilterByButton = By.XPath(".//button[contains(text(),'Filter by')]");
        public static By FilterTypeButton = By.XPath(".//button[contains(@value,'Type')][contains(text(),'Type')]");
        public static By FilterByValues = By.CssSelector("button[data-e2e-id=rowFilterOperationDropdownButton]");
        public static By FilterPdaValues = By.CssSelector("div#portal > div > div > div > button[value='PDA']");
        public static By SaveSearchAndFilterButton = By.XPath(".//button[@id='saveButtonSearchAndFilter']");
        public static By FilterLoader = By.CssSelector("svg[data-icon=spinner]");

        // Campaigns page objects        
        public static By CampaignsNameContainer = By.XPath(".//div[@id='CAMPAIGNS']/div/div[2]/div[contains(@class,'BottomLeftGrid_ScrollWrapper')]/div/div");
        public static By CampaignsNamesList = By.CssSelector("div[data-udt-column-id=name-cell]");
        public static By CampaignName = By.XPath(".//div/div");
        public static By CampaignNameLink = By.TagName("a");
        public static By NavigateNextPageButton = By.CssSelector("div[data-e2e-id=campaignsDashboard] > div > div:nth-of-type(5) > div:nth-of-type(3) > button:nth-child(2)");

        // Campaigns page objects. Export
        public static By ExportButton = By.XPath(".//button[contains(@class,'sc-bdVaJa eepElq')][contains(text(),'Export')]");

        // Tabs
        public static By CampaignTabContainer = By.CssSelector("#campaign_detail_tab_set_container");
        public static By CampaignTabSet = By.CssSelector("ul#campaign_detail_tab_set");
        
        public static By CampaignSettingsTab = By.CssSelector("#campaign_settings_tab_heading");
        public static By CampaignSettingsTable = By.CssSelector("#campaign_settings_tab_content > div > table");
        public static By CampaignReportsTab = By.CssSelector("#daily_report_tab_heading");
        
        public static By StartDate = By.XPath(".//div[@id='reportDurationContainer']/div[1]/span[contains(@class,'fieldContainer')]/div[1]/div[contains(@class,'reportStartDateContainer')]/span/input");
        public static By DownloadReportButton = By.CssSelector("button[name=saveButton]");
        public static By AfterDownloadReportNoData = By.XPath(".//div[@id='reportDurationContainer']/div[contains(@class,'downloadNoData')]");
        public static By AfterDownloadReportFailed = By.CssSelector("div#reportDurationContainer > div:nth-of-type(1) > span.controlsContainer > div:nth-of-type(2) > div > div.a-alert-content");
        public static By DownloadingLoader = By.XPath(".//div[contains(@class,'loading-small')][contains(@class,'submitting')]");

        // Campaign profiles menu
        public static By CurrentProfileButton = By.CssSelector("#brandDropDown");
        public static By ProfilesMenu = By.CssSelector("#dropDownBrandNameContainer");
        public static By ProfilesMenuItemContainer = By.CssSelector("div.dropDownBrandName");
        public static By ProfilesMenuItem = By.CssSelector("span > a");
    }
}
