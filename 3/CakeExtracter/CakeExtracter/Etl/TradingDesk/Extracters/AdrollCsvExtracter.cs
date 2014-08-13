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

        public AdrollCsvExtracter(string csvFilePath)
        {
            this.csvFilePath = csvFilePath;
        }

        protected override void Extract()
        {
            Logger.Info("Extracting DailySummaries from {0}", csvFilePath);
            var items = EnumerateRows();
            Add(items);
            End();
        }

        private IEnumerable<AdrollRow> EnumerateRows()
        {
            using (StreamReader reader = File.OpenText(csvFilePath))
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
