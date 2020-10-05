using CakeExtracter.Common;

namespace CakeExtracter.Etl.AdWords.Extractors
{
    internal class AdWordsAssistConvExtractor : AdWordsBaseApiExtractor
    {
        public AdWordsAssistConvExtractor(string clientCustomerId, DateRange dateRange)
        : base(clientCustomerId, dateRange)
        {
        }

        /// <inheritdoc/>
        protected override string StatsType => "ClickAssistConvStats";

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
                "ClickAssistedConversions",
                "ClickAssistedConversionValue",
            };
        }
    }
}
