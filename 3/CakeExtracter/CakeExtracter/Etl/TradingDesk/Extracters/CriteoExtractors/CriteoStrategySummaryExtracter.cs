using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Commands;
using CakeExtracter.Common;
using Criteo;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.TradingDesk.Extracters
{
    public class CriteoStrategySummaryExtracter : CriteoApiExtracter<StrategySummary>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CriteoStrategySummaryExtracter"/> class.
        /// </summary>
        /// <param name="criteoUtility">The Utility class criteo.</param>
        /// <param name="accountCode">The account Code.</param>
        /// <param name="accountId">The account identifier.</param>
        /// <param name="dateRange">The date range.</param>
        /// <param name="timezoneOffset">timezoneOffset</param>
        public CriteoStrategySummaryExtracter(CriteoUtility criteoUtility, string accountCode, int accountId,
            DateRange dateRange, int timezoneOffset)
            : base(criteoUtility, accountCode, accountId, dateRange, timezoneOffset)
        {
        }

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
            End();
        }

        private void Extract_Daily()
        {
            try
            {
                var reportUrl = CriteoUtility.GetCampaignReport(DateRange.FromDate, DateRange.ToDate);
                var reportRows = EnumerateXmlReportRows(reportUrl);
                Add(reportRows);
                End();
            }
            catch (Exception e)
            {
                ProcessFailedStatsExtraction(e, DateRange.FromDate, DateRange.ToDate, AccountId, StatsTypeAgg.StrategyArg);
            }
        }

        private void Extract_Hourly()
        {
            try
            {
                var adjustedBeginDate = DateRange.FromDate;
                var adjustedEndDate = DateRange.ToDate;
                if (TimezoneOffset < 0)
                {
                    adjustedEndDate = adjustedEndDate.AddDays(1);
                }
                else if (TimezoneOffset > 0)
                {
                    adjustedBeginDate = adjustedBeginDate.AddDays(-1);
                }
                var reportUrl = CriteoUtility.GetCampaignReport(adjustedBeginDate, adjustedEndDate, hourly: true);
                var dailySummaries = EnumerateRows_Hourly(reportUrl);
                Add(dailySummaries);
                End();
            }
            catch (Exception e)
            {
                ProcessFailedStatsExtraction(e, DateRange.FromDate, DateRange.ToDate, AccountId, StatsTypeAgg.StrategyArg);
            }
        }

        private IEnumerable<StrategySummary> EnumerateRows_Hourly(string reportUrl)
        {
            var hourlySummaries = EnumerateXmlReportRows(reportUrl);
            hourlySummaries = hourlySummaries.Where(s => s.Date >= DateRange.FromDate && s.Date <= DateRange.ToDate);
            var dailyGroups = hourlySummaries.GroupBy(s => new { s.StrategyEid, s.Date });
            foreach (var group in dailyGroups)
            {
                var sum = new StrategySummary
                {
                    StrategyEid = group.Key.StrategyEid,
                    Date = group.Key.Date,
                };
                sum.SetStats(group);
                yield return sum;
            }
        }

        private IEnumerable<Strategy> GetCampaigns()
        {
            var campaigns = CriteoUtility.GetCampaigns();
            //Only using Eids in the lookup because that's what the loader will use later (the extracter doesn't get campaign names)
            var strategies = campaigns.Select(x => new Strategy { ExternalId = x.campaignID.ToString() });
            return strategies;
        }
    }
}
