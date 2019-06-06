using System;
using System.Linq;
using SeleniumDataBrowser.Helpers;
using SeleniumDataBrowser.PDA.Models;
using SeleniumDataBrowser.PDA.PageActions;

namespace SeleniumDataBrowser.PDA.Helpers
{
    public class PdaLoginHelper
    {
        private const string SignInPageUrl = "https://advertising.amazon.com/sign-in";

        private AuthorizationModel authorizationModel;
        private AmazonPdaPageActions pageActions;
        private Action<string> logInfo;
        private Action<string> logWarn;

        public PdaLoginHelper(
            AuthorizationModel authorizationModel,
            AmazonPdaPageActions pageActions,
            Action<string> logInfo,
            Action<string> logWarn)
        {
            this.authorizationModel = authorizationModel;
            this.pageActions = pageActions;
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

        private void LoginWithoutCookie()
        {
            pageActions.NavigateToUrl(SignInPageUrl, AmazonPdaPageObjects.ForgotPassLink);
            pageActions.LoginProcess(authorizationModel.Login, authorizationModel.Password);
            var cookies = pageActions.GetAllCookies();
            CookieManager.SaveCookiesToFiles(cookies, authorizationModel.CookiesDir);
        }

        private void LoginWithCookie()
        {
            pageActions.NavigateToUrl(SignInPageUrl);
            foreach (var cookie in authorizationModel.Cookies)
            {
                pageActions.SetCookie(cookie);
            }
        }
    }
}