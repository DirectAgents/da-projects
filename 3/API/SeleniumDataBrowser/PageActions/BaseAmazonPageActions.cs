using System;
using OpenQA.Selenium;

namespace SeleniumDataBrowser.PageActions
{
    /// <inheritdoc cref="BasePageActions"/>
    /// <summary>
    /// Class for managing page actions of Amazon portals.
    /// </summary>
    public class BaseAmazonPageActions : BasePageActions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseAmazonPageActions"/> class.
        /// </summary>
        /// <param name="driver">Selenium web driver.</param>
        /// <param name="timeoutMinutes">Number of minutes for waiting of elements.</param>
        public BaseAmazonPageActions(IWebDriver driver, int timeoutMinutes)
            : base(driver, timeoutMinutes)
        {
        }

        /// <summary>
        /// Login process: via e-mail and password, with waiting security code.
        /// </summary>
        /// <param name="email">E-mail for login.</param>
        /// <param name="password">Password of login.</param>
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

        /// <summary>
        /// Login process: if necessary, the method enters only the password and waits for the page to load.
        /// </summary>
        /// <param name="password">Password to be entered.</param>
        /// <param name="waitElement">Web element that the method will wait for after logging in.</param>
        public void LoginWithPasswordAndWaiting(string password, By waitElement)
        {
            LogInfo("Need to repeat the password...");
            try
            {
                LoginWithPassword(password);
                WaitElementClickable(waitElement, Timeout);
            }
            catch (Exception e)
            {
                throw new Exception($"Could not to repeat password: {e.Message}", e);
            }
        }

        private void LoginWithPassword(string password)
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
            LogWarning("Waiting the code...");
            WaitLoading(BaseAmazonPageObjects.CodeInput, Timeout);
        }
    }
}