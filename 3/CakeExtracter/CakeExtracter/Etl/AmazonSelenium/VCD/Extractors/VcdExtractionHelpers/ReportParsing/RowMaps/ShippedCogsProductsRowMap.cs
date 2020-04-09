using CakeExtracter.Etl.AmazonSelenium.VCD.Extractors.VcdExtractionHelpers.ReportParsing.ParsingConverters;

namespace CakeExtracter.Etl.AmazonSelenium.VCD.Extractors.VcdExtractionHelpers.ReportParsing.RowMaps
{
    /// <inheritdoc />
    /// <summary>
    /// Row map configuration for Shipped COGS reports.
    /// </summary>
    internal sealed class ShippedCogsProductsRowMap : BaseProductRowMap
    {
        /// <inheritdoc cref="BaseProductRowMap"/>
        /// <summary>
        /// Initializes a new instance of the <see cref="ShippedCogsProductsRowMap" /> class.
        /// </summary>
        public ShippedCogsProductsRowMap()
        {
            Map(m => m.ShippedCogs).Name("shippedcogs").TypeConverter<DecimalAmountReportConverter>();
            Map(m => m.CustomerReturns).Name("customerreturns").TypeConverter<IntNumberReportConverter>();
            Map(m => m.FreeReplacements).Name("freereplacements").TypeConverter<IntNumberReportConverter>();
        }
    }
}