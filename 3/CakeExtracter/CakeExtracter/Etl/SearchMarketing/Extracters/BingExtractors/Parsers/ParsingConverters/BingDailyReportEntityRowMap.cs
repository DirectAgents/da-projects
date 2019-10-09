using System.Globalization;
using CakeExtracter.Etl.SearchMarketing.Extracters.BingExtractors.Models;
using CsvHelper.Configuration;

namespace CakeExtracter.Etl.SearchMarketing.Extracters.BingExtractors.Parsers.ParsingConverters
{
    /// <inheritdoc />
    /// <summary>
    /// Bing daily report csv convert rules.
    /// </summary>
    internal sealed class BingDailyReportEntityRowMap : CsvClassMap<BingDailyRow>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BingDailyReportEntityRowMap"/> class.
        /// </summary>
        public BingDailyReportEntityRowMap()
        {
            Map(m => m.TimePeriod);
            Map(m => m.Impressions).TypeConverterOption(NumberStyles.Integer);
            Map(m => m.Clicks).TypeConverterOption(NumberStyles.Integer);
            Map(m => m.Conversions).TypeConverterOption(NumberStyles.Integer);
            Map(m => m.Spend).TypeConverterOption(NumberStyles.Currency);
            Map(m => m.Revenue).TypeConverterOption(NumberStyles.Currency);
            Map(m => m.AccountId);
            Map(m => m.AccountName);
            Map(m => m.AccountNumber);
            Map(m => m.CampaignId);
            Map(m => m.CampaignName);
        }
    }
}
