using CakeExtracter.Etl.AmazonSelenium.VCD.Extractors.VcdExtractionHelpers.ReportParsing.ParsingConverters;
using CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Models;
using CsvHelper.Configuration;

namespace CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Extractors.VcdCustomReportsExtractionHelpers.ReportParsing.RowMaps
{
    /// <inheritdoc />
    /// <summary>
    /// Row map configuration for Net PPM reports.
    /// </summary>
    internal sealed class NetPpmProductsRowMap : CsvClassMap<NetPpmProduct>
    {
        /// <inheritdoc cref="BaseProductRowMap"/>
        /// <summary>
        /// Initializes a new instance of the <see cref="NetPpmProductsRowMap" /> class.
        /// </summary>
        public NetPpmProductsRowMap()
        {
            Map(m => m.Asin).Name("asin");
            Map(m => m.Name).Name("producttitle");
            Map(m => m.Subcategory).Name("subcategory");
            Map(m => m.NetPpm).Name("netppm").TypeConverter<DecimalPercentageReportConverter>();
            Map(m => m.NetPpmPriorYearPercentChange).Name("netppmprioryearpercentchange").TypeConverter<DecimalPercentageReportConverter>();
        }
    }
}
