using CakeExtracter.Etl.AmazonSelenium.VCD.Extractors.VcdExtractionHelpers.ReportParsing.ParsingConverters;
using CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Models.Products;

namespace CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Extractors.VcdCustomReportsExtractionHelpers.ReportParsing.RowMaps
{
    /// <inheritdoc />
    /// <summary>
    /// Row map configuration for Item Comparison reports.
    /// </summary>
    internal sealed class ItemComparisonRowMap : BaseCustomReportRowMap<ItemComparisonProduct>
    {
        /// <inheritdoc cref="BaseProductRowMap"/>
        /// <summary>
        /// Initializes a new instance of the <see cref="ItemComparisonRowMap" /> class.
        /// </summary>
        public ItemComparisonRowMap()
        {
            Map(m => m.Asin).Name("asin");
            Map(m => m.Name).Name("producttitle");
            Map(m => m.No1ComparedAsin).Name("no1comparedasin");
            Map(m => m.No1ComparedProductTitle).Name("no1comparedproducttitle");
            Map(m => m.No1ComparedPercent).Name("no1comparedpercent").TypeConverter<DecimalPercentageReportConverter>();
            Map(m => m.No2ComparedAsin).Name("no2comparedasin");
            Map(m => m.No2ComparedProductTitle).Name("no2comparedproducttitle");
            Map(m => m.No2ComparedPercent).Name("no2comparedpercent").TypeConverter<DecimalPercentageReportConverter>();
            Map(m => m.No3ComparedAsin).Name("no3comparedasin");
            Map(m => m.No3ComparedProductTitle).Name("no3comparedproducttitle");
            Map(m => m.No3ComparedPercent).Name("no3comparedpercent").TypeConverter<DecimalPercentageReportConverter>();
            Map(m => m.No4ComparedAsin).Name("no4comparedasin");
            Map(m => m.No4ComparedProductTitle).Name("no4comparedproducttitle");
            Map(m => m.No4ComparedPercent).Name("no4comparedpercent").TypeConverter<DecimalPercentageReportConverter>();
            Map(m => m.No5ComparedAsin).Name("no5comparedasin");
            Map(m => m.No5ComparedProductTitle).Name("no5comparedproducttitle");
            Map(m => m.No5ComparedPercent).Name("no5comparedpercent").TypeConverter<DecimalPercentageReportConverter>();
        }
    }
}
