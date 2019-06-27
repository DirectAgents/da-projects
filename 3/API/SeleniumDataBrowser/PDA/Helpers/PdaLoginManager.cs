using System.Linq;
using OpenQA.Selenium;
using SeleniumDataBrowser.Helpers;
using SeleniumDataBrowser.Models;
using SeleniumDataBrowser.PageActions;
using SeleniumDataBrowser.PDA.PageActions;

namespace SeleniumDataBrowser.PDA.Helpers
{
    /// <summary>
    /// Helper for PDA login process.
    /// </summary>
    public class PdaLoginManager
    {
        private const string SignInPageUrl = "https://advertising.amazon.com/sign-in";

        private readonly AuthorizationModel authorizationModel;
        private readonly AmazonPdaActionsWithPagesManager pageActionsManager;
        private readonly SeleniumLogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PdaLoginManager"/> class.
        /// </summary>
        /// <param name="authorizationModel">Authorization settings.</param>
        /// <param name="pageActionsManager">Page actions manager.</param>
        /// <param name="logger">Selenium data browser logger.</param>
        public PdaLoginManager(
            AuthorizationModel authorizationModel,
            AmazonPdaActionsWithPagesManager pageActionsManager,
            SeleniumLogger logger)
        {
            this.authorizationModel = authorizationModel;
            this.pageActionsManager = pageActionsManager;
            this.logger = logger;
        }

        /// <summary>
        /// Login to the advertising portal and saving cookies.
        /// </summary>
        public void LoginToPortal()
        {
            authorizationModel.Cookies = SeleniumCookieHelper.GetCookiesFromFiles(authorizationModel.CookiesDir);
            var cookiesExist = authorizationModel.Cookies.Any();
            if (cookiesExist)
            {
                logger.LogInfo("Login into the portal using cookies.");
                LoginWithCookie();
                return;
            }
            logger.LogWarning(
                "Login into the portal without using cookies." +
                "Please choose 'Amazon DSP console (formerly Amazon Advertising Platform)' and enter an authorization code!");
            LoginWithoutCookie();
        }

        /// <summary>
        /// To repeat the password, the method enters only the password and waits for the page to load.
        /// </summary>
        /// <param name="password">Password to be entered.</param>
        /// <param name="waitElement">Web element that the method will wait for after logging in.</param>
        public void RepeatPasswordForLogin(string password, By waitElement = null)
        {
            pageActionsManager.LoginWithPasswordAndWaiting(password, waitElement);
        }

        private void LoginWithoutCookie()
        {
            pageActionsManager.NavigateToUrl(SignInPageUrl, AmazonLoginPageObjects.ForgotPassLink);
            pageActionsManager.LoginProcess(authorizationModel.Login, authorizationModel.Password);
            var cookies = pageActionsManager.GetAllCookies();
            SeleniumCookieHelper.SaveCookiesToFiles(cookies, authorizationModel.CookiesDir);
        }

        private void LoginWithCookie()
        {
            pageActionsManager.NavigateToUrl(SignInPageUrl);
            foreach (var cookie in authorizationModel.Cookies)
            {
                pageActionsManager.SetCookie(cookie);
            }
        }
    }
}