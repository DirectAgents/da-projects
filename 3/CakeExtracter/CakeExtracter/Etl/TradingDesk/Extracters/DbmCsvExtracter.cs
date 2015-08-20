using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;

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
                csv.Configuration.IgnoreHeaderWhiteSpace = true;
                csv.Configuration.SkipEmptyRecords = true;
                csv.Configuration.IgnoreReadingExceptions = true;

                if (byCreative)
                {
                    csv.Configuration.RegisterClassMap<DbmRowWithCreativeMap>();

                    var csvRows = csv.GetRecords<DbmRowWithCreative>().ToList();
                    for (int i = 0; i < csvRows.Count; i++)
                    {
                        yield return csvRows[i];
                    }
                }
                else
                {
                    //NOTE: NOT TESTED. (Only tested W/DbmCloudStorageExtracter, which sets byCreative to true.)
                    csv.Configuration.RegisterClassMap<DbmRowMap>();

                    var csvRows = csv.GetRecords<DbmRow>().ToList();
                    for (int i = 0; i < csvRows.Count; i++)
                    {
                        yield return csvRows[i];
                    }
                }
            }
        }
    }

    public sealed class DbmRowMap : CsvClassMap<DbmRow> // NOT TESTED
    {
        public DbmRowMap()
        {
            Map(m => m.Date);
            Map(m => m.InsertionOrder);
            Map(m => m.InsertionOrderID);
            Map(m => m.Impressions);
            Map(m => m.Clicks);
            Map(m => m.TotalConversions);
            Map(m => m.Revenue).Name("Revenue(USD)");
        }
    }
    public sealed class DbmRowWithCreativeMap : CsvClassMap<DbmRowWithCreative>
    {
        public DbmRowWithCreativeMap()
        {
            Map(m => m.Date);
            Map(m => m.InsertionOrder);
            Map(m => m.InsertionOrderID);
            Map(m => m.Impressions);
            Map(m => m.Clicks);
            Map(m => m.TotalConversions);
            Map(m => m.Revenue).Name("Revenue(USD)");
            Map(m => m.Creative);
            Map(m => m.CreativeID);
        }
    }

    public class DbmRowBase
    {
        public DateTime Date { get; set; }

        public string InsertionOrder { get; set; }
        public int InsertionOrderID { get; set; }

        public string Impressions { get; set; } // int
        public string Clicks { get; set; } // int
        public string TotalConversions { get; set; } // int

        public string Revenue { get; set; } // decimal
    }

    public class DbmRow : DbmRowBase
    {
    }

    public class DbmRowWithCreative : DbmRowBase
    {
        public string Creative { get; set; }
        public string CreativeID { get; set; } // int
    }
}
