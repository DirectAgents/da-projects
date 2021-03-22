using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Amazon.Helpers;
using CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Extractors.VcdCustomReportsExtractionHelpers.ReportParsing.RowMaps;
using CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Models;
using CsvHelper;
using CsvHelper.Configuration;

namespace CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Extractors.VcdCustomReportsExtractionHelpers.ReportParsing
{
    /// <summary>
    /// Parser for Geographic Sales Insights vcd reports.
    /// </summary>
    internal class GeographicSalesInsightsReportCsvParser
    {
        private const string ValueDelimiter = ";;;";
        private readonly int accountId;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeographicSalesInsightsReportCsvParser"/> class.
        /// </summary>
        /// <param name="accountId">Internal account ID.</param>
        public GeographicSalesInsightsReportCsvParser(int accountId)
        {
            this.accountId = accountId;
        }

        /// <summary>
        /// Parses the  Geographic Sales Insight Report data.
        /// </summary>
        /// <param name="reportCsvText">The report CSV text.</param>
        /// <param name="date">The date.</param>
        /// <returns>Collection of products with filled shipped revenue data.</returns>
        public List<GeographicSalesInsightsProduct> ParseGeographicSalesInsightReportData(string reportCsvText, DateTime date)
        {
            var products = ParseProductsFromReport<GeographicSalesInsightsProductsRowMap>(
                reportCsvText, date, "geographicSalesInsights");
            return products;
        }

        private List<GeographicSalesInsightsProduct> ParseProductsFromReport<T>(string reportCsvText, DateTime date, string reportType)
            where T : CsvClassMap<GeographicSalesInsightsProduct>
        {
            try
            {
                var products = new List<GeographicSalesInsightsProduct>();
                Logger.Info(accountId, "Started parsing csv report");
                using (TextReader sr = new StringReader(reportCsvText))
                {
                    using (var csvHelper = new CsvReader(sr))
                    {
                        csvHelper.Configuration.SkipEmptyRecords = true;
                        csvHelper.Configuration.Delimiter = ValueDelimiter;
                        csvHelper.Configuration.RegisterClassMap<T>();
                        products = csvHelper.GetRecords<GeographicSalesInsightsProduct>().ToList();
                    }
                    return products;
                }
            }
            catch (Exception ex)
            {
                var exception = new Exception("Error occured while parsing report", ex);
                Logger.Warn(accountId, $"{exception.Message}: {ex.Message}");
                SaveUnsuccessfullyParseredReport(reportCsvText, date, reportType);
                throw exception;
            }
        }

        private void SaveUnsuccessfullyParseredReport(string reportContent, DateTime date, string reportType)
        {
            try
            {
                var logFileName = $"vcd_{accountId}_{date:MMddyyyy}_{reportType}.csv";
                FileManager.SaveToFileInExecutionFolder(logFileName, reportContent);
                Logger.Info(accountId, $"Report content was saved to {logFileName} file.");
            }
            catch (Exception ex)
            {
                Logger.Error(accountId, ex);
            }
        }
    }
}
