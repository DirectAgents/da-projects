using DirectAgents.Domain.Entities.CPProg;
using FacebookAPI.Providers;

namespace CakeExtracter.Etl.Facebook.Builders
{
    /// <inheritdoc />
    /// <summary>
    /// Facebook builder of insights data provider for Reach metrics.
    /// </summary>
    public class FacebookInsightsReachMetricProviderBuilder : FacebookInsightsDataProviderBuilder
    {
        /// <summary>
        /// Builds and gets builder of insights data provider for Reach metrics.
        /// </summary>
        /// <param name="account">Account.</param>
        /// <returns>Insights data provider for Reach metrics.</returns>
        public FacebookInsightsReachMetricProvider BuildInsightsReachMetricProvider(ExtAccount account)
        {
            var fbUtility = new FacebookInsightsReachMetricProvider(m => Logger.Info(account.Id, m), m => Logger.Warn(account.Id, m));
            SetUtilityFilters(fbUtility, account);
            return fbUtility;
        }
    }
}
