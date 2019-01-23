using CakeExtracter;
using CakeExtractor.SeleniumApplication.Helpers;
using CakeExtractor.SeleniumApplication.Models.CommonHelperModels;
using CakeExtractor.SeleniumApplication.PageActions.AmazonPda;
using CakeExtractor.SeleniumApplication.PageActions.AmazonVcd;
using System.Collections.Generic;
using System.Linq;

namespace CakeExtractor.SeleniumApplication.PageActions
{
    internal static class AmazonVcdLoginHelper
    {
        private const string signInPageUrl = "https://www.amazon.com/ap/signin";

        public static void LoginToAmazonPortal(AuthorizationModel authorizationModel, AmazonVcdPageActions pageManager)
        {
            pageManager.NavigateToSalesDiagnosticPage();
            if (IsLoginProcessNeeded(authorizationModel, pageManager)) ;
            {
                Login(authorizationModel, pageManager);
            }
        }

        private static bool IsLoginProcessNeeded(AuthorizationModel authorizationModel, AmazonVcdPageActions pageManager)
        {
            var currentUrl = pageManager.GetCurrentWindowUrl();
            return currentUrl.IndexOf(authorizationModel.SignInUrl) > 0;
        }

        private static void Login(AuthorizationModel authorizationModel, AmazonVcdPageActions pageManager)
        {
            authorizationModel.Cookies = CookieManager.GetCookiesFromFiles(authorizationModel.CookiesDir);
            var cookiesExist = authorizationModel.Cookies.Any();
            Logger.Info("Login into the portal{0} using cookies.", cookiesExist ? string.Empty : " without");
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
            pageManager.NavigateToUrl(authModel.SignInUrl, AmazonPdaPageObjects.ForgotPassLink);
            pageManager.LoginProcess(authModel.Login, authModel.Password);
            var cookies = pageManager.GetAllCookies();
            CookieManager.SaveCookiesToFiles(cookies, authModel.CookiesDir);
        }

        public static bool NeedResetPassword(BaseAmazonPageActions pageActions)
        {
            var currentUrl = pageActions.GetCurrentWindowUrl();
            return currentUrl.Contains(signInPageUrl) && pageActions.IsElementPresent(AmazonPdaPageObjects.LoginPassInput);
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
