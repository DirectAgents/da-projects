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
                foreach (var row in EnumerateRowsStatic(streamReader, byCreative: wantCreativeDailySummaries))
                    yield return row;
            }
            else
            {
                using (StreamReader reader = File.OpenText(csvFilePath))
                {
                    foreach (var row in EnumerateRowsStatic(reader, byCreative: wantCreativeDailySummaries))
                        yield return row;
                }
            }
        }

        public static IEnumerable<DbmRowBase> EnumerateRowsStatic(StreamReader reader, bool byCreative = false, bool bySite = false)
        {
            using (CsvReader csv = new CsvReader(reader))
            {
                csv.Configuration.IgnoreHeaderWhiteSpace = true;
                csv.Configuration.SkipEmptyRecords = true;
                csv.Configuration.IgnoreReadingExceptions = true;

                // if byCreative is true, ignores bySite
                if (byCreative)
                {
                    csv.Configuration.RegisterClassMap<DbmRowWithCreativeMap>();

                    var csvRows = csv.GetRecords<DbmRowWithCreative>().ToList();
                    for (int i = 0; i < csvRows.Count; i++)
                    {
                        yield return csvRows[i];
                    }
                }
                else if (bySite)
                {
                    csv.Configuration.RegisterClassMap<DbmRowWithSiteMap>();

                    var csvRows = csv.GetRecords<DbmRowWithSite>().ToList();
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
            Map(m => m.PostClickConversions).Name("Post-ClickConversions");
            Map(m => m.PostViewConversions).Name("Post-ViewConversions");
            Map(m => m.Revenue).Name("Revenue(USD)");
            Map(m => m.Creative);
            Map(m => m.CreativeID);
        }
    }
    public sealed class DbmRowWithSiteMap : CsvClassMap<DbmRowWithSite>
    {
        public DbmRowWithSiteMap()
        {
            Map(m => m.Date).Name("Month");
            Map(m => m.InsertionOrder);
            Map(m => m.InsertionOrderID);
            Map(m => m.Impressions);
            Map(m => m.Clicks);
            Map(m => m.TotalConversions);
            Map(m => m.PostClickConversions).Name("Post-ClickConversions");
            Map(m => m.PostViewConversions).Name("Post-ViewConversions");
            Map(m => m.Revenue).Name("Revenue(USD)");
            Map(m => m.Site);
            Map(m => m.SiteID);
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
        public string PostClickConversions { get; set; } // int
        public string PostViewConversions { get; set; } // int

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
    public class DbmRowWithSite : DbmRowBase
    {
        private string _site;
        public string Site
        {
            get { return _site; }
            set { _site = value.ToLower(); }
        }
        public string SiteID { get; set; } // int
    }
}
