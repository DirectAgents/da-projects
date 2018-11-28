using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper.Configuration;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.TradingDesk.Extracters.SummaryCsvExtracters
{
    public class SearchTermSummaryExtracter : SummaryCsvExtracter<SearchTermSummary>
    {
        public SearchTermSummaryExtracter(ColumnMapping columnMapping, StreamReader streamReader = null, string csvFilePath = null)
            : base("SearchTermSummaries", columnMapping, streamReader, csvFilePath)
        {
        }

        protected override void AddPropertiesMaps(CsvClassMap classMap, Type classType, ColumnMapping colMap)
        {
            base.AddPropertiesMaps(classMap, classType, colMap);
            CheckAddPropertyMap(classMap, classType, "SearchTermName", colMap.SearchTermName);
        }

        protected override IEnumerable<SearchTermSummary> GroupAndEnumerate(List<SearchTermSummary> csvRows)
        {
            var groupedRows = csvRows.GroupBy(r => new { r.Date, r.SearchTermName });
            var sums = groupedRows.Select(g => InitSummary(g, g.Key.Date, g.Key.SearchTermName));
            return sums.ToList();
        }

        private SearchTermSummary InitSummary(IEnumerable<SearchTermSummary> sums, DateTime date, string name)
        {
            var sum = new SearchTermSummary
            {
                Date = date,
                SearchTermName = name
            };
            sum.SetStats(sums);
            return sum;
        }
    }
}
