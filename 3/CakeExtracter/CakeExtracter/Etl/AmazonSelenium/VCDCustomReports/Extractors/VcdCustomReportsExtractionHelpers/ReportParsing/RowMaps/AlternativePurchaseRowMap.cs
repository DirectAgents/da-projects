using CakeExtracter.Etl.AmazonSelenium.VCD.Extractors.VcdExtractionHelpers.ReportParsing.ParsingConverters;
using CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Models.Products;

namespace CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Extractors.VcdCustomReportsExtractionHelpers.ReportParsing.RowMaps
{
    /// <inheritdoc />
    /// <summary>
    /// Row map configuration for Alternative Purchase reports.
    /// </summary>
    internal sealed class AlternativePurchaseRowMap : BaseCustomReportRowMap<AlternativePurchaseProduct>
    {
        /// <inheritdoc cref="BaseProductRowMap"/>
        /// <summary>
        /// Initializes a new instance of the <see cref="AlternativePurchaseRowMap" /> class.
        /// </summary>
        public AlternativePurchaseRowMap()
        {
            Map(m => m.Asin).Name("asin");
            Map(m => m.Name).Name("producttitle");
            Map(m => m.No1PurchasedAsin).Name("no1purchasedasin");
            Map(m => m.No1PurchasedProductTitle).Name("no1purchasedproducttitle");
            Map(m => m.No1PurchasedPercent).Name("no1purchasedpercent").TypeConverter<DecimalPercentageReportConverter>();
            Map(m => m.No2PurchasedAsin).Name("no2purchasedasin");
            Map(m => m.No2PurchasedProductTitle).Name("no2purchasedproducttitle");
            Map(m => m.No2PurchasedPercent).Name("no2purchasedpercent").TypeConverter<DecimalPercentageReportConverter>();
            Map(m => m.No3PurchasedAsin).Name("no3purchasedasin");
            Map(m => m.No3PurchasedProductTitle).Name("no3purchasedproducttitle");
            Map(m => m.No3PurchasedPercent).Name("no3purchasedpercent").TypeConverter<DecimalPercentageReportConverter>();
            Map(m => m.No4PurchasedAsin).Name("no4purchasedasin");
            Map(m => m.No4PurchasedProductTitle).Name("no4purchasedproducttitle");
            Map(m => m.No4PurchasedPercent).Name("no4purchasedpercent").TypeConverter<DecimalPercentageReportConverter>();
            Map(m => m.No5PurchasedAsin).Name("no5purchasedasin");
            Map(m => m.No5PurchasedProductTitle).Name("no5purchasedproducttitle");
            Map(m => m.No5PurchasedPercent).Name("no5purchasedpercent").TypeConverter<DecimalPercentageReportConverter>();
        }
    }
}
