﻿using System;
using System.Collections.Generic;
using System.Linq;
using BingAds.Utilities;
using CakeExtracter.Etl.SearchMarketing.Extracters.BingExtractors.Models;
using CakeExtracter.Etl.SearchMarketing.Extracters.BingExtractors.Parsers.ParsingConverters;

namespace CakeExtracter.Etl.SearchMarketing.Extracters.BingExtractors
{
    public class BingDailyNonShoppingSummaryExtractor : BingExtractorBase
    {
        public BingDailyNonShoppingSummaryExtractor(BingUtility bingUtility, long accountId, DateTime startDate, DateTime endDate)
            : base(bingUtility, accountId, startDate, endDate)
        {
        }

        protected override void Extract()
        {
            Logger.Info("Extracting SearchDailySummaries (non-shopping campaigns) for {0} from {1} to {2}", AccountId, StartDate, EndDate);
            ExtractData();
        }

        protected override IEnumerable<Dictionary<string, string>> ExtractAndEnumerateRows()
        {
            var reportFilePath = BingUtility.GetDailySummariesReport(AccountId, StartDate, EndDate);
            if (reportFilePath == null)
            {
                return new List<Dictionary<string, string>>();
            }

            var rowMap = new BingDailyReportEntityRowMap();
            var bingRows = GetReportRows<BingDailyRow>(reportFilePath, rowMap);
            var rows = EnumerateRowsAsDictionaries(bingRows);
            return rows.ToList();
        }
    }
}