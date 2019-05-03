using System.Collections.Generic;
using System.Linq;
using DirectAgents.Domain.Entities.CPProg.YAM;
using DirectAgents.Domain.Entities.CPProg.YAM.Summaries;

namespace CakeExtracter.Etl.YAM.Loaders
{
    internal class YamCreativeSummaryLoader : Loader<YamCreativeSummary>
    {
        private static readonly MergeHelper CreativeMergeHelper = new MergeHelper
        {
            EntitiesName = "Creatives",
            Locker = new object()
        };
        private static readonly MergeHelper CreativeSummariesMergeHelper = new MergeHelper
        {
            EntitiesName = "Creative Summaries",
            Locker = new object()
        };

        private readonly BaseYamSummaryLoader baseLoader;

        public YamCreativeSummaryLoader(int accountId = -1) : base(accountId)
        {
            baseLoader = new BaseYamSummaryLoader(accountId);
        }

        public bool MergeItemsWithExisted(List<YamCreativeSummary> items)
        {
            var entities = items.Select(x => x.Creative).ToList();
            var result = MergeDependentCreatives(entities);
            if (result)
            {
                result = baseLoader.MergeSummariesWithExisted(items, CreativeSummariesMergeHelper,
                    x => x.EntityId = x.Creative.Id);
            }

            return result;
        }

        public bool MergeDependentCreatives(List<YamCreative> items)
        {
            return baseLoader.MergeDependentEntitiesWithExisted(items, CreativeMergeHelper, creative => creative.AccountId = accountId);
        }

        protected override int Load(List<YamCreativeSummary> items)
        {
            Logger.Info(accountId, "Loading {0} YamCreativeSummaries..", items.Count);
            var result = MergeItemsWithExisted(items);
            return result ? items.Count : 0;
        }
    }
}
