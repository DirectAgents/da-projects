using CakeExtracter.Etl.AmazonSelenium.VCD.Extractors.VcdExtractionHelpers.ReportParsing.ParsingConverters;

namespace CakeExtracter.Etl.AmazonSelenium.VCD.Extractors.VcdExtractionHelpers.ReportParsing.RowMaps
{
    /// <inheritdoc />
    /// <summary>
    /// Row map configuration for Ordered Revenue reports.
    /// </summary>
    internal sealed class OrderedRevenueProductsRowMap : BaseProductRowMap
    {
        /// <inheritdoc cref="BaseProductRowMap"/>
        /// <summary>
        /// Initializes a new instance of the <see cref="OrderedRevenueProductsRowMap" /> class.
        /// </summary>
        public OrderedRevenueProductsRowMap()
        {
            Map(m => m.OrderedRevenue).Name("orderedrevenue").TypeConverter<DecimalAmountReportConverter>();
        }
    }
}