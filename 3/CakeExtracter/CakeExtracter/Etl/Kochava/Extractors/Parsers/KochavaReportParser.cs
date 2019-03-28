using CakeExtracter.Etl.Kochava.Extractors.Parsers.ParsingConverters;
using CakeExtracter.Etl.Kochava.Models;
using CsvHelper;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CakeExtracter.Etl.Kochava.Extractors.Parsers
{
    /// <summary>
    /// Kochava csv report parser
    /// </summary>
    internal class KochavaReportParser
    {
        /// <summary>
        /// Parses the kochava report.
        /// </summary>
        /// <param name="reportCSVText">The report CSV text.</param>
        /// <returns></returns>
        public List<KochavaReportItem> ParseKochavaReport(string reportCSVText)
        {
            using (TextReader sr = new StringReader(reportCSVText))
            {
                var csvHelper = new CsvReader(sr);
                csvHelper.Configuration.SkipEmptyRecords = true;
                csvHelper.Configuration.RegisterClassMap<KochavaReportEntityRowMap>();
                var kochavaItems = csvHelper.GetRecords<KochavaReportItem>().ToList();
                return kochavaItems;
            }
        }
    }
}
