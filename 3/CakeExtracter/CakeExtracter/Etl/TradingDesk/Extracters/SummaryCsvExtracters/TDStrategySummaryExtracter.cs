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
    }
}
