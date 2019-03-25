using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Common;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.TradingDesk.Extracters
{
    /// <summary>
    /// Data extractor from keywords level to daily level based on database data.
    /// </summary>
    /// <seealso cref="CakeExtracter.Etl.Extracter{DirectAgents.Domain.Entities.CPProg.DailySummary}" />
    public class DatabaseKeywordsToDailySummaryExtractor : Extracter<DailySummary>
    {
        protected readonly DateRange dateRange;
        protected readonly int accountId;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseKeywordsToDailySummaryExtracter"/> class.
        /// </summary>
        /// <param name="dateRange">The date range.</param>
        /// <param name="accountId">The account identifier.</param>
        public DatabaseKeywordsToDailySummaryExtractor(DateRange dateRange, int accountId)
        {
            this.dateRange = dateRange;
            this.accountId = accountId;
        }

        /// <summary>
        /// The derived class implements this method, which calls Add() for each item
        /// extracted and then calls End() when complete.
        /// </summary>
        protected override void Extract()
        {
            Logger.Info("Extracting DailySummaries from Db for ({0}) from {1:d} to {2:d}",
                this.accountId, this.dateRange.FromDate, this.dateRange.ToDate);
            var items = GetDailySummaryDataFromDataBase();
            Add(items);
            End();
        }

        /// <summary>
        /// Gets the daily summary data from data base based on keywords data.
        /// </summary>
        /// <returns></returns>
        protected IEnumerable<DailySummary> GetDailySummaryDataFromDataBase()
        {
            List<DailySummary> daySums = new List<DailySummary>();
            using (var db = new ClientPortalProgContext())
            {
                var stratSums = db.KeywordSummaries
                    .Where(x => x.Keyword.AccountId == accountId && dateRange.Dates.Contains(x.Date))
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

        private void SetStatsFromDb(DailySummary daySummary, DateTime date, IEnumerable<KeywordSummary> summariesFromDb)
        {
            foreach (var summary in summariesFromDb)
            {
                summary.InitialMetrics = summary.Metrics;
            }
            daySummary.SetStats(date, summariesFromDb);
        }
    }
}
