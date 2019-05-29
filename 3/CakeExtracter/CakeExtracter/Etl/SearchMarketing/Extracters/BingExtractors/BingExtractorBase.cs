using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using BingAds;
using CsvHelper;
using CsvHelper.TypeConversion;

namespace CakeExtracter.Etl.SearchMarketing.Extracters.BingExtractors
{
    public abstract class BingExtractorBase : Extracter<Dictionary<string, string>>
    {
        protected readonly long AccountId;
        protected readonly DateTime StartDate;
        protected readonly DateTime EndDate;
        protected readonly BingUtility BingUtility;

        protected BingExtractorBase(BingUtility bingUtility, long accountId, DateTime startDate, DateTime endDate)
        {
            AccountId = accountId;
            StartDate = startDate;
            EndDate = endDate;
            BingUtility = bingUtility;
        }

        protected abstract IEnumerable<Dictionary<string, string>> ExtractAndEnumerateRows();

        protected void ExtractData()
        {
            try
            {
                var items = ExtractAndEnumerateRows();
                Add(items);
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                throw exception;
            }

            End();
        }

        protected IEnumerable<Dictionary<string, string>> EnumerateRowsAsDictionaries<T>(IEnumerable<T> rows)
        {
            foreach (var row in rows)
            {
                var dict = new Dictionary<string, string>();

                // Use reflection to add values
                var type = typeof(T);
                var properties = type.GetProperties();
                foreach (var propertyInfo in properties)
                {
                    if (propertyInfo.Name == "AccountId" && String.IsNullOrWhiteSpace((string)propertyInfo.GetValue(row)))
                        dict["AccountId"] = this.AccountId.ToString();
                    else if (propertyInfo.PropertyType == typeof(int))
                        dict[propertyInfo.Name] = ((int)propertyInfo.GetValue(row)).ToString();
                    else if (propertyInfo.PropertyType == typeof(decimal))
                        dict[propertyInfo.Name] = ((decimal)propertyInfo.GetValue(row)).ToString();
                    else
                        dict[propertyInfo.Name] = (string)propertyInfo.GetValue(row);

                    //TODO: Have the extracter return objects with typed properties (and have the loader handle that)
                }
                yield return dict;
            }
        }
        
        protected IEnumerable<BingRow> GroupAndEnumerateBingRows(string filepath, bool throwOnMissingField)
        {
            var groups = EnumerateRowsGeneric<BingRow>(filepath, throwOnMissingField)
                .GroupBy(b => new { b.TimePeriod, b.AccountId, b.AccountName, b.AccountNumber, b.CampaignId, b.CampaignName });
            foreach (var g in groups)
            {
                var bingRow = new BingRow
                {
                    TimePeriod = g.Key.TimePeriod,
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

        protected static IEnumerable<T> EnumerateRowsGeneric<T>(string filepath, bool throwOnMissingField)
        {
            TypeConverterOptionsFactory.GetOptions(typeof(decimal)).NumberStyle = NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint;

            using (var reader = File.OpenText(filepath))
            {
                for (var i = 0; i < 9; i++)
                {
                    reader.ReadLine();
                }

                using (var csv = new CsvReader(reader))
                {
                    csv.Configuration.SkipEmptyRecords = true;
                    csv.Configuration.WillThrowOnMissingField = throwOnMissingField;

                    while (csv.Read())
                    {
                        T csvRow;
                        try
                        {
                            csvRow = csv.GetRecord<T>();
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

        protected class BingRow
        {
            public string TimePeriod { get; set; } // date
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

        protected class BingRowWithGoal : BingRow
        {
            public string GoalId { get; set; } // int?
            public string Goal { get; set; } // string
        }
    }

}
