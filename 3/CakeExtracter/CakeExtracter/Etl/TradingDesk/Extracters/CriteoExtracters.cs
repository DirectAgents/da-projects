using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Common;
using CakeExtracter.Etl.SearchMarketing.Extracters;
using Criteo;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.TradingDesk.Extracters
{
    public abstract class CriteoApiExtracter<T> : Extracter<T>
        where T : DatedStatsSummary
    {
        protected readonly string AccountCode;
        protected readonly DateRange DateRange;
        protected readonly int TimezoneOffset;

        protected CriteoUtility CriteoUtility;
        //TODO: make it so that within the criteoUtility, it doesn't need to open and close the service for every call

        protected CriteoApiExtracter(CriteoUtility criteoUtility, string accountCode, DateRange dateRange, int timezoneOffset = 0)
        {
            AccountCode = accountCode;
            DateRange = dateRange;
            TimezoneOffset = timezoneOffset;
            if (criteoUtility != null)
            {
                CriteoUtility = criteoUtility;
            }
            else
            {
                CriteoUtility = new CriteoUtility(m => Logger.Info(m), m => Logger.Warn(m));
                CriteoUtility.SetCredentials(accountCode);
            }
        }

        protected IEnumerable<StrategySummary> EnumerateXmlReportRows(string reportUrl)
        {
            var rows = CriteoApiExtracter.EnumerateCriteoXmlReportRows(reportUrl);
            foreach (var row in rows)
            {
                var sum = new StrategySummary
                {
                    StrategyEid = row["campaignID"],
                    Date = DateTime.Parse(row["dateTime"]).AddHours(TimezoneOffset).Date,
                    Cost = decimal.Parse(row["cost"]),
                    Impressions = int.Parse(row["impressions"]),
                    Clicks = int.Parse(row["click"]),
                    PostClickConv = int.Parse(row["sales"]),
                    PostClickRev = decimal.Parse(row["orderValue"]),
                };
                yield return sum;
            }
        }
    }

    public class CriteoStrategySummaryExtracter : CriteoApiExtracter<StrategySummary>
    {
        public CriteoStrategySummaryExtracter(CriteoUtility criteoUtility, string accountCode, DateRange dateRange, int timezoneOffset)
            : base(criteoUtility, accountCode, dateRange, timezoneOffset)
        { }

        protected override void Extract()
        {
            Logger.Info("Extracting StrategySummaries from Criteo API for {0} from {1:d} to {2:d}",
                AccountCode, DateRange.FromDate, DateRange.ToDate);
            if (TimezoneOffset == 0)
            {
                Extract_Daily();
            }
            else
            {
                Extract_Hourly();
            }
        }

        private void Extract_Daily()
        {
            var reportUrl = CriteoUtility.GetCampaignReport(DateRange.FromDate, DateRange.ToDate);
            var reportRows = EnumerateXmlReportRows(reportUrl);
            Add(reportRows);
            End();
        }

        private void Extract_Hourly()
        {
            var adjustedBeginDate = DateRange.FromDate;
            var adjustedEndDate = DateRange.ToDate;
            if (TimezoneOffset < 0)
                adjustedEndDate = adjustedEndDate.AddDays(1);
            else if (TimezoneOffset > 0)
                adjustedBeginDate = adjustedBeginDate.AddDays(-1);

            var reportUrl = CriteoUtility.GetCampaignReport(adjustedBeginDate, adjustedEndDate, hourly: true);

            var dailySummaries = EnumerateRows_Hourly(reportUrl);
            Add(dailySummaries);
            End();
        }

        private IEnumerable<StrategySummary> EnumerateRows_Hourly(string reportUrl)
        {
            IEnumerable<StrategySummary> hourlySummaries = EnumerateXmlReportRows(reportUrl).ToList();
            hourlySummaries = hourlySummaries.Where(s => s.Date >= DateRange.FromDate && s.Date <= DateRange.ToDate);
            var dailyGroups = hourlySummaries.GroupBy(s => new { s.StrategyEid, s.Date });
            foreach (var group in dailyGroups)
            {
                var sum = new StrategySummary
                {
                    StrategyEid = group.Key.StrategyEid,
                    Date = group.Key.Date
                };
                sum.SetStats(group);
                yield return sum;
            }
        }

        private IEnumerable<Strategy> GetCampaigns()
        {
            var campaigns = CriteoUtility.GetCampaigns();
            //Only using Eids in the lookup because that's what the loader will use later (the extracter doesn't get campaign names)
            var strategies = campaigns.Select(x => new Strategy {ExternalId = x.campaignID.ToString()});
            return strategies;
        }
    }
}
