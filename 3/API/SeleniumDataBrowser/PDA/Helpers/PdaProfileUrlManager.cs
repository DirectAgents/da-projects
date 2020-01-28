using System;
using System.Collections.Generic;
using SeleniumDataBrowser.PDA.PageActions;
using SeleniumDataBrowser.PageActions;
using Polly;

namespace SeleniumDataBrowser.PDA.Helpers
{
    /// <summary>
    /// Class for manage URLs of campaign profiles (accounts) on main page of the Advertiser Portal.
    /// </summary>
    public class PdaProfileUrlManager
    {
        private const string CampaignPageUrl = "https://advertising.amazon.com/cm/campaigns";

        protected readonly AmazonPdaActionsWithPagesManager pageActionsManager;
        protected readonly PdaLoginManager loginProcessManager;
        protected readonly int maxRetryAttempts;
        protected readonly TimeSpan pauseBetweenAttempts;

        /// <summary>
        /// Initializes a new instance of the <see cref="PdaProfileUrlManager"/> class.
        /// </summary>
        /// <param name="pageActionsManager">Manager of page actions.</param>
        /// <param name="loginProcessManager">Manager of login process.</param>
        /// <param name="maxRetryAttempts">Max number of retry attempts.</param>
        /// <param name="pauseBetweenAttempts">Time interval for pause between attempts.</param>
        public PdaProfileUrlManager(
            AmazonPdaActionsWithPagesManager pageActionsManager,
            PdaLoginManager loginProcessManager,
            int maxRetryAttempts,
            TimeSpan pauseBetweenAttempts)
        {
            this.pageActionsManager = pageActionsManager;
            this.loginProcessManager = loginProcessManager;
            this.maxRetryAttempts = maxRetryAttempts;
            this.pauseBetweenAttempts = pauseBetweenAttempts;
        }

        /// <summary>
        /// Gets the URLs for the available campaign profiles on main page of the portal.
        /// </summary>
        /// <returns>Dictionary of available profile URLs.</returns>
        public Dictionary<string, string> GetAvailableProfileUrls()
        {
            GoToPortalMainPage();
            var availableProfileUrls = TryGetAvailableProfiles();
            return availableProfileUrls;
        }

        protected Dictionary<string, string> TryGetAvailableProfiles()
        {
            return Policy
                .Handle<Exception>()
                .WaitAndRetry(maxRetryAttempts, i => pauseBetweenAttempts)
                .Execute(pageActionsManager.GetAvailableProfileUrls);
        }

        private void GoToPortalMainPage()
        {
            pageActionsManager.NavigateToUrl(CampaignPageUrl);
            if (!pageActionsManager.IsElementPresent(AmazonPdaPageObjects.FilterByButton)
                && pageActionsManager.IsElementPresent(AmazonLoginPageObjects.LoginPassInput))
            {
                // need to repeat the password
                loginProcessManager.RepeatPasswordForLogin(AmazonPdaPageObjects.FilterByButton);
            }
        }
    }
}