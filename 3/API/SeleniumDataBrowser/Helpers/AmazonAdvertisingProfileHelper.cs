using System;
using System.Web;
using SeleniumDataBrowser.PDA.Helpers;

namespace SeleniumDataBrowser.Helpers
{
    public class AmazonAdvertisingProfileHelper
    {
        private const string ReportProfileUrlPattern = "https://advertising.amazon.com/sspa/tresah?entityId={0}";

        public static string GetReportProfileUrlByCampaignUrl(string campaignProfileUrl)
        {
            var entityId = GetProfileEntityId(campaignProfileUrl);
            return string.Format(ReportProfileUrlPattern, entityId);
        }

        private static string GetProfileEntityId(string url)
        {
            var uri = new Uri(url);
            var queryParams = HttpUtility.ParseQueryString(uri.Query);
            var entityId = queryParams.Get(AmazonCmApiHelper.EntityIdArgName);
            return entityId;
        }
    }
}
