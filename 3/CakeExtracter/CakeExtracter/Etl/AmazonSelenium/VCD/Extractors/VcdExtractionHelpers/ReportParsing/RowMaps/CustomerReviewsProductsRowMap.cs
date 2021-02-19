using CakeExtracter.Etl.AmazonSelenium.VCD.Extractors.VcdExtractionHelpers.ReportParsing.ParsingConverters;

namespace CakeExtracter.Etl.AmazonSelenium.VCD.Extractors.VcdExtractionHelpers.ReportParsing.RowMaps
{
    /// <inheritdoc />
    /// <summary>
    /// Row map configuration for Customer Reviews reports.
    /// </summary>
    internal sealed class CustomerReviewsProductsRowMap : BaseProductRowMap
    {
        /// <inheritdoc cref="BaseProductRowMap"/>
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerReviewsProductsRowMap" /> class.
        /// </summary>
        public CustomerReviewsProductsRowMap()
        {
            Map(m => m.ParentAsin).Name("parentasin");
            Map(m => m.Ean).Name("ean");
            Map(m => m.Upc).Name("upc");
            Map(m => m.Brand).Name("brand");
            Map(m => m.ApparelSize).Name("apparelsize");
            Map(m => m.ApparelSizeWidth).Name("apparelsizewidth");
            Map(m => m.Binding).Name("binding");
            Map(m => m.Color).Name("color");
            Map(m => m.ModelStyleNumber).Name("modelstyle");
            Map(m => m.Name).Name("producttitle");
            Map(m => m.Category).Name("category");
            Map(m => m.Subcategory).Name("subcategory");
            Map(m => m.NumberOfCustomerReviews).Name("numberofcustomerreviews").TypeConverter<IntNumberReportConverter>();
            Map(m => m.NumberOfCustomerReviewsLifeToDate).Name("numberofcustomerreviewslifetodate").TypeConverter<IntNumberReportConverter>();
            Map(m => m.AverageCustomerRating).Name("averagecustomerrating").TypeConverter<DecimalAmountReportConverter>();
            Map(m => m.FiveStars).Name("fivestars").TypeConverter<IntNumberReportConverter>();
            Map(m => m.FourStars).Name("fourstars").TypeConverter<IntNumberReportConverter>();
            Map(m => m.ThreeStars).Name("threestars").TypeConverter<IntNumberReportConverter>();
            Map(m => m.TwoStars).Name("twostars").TypeConverter<IntNumberReportConverter>();
            Map(m => m.OneStar).Name("onestar").TypeConverter<IntNumberReportConverter>();
        }
    }
}