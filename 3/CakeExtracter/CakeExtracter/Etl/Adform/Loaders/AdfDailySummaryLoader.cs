using System.Collections.Generic;
using System.Linq;
using CakeExtracter.SimpleRepositories.BaseRepositories.Interfaces;
using DirectAgents.Domain.Entities.CPProg.Adform;
using DirectAgents.Domain.Entities.CPProg.Adform.Summaries;

namespace CakeExtracter.Etl.Adform.Loaders
{
    internal class AdfDailySummaryLoader : AdfBaseSummaryLoader<AdfBaseEntity, AdfDailySummary>
    {
        public AdfDailySummaryLoader(
            int accountId,
            IBaseRepository<AdfDailySummary> summaryRepository,
            AdfMediaTypeLoader mediaTypeLoader)
            : base(accountId, null, summaryRepository, mediaTypeLoader)
        {
        }

        public virtual bool MergeItemsWithExisted(List<AdfDailySummary> items)
        {
            var mediaTypes = items.Select(i => i.MediaType).ToList();
            var result = MergeDependentMediaTypesWithExisted(mediaTypes);
            if (result)
            {
                result = MergeSummariesWithExisted(items);
            }
            return result;
        }

        protected override int Load(List<AdfDailySummary> items)
        {
            Logger.Info(accountId, "Loading {0} Adform Daily Summaries..", items.Count);
            var result = MergeItemsWithExisted(items);
            return result ? items.Count : 0;
        }

        protected override void SetEntityParents(AdfBaseEntity entity)
        {
            throw new System.NotImplementedException();
        }

        protected override void SetSummaryParents(AdfDailySummary summary)
        {
            base.SetSummaryParents(summary);
            summary.EntityId = accountId;
        }
    }
}
