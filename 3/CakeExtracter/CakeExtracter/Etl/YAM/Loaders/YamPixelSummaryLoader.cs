using System.Collections.Generic;
using System.Linq;
using DirectAgents.Domain.Entities.CPProg.YAM;
using DirectAgents.Domain.Entities.CPProg.YAM.Summaries;

namespace CakeExtracter.Etl.YAM.Loaders
{

    internal class YamPixelSummaryLoader : Loader<YamPixelSummary>
    {
        private static readonly MergeHelper PixelMergeHelper = new MergeHelper
        {
            EntitiesName = "Pixel",
            Locker = new object()
        };
        private static readonly MergeHelper PixelSummariesMergeHelper = new MergeHelper
        {
            EntitiesName = "Pixel Summaries",
            Locker = new object()
        };

        private readonly BaseYamSummaryLoader baseLoader;

        public YamPixelSummaryLoader(int accountId = -1) : base(accountId)
        {
            baseLoader = new BaseYamSummaryLoader(accountId);
        }

        protected override int Load(List<YamPixelSummary> items)
        {
            Logger.Info(accountId, "Loading {0} YamPixelSummaries..", items.Count);
            MergeItemsWithExisted(items);
            return items.Count;
        }

        public void MergeDependentPixels(List<YamPixel> items)
        {
            baseLoader.MergeDependentEntitiesWithExisted(items, PixelMergeHelper, pixel => pixel.AccountId = accountId);
        }

        private void MergeItemsWithExisted(List<YamPixelSummary> items)
        {
            var entities = items.Select(x => x.Pixel).ToList();
            MergeDependentPixels(entities);
            baseLoader.MergeSummariesWithExisted(items, PixelSummariesMergeHelper, x => x.EntityId = x.Pixel.Id);
        }
    }
}
