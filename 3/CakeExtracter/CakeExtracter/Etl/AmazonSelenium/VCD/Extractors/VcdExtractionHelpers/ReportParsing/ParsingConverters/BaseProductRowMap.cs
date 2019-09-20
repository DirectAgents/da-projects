using CakeExtracter.Etl.AmazonSelenium.VCD.Models;
using CsvHelper.Configuration;

namespace CakeExtracter.Etl.AmazonSelenium.VCD.Extractors.VcdExtractionHelpers.ReportParsing.ParsingConverters
{
    /// <inheritdoc />
    /// <summary>
    /// Row map configuration common for three kind of VCD reports.
    /// </summary>
    internal class BaseProductRowMap : CsvClassMap<Product>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseProductRowMap"/> class.
        /// </summary>
        public BaseProductRowMap()
        {
            Map(m => m.Asin).Name("asin");
        }
    }
}