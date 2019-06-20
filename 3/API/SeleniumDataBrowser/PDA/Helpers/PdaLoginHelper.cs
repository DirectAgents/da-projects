using System.Linq;
using OpenQA.Selenium;
using SeleniumDataBrowser.Helpers;
using SeleniumDataBrowser.Models;
using SeleniumDataBrowser.PDA.PageActions;

namespace SeleniumDataBrowser.PDA.Helpers
{
    /// <summary>
    /// Helper for PDA login process.
    /// </summary>
    public class PdaLoginHelper
    {
        private const string SignInPageUrl = "https://advertising.amazon.com/sign-in";

        private readonly AuthorizationModel authorizationModel;
        private readonly AmazonPdaPageActions pageActionManager;
        private readonly SeleniumLogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PdaLoginHelper"/> class.
        /// </summary>
        /// <param name="authorizationModel">Authorization settings.</param>
        /// <param name="pageActionManager">Page actions manager.</param>
        /// <param name="logger">Selenium data browser logger.</param>
        public PdaLoginHelper(
            AuthorizationModel authorizationModel,
            AmazonPdaPageActions pageActionManager,
            SeleniumLogger logger)
        {
            this.authorizationModel = authorizationModel;
            this.pageActionManager = pageActionManager;
            this.logger = logger;
        }

        /// <summary>
        /// Login to the advertising portal and saving cookies.
        /// </summary>
        public void LoginToPortal()
        {
            authorizationModel.Cookies = CookieManager.GetCookiesFromFiles(authorizationModel.CookiesDir);
            var cookiesExist = authorizationModel.Cookies.Any();

            if (cookiesExist)
            {
                logger.LogInfo("Login into the portal using cookies.");
                LoginWithCookie();
                return;
            }
            logger.LogWarning("Login into the portal without using cookies. Please choose 'Amazon DSP console (formerly Amazon Advertising Platform)' and enter an authorization code!");
            LoginWithoutCookie();
        }

        /// <summary>
        /// To repeat the password, the method enters only the password and waits for the page to load.
        /// </summary>
        /// <param name="password">Password to be entered.</param>
        /// <param name="waitElement">Web element that the method will wait for after logging in.</param>
        public void RepeatPasswordForLogin(string password, By waitElement = null)
        {
            pageActionManager.LoginWithPasswordAndWaiting(password, waitElement);
        }

        private void LoginWithoutCookie()
        {
            pageActionManager.NavigateToUrl(SignInPageUrl, AmazonPdaPageObjects.ForgotPassLink);
            pageActionManager.LoginProcess(authorizationModel.Login, authorizationModel.Password);
            var cookies = pageActionManager.GetAllCookies();
            CookieManager.SaveCookiesToFiles(cookies, authorizationModel.CookiesDir);
        }

        private void LoginWithCookie()
        {
            pageActionManager.NavigateToUrl(SignInPageUrl);
            foreach (var cookie in authorizationModel.Cookies)
            {
                pageActionManager.SetCookie(cookie);
            }
        }
    }
}