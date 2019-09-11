namespace CakeExtracter.Etl.AmazonSelenium.VCD.Extractors.VcdExtractionHelpers.ReportParsing.ParsingConverters
{
    internal sealed class ShippedRevenueProductRowMap : BaseProductRowMap
    {
        public ShippedRevenueProductRowMap()
        {
            Map(m => m.ParentAsin).Name("Parent ASIN");
            Map(m => m.Ean).Name("EAN");
            Map(m => m.Upc).Name("UPC");
            Map(m => m.Brand).Name("Brand");
            Map(m => m.ApparelSize).Name("Apparel Size");
            Map(m => m.ApparelSizeWidth).Name("Apparel Size Width");
            Map(m => m.Binding).Name("Binding");
            Map(m => m.Color).Name("Color")
                .ConvertUsing(row => row.TryGetField<string>("Color", out var value)
                ? value
                : row.GetField<string>("Colour"));
            Map(m => m.ModelStyleNumber).Name("Model / Style Number");
            Map(m => m.ReleaseDate).Name("Release Date");
            Map(m => m.Name).Name("Product Title");
            Map(m => m.Category).Name("Category");
            Map(m => m.Subcategory).Name("Subcategory");
            Map(m => m.ShippedRevenue).Name("Shipped Revenue").TypeConverter<DecimalAmountReportConverter>();
            Map(m => m.ShippedUnits).Name("Shipped Units").TypeConverter<IntNumberReportConverter>();
            Map(m => m.OrderedUnits).Name("Ordered Units").TypeConverter<IntNumberReportConverter>();
        }
    }
}