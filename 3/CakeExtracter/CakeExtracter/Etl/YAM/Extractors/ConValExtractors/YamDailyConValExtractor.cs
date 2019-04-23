using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Common;
using CakeExtracter.Etl.TradingDesk.Extracters.SummaryCsvExtracters;
using DirectAgents.Domain.Entities.CPProg;
using Yahoo;

namespace CakeExtracter.Etl.YAM.Extractors.ConValExtractors
{
    public class YamDailyConValExtractor : BaseYamConValExtractor<DailySummary>
    {
        public YamDailyConValExtractor(YAMUtility yamUtility, DateRange dateRange, ExtAccount account)
            : base(yamUtility, dateRange, account)
        { }

        protected override void Extract()
        {
            var reportUrl = _yamUtility.TryGenerateReport(dateRange.FromDate, dateRange.ToDate, this.yamAdvertiserId,
                byPixelParameter: true);

            if (!string.IsNullOrWhiteSpace(reportUrl))
            {
                var streamReader = TDDailySummaryExtracter.CreateStreamReaderFromUrl(reportUrl);
                var extractor = new YamConValCsvExtracter<YAMRowMap>(streamReader: streamReader);
                var yamRows = extractor.EnumerateRows();
                var items = EnumerateRows(yamRows);
                Add(items);
            }
            End();
        }

        private IEnumerable<DailySummary> EnumerateRows(IEnumerable<YAMRow> csvRows)
        {
            var dates = new HashSet<DateTime>(dateRange.Dates);

            var sums = csvRows.GroupBy(x => x.Day).Select(x => InitDailySummary(x.Key.Date, x));
            foreach (var ds in sums)
            {
                dates.Remove(ds.Date);
                yield return ds;
            }

            // Create empty DailySummaries for any dates that weren't covered
            foreach (var date in dates)
            {
                var ds = new DailySummary { Date = date };
                yield return ds;
            }
        }

        private DailySummary InitDailySummary(DateTime date, IEnumerable<YAMRow> rows)
        {
            var sum = new DailySummary();
            InitSummary(sum, date, rows);
            return sum;
        }
    }
}