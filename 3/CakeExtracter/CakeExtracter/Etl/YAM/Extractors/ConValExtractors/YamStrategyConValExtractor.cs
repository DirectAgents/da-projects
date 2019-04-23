using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Common;
using CakeExtracter.Etl.TradingDesk.Extracters.SummaryCsvExtracters;
using DirectAgents.Domain.Entities.CPProg;
using Yahoo;

namespace CakeExtracter.Etl.YAM.Extractors.ConValExtractors
{
    public class YamStrategyConValExtractor : BaseYamConValExtractor<StrategySummary>
    {
        protected readonly string[] existingStrategyNames;

        public YamStrategyConValExtractor(YAMUtility yamUtility, DateRange dateRange, ExtAccount account, string[] existingStrategyNames = null)
            : base(yamUtility, dateRange, account)
        {
            this.existingStrategyNames = existingStrategyNames ?? new string[] { };
        }

        protected override void Extract()
        {
            var reportUrl = _yamUtility.TryGenerateReport(dateRange.FromDate, dateRange.ToDate, this.yamAdvertiserId,
                byPixelParameter: true, byLine: true);

            if (!string.IsNullOrWhiteSpace(reportUrl))
            {
                var streamReader = TDDailySummaryExtracter.CreateStreamReaderFromUrl(reportUrl);
                var extractor = new YamConValCsvExtracter<YAMRowMap_WithLine>(streamReader: streamReader);
                var yamRows = extractor.EnumerateRows();
                var items = EnumerateRows(yamRows);
                Add(items);
            }

            End();
        }

        private IEnumerable<StrategySummary> EnumerateRows(IEnumerable<YAMRow> csvRows)
        {
            var groupedRows = csvRows.GroupBy(x => new { x.Day, x.LineName, x.LineID }).Select(x => InitStrategySummary(x.Key.Day, x.Key.LineName, x.Key.LineID, x));

            // The dictionary key is the strategy name; The value, a HashSet, is used to see if all dates are represented for that strategy
            var strategyHashes = new Dictionary<string, HashSet<DateTime>>();
            foreach (var ss in groupedRows)
            {
                if (!strategyHashes.ContainsKey(ss.StrategyName))
                {
                    strategyHashes[ss.StrategyName] = new HashSet<DateTime>(dateRange.Dates);
                }

                strategyHashes[ss.StrategyName].Remove(ss.Date);
                yield return ss;
            }
            // Create empty StrategySummaries for any dates that weren't covered - for each strategy found
            foreach (var stratName in strategyHashes.Keys)
            {
                var dateHash = strategyHashes[stratName];
                foreach (var date in dateHash)
                {
                    var ss = new StrategySummary { Date = date, StrategyName = stratName };
                    yield return ss;
                }
            }
            // Handle any strategies with SS's in the db that weren't represented in the extracted stats
            var missingStrategies = existingStrategyNames.Where(x => !strategyHashes.Keys.Contains(x));
            foreach (var stratName in missingStrategies)
            {
                foreach (var date in dateRange.Dates)
                {
                    var ss = new StrategySummary { Date = date, StrategyName = stratName };
                    yield return ss;
                }
            }
        }

        private StrategySummary InitStrategySummary(DateTime date, string lineName, string lineId, IEnumerable<YAMRow> rows)
        {
            var sum = new StrategySummary
            {
                StrategyName = lineName,
                StrategyEid = lineId
            };
            InitSummary(sum, date, rows);
            return sum;
        }
    }
}
