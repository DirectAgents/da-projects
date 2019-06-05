using System;
using System.Collections.Generic;
using System.Linq;
using SeleniumDataBrowser.Helpers;
using SeleniumDataBrowser.PDA.Models;
using SeleniumDataBrowser.PDA.PageActions;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Polly;

namespace SeleniumDataBrowser.PDA.Helpers
{
    public class PdaLoginHelper
    {
        private const string SignInPageUrl = "https://advertising.amazon.com/sign-in";
        private const string CampaignPageUrl = "https://advertising.amazon.com/cm/campaigns";

        public static AuthorizationModel authorizationModel;
        private AmazonPdaPageActions pageActions;

        public PdaLoginHelper(/*AuthorizationModel authorizationModel,*/ AmazonPdaPageActions pageActions)
        {
            //this.authorizationModel = authorizationModel;
            this.pageActions = pageActions;
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
                Logger.Info("Login into the portal using cookies.");
                LoginWithCookie();
                return;
            }
            Logger.Warn("Login into the portal without using cookies. Please enter an authorization code!");
            LoginWithoutCookie();
        }

        /// <summary>
        /// The method gets the URLs for the available profiles on main page of portal
        /// </summary>
        public Dictionary<string, string> GetAvailableProfileUrls()
        {
            try
            {
                GoToPortalMainPage();
                return TryGetAvailableProfiles();
            }
            catch (Exception e)
            {
                throw new Exception($"Could not to get the profile URLs: {e.Message}", e);
            }
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

        private void GoToPortalMainPage()
        {
            pageActions.NavigateToUrl(CampaignPageUrl, AmazonPdaPageObjects.FilterByButton);
            if (!pageActions.IsElementPresent(AmazonPdaPageObjects.FilterByButton) &&
                pageActions.IsElementPresent(AmazonPdaPageObjects.LoginPassInput))
            {
                // need to repeat the password
                pageActions.LoginByPassword(authorizationModel.Password, AmazonPdaPageObjects.FilterByButton);
            }
        }

        private Dictionary<string, string> TryGetAvailableProfiles()
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
                .Execute(GetAvailableProfiles);
            return availableProfileUrls;
        }

        private Dictionary<string, string> GetAvailableProfiles()
        {
            var availableProfileUrls = pageActions.GetAvailableProfileUrls();
            Logger.Info("The following profiles were found for the current account:");
            availableProfileUrls.ForEach(x => Logger.Info($"{x.Key} - {x.Value}"));

            return availableProfileUrls;
        }
    }
}
