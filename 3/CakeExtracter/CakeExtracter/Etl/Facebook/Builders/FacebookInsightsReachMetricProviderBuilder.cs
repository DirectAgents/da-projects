using DirectAgents.Domain.Entities.CPProg;
using FacebookAPI.Providers;

namespace CakeExtracter.Etl.Facebook.Builders
{
    public class FacebookInsightsReachMetricProviderBuilder : FacebookInsightsDataProviderBuilder
    {
        public FacebookInsightsReachMetricProvider BuildInsightsReachMetricProvider(ExtAccount account)
        {
            var fbUtility = new FacebookInsightsReachMetricProvider(m => Logger.Info(account.Id, m), m => Logger.Warn(account.Id, m));
            SetUtilityFilters(fbUtility, account);
            return fbUtility;
        }
    }
}
