using System;
using System.Collections.Generic;
using SeleniumDataBrowser.PDA.Models;
using SeleniumDataBrowser.PDA.PageActions;
using Polly;

namespace SeleniumDataBrowser.PDA.Helpers
{
    public static class PdaProfileUrlManager
    {
        private const string CampaignPageUrl = "https://advertising.amazon.com/cm/campaigns";

        /// <summary>
        /// The method sets the URLs for the available profiles on main page of portal
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
                        //var message = $"Waiting {timeSpan} before setting URLs";
                        //LoggerHelper.LogWaiting(message, retryCount, x => Logger.Info(x));
                    })
                .Execute(() => GetAvailableProfiles(pageActions));
            return availableProfileUrls;
        }

        private static Dictionary<string, string> GetAvailableProfiles(AmazonPdaPageActions pageActions)
        {
            var availableProfileUrls = pageActions.GetAvailableProfileUrls();
            return availableProfileUrls;
        }
    }
}
