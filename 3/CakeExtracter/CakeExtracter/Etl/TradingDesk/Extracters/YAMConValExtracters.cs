using CakeExtracter.Common;
using CakeExtracter.Etl.TradingDesk.Extracters.SummaryCsvExtracters;
using DirectAgents.Domain.Entities.CPProg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Yahoo;

namespace CakeExtracter.Etl.TradingDesk.Extracters
{
    public abstract class YAMConValExtracter<T> : Extracter<T>
        where T: DatedStatsSummaryWithRev, new()
    {
        public const string ConValPattern = @"gv=(\d*\.?\d*)";

        protected readonly YAMUtility _yamUtility;
        protected readonly DateRange dateRange;
        protected readonly int yamAdvertiserId;
        protected readonly int accountId;

        public YAMConValExtracter(YAMUtility yamUtility, DateRange dateRange, ExtAccount account)
        {
            this._yamUtility = yamUtility;
            this.dateRange = dateRange;
            this.accountId = account.Id;
            this.yamAdvertiserId = int.Parse(account.ExternalId);
        }

        protected void InitSummary(T summary, DateTime date, IEnumerable<YAMRow> rows)
        {
            var clickThrus = rows.Where(x => x.ClickThruConvs > 0);
            var viewThrus = rows.Where(x => x.ViewThruConvs > 0);
            summary.Date = date;
            summary.PostClickRev = clickThrus.Sum(x => GetConVal(x.PixelParameter));
            summary.PostViewRev = viewThrus.Sum(x => GetConVal(x.PixelParameter));
        }

        protected decimal GetConVal(string pixelParameter)
        {
            if (pixelParameter == null)
            {
                return 0;
            }
            var match = Regex.Match(pixelParameter, ConValPattern);
            if (!match.Success)
            {
                return 0;
            }
            decimal conval;
            return decimal.TryParse(match.Groups[1].Value, out conval) ? conval : 0;
        }
    }

    public class YAMDailyConValExtracter : YAMConValExtracter<DailySummary>
    {
        public YAMDailyConValExtracter(YAMUtility yamUtility, DateRange dateRange, ExtAccount account)
            : base(yamUtility, dateRange, account)
        { }

        protected override void Extract()
        {
            var payload = _yamUtility.CreateReportRequestPayload(dateRange.FromDate, dateRange.ToDate, this.yamAdvertiserId, byPixelParameter: true);
            var reportUrl = _yamUtility.GenerateReport(payload);

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

    public class YAMStrategyConValExtracter : YAMConValExtracter<StrategySummary>
    {
        protected readonly string[] existingStrategyNames;

        public YAMStrategyConValExtracter(YAMUtility yamUtility, DateRange dateRange, ExtAccount account, string[] existingStrategyNames = null)
            : base(yamUtility, dateRange, account)
        {
            this.existingStrategyNames = existingStrategyNames ?? new string[] { };
        }

        protected override void Extract()
        {
            var payload = _yamUtility.CreateReportRequestPayload(dateRange.FromDate, dateRange.ToDate, this.yamAdvertiserId, byPixelParameter: true, byLine: true);
            var reportUrl = _yamUtility.GenerateReport(payload);

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
