﻿using Amazon.Helpers;
using CakeExtracter;
using CakeExtracter.Common;
using CakeExtractor.SeleniumApplication.Configuration.Models;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.Models;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.ReportParsing.ParsingConverters;
using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.ReportParsing
{
    /// <summary>
    /// Parser for vcd reports.
    /// </summary>
    internal class VcdReportCSVParser
    {
        /// <summary>
        /// Parses the shipped revenue report data.
        /// </summary>
        /// <param name="reportCsvText">The report CSV text.</param>
        /// <param name="accountInfo">The account information.</param>
        /// <param name="date">The date.</param>
        /// <returns>Collection of productes with filled shipped revenue data.</returns>
        public List<Product> ParseShippedRevenueReportData(string reportCsvText, AccountInfo accountInfo, DateTime date)
        {
            var products = ParseProductsFromReport<ShippedRevenueProductRowMap>(reportCsvText, accountInfo, date, "shippedRev");
            return products;
        }

        /// <summary>
        /// Parses the shipped cogs report data.
        /// </summary>
        /// <param name="reportCsvText">The report CSV text.</param>
        /// <param name="accountInfo">The account information.</param>
        /// <param name="date">The date.</param>
        /// <returns>Collection of productes with filled shipped cogs data.</returns>
        public List<Product> ParseShippedCogsReportData(string reportCsvText, AccountInfo accountInfo, DateTime date)
        {
            var products = ParseProductsFromReport<ShippedCogsProductsRowMap>(reportCsvText, accountInfo, date, "shippedCogs");
            return products;
        }

        /// <summary>
        /// Parses the ordered revenue report data.
        /// </summary>
        /// <param name="reportCsvText">The report CSV text.</param>
        /// <param name="accountInfo">The account information.</param>
        /// <param name="date">The date.</param>
        /// <returns>Collection of productes with filled ordered revenue data.</returns>
        public List<Product> ParseOrderedRevenueReportData(string reportCsvText, AccountInfo accountInfo, DateTime date)
        {
            var products = ParseProductsFromReport<OrderedRevenueProductsRowMap>(reportCsvText, accountInfo, date, "orderedRev");
            return products;
        }

        private List<Product> ParseProductsFromReport<T>(string reportCsvText, AccountInfo accountInfo, DateTime date,
            string reportType) where T : CsvClassMap<Product>
        {
            try
            {
                Logger.Info(accountInfo.Account.Id, "Started parsing csv report");
                reportCsvText = TextUtils.RemoveFirstLine(reportCsvText);
                using (TextReader sr = new StringReader(reportCsvText))
                {
                    var csvHelper = new CsvReader(sr);
                    csvHelper.Configuration.SkipEmptyRecords = true;
                    csvHelper.Configuration.RegisterClassMap<T>();
                    var products = csvHelper.GetRecords<Product>().ToList();
                    return products;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(accountInfo.Account.Id, new Exception("Error occured while parsing report", ex));
                SaveUnsuccessfullyParesedReport(reportCsvText, date, reportType, accountInfo.Account.Id);
                throw ex;
            }
        }

        private void SaveUnsuccessfullyParesedReport(string reportContent, DateTime date, string reportType, int accountId)
        {
            try
            {
                var logFileName = $"vcd_{accountId}_{date.ToString("MMddyyyy")}_{reportType}.csv";
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
