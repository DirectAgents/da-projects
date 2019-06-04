using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Etl.AmazonSelenium.Helpers;
using CakeExtracter.Etl.AmazonSelenium.PDA.Models.CommonHelperModels;
using CakeExtracter.Etl.AmazonSelenium.PDA.PageActions;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Polly;

namespace CakeExtracter.Etl.AmazonSelenium.PDA.Helpers
{
    public static class PdaLoginHelper
    {
        private const string SignInPageUrl = "https://advertising.amazon.com/sign-in";
        private const string CampaignPageUrl = "https://advertising.amazon.com/cm/campaigns";

        /// <summary>
        /// Login to the advertising portal and saving cookies.
        /// </summary>
        public static void LoginToPortal(AuthorizationModel authorizationModel, AmazonPdaPageActions pageActions)
        {
            authorizationModel.Cookies = CookieManager.GetCookiesFromFiles(authorizationModel.CookiesDir);
            var cookiesExist = authorizationModel.Cookies.Any();

            if (cookiesExist)
            {
                Logger.Info("Login into the portal using cookies.");
                LoginWithCookie(authorizationModel, pageActions);
                return;
            }
            Logger.Warn("Login into the portal without using cookies. Please enter an authorization code!");
            LoginWithoutCookie(authorizationModel, pageActions);
        }

        /// <summary>
        /// The method gets the URLs for the available profiles on main page of portal
        /// </summary>
        public static Dictionary<string, string> GetAvailableProfileUrls(AuthorizationModel authorizationModel, AmazonPdaPageActions pageActions)
        {
            try
            {
                GoToPortalMainPage(authorizationModel, pageActions);
                return TryGetAvailableProfiles(pageActions);
            }
            catch (Exception e)
            {
                throw new Exception($"Could not to get the profile URLs: {e.Message}", e);
            }
        }

        private static void LoginWithoutCookie(AuthorizationModel authorizationModel, AmazonPdaPageActions pageActions)
        {
            pageActions.NavigateToUrl(SignInPageUrl, AmazonPdaPageObjects.ForgotPassLink);
            pageActions.LoginProcess(authorizationModel.Login, authorizationModel.Password);
            var cookies = pageActions.GetAllCookies();
            CookieManager.SaveCookiesToFiles(cookies, authorizationModel.CookiesDir);
        }

        private static void LoginWithCookie(AuthorizationModel authModel, AmazonPdaPageActions pageActions)
        {
            pageActions.NavigateToUrl(SignInPageUrl);
            foreach (var cookie in authModel.Cookies)
            {
                pageActions.SetCookie(cookie);
            }
        }

        private static void GoToPortalMainPage(AuthorizationModel authorizationModel, AmazonPdaPageActions pageActions)
        {
            pageActions.NavigateToUrl(CampaignPageUrl, AmazonPdaPageObjects.FilterByButton);
            if (!pageActions.IsElementPresent(AmazonPdaPageObjects.FilterByButton) &&
                pageActions.IsElementPresent(AmazonPdaPageObjects.LoginPassInput))
            {
                // need to repeat the password
                pageActions.LoginByPassword(authorizationModel.Password, AmazonPdaPageObjects.FilterByButton);
            }
        }

        private static Dictionary<string, string> TryGetAvailableProfiles(AmazonPdaPageActions pageActions)
        {
            const int maxRetryAttempts = 5;
            var pauseBetweenAttempts = TimeSpan.FromSeconds(3);
            var availableProfileUrls = Policy
                .Handle<Exception>()
                .WaitAndRetry(
                    maxRetryAttempts,
                    i => pauseBetweenAttempts,
                    (exception, timeSpan, retryCount, context) =>
                    {
                        var message = $"Waiting {timeSpan} before setting URLs";
                        LoggerHelper.LogWaiting(message, retryCount, x => Logger.Info(x));
                    })
                .Execute(() => GetAvailableProfiles(pageActions));
            return availableProfileUrls;
        }

        private static Dictionary<string, string> GetAvailableProfiles(AmazonPdaPageActions pageActions)
        {
            var availableProfileUrls = pageActions.GetAvailableProfileUrls();
            Logger.Info("The following profiles were found for the current account:");
            availableProfileUrls.ForEach(x => Logger.Info($"{x.Key} - {x.Value}"));

            return availableProfileUrls;
        }
    }
}
