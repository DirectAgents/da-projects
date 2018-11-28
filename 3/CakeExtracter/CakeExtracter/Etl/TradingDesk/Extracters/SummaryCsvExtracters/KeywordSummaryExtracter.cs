using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper.Configuration;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.TradingDesk.Extracters.SummaryCsvExtracters
{
    public class KeywordSummaryExtracter : SummaryCsvExtracter<KeywordSummary>
    {
        public KeywordSummaryExtracter(ColumnMapping columnMapping, StreamReader streamReader = null, string csvFilePath = null)
            : base("KeywordSummaries", columnMapping, streamReader, csvFilePath)
        {
        }

        protected override void AddPropertiesMaps(CsvClassMap classMap, Type classType, ColumnMapping colMap)
        {
            base.AddPropertiesMaps(classMap, classType, colMap);
            CheckAddPropertyMap(classMap, classType, "StrategyName", colMap.StrategyName);
            CheckAddPropertyMap(classMap, classType, "StrategyEid", colMap.StrategyEid);
            CheckAddPropertyMap(classMap, classType, "AdSetName", colMap.AdSetName);
            CheckAddPropertyMap(classMap, classType, "AdSetEid", colMap.AdSetEid);
            CheckAddPropertyMap(classMap, classType, "KeywordName", colMap.KeywordName);
            CheckAddPropertyMap(classMap, classType, "KeywordEid", colMap.KeywordEid);
        }

        protected override IEnumerable<KeywordSummary> GroupAndEnumerate(List<KeywordSummary> csvRows)
        {
            return csvRows.Any(r => string.IsNullOrWhiteSpace(r.KeywordEid))
                ? EnumerateSummariesWithEidAndName(csvRows)
                : EnumerateSummariesWithEid(csvRows);
        }

        private IEnumerable<KeywordSummary> EnumerateSummariesWithEidAndName(IEnumerable<KeywordSummary> summaries)
        {
            var groupedRows = summaries.GroupBy(r => new { r.Date, r.KeywordEid, r.KeywordName});
            var sums = groupedRows.Select(g => InitSummary(g, g.Key.Date, g.Key.KeywordEid, g.Key.KeywordName));
            return sums.ToList();
        }

        private IEnumerable<KeywordSummary> EnumerateSummariesWithEid(IEnumerable<KeywordSummary> summaries)
        {
            var groupedRows = summaries.GroupBy(r => new { r.Date, r.KeywordEid });
            var sums = groupedRows.Select(g => InitSummary(g, g.Key.Date, g.Key.KeywordEid, GetKeywordNameIfSame(g)));
            return sums.ToList();
        }

        private string GetKeywordNameIfSame(IEnumerable<KeywordSummary> stats)
        {
            return GetIdIfSame(stats, s => s.KeywordName);
        }

        private string GetAdSetNameIfSame(IEnumerable<KeywordSummary> stats)
        {
            return GetIdIfSame(stats, s => s.AdSetName);
        }

        private string GetAdSetEidIfSame(IEnumerable<KeywordSummary> stats)
        {
            return GetIdIfSame(stats, s => s.AdSetEid);
        }

        private string GetStrategyNameIfSame(IEnumerable<KeywordSummary> stats)
        {
            return GetIdIfSame(stats, s => s.StrategyName);
        }

        private string GetStrategyEidIfSame(IEnumerable<KeywordSummary> stats)
        {
            return GetIdIfSame(stats, s => s.StrategyEid);
        }

        private KeywordSummary InitSummary(IEnumerable<KeywordSummary> sums, DateTime date, string eid, string name)
        {
            var sum = new KeywordSummary
            {
                Date = date,
                KeywordEid = eid,
                KeywordName = name,
                AdSetName = GetAdSetNameIfSame(sums),
                AdSetEid = GetAdSetEidIfSame(sums),
                StrategyName = GetStrategyNameIfSame(sums),
                StrategyEid = GetStrategyEidIfSame(sums)
            };
            sum.SetStats(sums);
            return sum;
        }
    }
}
