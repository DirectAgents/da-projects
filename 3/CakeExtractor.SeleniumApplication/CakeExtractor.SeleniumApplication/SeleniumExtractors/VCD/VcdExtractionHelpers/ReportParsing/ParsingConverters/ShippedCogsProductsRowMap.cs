using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.Models;
using CsvHelper.Configuration;

namespace CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.ReportParsing.ParsingConverters
{
    internal sealed class ShippedCogsProductsRowMap : CsvClassMap<Product>
    {
        public ShippedCogsProductsRowMap()
        {
            Map(m => m.Asin).Name("ASIN");
            Map(m => m.ParentAsin).Name("Parent ASIN");
            Map(m => m.Ean).Name("EAN");
            Map(m => m.Upc).Name("UPC");
            Map(m => m.Brand).Name("Brand");
            Map(m => m.ApparelSize).Name("Apparel Size");
            Map(m => m.ApparelSizeWidth).Name("Apparel Size Width");
            Map(m => m.Binding).Name("Binding");
            Map(m => m.Color).Name("Color");
            Map(m => m.ModelStyleNumber).Name("Model / Style Number");
            Map(m => m.ReleaseDate).Name("Release Date");
            Map(m => m.Name).Name("Product Title");
            Map(m => m.Category).Name("Category");
            Map(m => m.Subcategory).Name("Subcategory");
            Map(m => m.ShippedCogs).Name("Shipped COGS").TypeConverter<DecimalAmountReportConverter>();
            Map(m => m.CustomerReturns).Name("Customer Returns").TypeConverter<IntNumberReportConverter>();
            Map(m => m.FreeReplacements).Name("Free Replacements").TypeConverter<IntNumberReportConverter>();
        }
    }
}
