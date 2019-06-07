using System;
using System.Linq;
using OpenQA.Selenium;
using SeleniumDataBrowser.Helpers;
using SeleniumDataBrowser.PDA.Models;
using SeleniumDataBrowser.PDA.PageActions;

namespace SeleniumDataBrowser.PDA.Helpers
{
    /// <summary>
    /// Class for manage login process.
    /// </summary>
    public class PdaLoginHelper
    {
        private const string SignInPageUrl = "https://advertising.amazon.com/sign-in";

        private readonly AuthorizationModel authorizationModel;
        private readonly AmazonPdaPageActions pageActionManager;
        private readonly Action<string> logInfo;
        private readonly Action<string> logWarn;

        /// <summary>
        /// Initializes a new instance of the <see cref="PdaLoginHelper"/> class.
        /// </summary>
        /// <param name="authorizationModel">Authorization settings.</param>
        /// <param name="pageActionManager">Page actions manager.</param>
        /// <param name="logInfo">Action for logging (info level).</param>
        /// <param name="logWarn">Action for logging (error level).</param>
        public PdaLoginHelper(
            AuthorizationModel authorizationModel,
            AmazonPdaPageActions pageActionManager,
            Action<string> logInfo,
            Action<string> logWarn)
        {
            this.authorizationModel = authorizationModel;
            this.pageActionManager = pageActionManager;
            this.logInfo = logInfo;
            this.logWarn = logWarn;
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
                logInfo("Login into the portal using cookies.");
                LoginWithCookie();
                return;
            }
            logWarn("Login into the portal without using cookies. Please enter an authorization code!");
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