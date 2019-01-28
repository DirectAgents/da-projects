﻿using CakeExtracter.Common;
using CakeExtracter.Etl.DSP.Extractors.Parser.ParsingConverters;
using CakeExtracter.Etl.DSP.Models;
using CsvHelper;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CakeExtracter.Etl.DSP.Extractors
{
    internal class DspReportCsvParser
    {
        public List<CreativeReportEntity> GetReportCreatives(string reportCsvText)
        {
            var creatives = ParseProductsFromReport(reportCsvText);
            return creatives;
        }

        private List<CreativeReportEntity> ParseProductsFromReport(string reportCsvText)
        {
            using (TextReader sr = new StringReader(reportCsvText))
            {
                var csvHelper = new CsvReader(sr);
                csvHelper.Configuration.SkipEmptyRecords = true;
                csvHelper.Configuration.RegisterClassMap<CreativeReportentityRowMap>();
                var products = csvHelper.GetRecords<CreativeReportEntity>().ToList();
                return products;
            }
        }
    }
}
