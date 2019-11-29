using System.Collections.Generic;
using System.Text.RegularExpressions;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Entities.CPProg;
using FacebookAPI.Enums;
using FacebookAPI.Providers;

namespace CakeExtracter.Etl.Facebook.Builders
{
    /// <summary>
    /// Facebook InsightsData Provider Builder.
    /// </summary>
    public class FacebookInsightsDataProviderBuilder
    {
        private readonly Dictionary<string, PlatformFilter> networkFilters = new Dictionary<string, PlatformFilter>
        {
            {"FACEBOOK", PlatformFilter.Facebook},
            {"INSTAGRAM", PlatformFilter.Instagram},
            {"AUDIENCE", PlatformFilter.Audience},
            {"MESSENGER", PlatformFilter.Messenger},
        };

        private readonly Dictionary<string, ConversionActionType> configNamesForAccountsOfActionType = new Dictionary<string, ConversionActionType>
        {
            {"FB_ConversionsAsMobileAppInstalls", ConversionActionType.MobileAppInstall},
            {"FB_ConversionsAsPurchases", ConversionActionType.Purchase},
            {"FB_ConversionsAsRegistrations", ConversionActionType.Registration},
            {"FB_ConversionsAsVideoPlays", ConversionActionType.VideoPlay},
        };

        private readonly Dictionary<AttributionWindowType, Dictionary<string, AttributionWindowValue>> attributionWindowConfigurationMappings =
           new Dictionary<AttributionWindowType, Dictionary<string, AttributionWindowValue>>
       {
            {
                AttributionWindowType.Click, new Dictionary<string, AttributionWindowValue>
                {
                    { "FB_7d_click", AttributionWindowValue.Days7 },
                    { "FB_28d_click", AttributionWindowValue.Days28 },
                }
            },
            {
                AttributionWindowType.View, new Dictionary<string, AttributionWindowValue>
                {
                    { "FB_7d_view", AttributionWindowValue.Days7 },
                    { "FB_28d_view", AttributionWindowValue.Days28 },
                }
            },
       };

        /// <summary>
        /// Builds the insights data provider.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <returns>Insights data provider instance.</returns>
        public FacebookInsightsDataProvider BuildInsightsDataProvider(ExtAccount account)
        {
            var accountExternalId = account.ExternalId;
            var fbUtility = new FacebookInsightsDataProvider(m => Logger.Info(account.Id, m), m => Logger.Warn(account.Id, m));
            SetUtilityFilters(fbUtility, account);
            SetUtilityConversionType(fbUtility, accountExternalId);
            SetUtilityAttributionWindows(fbUtility, account);
            return fbUtility;
        }

        protected void SetUtilityFilters(FacebookInsightsDataProvider fbUtility, ExtAccount account)
        {
            if (account.Network != null)
            {
                SetUtilityPlatformFilters(fbUtility, account.Network.Name);
            }

            fbUtility.SetCampaignFilter(account.Filter);
        }

        private void SetUtilityPlatformFilters(FacebookInsightsDataProvider fbUtility, string networkName)
        {
            var network = Regex.Replace(networkName, @"\s+", "").ToUpper();
            foreach (var filter in networkFilters)
            {
                if (network.StartsWith(filter.Key))
                {
                    fbUtility.SetPlatformFilter(filter.Value);
                }
            }
        }

        private void SetUtilityConversionType(FacebookInsightsDataProvider fbUtility, string accountExternalId)
        {
            foreach (var configName in configNamesForAccountsOfActionType)
            {
                var accounts = ConfigurationHelper.ExtractEnumerableFromConfig(configName.Key);
                if (accounts.Contains(accountExternalId))
                {
                    fbUtility.SetConversionActionType(configName.Value);
                }
            }
        }

        private void SetUtilityAttributionWindows(FacebookInsightsDataProvider fbUtility, ExtAccount account)
        {
            SetUtilityClickAttributionWindows(fbUtility, account);
            SetUtilityViewAttributionWindows(fbUtility, account);
        }

        private void SetUtilityClickAttributionWindows(FacebookInsightsDataProvider fbUtility, ExtAccount account)
        {
            var window = GetAttributionWindow(account, AttributionWindowType.Click);
            if (window != 0)
            {
                fbUtility.SetClickAttributionWindow(window);
            }
        }

        private void SetUtilityViewAttributionWindows(FacebookInsightsDataProvider fbUtility, ExtAccount account)
        {
            var window = GetAttributionWindow(account, AttributionWindowType.View);
            if (window != 0)
            {
                fbUtility.SetViewAttributionWindow(window);
            }
        }

        private AttributionWindowValue GetAttributionWindow(ExtAccount account, AttributionWindowType attributionWindowType)
        {
            AttributionWindowValue windowValue = 0;
            foreach (var configMapping in attributionWindowConfigurationMappings[attributionWindowType])
            {
                var accounts = ConfigurationHelper.ExtractEnumerableFromConfig(configMapping.Key);
                if (accounts.Contains(account.Id.ToString()))
                {
                    windowValue = configMapping.Value;
                }
            }
            return windowValue;
        }
    }
}
