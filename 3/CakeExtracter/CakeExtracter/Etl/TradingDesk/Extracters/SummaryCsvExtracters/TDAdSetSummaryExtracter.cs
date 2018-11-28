using CsvHelper.Configuration;
using DirectAgents.Domain.Entities.CPProg;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CakeExtracter.Etl.TradingDesk.Extracters.SummaryCsvExtracters
{
    public class TDAdSetSummaryExtracter : SummaryCsvExtracter<AdSetSummary>
    {
        public TDAdSetSummaryExtracter(ColumnMapping columnMapping, StreamReader streamReader = null, string csvFilePath = null)
            : base("AdSetSummaries", columnMapping, streamReader, csvFilePath)
        {
        }

        protected override void AddPropertiesMaps(CsvClassMap classMap, Type classType, ColumnMapping colMap)
        {
            base.AddPropertiesMaps(classMap, classType, colMap);
            CheckAddPropertyMap(classMap, classType, "StrategyName", colMap.StrategyName); //optional ?
            CheckAddPropertyMap(classMap, classType, "StrategyEid", colMap.StrategyEid); //optional ?
            CheckAddPropertyMap(classMap, classType, "AdSetName", colMap.AdSetName);
            CheckAddPropertyMap(classMap, classType, "AdSetEid", colMap.AdSetEid);
        }

        protected override IEnumerable<AdSetSummary> GroupAndEnumerate(List<AdSetSummary> csvRows)
        {
            return csvRows.Any(r => string.IsNullOrWhiteSpace(r.AdSetEid)) 
                ? EnumerateSummariesWithEidAndName(csvRows) 
                : EnumerateSummariesWithEid(csvRows);
        }

        private IEnumerable<AdSetSummary> EnumerateSummariesWithEidAndName(IEnumerable<AdSetSummary> adSummaries)
        {
            var groupedRows = adSummaries.GroupBy(r => new { r.Date, r.AdSetEid, r.AdSetName });
            var sums = groupedRows.Select(g => InitSummary(g, g.Key.Date, g.Key.AdSetEid, g.Key.AdSetName));
            return sums.ToList();
        }

        private IEnumerable<AdSetSummary> EnumerateSummariesWithEid(IEnumerable<AdSetSummary> adSummaries)
        {
            var groupedRows = adSummaries.GroupBy(r => new { r.Date, r.AdSetEid });
            var sums = groupedRows.Select(g => InitSummary(g, g.Key.Date, g.Key.AdSetEid, GetAdSetNameIfSame(g)));
            return sums.ToList();
        }

        private string GetAdSetNameIfSame(IEnumerable<AdSetSummary> stats)
        {
            return GetIdIfSame(stats, s => s.AdSetName);
        }

        private string GetStrategyEidIfSame(IEnumerable<AdSetSummary> stats)
        {
            return GetIdIfSame(stats, s => s.StrategyEid);
        }

        private string GetStrategyNameIfSame(IEnumerable<AdSetSummary> stats)
        {
            return GetIdIfSame(stats, s => s.StrategyName);
        }

        private AdSetSummary InitSummary(IEnumerable<AdSetSummary> sums, DateTime date, string adSetEid, string adSetName)
        {
            var sum = new AdSetSummary
            {
                Date = date,
                AdSetEid = adSetEid,
                AdSetName = adSetName,
                StrategyEid = GetStrategyEidIfSame(sums),
                StrategyName = GetStrategyNameIfSame(sums)
            };
            sum.SetStats(sums);
            return sum;
        }
    }
}
