using System.Collections.Generic;
using DirectAgents.Domain.Entities.CPProg.YAM.Summaries;

namespace CakeExtracter.Etl.YAM.Loaders
{
    internal class YamDailySummaryLoader : Loader<YamDailySummary>
    {
        private static readonly MergeHelper DailySummaryMergeHelper = new MergeHelper
        {
            EntitiesName = "Daily Summaries",
            Locker = new object()
        };

        private readonly BaseYamSummaryLoader baseLoader;

        public YamDailySummaryLoader(int accountId = -1) : base(accountId)
        {
            baseLoader = new BaseYamSummaryLoader(accountId);
        }

        protected override int Load(List<YamDailySummary> items)
        {
            Logger.Info(accountId, "Loading {0} YamDailySummaries..", items.Count);
            baseLoader.MergeSummariesWithExisted(items, DailySummaryMergeHelper, x => x.EntityId = accountId);
            return items.Count;
        }
    }
}
