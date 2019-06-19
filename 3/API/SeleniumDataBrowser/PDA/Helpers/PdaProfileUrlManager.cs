using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using SeleniumDataBrowser.Models;
using SeleniumDataBrowser.PDA.PageActions;
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

        /// <summary>
        /// URLs of profiles which available on the portal.
        /// </summary>
        public Dictionary<string, string> AvailableProfileUrls;

        private readonly AuthorizationModel authorizationModel;
        private readonly AmazonPdaPageActions pageActionManager;
        private readonly PdaLoginHelper loginProcessManager;
        private readonly int maxRetryAttempts;
        private readonly TimeSpan pauseBetweenAttempts;
        private readonly SeleniumLogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PdaProfileUrlManager"/> class.
        /// </summary>
        /// <param name="authorizationModel">Authorization settings.</param>
        /// <param name="pageActionManager">Manager of page actions.</param>
        /// <param name="loginProcessManager">Manager of login process.</param>
        /// <param name="maxRetryAttempts">Max number of retry attempts.</param>
        /// <param name="pauseBetweenAttempts">Time interval for pause between attempts.</param>
        /// <param name="logInfo">Action for logging (info level).</param>
        public PdaProfileUrlManager(
            AuthorizationModel authorizationModel,
            AmazonPdaPageActions pageActionManager,
            PdaLoginHelper loginProcessManager,
            int maxRetryAttempts,
            TimeSpan pauseBetweenAttempts,
            SeleniumLogger logger)
        {
            this.authorizationModel = authorizationModel;
            this.pageActionManager = pageActionManager;
            this.loginProcessManager = loginProcessManager;
            this.maxRetryAttempts = maxRetryAttempts;
            this.pauseBetweenAttempts = pauseBetweenAttempts;
            this.logger = logger;
        }

        /// <summary>
        /// The method sets the URLs for the available campaign profiles on main page of portal.
        /// </summary>
        public void SetAvailableProfileUrls()
        {
            GoToPortalMainPage();
            AvailableProfileUrls = TryGetAvailableProfiles();

            logger.LogInfo("The following profiles were found for the current account:");
            AvailableProfileUrls.ForEach(x => logger.LogInfo($"{x.Key} - {x.Value}"));
        }

        private void GoToPortalMainPage()
        {
            pageActionManager.NavigateToUrl(CampaignPageUrl, AmazonPdaPageObjects.FilterByButton);

            if (!pageActionManager.IsElementPresent(AmazonPdaPageObjects.FilterByButton)
                && pageActionManager.IsElementPresent(AmazonPdaPageObjects.LoginPassInput))
            {
                // need to repeat the password
                loginProcessManager.RepeatPasswordForLogin(authorizationModel.Password, AmazonPdaPageObjects.FilterByButton);
            }
        }

        private Dictionary<string, string> TryGetAvailableProfiles()
        {
            return Policy
                .Handle<Exception>()
                .WaitAndRetry(maxRetryAttempts, i => pauseBetweenAttempts)
                .Execute(pageActionManager.GetAvailableProfileUrls);
        }
    }
}
