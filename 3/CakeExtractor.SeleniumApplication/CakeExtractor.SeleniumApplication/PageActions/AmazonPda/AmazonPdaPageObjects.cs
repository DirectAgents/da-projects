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

        // Login step 2 screen objects
        public static By CodeInput = By.Id("auth-mfa-otpcode");

        // Home screen objects
        public static By AccountButton = By.CssSelector("button[data-id=user-account]");

        // Campaigns page objects. Filter
        //public static By FilterByButton = By.XPath(".//button[contains(@type,'button')][contains(text(),'Filter by')]");
        public static By FilterByButton = By.XPath(".//div[contains(@class,'page-container')]/div/div[contains(@data-e2e-id,'campaignsDashboard')]/div/div[1]/div[2]/label/button[contains(text(),'Filter by')]");
        public static By FilterTypeButton = By.XPath(".//button[contains(@value,'Type')][contains(text(),'Type')]");
        public static By FilterByValues = By.XPath(".//div[contains(@class,'sc-cHGsZl bJrez')]/label/button");
        public static By FilterPdaValues = By.XPath(".//div[contains(@class,'sc-kgoBCf fQYKbt')]/div/button[contains(@value,'PDA')]");        
        public static By SaveSearchAndFilterButton = By.XPath(".//button[@id='saveButtonSearchAndFilter']");

        // Campaigns page objects        
        public static By CampaignsContainer = By.XPath(".//div[@id='CAMPAIGNS']/div/div[2]/div/div/div");
        public static By CampaignsNamesList = By.CssSelector("div[data-udt-column-id=name-cell]");
        public static By CampaignName = By.XPath(".//div/div");
        public static By CampaignNameLink = By.TagName("a"); 

        // Campaigns page objects. Export
        public static By ExportButton = By.XPath(".//button[contains(@class,'sc-bdVaJa eepElq')][contains(text(),'Export')]");

        // Campaign page objects
        public static By CampaignDataContainer = By.XPath(".//div[@id='cm-app']/div[contains(@class,'page-container')]/div[2]/div[2]/div[4]");
        public static By CampaignTabContainer = By.XPath(".//div[@id='campaign_detail_tab_set_container']");

        public static By CampaignSettingsTab = By.XPath(".//div[@id='campaign_detail_tab_set_container']/ul[@id='campaign_detail_tab_set']/li[@id='campaign_settings_tab_heading']/a");
        public static By CampaignSettingsContent = By.XPath(".//div[@id='campaign_settings_tab_content']");
        public static By CampaignSettingsTable = By.XPath(".//div[@id='campaign_settings_tab_content']/div/table");

        public static By CampaignReportsTab = By.XPath(".//div[@id='campaign_detail_tab_set_container']/ul[@id='campaign_detail_tab_set']/li[@id='daily_report_tab_heading']/a");
        public static By CampaignReportsContent = By.XPath(".//div[@id='daily_report_tab_content']");
        public static By DownloadReportButton = By.CssSelector("button[name=saveButton]");
        public static By AfterDownloadReportSuccess = By.XPath(".//div[@id='reportDurationContainer']/div[contains(@class,'downloadSuccess')]");
        public static By AfterDownloadReportNoData = By.XPath(".//div[@id='reportDurationContainer']/div[contains(@class,'downloadNoData')]");
        
        public static By NavigateNextPageButton = By.XPath(".//div[contains(@class,'sc-gtfDJT gMbShB')]/button[2]");

        // Campaign profiles menu
        public static By CurrentProfileButton = By.XPath(".//div[@id='brandDropDown']");
        public static By ProfilesMenu = By.XPath(".//div[@id='dropDownBrandNameContainer']");
        public static By ProfilesMenuItemContainer = By.XPath(".//div[contains(@class,'dropDownBrandName')]");
        public static By ProfilesMenuItem = By.XPath(".//span/a");
    }
}
