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

        protected override int Load(List<YamAdSummary> items)
        {
            Logger.Info(accountId, "Loading {0} YamAdSummaries..", items.Count);
            MergeItemsWithExisted(items);
            return items.Count;
        }

        public void MergeDependentAds(List<YamAd> items)
        {
            var lines = items.Select(x => x.Line).ToList();
            lineLoader.MergeDependentLines(lines);
            var creatives = items.Select(x => x.Creative).ToList();
            creativeLoader.MergeDependentCreatives(creatives);
            baseLoader.MergeDependentEntitiesWithExisted(items, AdMergeHelper, ad =>
            {
                ad.LineId = ad.Line.Id;
                ad.CreativeId = ad.Creative.Id;
            });
        }

        private void MergeItemsWithExisted(List<YamAdSummary> items)
        {
            var entities = items.Select(x => x.Ad).ToList();
            MergeDependentAds(entities);
            baseLoader.MergeSummariesWithExisted(items, AdSummariesMergeHelper, x => x.EntityId = x.Ad.Id);
        }
    }
}
