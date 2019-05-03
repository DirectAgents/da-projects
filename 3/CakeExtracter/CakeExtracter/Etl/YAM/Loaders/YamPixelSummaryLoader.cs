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

        public bool MergeItemsWithExisted(List<YamPixelSummary> items)
        {
            var entities = items.Select(x => x.Pixel).ToList();
            var result = MergeDependentPixels(entities);
            if (result)
            {
                result = baseLoader.MergeSummariesWithExisted(items, PixelSummariesMergeHelper, x => x.EntityId = x.Pixel.Id);
            }

            return result;
        }

        public bool MergeDependentPixels(List<YamPixel> items)
        {
            return baseLoader.MergeDependentEntitiesWithExisted(items, PixelMergeHelper,
                pixel => pixel.AccountId = accountId,
                options => { options.ColumnPrimaryKeyExpression = x => new {x.AccountId, x.ExternalId}; });
        }

        protected override int Load(List<YamPixelSummary> items)
        {
            Logger.Info(accountId, "Loading {0} YamPixelSummaries..", items.Count);
            var result = MergeItemsWithExisted(items);
            return result ? items.Count : 0;
        }
    }
}
