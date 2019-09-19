namespace CakeExtracter.Etl.AmazonSelenium.VCD.Extractors.VcdExtractionHelpers.ReportParsing.ParsingConverters
{
    internal sealed class ShippedRevenueProductRowMap : BaseProductRowMap
    {
        public ShippedRevenueProductRowMap()
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
            Map(m => m.ReleaseDate).Name("releasedate").TypeConverter<DateTimeReportConverter>();
            Map(m => m.Name).Name("producttitle");
            Map(m => m.Category).Name("category");
            Map(m => m.Subcategory).Name("subcategory");
            Map(m => m.ShippedRevenue).Name("shippedrevenue").TypeConverter<DecimalAmountReportConverter>();
            Map(m => m.ShippedUnits).Name("shippedunits").TypeConverter<IntNumberReportConverter>();
            Map(m => m.OrderedUnits).Name("orderedunits").TypeConverter<IntNumberReportConverter>();
        }
    }
}