using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace CakeExtracter.Etl.SearchMarketing.Extracters
{
    public class BingDailySummaryExtracter : Extracter<Dictionary<string, string>>
    {
        private readonly long accountId; // 886985 ramjet
        private readonly DateTime startDate;
        private readonly DateTime endDate;
        private readonly bool includeShopping; // (shopping campaigns)
        private readonly bool includeNonShopping;

        public BingDailySummaryExtracter(long accountId, DateTime startDate, DateTime endDate, bool includeShopping = true, bool includeNonShopping = true)
        {
            this.accountId = accountId;
            this.startDate = startDate;
            this.endDate = endDate;
            this.includeShopping = includeShopping;
            this.includeNonShopping = includeNonShopping;
        }

        protected override void Extract()
        {
            Logger.Info("Extracting SearchDailySummaries for {0} from {1} to {2}", accountId, startDate, endDate);
            if (includeNonShopping)
            {
                var items = EnumerateRows(forShoppingCampaigns: false);
                Add(items);
            }
            if (includeShopping)
            {
                var items = EnumerateRows(forShoppingCampaigns: true);
                Add(items);
            }
            End();
        }

        private IEnumerable<Dictionary<string, string>> EnumerateRows(bool forShoppingCampaigns = false)
        {
            var bingUtility = new BingAds.BingUtility(m => Logger.Info(m), m => Logger.Warn(m));
            var filepath = bingUtility.GetDailySummaries(accountId, startDate, endDate, forShoppingCampaigns: forShoppingCampaigns);
            if (filepath == null)
                yield break;

            IEnumerable<BingRow> bingRows;
            if (forShoppingCampaigns)
                bingRows = GroupAndEnumerateBingRows(filepath, throwOnMissingField: false);
            else
                bingRows = EnumerateBingRows(filepath, throwOnMissingField: true);

            foreach (var bingRow in bingRows)
            {
                var row = new Dictionary<string, string>();

                // Use reflection to add values
                var type = typeof(BingRow);
                var properties = type.GetProperties();
                foreach (var propertyInfo in properties)
                {
                    if (propertyInfo.Name == "AccountId" && String.IsNullOrWhiteSpace(bingRow.AccountId))
                        row["AccountId"] = this.accountId.ToString();
                    else if (propertyInfo.PropertyType == typeof(int))
                        row[propertyInfo.Name] = ((int)propertyInfo.GetValue(bingRow)).ToString();
                    else if (propertyInfo.PropertyType == typeof(decimal))
                        row[propertyInfo.Name] = ((decimal)propertyInfo.GetValue(bingRow)).ToString();
                    else
                        row[propertyInfo.Name] = (string)propertyInfo.GetValue(bingRow);

                    //TODO: Have the extracter return objects with typed properties (and have the loader handle that)
                }
                yield return row;
            }
        }

        private IEnumerable<BingRow> GroupAndEnumerateBingRows(string filepath, bool throwOnMissingField)
        {
            var groups = EnumerateBingRows(filepath, throwOnMissingField)
                .GroupBy(b => new { b.GregorianDate, b.AccountId, b.AccountName, b.AccountNumber, b.CampaignId, b.CampaignName });
            foreach (var g in groups)
            {
                var bingRow = new BingRow
                {
                    GregorianDate = g.Key.GregorianDate,
                    AccountId = g.Key.AccountId,
                    AccountName = g.Key.AccountName,
                    AccountNumber = g.Key.AccountNumber,
                    CampaignId = g.Key.CampaignId,
                    CampaignName = g.Key.CampaignName,
                    Impressions = g.Sum(r => r.Impressions),
                    Clicks = g.Sum(r => r.Clicks),
                    Conversions = g.Sum(r => r.Conversions),
                    Spend = g.Sum(r => r.Spend),
                    Revenue = g.Sum(r => r.Revenue)
                };
                yield return bingRow;
            }
        }
        private IEnumerable<BingRow> EnumerateBingRows(string filepath, bool throwOnMissingField)
        {
            TypeConverterOptionsFactory.GetOptions(typeof(decimal)).NumberStyle = NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint;

            using (StreamReader reader = File.OpenText(filepath))
            {
                for (int i = 0; i < 9; i++)
                    reader.ReadLine();

                using (CsvReader csv = new CsvReader(reader))
                {
                    csv.Configuration.SkipEmptyRecords = true;
                    csv.Configuration.WillThrowOnMissingField = throwOnMissingField;

                    while (csv.Read())
                    {
                        BingRow csvRow;
                        try
                        {
                            csvRow = csv.GetRecord<BingRow>();
                        }
                        catch (CsvHelperException ex)
                        {
                            continue; // if error converting the row
                        }
                        yield return csvRow;
                    }
                }
            }
        }

        //public sealed class BingRowMap : CsvClassMap<BingRow>
        //{
        //    public BingRowMap() ...
        //      Map(m => m.Spend).TypeConverterOption(NumberStyles.Currency);
        //}
        //NOTE: setting globally (TypeConverterOptionsFactory options - above) instead

        private class BingRow
        {
            public string GregorianDate { get; set; } // date
            public int Impressions { get; set; } // int
            public int Clicks { get; set; } // int
            public int Conversions { get; set; } // int
            public decimal Spend { get; set; } // decimal
            public decimal Revenue { get; set; } // decimal
            public string AccountId { get; set; } // int
            public string AccountName { get; set; } // string
            public string AccountNumber { get; set; } // string
            public string CampaignId { get; set; } // int
            public string CampaignName { get; set; } // string

            //public string MerchantProductId { get; set; }
            //public string CurrencyCode { get; set; }
        }
    }

}
