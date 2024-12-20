﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Amazon.Helpers;
using CakeExtracter.Etl.AmazonSelenium.VCD.Extractors.VcdExtractionHelpers.ReportParsing.RowMaps;
using CakeExtracter.Etl.AmazonSelenium.VCD.Models;
using CsvHelper;
using CsvHelper.Configuration;

namespace CakeExtracter.Etl.AmazonSelenium.VCD.Extractors.VcdExtractionHelpers.ReportParsing
{
    /// <summary>
    /// Parser for vcd reports.
    /// </summary>
    internal class VcdReportCsvParser
    {
        private const string ValueDelimiter = ";;;";
        private readonly int accountId;

        /// <summary>
        /// Initializes a new instance of the <see cref="VcdReportCsvParser"/> class.
        /// </summary>
        /// <param name="accountId">Internal account ID.</param>
        public VcdReportCsvParser(int accountId)
        {
            this.accountId = accountId;
        }

        /// <summary>
        /// Parses the shipped revenue report data.
        /// </summary>
        /// <param name="reportCsvText">The report CSV text.</param>
        /// <param name="date">The date.</param>
        /// <returns>Collection of products with filled shipped revenue data.</returns>
        public List<Product> ParseShippedRevenueReportData(string reportCsvText, DateTime date)
        {
            var products = ParseProductsFromReport<ShippedRevenueProductsRowMap>(
                reportCsvText, date, "shippedRev");
            return products;
        }

        /// <summary>
        /// Parses the shipped cogs report data.
        /// </summary>
        /// <param name="reportCsvText">The report CSV text.</param>
        /// <param name="date">The date.</param>
        /// <returns>Collection of products with filled shipped cogs data.</returns>
        public List<Product> ParseShippedCogsReportData(string reportCsvText, DateTime date)
        {
            var products = ParseProductsFromReport<ShippedCogsProductsRowMap>(
                reportCsvText, date, "shippedCogs");
            return products;
        }

        /// <summary>
        /// Parses the ordered revenue report data.
        /// </summary>
        /// <param name="reportCsvText">The report CSV text.</param>
        /// <param name="date">The date.</param>
        /// <returns>Collection of products with filled ordered revenue data.</returns>
        public List<Product> ParseOrderedRevenueReportData(string reportCsvText, DateTime date)
        {
            var products = ParseProductsFromReport<OrderedRevenueProductsRowMap>(
                reportCsvText, date, "orderedRev");
            return products;
        }

        /// <summary>
        /// Parses the inventory health report data.
        /// </summary>
        /// <param name="reportCsvText">The report CSV text.</param>
        /// <param name="date">The date.</param>
        /// <returns>Collection of products with filled ordered revenue data.</returns>
        public List<Product> ParseInventoryHealthReportData(string reportCsvText, DateTime date)
        {
            var products = ParseProductsFromReport<InventoryHealthProductsRowMap>(
                reportCsvText, date, "inventoryHealth");
            return products;
        }

        private List<Product> ParseProductsFromReport<T>(string reportCsvText, DateTime date, string reportType)
            where T : CsvClassMap<Product>
        {
            try
            {
                var products = new List<Product>();
                Logger.Info(accountId, "Started parsing csv report");
                using (TextReader sr = new StringReader(reportCsvText))
                {
                    using (var csvHelper = new CsvReader(sr))
                    {
                        csvHelper.Configuration.SkipEmptyRecords = true;
                        csvHelper.Configuration.Delimiter = ValueDelimiter;
                        csvHelper.Configuration.RegisterClassMap<T>();
                        products = csvHelper.GetRecords<Product>().ToList();
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