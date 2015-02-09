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
        private readonly StreamReader streamReader;
        // if streamReader is not null, use it. otherwise use csvFilePath.

        private readonly bool wantCreativeDailySummaries;

        public DbmCsvExtracter(string csvFilePath, bool wantCreativeDailySummaries)
        {
            this.csvFilePath = csvFilePath;
            this.wantCreativeDailySummaries = wantCreativeDailySummaries;
        }

        public DbmCsvExtracter(StreamReader streamReader, bool wantCreativeDailySummaries)
        {
            this.streamReader = streamReader;
            this.wantCreativeDailySummaries = wantCreativeDailySummaries;
        }

        protected override void Extract()
        {
            Logger.Info("Extracting DailySummaries from {0}", csvFilePath ?? "StreamReader");
            var items = EnumerateRows();
            Add(items);
            End();
        }

        private IEnumerable<DbmRowBase> EnumerateRows()
        {
            if (streamReader != null)
            {
                foreach (var row in EnumerateRowsStatic(streamReader, wantCreativeDailySummaries))
                    yield return row;
            }
            else
            {
                using (StreamReader reader = File.OpenText(csvFilePath))
                {
                    foreach (var row in EnumerateRowsStatic(reader, wantCreativeDailySummaries))
                        yield return row;
                }
            }
        }

        public static IEnumerable<DbmRowBase> EnumerateRowsStatic(StreamReader reader, bool byCreative)
        {
            using (CsvReader csv = new CsvReader(reader))
            {
                if (byCreative)
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

    public class DbmRowBase
    {
        public string Date { get; set; }

        [CsvField(Name = "Insertion Order")]
        public string InsertionOrder { get; set; }
        [CsvField(Name = "Insertion Order ID")]
        public string InsertionOrderID { get; set; } // int

        public string Impressions { get; set; } // int
        public string Clicks { get; set; } // int
        [CsvField(Name = "Total Conversions")]
        public string TotalConversions { get; set; } // int

        [CsvField(Name = "Revenue (USD)")]
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
