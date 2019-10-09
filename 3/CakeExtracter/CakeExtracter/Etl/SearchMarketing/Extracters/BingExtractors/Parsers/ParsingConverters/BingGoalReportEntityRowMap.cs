using System.Globalization;
using CakeExtracter.Etl.SearchMarketing.Extracters.BingExtractors.Models;
using CsvHelper.Configuration;

namespace CakeExtracter.Etl.SearchMarketing.Extracters.BingExtractors.Parsers.ParsingConverters
{
    /// <inheritdoc />
    /// <summary>
    /// Bing goal report csv convert rules.
    /// </summary>
    internal sealed class BingGoalReportEntityRowMap : CsvClassMap<BingGoalRow>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BingGoalReportEntityRowMap"/> class.
        /// </summary>
        public BingGoalReportEntityRowMap()
        {
            Map(m => m.TimePeriod);
            Map(m => m.GoalId);
            Map(m => m.Goal);
            Map(m => m.AllConversions).TypeConverterOption(NumberStyles.Integer);
            Map(m => m.AllRevenue).TypeConverterOption(NumberStyles.Currency);
            Map(m => m.AccountId);
            Map(m => m.AccountName);
            Map(m => m.AccountNumber);
            Map(m => m.CampaignId);
            Map(m => m.CampaignName);
        }
    }
}
