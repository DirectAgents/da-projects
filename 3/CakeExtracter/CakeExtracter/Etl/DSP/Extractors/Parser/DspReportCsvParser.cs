using System.Collections.Generic;
using System.IO;
using System.Linq;
using CakeExtracter.Etl.DSP.Extractors.Parser.Models;
using CakeExtracter.Etl.DSP.Extractors.Parser.ParsingConverters;
using CsvHelper;

namespace CakeExtracter.Etl.DSP.Extractors.Parser
{
    /// <summary>Dsp report parser.</summary>
    internal class DspReportCsvParser
    {
        /// <summary>Gets the report creatives from text csv report content.</summary>
        /// <param name="reportCsvText">The report CSV text.</param>
        /// <returns></returns>
        public List<CreativeReportRow> GetReportCreatives(string reportCsvText)
        {
            using (TextReader sr = new StringReader(reportCsvText))
            {
                var csvHelper = new CsvReader(sr);
                csvHelper.Configuration.SkipEmptyRecords = true;
                csvHelper.Configuration.RegisterClassMap<CreativeReportEntityRowMap>();
                var creatives = csvHelper.GetRecords<CreativeReportRow>().ToList();
                return creatives;
            }
        }
    }
}
