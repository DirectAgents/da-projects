using System;
using System.Collections.Generic;
using System.IO;
using CakeExtracter.Common;
using CakeExtracter.Etl.DBM.Composer;
using CakeExtracter.Etl.DBM.Extractors.Parsers.Models;
using CakeExtracter.Etl.DBM.Extractors.Parsers.ParsingConverters;
using CakeExtracter.Etl.DBM.Models;
using DBM;
using DirectAgents.Domain.Entities.CPProg.DBM.SummaryMetrics;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.DBM.Extractors
{
    /// <inheritdoc />
    /// <summary>
    /// Extractor of DBM creative summaries.
    /// </summary>
    internal class DbmCreativeExtractor : DbmExtractor
    {
        private readonly int reportId;
        private readonly DbmReportDataComposer composer;
        private readonly DbmReportDataConverter converter;

        /// <inheritdoc cref="DbmExtractor"/>
        /// <summary>
        /// Initializes a new instance of the <see cref="DbmCreativeExtractor" /> class.
        /// </summary>
        public DbmCreativeExtractor(
            DBMUtility dbmUtility, DateRange dateRange, IEnumerable<ExtAccount> accounts, int creativeReportId, bool keepReports)
            : base(dbmUtility, dateRange, keepReports)
        {
            reportId = creativeReportId;
            composer = new DbmReportDataComposer(accounts);
            converter = new DbmReportDataConverter();
        }

        /// <summary>
        /// Extracts DBM creative summaries from report.
        /// </summary>
        /// <returns>List of DBM creative summaries.</returns>
        public List<DbmCreativeSummary> Extract()
        {
            try
            {
                Logger.Info($"Start extracting creative report [report ID: {reportId}]...");
                var summariesGroups = PrepareCreativeReportData();
                var summaries = GetCreativeSummaries(summariesGroups);
                Logger.Info($"Finished extracting creative report [report ID: {reportId}] (items extracted [{summaries.Count}])");
                return summaries;
            }
            catch (Exception e)
            {
                throw new Exception("Failed extracting creative summaries.", e);
            }
        }

        private IEnumerable<DbmAccountCreativeReportData> PrepareCreativeReportData()
        {
            var reportContent = GetReportContent(reportId);
            var reportRows = GetCreativeRows(reportContent);
            var summariesGroups = GetCreativeSummariesGroupedByAccount(reportRows);
            return summariesGroups;
        }

        private List<DbmCreativeSummary> GetCreativeSummaries(IEnumerable<DbmAccountCreativeReportData> summariesGroups)
        {
            var allCreativeSummaries = new List<DbmCreativeSummary>();
            foreach (var creativeReportData in summariesGroups)
            {
                var summariesForAccount = GetAccountSummariesFromReportData(creativeReportData);
                allCreativeSummaries.AddRange(summariesForAccount);
            }
            return allCreativeSummaries;
        }

        private IEnumerable<DbmCreativeSummary> GetAccountSummariesFromReportData(DbmAccountCreativeReportData reportData)
        {
            var summariesForAccount = converter.ConvertCreativeReportDataToSummaries(reportData);
            return summariesForAccount;
        }

        private IEnumerable<DbmCreativeReportRow> GetCreativeRows(StreamReader reportContent)
        {
            var rowMap = new DbmCreativeReportEntityRowMap();
            var creativeReportRows = GetReportRows<DbmCreativeReportRow>(reportContent, rowMap);
            return creativeReportRows;
        }

        private IEnumerable<DbmAccountCreativeReportData> GetCreativeSummariesGroupedByAccount(IEnumerable<DbmCreativeReportRow> reportRows)
        {
            Logger.Info("Composing report data...");
            var summariesGroupedByAccount = composer.ComposeCreativeReportData(reportRows);
            Logger.Info($"Report data composed. Retrieved groups by account (group count: {summariesGroupedByAccount.Count})");
            return summariesGroupedByAccount;
        }
    }
}