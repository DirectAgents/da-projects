using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CakeExtracter.Common;
using CakeExtracter.Etl.DBM.Downloader;
using CakeExtracter.Etl.DBM.Extractors.Parsers;
using CsvHelper.Configuration;
using DBM;
using DBM.Helpers;
using DBM.Parsers.Models;

namespace CakeExtracter.Etl.DBM.Extractors
{
    internal class DbmExtractor
    {
        private readonly DBMUtility dbmUtility;
        private readonly DateRange dateRange;
        private readonly bool keepReports;

        private const string savedReportFileName = "dbm_{0}.csv";
        private const string savedReportsDirectoryName = "SavedReports";

        public DbmExtractor(DBMUtility dbmUtility, DateRange dateRange, bool keepReports)
        {
            this.dbmUtility = dbmUtility;
            this.dateRange = dateRange;
            this.keepReports = keepReports;
        }

        protected StreamReader GetReportContent(int reportId)
        {
            var reportUrl = GetReportUrl(reportId);
            var reportContent = GetReportContentFromUrl(reportUrl);
            return reportContent;
        }

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
            var reportFileName = string.Format(savedReportFileName, DateTime.Now.ToString("yyyyMMdd_HHmmss"));
            try
            {
                FileManager.SaveToFileInExecutionFolder(savedReportsDirectoryName, reportFileName, reportContent);
                Logger.Info($"Report content was saved to {reportFileName} file.");
            }
            catch (Exception e)
            {
                Logger.Error(new Exception($"Saving report failed ({reportFileName})", e));
            }
        }
    }
}
