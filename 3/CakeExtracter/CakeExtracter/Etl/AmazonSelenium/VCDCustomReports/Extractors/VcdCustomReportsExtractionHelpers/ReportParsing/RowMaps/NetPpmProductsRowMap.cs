using CakeExtracter.Etl.AmazonSelenium.VCD.Extractors.VcdExtractionHelpers.ReportParsing.ParsingConverters;
using CakeExtracter.Etl.AmazonSelenium.VCD.Extractors.VcdExtractionHelpers.ReportParsing.RowMaps;
using CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Models.Products;

namespace CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Extractors.VcdCustomReportsExtractionHelpers.ReportParsing.RowMaps
{
    /// <inheritdoc />
    /// <summary>
    /// Row map configuration for Net PPM reports.
    /// </summary>
    internal sealed class NetPpmProductsRowMap : BaseCustomReportRowMap<NetPpmProduct>
    {
        /// <inheritdoc cref="BaseProductRowMap"/>
        /// <summary>
        /// Initializes a new instance of the <see cref="NetPpmProductsRowMap" /> class.
        /// </summary>
        public NetPpmProductsRowMap()
        {
            Map(m => m.Subcategory).Name("subcategory");
            Map(m => m.NetPpm).Name("netppm").TypeConverter<DecimalPercentageReportConverter>();
            Map(m => m.NetPpmPriorYearPercentChange).Name("netppmprioryearpercentchange").TypeConverter<DecimalPercentageReportConverter>();
        }
    }
}
