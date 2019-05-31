using System;
using CakeExtracter.Etl.AmazonSelenium.PDA.PageActions;
using OpenQA.Selenium;

namespace CakeExtracter.Etl.AmazonSelenium.PageActions
{
    public class BaseAmazonPageActions : BasePageActions
    {
        public BaseAmazonPageActions(IWebDriver driver, int timeoutMinutes) : base(driver, timeoutMinutes)
        {
        }

        public void LoginProcess(string email, string password)
        {
            Logger.Info("Login with e-mail [{0}]...", email);
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
            Logger.Info("Need to repeat the password...");
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

        public void LoginWithPassword(string password)
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

            var exc = new Exception("Password is incorrect");
            Logger.Error(exc);
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
            Logger.Info("Waiting the code...");
            WaitLoading(AmazonPdaPageObjects.CodeInput, timeout);
        }
    }
}