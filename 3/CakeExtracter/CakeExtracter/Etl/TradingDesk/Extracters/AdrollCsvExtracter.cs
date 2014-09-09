using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CakeExtracter.Etl.TradingDesk.Extracters
{
    public class AdrollCsvExtracter : Extracter<AdrollRow>
    {
        private readonly string csvFilePath;
        private readonly StreamReader streamReader;
        // if streamReader is not null, use it. otherwise use csvFilePath.

        public AdrollCsvExtracter(string csvFilePath, StreamReader streamReader)
        {
            this.csvFilePath = csvFilePath;
            this.streamReader = streamReader;
        }

        protected override void Extract()
        {
            Logger.Info("Extracting DailySummaries from {0}", csvFilePath ?? "StreamReader");
            var items = EnumerateRows();
            Add(items);
            End();
        }

        private IEnumerable<AdrollRow> EnumerateRows()
        {
            if (streamReader != null)
            {
                foreach (var row in EnumerateRowsInner(streamReader))
                    yield return row;
            }
            else
            {
                using (StreamReader reader = File.OpenText(csvFilePath))
                {
                    foreach (var row in EnumerateRowsInner(reader))
                        yield return row;
                }
            }
        }

        private IEnumerable<AdrollRow> EnumerateRowsInner(StreamReader reader)
        {
            using (CsvReader csv = new CsvReader(reader))
            {
                var csvRows = csv.GetRecords<AdrollRow>().ToList();
                for (int i = 0; i < csvRows.Count && !String.IsNullOrWhiteSpace(csvRows[i].AdName); i++)
                {
                    yield return csvRows[i];
                }
            }
        }

    }

    public class AdrollRow
    {
        public string Date { get; set; }

        [CsvField(Name = "Ad Name")]
        public string AdName { get; set; }
        public string Size { get; set; }
        public string Type { get; set; }
        [CsvField(Name = "Created Date")]
        public string CreateDate { get; set; }

        [CsvField(Name = "Spend over Period")]
        public string Spend { get; set; }
        public string Impressions { get; set; }
        public string Clicks { get; set; }
        [CsvField(Name = "Total Conv.")]
        public string TotalConversions { get; set; }
    }
}
