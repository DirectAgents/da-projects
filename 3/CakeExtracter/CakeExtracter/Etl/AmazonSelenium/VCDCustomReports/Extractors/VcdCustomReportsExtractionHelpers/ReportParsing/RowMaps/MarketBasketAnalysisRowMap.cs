using CakeExtracter.Etl.AmazonSelenium.VCD.Extractors.VcdExtractionHelpers.ReportParsing.ParsingConverters;
using CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Models.Products;

namespace CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Extractors.VcdCustomReportsExtractionHelpers.ReportParsing.RowMaps
{
    /// <inheritdoc />
    /// <summary>
    /// Row map configuration for Market Basket Analysis reports.
    /// </summary>
    class MarketBasketAnalysisRowMap : BaseCustomReportRowMap<MarketBasketAnalysisProduct>
    {
        /// <inheritdoc cref="BaseProductRowMap"/>
        /// <summary>
        /// Initializes a new instance of the <see cref="MarketBasketAnalysisRowMap" /> class.
        /// </summary>
        public MarketBasketAnalysisRowMap()
        {
            Map(m => m.Asin).Name("asin");
            Map(m => m.Name).Name("producttitle");
            Map(m => m.N1PurchasedAsin).Name("n1purchasedasin");
            Map(m => m.N1PurchasedTitle).Name("n1purchasedtitle");
            Map(m => m.N1Combination).Name("n1combination").TypeConverter<DecimalPercentageReportConverter>();
            Map(m => m.N2PurchasedAsin).Name("n2purchasedasin");
            Map(m => m.N2PurchasedTitle).Name("n2purchasedtitle");
            Map(m => m.N2Combination).Name("n2combination").TypeConverter<DecimalPercentageReportConverter>();
            Map(m => m.N3PurchasedAsin).Name("n3purchasedasin");
            Map(m => m.N3PurchasedTitle).Name("n3purchasedtitle");
            Map(m => m.N3Combination).Name("n3combination").TypeConverter<DecimalPercentageReportConverter>();
        }
    }
}
