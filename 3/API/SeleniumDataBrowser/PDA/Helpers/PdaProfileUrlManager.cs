using System;
using System.Collections.Generic;
using SeleniumDataBrowser.PDA.PageActions;
using SeleniumDataBrowser.PageActions;
using Polly;
using SeleniumDataBrowser.Helpers;

namespace SeleniumDataBrowser.PDA.Helpers
{
    /// <summary>
    /// Class for manage URLs of campaign profiles (accounts) on main page of the Advertiser Portal.
    /// </summary>
    public class PdaProfileUrlManager
    {
        private const string CampaignPageUrl = "https://advertising.amazon.com/cm/campaigns";

        private readonly AmazonPdaActionsWithPagesManager pageActionsManager;
        private readonly PdaLoginManager loginProcessManager;
        private readonly int maxRetryAttempts;
        private readonly TimeSpan pauseBetweenAttempts;

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

        /// <summary>
        /// Gets the URLs for the available profiles to the "Reports" page of the Advertising Portal.
        /// </summary>
        /// <returns>Dictionary of available profile URLs.</returns>
        public Dictionary<string, string> GetAvailableReportsProfileUrls()
        {
            GoToPortalMainPage();
            var availableProfileUrls = TryGetAvailableProfiles();
            var reportProfileUrls = ModifyCampaignUrlsToReportUrls(availableProfileUrls);
            return reportProfileUrls;
        }

        private void GoToPortalMainPage()
        {
            pageActionsManager.NavigateToUrl(CampaignPageUrl, AmazonPdaPageObjects.FilterByButton);
            if (!pageActionsManager.IsElementPresent(AmazonPdaPageObjects.FilterByButton)
                && pageActionsManager.IsElementPresent(AmazonLoginPageObjects.LoginPassInput))
            {
                // need to repeat the password
                loginProcessManager.RepeatPasswordForLogin(AmazonPdaPageObjects.FilterByButton);
            }
        }

        private Dictionary<string, string> TryGetAvailableProfiles()
        {
            return Policy
                .Handle<Exception>()
                .WaitAndRetry(maxRetryAttempts, i => pauseBetweenAttempts)
                .Execute(pageActionsManager.GetAvailableProfileUrls);
        }

        private Dictionary<string, string> ModifyCampaignUrlsToReportUrls(Dictionary<string, string> campaignProfileUrls)
        {
            var reportProfileUrls = new Dictionary<string, string>();
            foreach (var campaignProfile in campaignProfileUrls)
            {
                var reportProfileUrl = AmazonAdvertisingProfileHelper.GetReportProfileUrlByCampaignUrl(campaignProfile.Value);
                reportProfileUrls.Add(campaignProfile.Key, reportProfileUrl);
            }
            return reportProfileUrls;
        }
    }
}