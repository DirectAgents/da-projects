using System.Collections.Generic;
using CakeExtracter.SimpleRepositories.BaseRepositories.Interfaces;
using DirectAgents.Domain.Entities.CPProg.Adform;
using DirectAgents.Domain.Entities.CPProg.Adform.Summaries;

namespace CakeExtracter.Etl.Adform.Loaders
{
    internal class AdfDailySummaryLoader : AdfBaseSummaryLoader<AdfBaseEntity, AdfDailySummary>
    {
        public AdfDailySummaryLoader(int accountId, IBaseRepository<AdfDailySummary> summaryRepository)
            : base(accountId, null, summaryRepository)
        {
        }

        public virtual bool MergeItemsWithExisted(List<AdfDailySummary> items)
        {
            return MergeSummariesWithExisted(items);
        }

        protected override int Load(List<AdfDailySummary> items)
        {
            Logger.Info(accountId, "Loading {0} YamDailySummaries..", items.Count);
            var result = MergeItemsWithExisted(items);
            return result ? items.Count : 0;
        }

        protected override void SetEntityParents(AdfBaseEntity entity)
        {
            throw new System.NotImplementedException();
        }

        protected override void SetSummaryParents(AdfDailySummary summary)
        {
            summary.EntityId = accountId;
        }
    }
}
