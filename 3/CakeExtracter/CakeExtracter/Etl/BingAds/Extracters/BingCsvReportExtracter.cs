using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CakeExtracter.Etl.BingAds.Extracters
{
    public class BingCsvReportExtracter : Extracter<Dictionary<string, string>>
    {
        private readonly string csvFilePath;

        public BingCsvReportExtracter(string csvFilePath) //, string accountName)
        {
            this.csvFilePath = csvFilePath;
        }

        protected override void Extract()
        {
            Logger.Info("Extracting SearchDailySummaries from {0}", csvFilePath);
            var items = EnumerateRows();
            Add(items);
            End();
        }

        private IEnumerable<Dictionary<string, string>> EnumerateRows()
        {
            using (StreamReader reader = File.OpenText(csvFilePath))
            {
                for (int i = 0; i < 9; i++)
                    reader.ReadLine();

                using (CsvReader csv = new CsvReader(reader))
                {
                    var csvRows = csv.GetRecords<BingRow>().ToList();
                    foreach (var csvRow in csvRows)
                    {
                        var row = new Dictionary<string, string>();
                        //row["Gregorian_date"] = csvRow.Gregorian_date;

                        // Use reflection to add values
                        var type = typeof(BingRow);
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
    }

    public class BingRow
    {
        [CsvField(Name="Gregorian date")]
        public string Gregorian_date { get; set; }

        public string Impressions { get; set; } // int
        public string Clicks { get; set; } // int
        public string Spend { get; set; } // decimal
        public string Conversions { get; set; } // int
        public string Revenue { get; set; } // decimal

        [CsvField(Name="Account name")]
        public string Account_name { get; set; }
        [CsvField(Name = "Account ID")]
        public string Account_ID { get; set; } // int
        [CsvField(Name = "Account number")]
        public string Account_number { get; set; }
        [CsvField(Name="Campaign name")]
        public string Campaign_name { get; set; }
        [CsvField(Name = "Campaign ID")]
        public string Campaign_ID { get; set; } // int
        //[CsvField(Name="Currency code")]
        //public string Currency_code { get; set; }
    }
}
