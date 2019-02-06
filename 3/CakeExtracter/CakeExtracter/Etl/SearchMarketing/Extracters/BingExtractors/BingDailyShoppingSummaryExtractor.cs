﻿using System;
using System.Collections.Generic;
using System.Linq;
using BingAds;

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
            var items = ExtractAndEnumerateRows();
            Add(items);
            End();
        }

        private IEnumerable<Dictionary<string, string>> ExtractAndEnumerateRows()
        {
            var filepath = BingUtility.GetReport_DailySummaries(AccountId, StartDate, EndDate, true);
            if (filepath == null)
            {
                return new List<Dictionary<string, string>>();
            }

            var bingRows = GroupAndEnumerateBingRows(filepath, false);
            var rows = EnumerateRowsAsDictionaries(bingRows);
            return rows.ToList();
        }
    }
}