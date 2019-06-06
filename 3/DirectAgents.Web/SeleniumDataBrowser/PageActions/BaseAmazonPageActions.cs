using System;
using OpenQA.Selenium;

namespace SeleniumDataBrowser.PageActions
{
    public class BaseAmazonPageActions : BasePageActions
    {
        public BaseAmazonPageActions(IWebDriver driver, int timeoutMinutes)
            : base(driver, timeoutMinutes)
        {
        }

        public void LoginProcess(string email, string password)
        {
            LogInfo($"Login with e-mail [{email}]...");
            try
            {
                LoginWithEmailAndPassword(email, password);
            }
            catch (Exception e)
            {
                throw new Exception($"Login failed [{email}]: {e.Message}", e);
            }
        }

        protected void LoginWithPassword(string password)
        {
            EnterPassword(password);
            ClickElement(BaseAmazonPageObjects.RememberMeCheckBox);
            ClickElement(BaseAmazonPageObjects.LoginButton);
            IsPasswordCorrect();
            WaitSecurityCodeIfNecessary();
        }

        private void LoginWithEmailAndPassword(string email, string password)
        {
            EnterEmail(email);
            LoginWithPassword(password);
        }

        private void IsPasswordCorrect()
        {
            if (!IsElementPresent(BaseAmazonPageObjects.IncorrectPasswordSpan))
            {
                return;
            }
            LogError("Password is incorrect");
        }

        private void EnterEmail(string email)
        {
            ClickElement(BaseAmazonPageObjects.LoginEmailInput);
            SendKeys(BaseAmazonPageObjects.LoginEmailInput, email);
        }

        private void EnterPassword(string password)
        {
            ClickElement(BaseAmazonPageObjects.LoginPassInput);
            SendKeys(BaseAmazonPageObjects.LoginPassInput, password);
        }

        private void WaitSecurityCodeIfNecessary()
        {
            if (!IsElementPresent(BaseAmazonPageObjects.CodeInput))
            {
                return;
            }

            WaitElementClickable(BaseAmazonPageObjects.CodeInput, Timeout);
            ClickElement(BaseAmazonPageObjects.DontAskCodeCheckBox);
            ClickElement(BaseAmazonPageObjects.CodeInput);
            WaitSecurityCode();
        }

        private void WaitSecurityCode()
        {
            LogInfo("Waiting the code...");
            WaitLoading(BaseAmazonPageObjects.CodeInput, Timeout);
        }
    }
}