namespace CakeExtracter.Etl.AmazonSelenium.VCD.Extractors.VcdExtractionHelpers.ReportParsing.ParsingConverters
{
    /// <inheritdoc />
    /// <summary>
    /// Row map configuration for Shipped Revenue reports.
    /// </summary>
    internal sealed class ShippedRevenueProductsRowMap : BaseProductRowMap
    {
        /// <inheritdoc cref="BaseProductRowMap"/>
        /// <summary>
        /// Initializes a new instance of the <see cref="ShippedRevenueProductsRowMap" /> class.
        /// </summary>
        public ShippedRevenueProductsRowMap()
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
            Map(m => m.ReleaseDate).Name("releasedate");
            Map(m => m.Name).Name("producttitle");
            Map(m => m.Category).Name("category");
            Map(m => m.Subcategory).Name("subcategory");
            Map(m => m.ShippedRevenue).Name("shippedrevenue").TypeConverter<DecimalAmountReportConverter>();
            Map(m => m.ShippedUnits).Name("shippedunits").TypeConverter<IntNumberReportConverter>();
            Map(m => m.OrderedUnits).Name("orderedunits").TypeConverter<IntNumberReportConverter>();
        }
    }
}