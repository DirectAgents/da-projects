namespace CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.ReportParsing.ParsingConverters
{
    internal sealed class ShippedRevenueProductRowMap : BaseProductRowMap
    {
        public ShippedRevenueProductRowMap()
        {
            Map(m => m.ShippedRevenue).Name("Shipped Revenue").TypeConverter<DecimalAmountReportConverter>();
            Map(m => m.ShippedUnits).Name("Shipped Units").TypeConverter<IntNumberReportConverter>();
            Map(m => m.OrderedUnits).Name("Ordered Units").TypeConverter<IntNumberReportConverter>();
        }
    }
}
