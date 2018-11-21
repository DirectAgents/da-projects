using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper.Configuration;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.TradingDesk.Extracters.SummaryCsvExtracters
{
    public class TDadSummaryExtracter : SummaryCsvExtracter<TDadSummary>
    {
        public TDadSummaryExtracter(ColumnMapping columnMapping, StreamReader streamReader = null, string csvFilePath = null)
            : base("TDadSummaries", columnMapping, streamReader, csvFilePath)
        {
        }

        protected override void AddPropertiesMaps(CsvClassMap classMap, Type classType, ColumnMapping colMap)
        {
            base.AddPropertiesMaps(classMap, classType, colMap);
            CheckAddPropertyMap(classMap, classType, "AdSetName", colMap.AdSetName);
            CheckAddPropertyMap(classMap, classType, "AdSetEid", colMap.AdSetEid);
            CheckAddPropertyMap(classMap, classType, "TDadName", colMap.TDadName);
            CheckAddPropertyMap(classMap, classType, "TDadEid", colMap.TDadEid);
        }

        protected override IEnumerable<TDadSummary> GroupAndEnumerate(List<TDadSummary> csvRows)
        {
            return csvRows.Any(r => string.IsNullOrWhiteSpace(r.TDadEid))
                ? EnumerateSummariesWithEidAndName(csvRows)
                : EnumerateSummariesWithEid(csvRows);
        }

        private IEnumerable<TDadSummary> EnumerateSummariesWithEidAndName(IEnumerable<TDadSummary> summaries)
        {
            var groupedRows = summaries.GroupBy(r => new { r.Date, r.TDadEid, r.TDadName });
            var sums = groupedRows.Select(g => InitSummary(g, g.Key.Date, g.Key.TDadEid, g.Key.TDadName));
            return sums.ToList();
        }

        private IEnumerable<TDadSummary> EnumerateSummariesWithEid(IEnumerable<TDadSummary> summaries)
        {
            var groupedRows = summaries.GroupBy(r => new { r.Date, r.TDadEid });
            var sums = groupedRows.Select(g => InitSummary(g, g.Key.Date, g.Key.TDadEid, GetTDadNameIfSame(g)));
            return sums.ToList();
        }

        private string GetTDadNameIfSame(IEnumerable<TDadSummary> stats)
        {
            return GetIdIfSame(stats, s => s.TDadName);
        }

        private string GetAdSetNameIfSame(IEnumerable<TDadSummary> stats)
        {
            return GetIdIfSame(stats, s => s.AdSetName);
        }

        private string GetAdSetEidIfSame(IEnumerable<TDadSummary> stats)
        {
            return GetIdIfSame(stats, s => s.AdSetEid);
        }

        private TDadSummary InitSummary(IEnumerable<TDadSummary> sums, DateTime date, string tdAdEid, string tdAdName)
        {
            var sum = new TDadSummary
            {
                Date = date,
                TDadEid = tdAdEid,
                TDadName = tdAdName,
                AdSetEid = GetAdSetEidIfSame(sums),
                AdSetName = GetAdSetNameIfSame(sums),
                //,Width = group.First().Width
            };
            sum.SetStats(sums);
            return sum;
        }
    }
}
