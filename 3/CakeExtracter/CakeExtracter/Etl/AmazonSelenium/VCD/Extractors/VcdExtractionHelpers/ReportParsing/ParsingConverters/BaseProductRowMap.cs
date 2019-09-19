using CakeExtracter.Etl.AmazonSelenium.VCD.Models;
using CsvHelper.Configuration;

namespace CakeExtracter.Etl.AmazonSelenium.VCD.Extractors.VcdExtractionHelpers.ReportParsing.ParsingConverters
{
    internal class BaseProductRowMap : CsvClassMap<Product>
    {
        public BaseProductRowMap()
        {
            Map(m => m.Asin).Name("asin");
        }
    }
}