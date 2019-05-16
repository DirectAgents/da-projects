using System.Collections.Generic;
using CakeExtracter.SimpleRepositories.BaseRepositories.Interfaces;
using DirectAgents.Domain.Entities.CPProg.YAM;
using DirectAgents.Domain.Entities.CPProg.YAM.Summaries;

namespace CakeExtracter.Etl.YAM.Loaders
{
    public class YamDailySummaryLoader : BaseYamSummaryLoader<BaseYamEntity, YamDailySummary>
    {
        public YamDailySummaryLoader(int accountId, IBaseRepository<YamDailySummary> summaryRepository) : base(
            accountId, null, summaryRepository)
        {
        }

        public virtual bool MergeItemsWithExisted(List<YamDailySummary> items)
        {
            return MergeSummariesWithExisted(items);
        }

        protected override int Load(List<YamDailySummary> items)
        {
            Logger.Info(accountId, "Loading {0} YamDailySummaries..", items.Count);
            var result = MergeItemsWithExisted(items);
            return result ? items.Count: 0;
        }

        protected override void SetEntityParents(BaseYamEntity entity)
        {
            throw new System.NotImplementedException();
        }

        protected override void SetSummaryParents(YamDailySummary summary)
        {
            summary.EntityId = accountId;
        }
    }
}
