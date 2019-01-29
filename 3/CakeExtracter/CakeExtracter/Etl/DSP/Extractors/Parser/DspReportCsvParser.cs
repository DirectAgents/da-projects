using System.Collections.Generic;
using System.IO;
using System.Linq;
using CakeExtracter.Etl.DSP.Extractors.Parser.Models;
using CakeExtracter.Etl.DSP.Extractors.Parser.ParsingConverters;
using CsvHelper;

namespace CakeExtracter.Etl.DSP.Extractors.Parser
{
    internal class DspReportCsvParser
    {
        public List<CreativeReportRow> GetReportCreatives(string reportCsvText)
        {
            var creatives = ParseProductsFromReport(reportCsvText);
            return creatives;
        }

        private List<CreativeReportRow> ParseProductsFromReport(string reportCsvText)
        {
            using (TextReader sr = new StringReader(reportCsvText))
            {
                var csvHelper = new CsvReader(sr);
                csvHelper.Configuration.SkipEmptyRecords = true;
                csvHelper.Configuration.RegisterClassMap<CreativeReportEntityRowMap>();
                var products = csvHelper.GetRecords<CreativeReportRow>().ToList();
                return products;
            }
        }        
    }
}
