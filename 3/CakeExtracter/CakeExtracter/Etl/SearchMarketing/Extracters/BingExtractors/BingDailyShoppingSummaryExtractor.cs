using System;
using System.Collections.Generic;
using System.Linq;
using BingAds.Utilities;
using CakeExtracter.Etl.SearchMarketing.Extracters.BingExtractors.Models;
using CakeExtracter.Etl.SearchMarketing.Extracters.BingExtractors.Parsers.ParsingConverters;

namespace CakeExtracter.Etl.SearchMarketing.Extracters.BingExtractors
{
    public class BingDailyShoppingSummaryExtractor : BingExtractorBase
    {
        public BingDailyShoppingSummaryExtractor(BingUtility bingUtility, long accountId, DateTime startDate, DateTime endDate)
            : base(bingUtility, accountId, startDate, endDate)
        {
        }

        protected override void Extract()
        {
            Logger.Info("Extracting SearchDailySummaries (shopping campaigns) for {0} from {1} to {2}", AccountId, StartDate, EndDate);
            ExtractData();
        }

        protected override IEnumerable<Dictionary<string, string>> ExtractAndEnumerateRows()
        {
            var reportFilePath = BingUtility.GetDailySummariesReport(AccountId, StartDate, EndDate, true);
            if (reportFilePath == null)
            {
                return new List<Dictionary<string, string>>();
            }

            var bingRows = GroupAndEnumerateBingRows(reportFilePath);
            var rows = EnumerateRowsAsDictionaries(bingRows);
            return rows.ToList();
        }

        private IEnumerable<BingDailyRow> GroupAndEnumerateBingRows(string reportFilePath)
        {
            var rowMap = new BingDailyReportEntityRowMap();
            var allRows = GetReportRows<BingDailyRow>(reportFilePath, rowMap);
            var uniqueRows = GetUniqueBingRow(allRows);
            return uniqueRows;
        }

        private IEnumerable<BingDailyRow> GetUniqueBingRow(IEnumerable<BingDailyRow> allRows)
        {
            var groupedRows = allRows.GroupBy(b => new { b.TimePeriod, b.AccountId, b.AccountName, b.AccountNumber, b.CampaignId, b.CampaignName });
            return groupedRows.Select(group =>
            {
                return new BingDailyRow
                {
                    TimePeriod = group.Key.TimePeriod,
                    AccountId = group.Key.AccountId,
                    AccountName = group.Key.AccountName,
                    AccountNumber = group.Key.AccountNumber,
                    CampaignId = group.Key.CampaignId,
                    CampaignName = group.Key.CampaignName,
                    Impressions = group.Sum(r => r.Impressions),
                    Clicks = group.Sum(r => r.Clicks),
                    Conversions = group.Sum(r => r.Conversions),
                    Spend = group.Sum(r => r.Spend),
                    Revenue = group.Sum(r => r.Revenue),
                };
            });
        }
    }
}