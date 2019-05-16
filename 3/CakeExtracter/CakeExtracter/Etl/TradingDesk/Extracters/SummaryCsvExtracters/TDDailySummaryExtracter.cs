using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.TradingDesk.Extracters.SummaryCsvExtracters
{
    public class TDDailySummaryExtracter : SummaryCsvExtracter<DailySummary>
    {
        public TDDailySummaryExtracter(ColumnMapping columnMapping, StreamReader streamReader = null, string csvFilePath = null)
            : base("DailySummaries", columnMapping, streamReader, csvFilePath)
        {
        }

        protected override IEnumerable<DailySummary> GroupAndEnumerate(List<DailySummary> csvRows)
        {
            var groupedRows = csvRows.GroupBy(r => r.Date);
            var sums = groupedRows.Select(x => InitSummary(x, x.Key.Date));
            return sums.ToList();
        }

        private DailySummary InitSummary(IEnumerable<DailySummary> sums, DateTime date)
        {
            var sum = new DailySummary
            {
                Date = date
            };
            sum.SetStats(sums);
            return sum;
        }
    }
}
