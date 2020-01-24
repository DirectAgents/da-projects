using System;
using System.Collections.Generic;
using System.Web;
using SeleniumDataBrowser.GenerationReportsTrigger.PageActions;
using SeleniumDataBrowser.PDA.Helpers;

namespace SeleniumDataBrowser.GenerationReportsTrigger.Helpers
{
    public class GenerationReportsProfileUrlManager : PdaProfileUrlManager
    {
        private const string ReportProfileUrlPattern = "https://advertising.amazon.com/sspa/tresah?entityId={0}";

        public GenerationReportsProfileUrlManager(
            GenerationReportsActionsWithPagesManager pageActionsManager,
            PdaLoginManager loginProcessManager,
            int maxRetryAttempts,
            TimeSpan pauseBetweenAttempts)
            : base(pageActionsManager, loginProcessManager, maxRetryAttempts, pauseBetweenAttempts)
        {
        }

        /// <summary>
        /// Gets the URLs for the available profiles to the "Reports" page of the Advertising Portal.
        /// </summary>
        /// <returns>Dictionary of available profile URLs.</returns>
        public Dictionary<string, string> GetAvailableReportsProfileUrls()
        {
            var availableProfileUrls = GetAvailableProfileUrls();
            var reportProfileUrls = ModifyCampaignUrlsToReportUrls(availableProfileUrls);
            return reportProfileUrls;
        }

        private Dictionary<string, string> ModifyCampaignUrlsToReportUrls(Dictionary<string, string> campaignProfileUrls)
        {
            var reportProfileUrls = new Dictionary<string, string>();
            foreach (var campaignProfile in campaignProfileUrls)
            {
                var reportProfileUrl = GetReportProfileUrlByCampaignUrl(campaignProfile.Value);
                reportProfileUrls.Add(campaignProfile.Key, reportProfileUrl);
            }
            return reportProfileUrls;
        }

        private string GetReportProfileUrlByCampaignUrl(string campaignProfileUrl)
        {
            var entityId = GetProfileEntityId(campaignProfileUrl);
            return string.Format(ReportProfileUrlPattern, entityId);
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
