using CakeExtracter.Etl.AmazonSelenium.VCD.Extractors.VcdExtractionHelpers.ReportParsing.ParsingConverters;
using CakeExtracter.Etl.AmazonSelenium.VCD.Extractors.VcdExtractionHelpers.ReportParsing.RowMaps;
using CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Models.Products;

namespace CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Extractors.VcdCustomReportsExtractionHelpers.ReportParsing.RowMaps
{
    /// <inheritdoc />
    /// <summary>
    /// Row map configuration for Repeat Purchase Behavior reports.
    /// </summary>
    internal sealed class RepeatPurchaseBehaviorProductsRowMap : BaseCustomReportRowMap<RepeatPurchaseBehaviorProduct>
    {
        /// <inheritdoc cref="BaseProductRowMap"/>
        /// <summary>
        /// Initializes a new instance of the <see cref="RepeatPurchaseBehaviorProductsRowMap" /> class.
        /// </summary>
        public RepeatPurchaseBehaviorProductsRowMap()
        {
            Map(m => m.UniqueCustomers).Name("uniquecustomers").TypeConverter<IntNumberReportConverter>();
            Map(m => m.RepeatPurchaseRevenue).Name("repeatpurchaserevenue").TypeConverter<DecimalPercentageReportConverter>();
            Map(m => m.RepeatPurchaseRevenuePriorPeriod).Name("repeatpurchaserevenuepriorperiod").TypeConverter<DecimalPercentageReportConverter>();
        }
    }
}
