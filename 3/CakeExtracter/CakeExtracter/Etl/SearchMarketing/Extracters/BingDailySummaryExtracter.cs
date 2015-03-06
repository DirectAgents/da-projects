using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeExtracter.Etl.SearchMarketing.Extracters
{
    public class BingDailySummaryExtracter : Extracter<Dictionary<string, string>>
    {
        private readonly long accountId; // 886985 ramjet
        private readonly DateTime startDate;
        private readonly DateTime endDate;

        public BingDailySummaryExtracter(long accountId, DateTime startDate, DateTime endDate)
        {
            this.accountId = accountId;
            this.startDate = startDate;
            this.endDate = endDate;
        }

        protected override void Extract()
        {
            Logger.Info("Extracting SearchDailySummaries for {0} from {1} to {2}", accountId, startDate, endDate);
            var items = EnumerateRows();
            Add(items);
            End();
        }

        private IEnumerable<Dictionary<string, string>> EnumerateRows()
        {
            var bingUtility = new BingAds.BingUtility(m => Logger.Info(m), m => Logger.Warn(m));
            var filepath = bingUtility.GetDailySummaries(accountId, startDate, endDate);
            if (filepath == null)
                yield break;

            using (StreamReader reader = File.OpenText(filepath))
            {
                for (int i = 0; i < 9; i++)
                    reader.ReadLine();

                using (CsvReader csv = new CsvReader(reader))
                {
                    var csvRows = csv.GetRecords<BingRow>().ToList();
                    foreach (var csvRow in csvRows)
                    {
                        if (csvRow.GregorianDate.ToLower().Contains("microsoft"))
                            continue; // skip footer

                        var row = new Dictionary<string, string>();

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

        public class BingRow
        {
            public string GregorianDate { get; set; } // date
            public string Impressions { get; set; } // int
            public string Clicks { get; set; } // int
            public string Conversions { get; set; } // int
            public string Spend { get; set; } // decimal
            public string Revenue { get; set; } // decimal
            public string AccountId { get; set; } // int
            public string AccountName { get; set; } // string
            public string AccountNumber { get; set; } // string
            public string CampaignId { get; set; } // int
            public string CampaignName { get; set; } // string

            //public string CurrencyCode { get; set; }
        }
    }

}
