using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using DirectAgents.Domain.Entities.TD;

namespace CakeExtracter.Etl.TradingDesk.Extracters
{
    public class TDStrategySummaryExtracter : Extracter<StrategySummary>
    {
        // if streamReader is not null, use it. otherwise use csvFilePath.
        private readonly StreamReader streamReader;
        private readonly string csvFilePath;

        private readonly ColumnMapping columnMapping;

        public TDStrategySummaryExtracter(ColumnMapping columnMapping, StreamReader streamReader = null, string csvFilePath = null)
        {
            this.columnMapping = columnMapping;
            this.streamReader = streamReader;
            this.csvFilePath = csvFilePath;
        }

        protected override void Extract()
        {
            Logger.Info("Extracting StrategySummaries from {0}", csvFilePath ?? "StreamReader");
            var items = EnumerateRows();
            Add(items);
            End();
        }

        private IEnumerable<StrategySummary> EnumerateRows()
        {
            if (this.streamReader != null)
            {
                foreach (var row in EnumerateRowsInner(this.streamReader))
                    yield return row;
            }
            else if (File.Exists(this.csvFilePath))
            {
                using (StreamReader reader = File.OpenText(this.csvFilePath))
                {
                    foreach (var row in EnumerateRowsInner(reader))
                        yield return row;
                }
            }
        }

        private IEnumerable<StrategySummary> EnumerateRowsInner(StreamReader reader)
        {
            using (CsvReader csv = new CsvReader(reader))
            {
                csv.Configuration.SkipEmptyRecords = true;
                csv.Configuration.WillThrowOnMissingField = false;
                csv.Configuration.IgnoreReadingExceptions = true; // This is at the row level
                csv.Configuration.ReadingExceptionCallback = (ex, row) =>
                {
                    Logger.Error(ex);
                };

                var classMap = CreateCsvClassMap(this.columnMapping);
                csv.Configuration.RegisterClassMap(classMap);

                List<StrategySummary> csvRows = null;
                try
                {
                    csvRows = csv.GetRecords<StrategySummary>().ToList();
                }
                catch (CsvHelperException ex)
                {
                    Logger.Error(ex);
                }
                if (csvRows != null)
                {
                    foreach (var sum in GroupAndEnumerate(csvRows))
                        yield return sum;
                }
            }
        }

        private IEnumerable<StrategySummary> GroupAndEnumerate(List<StrategySummary> csvRows)
        {
            // if StrategyEid's aren't all filled in...
            if (csvRows.Any(r => string.IsNullOrWhiteSpace(r.StrategyEid)))
            {
                var groupedRows = csvRows.GroupBy(r => new { r.Date, r.StrategyEid, r.StrategyName });
                foreach (var group in groupedRows)
                {
                    var sum = new StrategySummary
                    {
                        Date = group.Key.Date,
                        StrategyEid = group.Key.StrategyEid,
                        StrategyName = group.Key.StrategyName,
                        Impressions = group.Sum(g => g.Impressions),
                        Clicks = group.Sum(g => g.Clicks),
                        PostClickConv = group.Sum(g => g.PostClickConv),
                        PostViewConv = group.Sum(g => g.PostViewConv),
                        Cost = group.Sum(g => g.Cost)
                    };
                    yield return sum;
                }
            }
            else
            { // if all StrategyEid's are filled in...
                var groupedRows = csvRows.GroupBy(r => new { r.Date, r.StrategyEid });
                foreach (var group in groupedRows)
                {
                    string stratName = null;
                    if (group.Count() == 1)
                        stratName = group.First().StrategyName;
                    var sum = new StrategySummary
                    {
                        Date = group.Key.Date,
                        StrategyEid = group.Key.StrategyEid,
                        StrategyName = stratName,
                        Impressions = group.Sum(g => g.Impressions),
                        Clicks = group.Sum(g => g.Clicks),
                        PostClickConv = group.Sum(g => g.PostClickConv),
                        PostViewConv = group.Sum(g => g.PostViewConv),
                        Cost = group.Sum(g => g.Cost)
                    };
                    yield return sum;
                }
            }
        }

        private CsvClassMap CreateCsvClassMap(ColumnMapping colMap)
        {
            var classMap = new DefaultCsvClassMap<StrategySummary>();
            Type classType = typeof(StrategySummary);
            TDDailySummaryExtracter.AddBasicPropertyMaps(classMap, classType, colMap);
            TDDailySummaryExtracter.CheckAddPropertyMap(classMap, classType, "StrategyName", colMap.StrategyName);
            TDDailySummaryExtracter.CheckAddPropertyMap(classMap, classType, "StrategyEid", colMap.StrategyEid);
            return classMap;
        }
    }
}
