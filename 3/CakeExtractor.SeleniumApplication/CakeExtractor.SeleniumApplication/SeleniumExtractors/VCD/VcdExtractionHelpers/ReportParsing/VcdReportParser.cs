using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.Models;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.ReportParsing.ParsingConverters;
using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.ReportParsing
{
    internal class VcdReportCSVParser
    {
        public List<Product> ParseShippedRevenueReportData(string reportCsvText)
        {
            var products = ParseProductsFromReport<ShippedRevenueProductRowMap>(reportCsvText);
            return products;
        }

        public List<Product> ParseShippedCogsReportData(string reportCsvText)
        {
            var products = ParseProductsFromReport<ShippedCogsProductsRowMap>(reportCsvText);
            return products;
        }

        private List<Product> ParseProductsFromReport<T>(string reportCsvText) where T: CsvClassMap<Product>
        {
            reportCsvText = RemoveFirstLine(reportCsvText);
            using (TextReader sr = new StringReader(reportCsvText))
            {
                var csvHelper = new CsvReader(sr);
                csvHelper.Configuration.SkipEmptyRecords = true;
                csvHelper.Configuration.RegisterClassMap<T>();
                var products = csvHelper.GetRecords<Product>().ToList();
                return products;
            }
        }

        private string RemoveFirstLine(string text)
        {
            var lines = Regex.Split(text, "\r\n|\r|\n").Skip(1);
            return string.Join(Environment.NewLine, lines.ToArray());
        }
    }
}
