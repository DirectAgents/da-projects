using CakeExtracter.Common;

namespace CakeExtracter.Etl.AdWords.Extractors
{
    internal class AdWordsStandartExtractor : AdWordsBaseApiExtractor
    {
        public AdWordsStandartExtractor(string clientCustomerId, DateRange dateRange)
            : base(clientCustomerId, dateRange)
        {
        }

        /// <inheritdoc/>
        protected override string StatsType => string.Empty;

        /// <inheritdoc/>
        protected override string[] GetReportFields()
        {
            return new[]
            {
                "AccountDescriptiveName",
                "AccountCurrencyCode",
                "ExternalCustomerId",
                "AccountTimeZone",
                "CampaignId",
                "CampaignName",
                "CampaignStatus",
                "AdvertisingChannelSubType",
                "Date",
                "Impressions",
                "Clicks",
                "Conversions",
                "Cost",
                "ConversionValue",
                "AdNetworkType1",
                "Device",
                "ViewThroughConversions",
                "VideoQuartile100Rate",
                "VideoQuartile75Rate",
                "VideoQuartile50Rate",
                "VideoQuartile25Rate",
                "ActiveViewImpressions",
                "VideoViews",
            };
        }
    }
}
