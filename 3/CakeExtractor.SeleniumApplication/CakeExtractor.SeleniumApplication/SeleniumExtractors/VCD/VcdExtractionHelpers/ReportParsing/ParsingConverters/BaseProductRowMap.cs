using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.Models;
using CsvHelper.Configuration;

namespace CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.ReportParsing.ParsingConverters
{
    internal class BaseProductRowMap : CsvClassMap<Product>
    {
        public BaseProductRowMap()
        {
            Map(m => m.Asin).Name("ASIN");
        }
    }
}
