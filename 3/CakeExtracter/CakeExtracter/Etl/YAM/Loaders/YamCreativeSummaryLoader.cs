using System.Collections.Generic;
using DirectAgents.Domain.Entities.CPProg.YAM.Summaries;

namespace CakeExtracter.Etl.YAM.Loaders
{
    internal class YamCreativeSummaryLoader : Loader<YamCreativeSummary>
    {
        public YamCreativeSummaryLoader(int accountId = -1) : base(accountId)
        {
        }

        protected override int Load(List<YamCreativeSummary> items)
        {
            Logger.Info(accountId, "Loading {0} YamCreativeSummaries..", items.Count);
            return 0;
        }
    }
}
