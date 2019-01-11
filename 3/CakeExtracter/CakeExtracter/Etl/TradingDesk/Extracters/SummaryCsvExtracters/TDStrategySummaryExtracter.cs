using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper.Configuration;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.TradingDesk.Extracters.SummaryCsvExtracters
{
    public class TDStrategySummaryExtracter : SummaryCsvExtracter<StrategySummary>
    {
        public TDStrategySummaryExtracter(ColumnMapping columnMapping, StreamReader streamReader = null, string csvFilePath = null)
            : base("StrategySummaries", columnMapping, streamReader, csvFilePath)
        {
        }

        protected override void AddPropertiesMaps(CsvClassMap classMap, Type classType, ColumnMapping colMap)
        {
            base.AddPropertiesMaps(classMap, classType, colMap);
            CheckAddPropertyMap(classMap, classType, "StrategyName", colMap.StrategyName);
            CheckAddPropertyMap(classMap, classType, "StrategyEid", colMap.StrategyEid);
        }

        protected override IEnumerable<StrategySummary> GroupAndEnumerate(List<StrategySummary> csvRows)
        {
            return csvRows.Any(r => string.IsNullOrWhiteSpace(r.StrategyEid)) 
                ? EnumerateSummariesWithEidAndName(csvRows) 
                : EnumerateSummariesWithEid(csvRows);
        }

        private IEnumerable<StrategySummary> EnumerateSummariesWithEidAndName(IEnumerable<StrategySummary> strategySummaries)
        {
            var groupedRows = strategySummaries.GroupBy(r => new { r.Date, r.StrategyEid, r.StrategyName });
            var sums = groupedRows.Select(g => InitSummary(g, g.Key.Date, g.Key.StrategyEid, g.Key.StrategyName));
            return sums.ToList();
        }

        private IEnumerable<StrategySummary> EnumerateSummariesWithEid(IEnumerable<StrategySummary> strategySummaries)
        {
            var groupedRows = strategySummaries.GroupBy(r => new { r.Date, r.StrategyEid });
            var sums = groupedRows.Select(g => InitSummary(g, g.Key.Date, g.Key.StrategyEid, GetStrategyNameIfSame(g)));
            return sums.ToList();
        }

        private string GetStrategyNameIfSame(IEnumerable<StrategySummary> stats)
        {
            return GetIdIfSame(stats, s => s.StrategyName);
        }

        private StrategySummary InitSummary(IEnumerable<StrategySummary> sums, DateTime date, string strategyEid, string strategyName)
        {
            var sum = new StrategySummary
            {
                Date = date,
                StrategyEid = strategyEid,
                StrategyName = strategyName
            };
            sum.SetStats(sums);
            return sum;
        }

        protected override bool ShouldReaderSkipRecord(string[] fields)
        {
            var isSkipRecord = base.ShouldReaderSkipRecord(fields);
            var isFirstColumnIsNotDateTime = ShouldReaderSkipRecordNotDateTime(fields[0]);

            return isSkipRecord || isFirstColumnIsNotDateTime;
        }

        /// <summary>
        /// Method skips those records for which the value of the first column isn't Date Time (except for the title)
        /// </summary>
        /// <param name="firstColumn">The first column of a row from a CSV report, the value of the first column must be of a type DateTime</param>
        /// <returns>True - skip the record; False - not skip the record</returns>
        private bool ShouldReaderSkipRecordNotDateTime(string firstColumn)
        {
            const string columnHeaderName = "Date";
            if (string.Equals(firstColumn, columnHeaderName))
            {
                return false; // not skip because this column is a header
            }            
            return !DateTime.TryParse(firstColumn, out _);
        }
    }
}
