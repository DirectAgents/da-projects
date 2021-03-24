using CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Models.Base;
using CsvHelper.Configuration;

namespace CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Extractors.VcdCustomReportsExtractionHelpers.ReportParsing.RowMaps
{
    internal class BaseCustomReportRowMap<TProduct> : CsvClassMap<TProduct>
        where TProduct : VcdCustomProduct
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCustomReportRowMap"/> class.
        /// </summary>
        public BaseCustomReportRowMap()
        {
            Map(m => m.Asin).Name("asin");
            Map(m => m.Name).Name("producttitle");
        }
    }
}