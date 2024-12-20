﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CakeExtracter.Common;
using CakeExtracter.Etl.DBM.Downloader;
using CakeExtracter.Etl.DBM.Extractors.Parsers;
using CakeExtracter.Etl.DBM.Extractors.Parsers.Models;
using CsvHelper.Configuration;
using DBM;
using DBM.Helpers;

namespace CakeExtracter.Etl.DBM.Extractors
{
    /// <summary>
    /// Basic DBM extractor.
    /// </summary>
    internal class DbmExtractor
    {
        private const string SavedReportFileName = "dbm_{0}.csv";
        private const string SavedReportsDirectoryName = "SavedReports";
        private readonly DBMUtility dbmUtility;
        private readonly DateRange dateRange;
        private readonly bool keepReports;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbmExtractor"/> class.
        /// </summary>
        /// <param name="dbmUtility">DBM utility.</param>
        /// <param name="dateRange">Date range for extract.</param>
        /// <param name="keepReports">Store received DBM reports in a separate folder.</param>
        public DbmExtractor(DBMUtility dbmUtility, DateRange dateRange, bool keepReports)
        {
            this.dbmUtility = dbmUtility;
            this.dateRange = dateRange;
            this.keepReports = keepReports;
        }

        /// <summary>
        /// Returns the content of report by its identifier.
        /// </summary>
        /// <param name="reportId">Identifier of report.</param>
        /// <returns>Content of report.</returns>
        protected StreamReader GetReportContent(int reportId)
        {
            var reportUrl = GetReportUrl(reportId);
            var reportContent = GetReportContentFromUrl(reportUrl);
            return reportContent;
        }

        /// <summary>
        /// Parses the report content.
        /// </summary>
        /// <typeparam name="TDbmReportRow">Base DBM report row type.</typeparam>
        /// <param name="stream">Stream with content of report.</param>
        /// <param name="rowMap">Row map for parsing.</param>
        /// <returns>List of report rows.</returns>
        protected List<TDbmReportRow> GetReportRows<TDbmReportRow>(StreamReader stream, CsvClassMap rowMap)
            where TDbmReportRow : DbmBaseReportRow
        {
            Logger.Info("Parsing a report...");
            var parser = new DbmReportCsvParser<TDbmReportRow>(dateRange, rowMap, streamReader: stream);
            var reportRows = parser.EnumerateRows().ToList();
            Logger.Info($"The report parsed successfully (row count: {reportRows.Count})");
            return reportRows;
        }

        private string GetReportUrl(int reportId)
        {
            Logger.Info($"Retrieve a report location URL from the query ID [{reportId}]...");
            var reportUrl = dbmUtility.GetURLForReport(reportId);
            if (string.IsNullOrWhiteSpace(reportUrl))
            {
                throw new Exception("Could not retrieve the report location URL.");
            }
            Logger.Info($"The report location URL: {reportUrl}.");
            return reportUrl;
        }

        private StreamReader GetReportContentFromUrl(string reportUrl)
        {
            Logger.Info("Downloading a report...");
            var reportContent = DbmReportDownloader.GetStreamReaderFromUrl(reportUrl);
            if (reportContent == StreamReader.Null)
            {
                throw new Exception($"Failed downloading a report [report URL: {reportUrl}]");
            }
            if (keepReports)
            {
                var reportContentToSave = DbmReportDownloader.GetStreamReaderFromUrl(reportUrl);
                SaveReport(reportContentToSave);
            }
            Logger.Info("The report downloaded successfully");
            return reportContent;
        }

        private static void SaveReport(StreamReader reportContent)
        {
            var reportFileName = string.Format(SavedReportFileName, DateTime.Now.ToString("yyyyMMdd_HHmmss"));
            try
            {
                FileManager.SaveToFileInExecutionFolder(SavedReportsDirectoryName, reportFileName, reportContent);
                Logger.Info($"Report content was saved to {reportFileName} file.");
            }
            catch (Exception e)
            {
                Logger.Error(new Exception($"Saving report failed ({reportFileName})", e));
            }
        }
    }
}