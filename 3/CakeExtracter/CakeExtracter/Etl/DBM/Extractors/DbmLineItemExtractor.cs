using System;
using System.Collections.Generic;
using System.IO;
using CakeExtracter.Common;
using CakeExtracter.Etl.DBM.Composer;
using CakeExtracter.Etl.DBM.Extractors.Parsers.Models;
using CakeExtracter.Etl.DBM.Extractors.Parsers.ParsingConverters;
using CakeExtracter.Etl.DBM.Models;
using DBM;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.DBM.SummaryMetrics;

namespace CakeExtracter.Etl.DBM.Extractors
{
    /// <inheritdoc />
    /// <summary>
    /// Extractor of DBM line item summaries.
    /// </summary>
    internal class DbmLineItemExtractor : DbmExtractor
    {
        private readonly int reportId;
        private readonly DbmReportDataComposer composer;
        private readonly DbmReportDataConverter converter;

        /// <inheritdoc cref="DbmExtractor"/>
        /// <summary>
        /// Initializes a new instance of the <see cref="DbmLineItemExtractor" /> class.
        /// </summary>
        public DbmLineItemExtractor(
            DBMUtility dbmUtility, DateRange dateRange, IEnumerable<ExtAccount> accounts, int creativeReportId, bool keepReports)
            : base(dbmUtility, dateRange, keepReports)
        {
            reportId = creativeReportId;
            composer = new DbmReportDataComposer(accounts);
            converter = new DbmReportDataConverter();
        }

        /// <summary>
        /// Extracts DBM line item summaries from report.
        /// </summary>
        /// <returns>List of DBM line item summaries.</returns>
        public List<DbmLineItemSummary> Extract()
        {
            try
            {
                Logger.Info($"Start extracting line item report [report ID: {reportId}]...");
                var summariesGroups = PrepareLineItemReportData();
                var summaries = GetLineItemSummaries(summariesGroups);
                Logger.Info($"Finished extracting line item report [report ID: {reportId}] (items extracted [{summaries.Count}])");
                return summaries;
            }
            catch (Exception e)
            {
                throw new Exception("Failed extracting line item summaries.", e);
            }
        }

        private IEnumerable<DbmAccountLineItemReportData> PrepareLineItemReportData()
        {
            var reportContent = GetReportContent(reportId);
            var reportRows = GetLineItemRows(reportContent);
            var summariesGroups = GetLineItemSummariesGroupedByAccount(reportRows);
            return summariesGroups;
        }

        private List<DbmLineItemSummary> GetLineItemSummaries(IEnumerable<DbmAccountLineItemReportData> summariesGroups)
        {
            var allLineItemSummaries = new List<DbmLineItemSummary>();
            foreach (var lineItemReportData in summariesGroups)
            {
                var summariesForAccount = GetAccountSummariesFromReportData(lineItemReportData);
                allLineItemSummaries.AddRange(summariesForAccount);
            }
            return allLineItemSummaries;
        }

        private IEnumerable<DbmLineItemSummary> GetAccountSummariesFromReportData(DbmAccountLineItemReportData reportData)
        {
            var summariesForAccount = converter.ConvertLineItemReportDataToSummaries(reportData);
            return summariesForAccount;
        }

        private IEnumerable<DbmLineItemReportRow> GetLineItemRows(StreamReader reportContent)
        {
            var rowMap = new DbmLineItemReportEntityRowMap();
            var lineItemReportRows = GetReportRows<DbmLineItemReportRow>(reportContent, rowMap);
            return lineItemReportRows;
        }

        private IEnumerable<DbmAccountLineItemReportData> GetLineItemSummariesGroupedByAccount(IEnumerable<DbmLineItemReportRow> reportRows)
        {
            Logger.Info("Composing report data...");
            var summariesGroupedByAccount = composer.ComposeLineItemReportData(reportRows);
            Logger.Info($"Report data composed. Retrieved groups by account (group count: {summariesGroupedByAccount.Count})");
            return summariesGroupedByAccount;
        }
    }
}