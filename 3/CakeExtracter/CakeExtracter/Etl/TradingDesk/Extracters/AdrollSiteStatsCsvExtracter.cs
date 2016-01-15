using CakeExtracter.Etl;
using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CakeExtracter.Etl.TradingDesk.Extracters
{
    public class AdrollSiteStatsCsvExtracter : Extracter<AdrollSiteStatsRow>
    {
        private readonly string csvFilePath;
        private readonly StreamReader streamReader;
        // if streamReader is not null, use it. otherwise use csvFilePath.

        public AdrollSiteStatsCsvExtracter(string csvFilePath, StreamReader streamReader)
        {
            this.csvFilePath = csvFilePath;
            this.streamReader = streamReader;
        }

        protected override void Extract()
        {
            Logger.Info("Extracting Site Stats from {0}", csvFilePath ?? "StreamReader");
            var items = EnumerateRows();
            Add(items);
            End();
        }

        private IEnumerable<AdrollSiteStatsRow> EnumerateRows()
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

        private IEnumerable<AdrollSiteStatsRow> EnumerateRowsInner(StreamReader reader)
        {
            using (CsvReader csv = new CsvReader(reader))
            {
                csv.Configuration.IgnoreHeaderWhiteSpace = true;
                csv.Configuration.SkipEmptyRecords = true;
                csv.Configuration.RegisterClassMap<AdrollSiteStatsRowMap>();

                var csvRows = csv.GetRecords<AdrollSiteStatsRow>().ToList();
                for (int i = 0; i < csvRows.Count; i++)
                {
                    yield return csvRows[i];
                }
            }
        }
    }

 
 public sealed class AdrollSiteStatsRowMap : CsvClassMap<AdrollSiteStatsRow>
    {
        public AdrollSiteStatsRowMap()
        {
            Map(m => m.website).Name("Website");
            Map(m => m.size).Name("Size");
            Map(m => m.cost).Name("Spend over Period");
            Map(m => m.impression).Name("Impressions");
            Map(m => m.click).Name("Clicks");
        }
    }

    public class AdrollSiteStatsRow
    {
        public string website { get; set; }
        public string size { get; set; }
        public string cost { get; set; }
        public string impression { get; set; }
        public string click { get; set; }

    }
}