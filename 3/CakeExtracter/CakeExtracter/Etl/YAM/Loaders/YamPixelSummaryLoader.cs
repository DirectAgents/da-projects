using System.Collections.Generic;
using System.Linq;
using CakeExtracter.SimpleRepositories.BaseRepositories.Interfaces;
using DirectAgents.Domain.Entities.CPProg.YAM;
using DirectAgents.Domain.Entities.CPProg.YAM.Summaries;

namespace CakeExtracter.Etl.YAM.Loaders
{
    internal class YamPixelSummaryLoader : BaseYamSummaryLoader<YamPixel, YamPixelSummary>
    {
        public YamPixelSummaryLoader(int accountId, IBaseRepository<YamPixel> entityRepository,
            IBaseRepository<YamPixelSummary> summaryRepository)
            : base(accountId, entityRepository, summaryRepository)
        {
        }

        public bool MergeItemsWithExisted(List<YamPixelSummary> items)
        {
            var entities = items.Select(x => x.Pixel).ToList();
            var result = MergeDependentPixels(entities);
            if (result)
            {
                result = MergeSummariesWithExisted(items);
            }

            return result;
        }

        public bool MergeDependentPixels(List<YamPixel> items)
        {
            return MergeDependentEntitiesWithExisted(items,
                options => { options.ColumnPrimaryKeyExpression = x => new {x.AccountId, x.ExternalId}; });
        }

        protected override int Load(List<YamPixelSummary> items)
        {
            Logger.Info(accountId, "Loading {0} YamPixelSummaries..", items.Count);
            var result = MergeItemsWithExisted(items);
            return result ? items.Count : 0;
        }

        protected override void SetEntityParents(YamPixel entity)
        {
            entity.AccountId = accountId;
        }

        protected override void SetSummaryParents(YamPixelSummary summary)
        {
            summary.EntityId = summary.Pixel.Id;
        }
    }
}
