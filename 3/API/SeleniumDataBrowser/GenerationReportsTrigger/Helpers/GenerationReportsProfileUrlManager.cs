using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Polly;
using SeleniumDataBrowser.GenerationReportsTrigger.PageActions;
using SeleniumDataBrowser.PageActions;
using SeleniumDataBrowser.PDA.Helpers;

namespace SeleniumDataBrowser.GenerationReportsTrigger.Helpers
{
    public class GenerationReportsProfileUrlManager
    {
        private const string ChooseEntityUrl = "https://advertising.amazon.com/dsp/entities";
        private const string DefaultMarketplaceReportUrlPattern = "https://advertising.amazon.com/sspa/tresah?entityId={0}";
        private const string OtherMarketplaceReportUrlPattern = "https://advertising.amazon.ca/sspa/tresah?entityId={0}";

        private readonly GenerationReportsActionsWithPagesManager pageActionsManager;
        private readonly PdaLoginManager loginProcessManager;
        private readonly int maxRetryAttempts;
        private readonly TimeSpan pauseBetweenAttempts;

        public GenerationReportsProfileUrlManager(
            GenerationReportsActionsWithPagesManager pageActionsManager,
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
        /// Gets the URLs for the available profiles to the "Reports" page of the Advertising Portal.
        /// </summary>
        /// <returns>List of available profile URLs.</returns>
        public List<string> GetAvailableReportsProfileUrls()
        {
            var defaultMarketplaceProfileUrls = GetDefaultMarketplaceProfileUrls();
            var otherMarketplaceProfileUrls = GetOtherMarketplaceProfileUrls();
            var allMarketplaceUrls = defaultMarketplaceProfileUrls.Concat(otherMarketplaceProfileUrls).ToList();
            return ModifyCampaignUrlsToReportUrls(allMarketplaceUrls);
        }

        public void GoToReportProfile(string reportProfileUrl)
        {
            pageActionsManager.NavigateToUrl(reportProfileUrl);
            if (!pageActionsManager.IsElementPresent(GenerationReportsPageObjects.ReportsDashboard)
                && pageActionsManager.IsElementPresent(AmazonLoginPageObjects.LoginPassInput))
            {
                // need to log in
                loginProcessManager.LoginWithoutCookie();
                pageActionsManager.NavigateToUrl(reportProfileUrl, GenerationReportsPageObjects.ReportsDashboard);
            }
        }

        private List<string> GetDefaultMarketplaceProfileUrls()
        {
            GoToPortalChooseEntityPage();
            return TryGetProfileUrls(pageActionsManager.GetDefaultMarketplaceProfileUrls);
        }

        private List<string> GetOtherMarketplaceProfileUrls()
        {
            return TryGetProfileUrls(pageActionsManager.GetOtherMarketplaceProfileUrls);
        }

        private void GoToPortalChooseEntityPage()
        {
            pageActionsManager.NavigateToUrl(ChooseEntityUrl);
            if (!pageActionsManager.IsElementPresent(GenerationReportsPageObjects.ChooseEntityText))
            {
                if (!pageActionsManager.IsElementPresent(AmazonLoginPageObjects.LoginEmailInput)
                    && pageActionsManager.IsElementPresent(AmazonLoginPageObjects.LoginPassInput))
                {
                    // need to repeat the password
                    loginProcessManager.RepeatPasswordForLogin();
                    pageActionsManager.NavigateToUrl(ChooseEntityUrl, GenerationReportsPageObjects.ChooseEntityText);
                }
                if (pageActionsManager.IsElementPresent(AmazonLoginPageObjects.LoginEmailInput)
                    && pageActionsManager.IsElementPresent(AmazonLoginPageObjects.LoginPassInput))
                {
                    // need to log in
                    loginProcessManager.LoginWithoutCookie();
                    pageActionsManager.NavigateToUrl(ChooseEntityUrl, GenerationReportsPageObjects.ChooseEntityText);
                }
            }
        }

        private List<string> TryGetProfileUrls(Func<List<string>> fGetSomeProfileUrls)
        {
            return Policy
                .Handle<Exception>()
                .WaitAndRetry(maxRetryAttempts, i => pauseBetweenAttempts)
                .Execute(fGetSomeProfileUrls);
        }

        private List<string> ModifyCampaignUrlsToReportUrls(List<string> profileUrls)
        {
            var reportProfileUrls = new List<string>();
            foreach (var profileUrl in profileUrls)
            {
                var reportProfileUrl = GetReportProfileUrlByCampaignUrl(profileUrl);
                reportProfileUrls.Add(reportProfileUrl);
            }
            return reportProfileUrls;
        }

        private string GetReportProfileUrlByCampaignUrl(string profileUrl)
        {
            var entityId = GetProfileEntityId(profileUrl);
            return string.Format(
                pageActionsManager.IsCurrentUrlForCanadaMarketplace()
                ? OtherMarketplaceReportUrlPattern
                : DefaultMarketplaceReportUrlPattern, entityId);
        }

        private string GetProfileEntityId(string url)
        {
            var uri = new Uri(url);
            var queryParams = HttpUtility.ParseQueryString(uri.Query);
            var entityId = queryParams.Get(AmazonCmApiHelper.EntityIdArgName);
            return entityId;
        }
    }
}
