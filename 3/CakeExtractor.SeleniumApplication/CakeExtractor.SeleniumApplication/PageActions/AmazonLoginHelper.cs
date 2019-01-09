﻿using CakeExtracter;
using CakeExtractor.SeleniumApplication.Helpers;
using CakeExtractor.SeleniumApplication.Models.CommonHelperModels;
using CakeExtractor.SeleniumApplication.PageActions.AmazonPda;
using System.Linq;

namespace CakeExtractor.SeleniumApplication.PageActions
{
    internal static class AmazonLoginHelper
    {
        public static void LoginToAmazonPortal(AuthorizationModel authorizationModel, BaseAmazonPageActions pageManager)
        {
            authorizationModel.Cookies = CookieManager.GetCookiesFromFiles(authorizationModel.CookiesDir);
            var cookiesExist = authorizationModel.Cookies.Any();
            Logger.Info("Login into the portal{0} using cookies.", cookiesExist ? string.Empty : " without");
            if (cookiesExist)
            {
                LoginWithCookie(authorizationModel, pageManager);
                return;
            }
            LoginWithoutCookie(authorizationModel, pageManager);
        }

        private static void LoginWithoutCookie(AuthorizationModel authModel, BaseAmazonPageActions pageManager)
        {
            pageManager.NavigateToUrl(authModel.SignInUrl, AmazonPdaPageObjects.ForgotPassLink); // Rid of link to Pda Page Objects
            pageManager.LoginProcess(authModel.Login, authModel.Password);
            var cookies = pageManager.GetAllCookies();
            CookieManager.SaveCookiesToFiles(cookies, authModel.CookiesDir);
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
