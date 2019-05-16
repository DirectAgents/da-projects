using System.Collections.Generic;
using System.Linq;
using CakeExtracter.SimpleRepositories.BaseRepositories.Interfaces;
using DirectAgents.Domain.Entities.CPProg.YAM;
using DirectAgents.Domain.Entities.CPProg.YAM.Summaries;

namespace CakeExtracter.Etl.YAM.Loaders
{
    internal class YamCreativeSummaryLoader : BaseYamSummaryLoader<YamCreative, YamCreativeSummary>
    {
        public YamCreativeSummaryLoader(int accountId, IBaseRepository<YamCreative> entityRepository,
            IBaseRepository<YamCreativeSummary> summaryRepository)
            : base(accountId, entityRepository, summaryRepository)
        {
        }

        public bool MergeItemsWithExisted(List<YamCreativeSummary> items)
        {
            var entities = items.Select(x => x.Creative).ToList();
            var result = MergeDependentCreatives(entities);
            if (result)
            {
                result = MergeSummariesWithExisted(items);
            }

            return result;
        }

        public bool MergeDependentCreatives(List<YamCreative> items)
        {
            return MergeDependentEntitiesWithExisted(items);
        }

        protected override int Load(List<YamCreativeSummary> items)
        {
            Logger.Info(accountId, "Loading {0} YamCreativeSummaries..", items.Count);
            var result = MergeItemsWithExisted(items);
            return result ? items.Count : 0;
        }

        protected override void SetEntityParents(YamCreative entity)
        {
            entity.AccountId = accountId;
        }

        protected override void SetSummaryParents(YamCreativeSummary summary)
        {
            summary.EntityId = summary.Creative.Id;
        }
    }
}
