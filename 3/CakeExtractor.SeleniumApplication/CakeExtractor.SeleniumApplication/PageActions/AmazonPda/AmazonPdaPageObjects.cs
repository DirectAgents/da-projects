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
        public static By SignInButton = By.XPath(".//input[@id='auth-signin-button']");

        // Home screen objects
        public static By AccountButton = By.CssSelector("button[data-id=user-account]"); 

        // Campaigns page objects. Filter
        public static By FilterByButton = By.XPath(".//button[contains(@type,'button')][contains(text(),'Filter by')]");
        public static By FilterTypeButton = By.XPath(".//button[contains(@value,'Type')][contains(text(),'Type')]");
        public static By FilterByValues = By.XPath(".//div[contains(@class,'sc-cHGsZl bJrez')]/label/button");
        public static By FilterPdaValues = By.XPath(".//div[contains(@class,'sc-kgoBCf fQYKbt')]/div/button[contains(@value,'PDA')]");        
        public static By SaveSearchAndFilterButton = By.XPath(".//button[@id='saveButtonSearchAndFilter']");

        // Campaigns page objects
        public static By ChartContainer = By.XPath(".//div[contains(@class,'Chart__chartContainer__238nH')]");

        // Campaigns page objects. Export
        public static By ExportButton = By.XPath(".//button[contains(@class,'sc-bdVaJa eepElq')][contains(text(),'Export')]");
    }
}
