using CakeExtracter.Common;

namespace CakeExtracter.Etl.AdWords.Extractors
{
    internal class AdWordsConversionTypeExtractor : AdWordsBaseApiExtractor
    {
        public AdWordsConversionTypeExtractor(string clientCustomerId, DateRange dateRange)
        : base(clientCustomerId, dateRange)
        {
        }

        /// <inheritdoc/>
        protected override string StatsType => "ConversionTypeStats";

        /// <inheritdoc/>
        protected override string[] GetReportFields()
        {
            return new[]
            {
                "AccountDescriptiveName",
                "AccountCurrencyCode",
                "ExternalCustomerId",
                "CampaignId",
                "CampaignName",
                "Date",
                "AdNetworkType1",
                "Device",
                "ConversionTypeName",
                "Conversions",
                "ConversionValue",
                "AllConversions",
                "AllConversionValue",
            };
        }
    }
}
