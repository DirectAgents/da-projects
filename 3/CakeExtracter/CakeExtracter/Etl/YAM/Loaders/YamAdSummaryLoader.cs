using System.Collections.Generic;
using System.Linq;
using CakeExtracter.SimpleRepositories.BaseRepositories.Interfaces;
using DirectAgents.Domain.Entities.CPProg.YAM;
using DirectAgents.Domain.Entities.CPProg.YAM.Summaries;

namespace CakeExtracter.Etl.YAM.Loaders
{
    internal class YamAdSummaryLoader : BaseYamSummaryLoader<YamAd, YamAdSummary>
    {
        private readonly YamLineSummaryLoader lineLoader;
        private readonly YamCreativeSummaryLoader creativeLoader;

        public YamAdSummaryLoader(int accountId, IBaseRepository<YamAd> entityRepository,
            IBaseRepository<YamAdSummary> summaryRepository, YamLineSummaryLoader lineLoader, YamCreativeSummaryLoader creativeLoader)
            : base(accountId, entityRepository, summaryRepository)
        {
            this.lineLoader = lineLoader;
            this.creativeLoader = creativeLoader;
        }

        public bool MergeItemsWithExisted(List<YamAdSummary> items)
        {
            var entities = items.Select(x => x.Ad).ToList();
            var result = MergeDependentAds(entities);
            if (result)
            {
                result = MergeSummariesWithExisted(items);
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

            return MergeDependentEntitiesWithExisted(items);
        }

        protected override int Load(List<YamAdSummary> items)
        {
            Logger.Info(accountId, "Loading {0} YamAdSummaries..", items.Count);
            var result = MergeItemsWithExisted(items);
            return result ? items.Count : 0;
        }

        protected override void SetEntityParents(YamAd entity)
        {
            entity.LineId = entity.Line.Id;
            entity.CreativeId = entity.Creative.Id;
        }

        protected override void SetSummaryParents(YamAdSummary summary)
        {
            summary.EntityId = summary.Ad.Id;
        }
    }
}
