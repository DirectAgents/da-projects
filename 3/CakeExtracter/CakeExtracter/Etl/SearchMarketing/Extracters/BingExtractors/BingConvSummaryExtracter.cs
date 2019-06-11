﻿using System;
using System.Collections.Generic;
using System.Linq;
using BingAds.Utilities;

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
            var filepath = BingUtility.GetDailySummariesByGoalReport(AccountId, StartDate, EndDate);
            if (filepath == null)
            {
                return new List<Dictionary<string, string>>();
            }

            var bingRowsWithGoal = EnumerateRowsGeneric<BingRowWithGoal>(filepath, false);
            var rows = EnumerateRowsAsDictionaries(bingRowsWithGoal);
            return rows.ToList();
        }
    }
}