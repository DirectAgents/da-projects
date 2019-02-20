namespace CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.ReportParsing.ParsingConverters
{
    internal sealed class OrderedRevenueProductsRowMap : BaseProductRowMap
    {
        public OrderedRevenueProductsRowMap()
        {
            Map(m => m.OrderedRevenue).Name("Ordered Revenue").TypeConverter<DecimalAmountReportConverter>();
        }
    }
}
