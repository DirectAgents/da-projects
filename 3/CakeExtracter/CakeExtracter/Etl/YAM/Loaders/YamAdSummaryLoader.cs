using System.Collections.Generic;
using System.Linq;
using DirectAgents.Domain.Entities.CPProg.YAM;
using DirectAgents.Domain.Entities.CPProg.YAM.Summaries;

namespace CakeExtracter.Etl.YAM.Loaders
{
    internal class YamAdSummaryLoader : Loader<YamAdSummary>
    {
        private static readonly MergeHelper AdMergeHelper = new MergeHelper
        {
            EntitiesName = "Ads",
            Locker = new object()
        };
        private static readonly MergeHelper AdSummariesMergeHelper = new MergeHelper
        {
            EntitiesName = "Ad Summaries",
            Locker = new object()
        };

        private readonly BaseYamSummaryLoader baseLoader;
        private readonly YamLineSummaryLoader lineLoader;
        private readonly YamCreativeSummaryLoader creativeLoader;

        public YamAdSummaryLoader(int accountId = -1) : base(accountId)
        {
            baseLoader = new BaseYamSummaryLoader(accountId);
            lineLoader = new YamLineSummaryLoader(accountId);
            creativeLoader = new YamCreativeSummaryLoader(accountId);
        }

        public bool MergeItemsWithExisted(List<YamAdSummary> items)
        {
            var entities = items.Select(x => x.Ad).ToList();
            var result = MergeDependentAds(entities);
            if (result)
            {
                result = baseLoader.MergeSummariesWithExisted(items, AdSummariesMergeHelper, x => x.EntityId = x.Ad.Id);
            }

            return result;
        }

        public bool MergeDependentAds(List<YamAd> items)
        {
            var lines = items.Select(x => x.Line).ToList();
            var result = lineLoader.MergeDependentLines(lines);
            if (!result)
            {
                return false;
            }

            var creatives = items.Select(x => x.Creative).ToList();
            result = creativeLoader.MergeDependentCreatives(creatives);
            if (!result)
            {
                return false;
            }

            return baseLoader.MergeDependentEntitiesWithExisted(items, AdMergeHelper, ad =>
            {
                ad.LineId = ad.Line.Id;
                ad.CreativeId = ad.Creative.Id;
            });
        }

        protected override int Load(List<YamAdSummary> items)
        {
            Logger.Info(accountId, "Loading {0} YamAdSummaries..", items.Count);
            var result = MergeItemsWithExisted(items);
            return result ? items.Count : 0;
        }
    }
}
