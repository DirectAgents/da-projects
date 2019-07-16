using System;
using OpenQA.Selenium;
using SeleniumDataBrowser.Helpers;

namespace SeleniumDataBrowser.PageActions
{
    /// <inheritdoc cref="ActionsWithPagesManager"/>
    /// <summary>
    /// Class for managing actions for login process with web-pages of Amazon portals.
    /// </summary>
    public class AmazonLoginActionsWithPagesManager : ActionsWithPagesManager
    {
        /// <inheritdoc cref="ActionsWithPagesManager"/> />
        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonLoginActionsWithPagesManager" /> class.
        /// </summary>
        /// <param name="driver">Selenium web driver.</param>
        /// <param name="timeoutMinutes">Number of minutes for waiting of elements.</param>
        /// <param name="logger">Logger for selenium command.</param>
        public AmazonLoginActionsWithPagesManager(IWebDriver driver, int timeoutMinutes, SeleniumLogger logger)
            : base(driver, timeoutMinutes, logger)
        {
        }

        /// <summary>
        /// Login process: via e-mail and password, with waiting security code.
        /// </summary>
        /// <param name="email">E-mail for login.</param>
        /// <param name="password">Password of login.</param>
        public void LoginProcess(string email, string password)
        {
            Logger.LogInfo($"Login with e-mail [{email}]...");
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
            Logger.LogInfo("Need to repeat the password...");
            try
            {
                LoginWithPassword(password);
                WaitElementClickable(waitElement);
            }
            catch (Exception e)
            {
                throw new Exception($"Could not to repeat password: {e.Message}", e);
            }
        }

        /// <summary>
        /// Login process: the method enters the password,
        /// waits entering characters (if needed),
        /// checks the password is correct,
        /// waits entering the security code (is needs).
        /// </summary>
        /// <param name="password">Password to be entered.</param>
        public void LoginWithPassword(string password)
        {
            EnterPassword(password);
            ClickElement(AmazonLoginPageObjects.RememberMeCheckBox);
            ClickElement(AmazonLoginPageObjects.LoginButton);
            WaitEnterCharactersIfNeeded(password);
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
            if (!IsElementPresent(AmazonLoginPageObjects.IncorrectPasswordSpan))
            {
                return;
            }
            Logger.LogError(new Exception("Password is incorrect"));
        }

        private void WaitEnterCharactersIfNeeded(string password)
        {
            if (!IsElementPresent(AmazonLoginPageObjects.AuthWarningMessageBox))
            {
                return;
            }
            Logger.LogWarning("Waiting enter the characters...");
            EnterPassword(password);
            WaitLoading(AmazonLoginPageObjects.AuthWarningMessageBox);
        }

        private void EnterEmail(string email)
        {
            ClickElement(AmazonLoginPageObjects.LoginEmailInput);
            SendKeys(AmazonLoginPageObjects.LoginEmailInput, email);
        }

        private void EnterPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new Exception("Password is empty!");
            }
            ClickElement(AmazonLoginPageObjects.LoginPassInput);
            SendKeys(AmazonLoginPageObjects.LoginPassInput, password);
        }

        private void WaitSecurityCodeIfNecessary()
        {
            if (!IsElementPresent(AmazonLoginPageObjects.CodeInput))
            {
                return;
            }
            WaitElementClickable(AmazonLoginPageObjects.CodeInput);
            ClickElement(AmazonLoginPageObjects.DontAskCodeCheckBox);
            ClickElement(AmazonLoginPageObjects.CodeInput);
            WaitSecurityCode();
        }

        private void WaitSecurityCode()
        {
            Logger.LogWarning("Waiting the code...");
            WaitLoading(AmazonLoginPageObjects.CodeInput);
        }
    }
}