using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Common;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.TradingDesk.Extracters
{
    public class DatabaseStrategyToDailySummaryExtracter : Extracter<DailySummary>
    {
        protected readonly DateRange dateRange;
        protected readonly int accountId;

        public DatabaseStrategyToDailySummaryExtracter(DateRange dateRange, int accountId)
        {
            this.dateRange = dateRange;
            this.accountId = accountId;
        }
        protected override void Extract()
        {
            Logger.Info("Extracting DailySummaries from Db for ({0}) from {1:d} to {2:d}",
                this.accountId, this.dateRange.FromDate, this.dateRange.ToDate);
            var items = EnumerateRows();
            Add(items);
            End();
        }
        public IEnumerable<DailySummary> EnumerateRows()
        {
            List<DailySummary> daySums = new List<DailySummary>();
            using (var db = new ClientPortalProgContext())
            {
                var stratSums = db.StrategySummaries
                    .Where(x => x.Strategy.AccountId == accountId && dateRange.Dates.Contains(x.Date))
                    .ToList();
                var dayGroups = stratSums.GroupBy(x => x.Date);
                foreach (var group in dayGroups)
                {
                    var daySum = new DailySummary();
                    SetStatsFromDb(daySum, group.Key, group);
                    daySums.Add(daySum);
                }
            }
            return daySums;
        }

        public void SetStatsFromDb(DailySummary daySummary, DateTime date, IEnumerable<StrategySummary> summariesFromDb)
        {
            foreach (var summary in summariesFromDb)
            {
                summary.InitialMetrics = summary.Metrics;
            }
            daySummary.SetStats(date, summariesFromDb);
        }
    }
}
