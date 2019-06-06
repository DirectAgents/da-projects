using OpenQA.Selenium;

namespace SeleniumDataBrowser.PageActions
{
    public class BaseAmazonPageObjects
    {
        public static By LoginEmailInput = By.XPath(".//input[@id='ap_email']");
        public static By LoginPassInput = By.XPath(".//input[@id='ap_password']");
        public static By CodeInput = By.Id("auth-mfa-otpcode");
        public static By RememberMeCheckBox = By.XPath(".//input[@name='rememberMe']");
        public static By LoginButton = By.Id("signInSubmit");
        public static By IncorrectPasswordSpan = By.XPath(".//span[contains(text(),'Your password is incorrect')]");
        public static By DontAskCodeCheckBox = By.CssSelector("input#auth-mfa-remember-device");
    }
}
