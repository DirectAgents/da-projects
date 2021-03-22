using CakeExtracter.Etl.AmazonSelenium.VCD.Extractors.VcdExtractionHelpers.ReportParsing.ParsingConverters;
using CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Models;
using CsvHelper.Configuration;

namespace CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Extractors.VcdCustomReportsExtractionHelpers.ReportParsing.RowMaps
{
    /// <inheritdoc />
    /// <summary>
    /// Row map configuration for Repeat Purchase Behavior reports.
    /// </summary>
    internal sealed class RepeatPurchaseBehaviorProductsRowMap : CsvClassMap<RepeatPurchaseBehaviorProduct>
    {
        /// <inheritdoc cref="BaseProductRowMap"/>
        /// <summary>
        /// Initializes a new instance of the <see cref="RepeatPurchaseBehaviorProductsRowMap" /> class.
        /// </summary>
        public RepeatPurchaseBehaviorProductsRowMap()
        {
            Map(m => m.Asin).Name("asin");
            Map(m => m.Name).Name("producttitle");
            Map(m => m.UniqueCustomers).Name("uniquecustomers").TypeConverter<IntNumberReportConverter>();
            Map(m => m.RepeatPurchaseRevenue).Name("repeatpurchaserevenue").TypeConverter<DecimalPercentageReportConverter>();
            Map(m => m.RepeatPurchaseRevenuePriorPeriod).Name("repeatpurchaserevenuepriorperiod").TypeConverter<DecimalPercentageReportConverter>();
        }
    }
}
