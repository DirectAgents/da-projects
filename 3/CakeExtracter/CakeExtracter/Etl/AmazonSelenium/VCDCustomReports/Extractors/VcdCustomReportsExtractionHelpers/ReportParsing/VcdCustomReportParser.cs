using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Amazon.Helpers;
using CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Extractors.VcdCustomReportsExtractionHelpers.ReportParsing.RowMaps;
using CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Models.Base;
using CsvHelper;
using CsvHelper.Configuration;

namespace CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Extractors.VcdCustomReportsExtractionHelpers.ReportParsing
{
    /// <summary>
    /// Parser for vcd custom reports.
    /// </summary>
    internal class VcdCustomReportParser<TProduct>
        where TProduct : VcdCustomProduct
    {
        private const string ValueDelimiter = ";;;";
        private readonly int accountId;

        /// <summary>
        /// Initializes a new instance of the <see cref="VcdCustomReportParser"/> class.
        /// </summary>
        /// <param name="accountId">Internal account ID.</param>
        public VcdCustomReportParser(int accountId)
        {
            this.accountId = accountId;
        }

        /// <summary>
        /// Parses the Repeat Purchase Behavior Report data.
        /// </summary>
        /// <param name="reportCsvText">The report CSV text.</param>
        /// <param name="reportPeriod">The period for report.</param>
        /// <returns>Collection of products with filled shipped revenue data.</returns>
        public List<TProduct> ParseReportData<TReportRowMap>(string reportCsvText, string reportPeriod, string reportType)
            where TReportRowMap : BaseCustomReportRowMap<TProduct>
        {
            var products = ParseProductsFromReport<TReportRowMap>(reportCsvText, reportType, reportPeriod);
            return products;
        }

        private List<TProduct> ParseProductsFromReport<T>(string reportCsvText, string reportType, string reportPeriod)
            where T : CsvClassMap<TProduct>
        {
            try
            {
                Logger.Info(accountId, "Started parsing csv report");
                using (TextReader sr = new StringReader(reportCsvText))
                {
                    List<TProduct> products;
                    using (var csvHelper = new CsvReader(sr))
                    {
                        csvHelper.Configuration.SkipEmptyRecords = true;
                        csvHelper.Configuration.Delimiter = ValueDelimiter;
                        csvHelper.Configuration.RegisterClassMap<T>();
                        products = csvHelper.GetRecords<TProduct>().ToList();
                    }
                    return products;
                }
            }
            catch (Exception ex)
            {
                var exception = new Exception("Error occured while parsing report", ex);
                Logger.Warn(accountId, $"{exception.Message}: {ex.Message}");
                SaveUnsuccessfullyParseredReport(reportCsvText, reportType, reportPeriod);
                throw exception;
            }
        }

        private void SaveUnsuccessfullyParseredReport(string reportContent, string reportType, string reportPeriod)
        {
            try
            {
                var logFileName = $"vcd_{accountId}_{reportPeriod}_{reportType}.csv";
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