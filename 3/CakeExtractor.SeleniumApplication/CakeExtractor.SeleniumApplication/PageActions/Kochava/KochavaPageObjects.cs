using OpenQA.Selenium;

namespace CakeExtractor.SeleniumApplication.PageActions.Kochava
{
    internal class KochavaPageObjects
    {
        public static By LoginUserNameInput = By.XPath(".//input[@id='username']");
        public static By LoginPassInput = By.XPath(".//input[@id='password']");
        public static By LoginButton = By.XPath(".//input[@type='submit']");
       
        public static By StaySignedInCheckbox = By.XPath(".//label[@for='session-keep-alive']/span");
    }
}
