using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CakeExtracter.Etl.TradingDesk.Extracters
{
    // DoubleClick BidManager Extracter
    public class DbmCsvExtracter : Extracter<Dictionary<string, string>>
    {
        private readonly string csvFilePath;

        public DbmCsvExtracter(string csvFilePath)
        {
            this.csvFilePath = csvFilePath;
        }

        protected override void Extract()
        {
            Logger.Info("Extracting ???DailySummaries??? from {0}", csvFilePath);
            var items = EnumerateRows();
            Add(items);
            End();
        }

        private IEnumerable<Dictionary<string, string>> EnumerateRows()
        {
            using (StreamReader reader = File.OpenText(csvFilePath))
            {
                // Skip row(s)
                for (int i = 0; i < 1; i++)
                    reader.ReadLine();

                using (CsvReader csv = new CsvReader(reader))
                {
                    var csvRows = csv.GetRecords<DbmRow>().ToList();
                    foreach (var csvRow in csvRows)
                    {
                        var row = new Dictionary<string, string>();
                        //row["Gregorian_date"] = csvRow.Gregorian_date;

                        // Use reflection to add values
                        var type = typeof(DbmRow);
                        var properties = type.GetProperties();
                        foreach (var propertyInfo in properties)
                        {
                            row[propertyInfo.Name] = (string)propertyInfo.GetValue(csvRow);
                        }
                        yield return row;
                    }
                }
            }
        }

        public class DbmRow
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
            [CsvField(Name="Total Conversions")]
            public string TotalConversions { get; set; } // int

            [CsvField(Name="Revenue (Adv Currency)")]
            public string Revenue { get; set; } // decimal
        }
    }
}
