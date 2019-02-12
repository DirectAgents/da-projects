using System.Collections.Generic;
using CakeExtracter.Helpers;
using CakeExtracter.Logging.TimeWatchers;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA.AmazonLoaders
{
    public class AmazonDailySummaryLoader : BaseAmazonLevelLoader<DailySummary,DailySummaryMetric>
    {
        private readonly TDDailySummaryLoader summaryItemsLoader;

        public AmazonDailySummaryLoader(int accountId) 
            : base(accountId)
        {
            summaryItemsLoader = new TDDailySummaryLoader(accountId);
        }

        protected override string LevelName => AmazonJobLevels.account;

        protected override object LockerObject => SafeContextWrapper.DailyLocker;

        protected override void EnsureRelatedItems(List<DailySummary> summaryItems)
        {
            summaryItemsLoader.AssignIdsToItems(summaryItems);
        }

        protected override void SetSummaryMetricEntityId(DailySummary summary, SummaryMetric summaryMetric)
        {
            summaryMetric.EntityId = summary.AccountId;
        }
    }
}
