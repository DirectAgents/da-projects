using System;
using System.Linq;
using SeleniumDataBrowser.PageActions;
using SeleniumDataBrowser.Helpers;
using SeleniumDataBrowser.Models;
using SeleniumDataBrowser.VCD.PageActions;

namespace SeleniumDataBrowser.VCD.Helpers
{
    public class AmazonVcdLoginHelper
    {
        private const string SignInPageUrl = "https://www.amazon.com/ap/signin";

        private readonly AuthorizationModel authorizationModel;
        private readonly AmazonVcdPageActions pageActionManager;
        private readonly Action<string> logInfo;

        public AmazonVcdLoginHelper(
            AuthorizationModel authorizationModel,
            AmazonVcdPageActions pageActionManager,
            Action<string> logInfo)
        {
            this.authorizationModel = authorizationModel;
            this.pageActionManager = pageActionManager;
            this.logInfo = logInfo;
        }

        public void LoginToAmazonPortal()
        {
            pageActionManager.NavigateToSalesDiagnosticPage();
            if (IsLoginProcessNeeded(authorizationModel, pageActionManager))
            {
                Login(authorizationModel, pageActionManager);
            }
        }

        private static bool IsLoginProcessNeeded(AuthorizationModel authorizationModel, AmazonVcdPageActions pageManager)
        {
            var currentUrl = pageManager.GetCurrentWindowUrl();
            return currentUrl.IndexOf(authorizationModel.SignInUrl, StringComparison.Ordinal) <= 0;
        }

        private void Login(AuthorizationModel authorizationModel, AmazonVcdPageActions pageManager)
        {
            authorizationModel.Cookies = CookieManager.GetCookiesFromFiles(authorizationModel.CookiesDir);
            var cookiesExist = authorizationModel.Cookies.Any();
            logInfo($"Login into the portal{(cookiesExist ? string.Empty : " without")} using cookies.");
            if (cookiesExist)
            {
                LoginWithCookie(authorizationModel, pageManager);
            }
            else
            {
                LoginWithoutCookie(authorizationModel, pageManager);
            }
            pageManager.RefreshSalesDiagnosticPage(authorizationModel);
        }

        private static void LoginWithoutCookie(AuthorizationModel authModel, BaseAmazonPageActions pageManager)
        {
            pageManager.NavigateToUrl(authModel.SignInUrl, BaseAmazonPageObjects.ForgotPassLink);
            pageManager.LoginProcess(authModel.Login, authModel.Password);
            var cookies = pageManager.GetAllCookies();
            CookieManager.SaveCookiesToFiles(cookies, authModel.CookiesDir);
        }

        public static bool NeedResetPassword(BaseAmazonPageActions pageActions)
        {
            var currentUrl = pageActions.GetCurrentWindowUrl();
            return currentUrl.Contains(SignInPageUrl) && pageActions.IsElementPresent(BaseAmazonPageObjects.LoginPassInput);
        }

        public static void ResetPassword(BaseAmazonPageActions pageActions, AuthorizationModel authModel)
        {
            pageActions.LoginWithPassword(authModel.Password);
        }

        private static void LoginWithCookie(AuthorizationModel authModel, BaseAmazonPageActions pageManager)
        {
            pageManager.NavigateToUrl(authModel.SignInUrl);
            foreach (var cookie in authModel.Cookies)
            {
                pageManager.SetCookie(cookie);
            }
        }
    }
}
