using CakeExtracter.Common;
using CakeExtracter.Logging.TimeWatchers;
using CakeExtracter.Logging.TimeWatchers.Amazon;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using System.Collections.Generic;
using System.Linq;

namespace CakeExtracter.Etl.TradingDesk.Extracters.AmazonExtractors.AmazonApiExtractors
{
    //The daily extracter will load data based on date range and sum up the total of all campaigns
    public class AmazonDailyStrategyFromDataBaseSummaryExtractor : DatabaseStrategyToDailySummaryExtracter
    {
        public AmazonDailyStrategyFromDataBaseSummaryExtractor(DateRange dateRange, int accountId)
            :base(dateRange, accountId)
        {
        }

        protected override void Extract()
        {
            RemoveOldData(dateRange);
            IEnumerable<DailySummary> items = null;
            AmazonTimeTracker.Instance.ExecuteWithTimeTracking(() => {
                items = EnumerateRows();
            }, accountId, AmazonJobLevels.account, AmazonJobOperations.cleanExistingData);
            Add(items);
            End();
        }

        private void RemoveOldData(DateRange dateRange)
        {
            Logger.Info(accountId, "The cleaning of DailySummaries for account ({0}) has begun - {1}.", accountId, dateRange);
            AmazonTimeTracker.Instance.ExecuteWithTimeTracking(() => {
                using (var db = new ClientPortalProgContext())
                {
                    var items = db.DailySummaries.Where(x => x.AccountId == accountId &&  
                        (x.Date >= dateRange.FromDate && x.Date <= dateRange.ToDate)).ToList();
                    var metrics = db.DailySummaryMetrics.Where(x => x.EntityId == accountId && (x.Date >= dateRange.FromDate && x.Date <= dateRange.ToDate)).ToList();
                    db.BulkDelete(metrics);
                    db.BulkDelete(items);
                }
            }, accountId, AmazonJobLevels.account, AmazonJobOperations.cleanExistingData);
            Logger.Info(accountId, "The cleaning of DailySummaries for account ({0}) is over - {1}. ", accountId, dateRange);
        }
    }
}
