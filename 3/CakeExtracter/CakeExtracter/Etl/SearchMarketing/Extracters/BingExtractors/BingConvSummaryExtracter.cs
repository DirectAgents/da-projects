using System;
using System.Collections.Generic;
using System.Linq;
using BingAds.Utilities;
using CakeExtracter.Etl.SearchMarketing.Extracters.BingExtractors.Models;
using CakeExtracter.Etl.SearchMarketing.Extracters.BingExtractors.Parsers.ParsingConverters;

namespace CakeExtracter.Etl.SearchMarketing.Extracters.BingExtractors
{
    public class BingConvSummaryExtractor : BingExtractorBase
    {
        public BingConvSummaryExtractor(BingUtility bingUtility, long accountId, DateTime startDate, DateTime endDate)
            : base(bingUtility, accountId, startDate, endDate) { }

        protected override void Extract()
        {
            Logger.Info("Extracting SearchConvSummaries for {0} from {1} to {2}", AccountId, StartDate, EndDate);
            ExtractData();
        }

        protected override IEnumerable<Dictionary<string, string>> ExtractAndEnumerateRows()
        {
            var reportFilePath = BingUtility.GetDailySummariesByGoalReport(AccountId, StartDate, EndDate);
            if (reportFilePath == null)
            {
                return new List<Dictionary<string, string>>();
            }

            var rowMap = new BingGoalReportEntityRowMap();
            var bingRowsWithGoal = GetReportRows<BingGoalRow>(reportFilePath, rowMap);
            var rows = EnumerateRowsAsDictionaries(bingRowsWithGoal);
            return rows.ToList();
        }
    }
}