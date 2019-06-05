using System;
using OpenQA.Selenium;

namespace SeleniumDataBrowser.PageActions
{
    public class BaseAmazonPageActions : BasePageActions
    {
        public BaseAmazonPageActions(IWebDriver driver, int timeoutMinutes, 
            Action<string> logInfo, Action<string> logError, Action<string> logWarning)
            : base(driver, timeoutMinutes, logInfo, logError, logWarning)
        {
        }

        public void LoginProcess(string email, string password)
        {
            logInfo($"Login with e-mail [{email}]...");
            try
            {
                LoginWithEmailAndPassword(email, password);
            }
            catch (Exception e)
            {
                throw new Exception($"Login failed [{email}]: {e.Message}", e);
            }
        }

        public void LoginByPassword(string password, By waitElement = null)
        {
            logInfo("Need to repeat the password...");
            try
            {
                LoginWithPassword(password);
                if (waitElement != null)
                {
                    WaitElementClickable(waitElement, timeout);
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Could not to repeat password: {e.Message}", e);
            }
        }

        private void LoginWithEmailAndPassword(string email, string password)
        {
            EnterEmail(email);
            LoginWithPassword(password);
        }

        private void LoginWithPassword(string password)
        {
            EnterPassword(password);
            ClickElement(AmazonPdaPageObjects.RememberMeCheckBox);
            ClickElement(AmazonPdaPageObjects.LoginButton);
            IsPasswordCorrect();
            WaitSecurityCodeIfNecessary();
        }

        private void IsPasswordCorrect()
        {
            if (!IsElementPresent(AmazonPdaPageObjects.IncorrectPasswordSpan))
            {
                return;
            }

            logError("Password is incorrect");
        }

        private void EnterEmail(string email)
        {
            ClickElement(AmazonPdaPageObjects.LoginEmailInput);
            SendKeys(AmazonPdaPageObjects.LoginEmailInput, email);
        }

        private void EnterPassword(string password)
        {
            ClickElement(AmazonPdaPageObjects.LoginPassInput);
            SendKeys(AmazonPdaPageObjects.LoginPassInput, password);
        }

        private void WaitSecurityCodeIfNecessary()
        {
            if (!IsElementPresent(AmazonPdaPageObjects.CodeInput))
            {
                return;
            }

            WaitElementClickable(AmazonPdaPageObjects.CodeInput, timeout);
            ClickElement(AmazonPdaPageObjects.DontAskCodeCheckBox);
            ClickElement(AmazonPdaPageObjects.CodeInput);
            WaitSecurityCode();
        }

        private void WaitSecurityCode()
        {
            logInfo("Waiting the code...");
            WaitLoading(AmazonPdaPageObjects.CodeInput, timeout);
        }
    }
}