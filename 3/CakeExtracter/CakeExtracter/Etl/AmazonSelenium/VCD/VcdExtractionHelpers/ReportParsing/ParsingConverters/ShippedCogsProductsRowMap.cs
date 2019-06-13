namespace CakeExtracter.Etl.AmazonSelenium.VCD.VcdExtractionHelpers.ReportParsing.ParsingConverters
{
    internal sealed class ShippedCogsProductsRowMap : BaseProductRowMap
    {
        public ShippedCogsProductsRowMap()
        {
            Map(m => m.ShippedCogs).Name("Shipped COGS").TypeConverter<DecimalAmountReportConverter>();
            Map(m => m.CustomerReturns).Name("Customer Returns").TypeConverter<IntNumberReportConverter>();
            Map(m => m.FreeReplacements).Name("Free Replacements").TypeConverter<IntNumberReportConverter>();
        }
    }
}
