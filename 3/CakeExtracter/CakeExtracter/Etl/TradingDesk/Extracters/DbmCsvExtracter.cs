using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CakeExtracter.Etl.TradingDesk.Extracters
{
    // DoubleClick BidManager Extracter
    public class DbmCsvExtracter : Extracter<DbmRowBase>
    {
        private readonly string csvFilePath;
        private readonly bool wantCreativeDailySummaries;

        public DbmCsvExtracter(string csvFilePath, bool wantCreativeDailySummaries)
        {
            this.csvFilePath = csvFilePath;
            this.wantCreativeDailySummaries = wantCreativeDailySummaries;
        }

        protected override void Extract()
        {
            Logger.Info("Extracting DailySummaries from {0}", csvFilePath);
            var items = EnumerateRows();
            Add(items);
            End();
        }

        private IEnumerable<DbmRowBase> EnumerateRows()
        {
            using (StreamReader reader = File.OpenText(csvFilePath))
            {
                using (CsvReader csv = new CsvReader(reader))
                {
                    if (wantCreativeDailySummaries)
                    {
                        var csvRows = csv.GetRecords<DbmRowWithCreative>().ToList();
                        for (int i = 0; i < csvRows.Count && !String.IsNullOrWhiteSpace(csvRows[i].InsertionOrder); i++)
                        {
                            yield return csvRows[i];
                        }
                    }
                    else
                    {
                        var csvRows = csv.GetRecords<DbmRow>().ToList();
                        for (int i = 0; i < csvRows.Count && !String.IsNullOrWhiteSpace(csvRows[i].InsertionOrder); i++)
                        {
                            yield return csvRows[i];
                        }
                    }
                }
            }
        }
    }

    public class DbmRowBase
    {
        public string Date { get; set; }

        [CsvField(Name = "Insertion Order")]
        public string InsertionOrder { get; set; }
        [CsvField(Name = "Insertion Order ID")]
        public string InsertionOrderID { get; set; } // int

        [CsvField(Name = "Advertiser Currency")]
        public string AdvertiserCurrency { get; set; }

        public string Impressions { get; set; } // int
        public string Clicks { get; set; } // int
        [CsvField(Name = "Total Conversions")]
        public string TotalConversions { get; set; } // int

        [CsvField(Name = "Revenue (Adv Currency)")]
        public string Revenue { get; set; } // decimal
    }

    public class DbmRow : DbmRowBase
    {
    }

    public class DbmRowWithCreative : DbmRowBase
    {
        public string Creative { get; set; }
        [CsvField(Name = "Creative ID")]
        public string CreativeID { get; set; } // int
    }
}
