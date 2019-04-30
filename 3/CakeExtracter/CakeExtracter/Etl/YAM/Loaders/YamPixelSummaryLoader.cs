using System.Collections.Generic;
using DirectAgents.Domain.Entities.CPProg.YAM.Summaries;

namespace CakeExtracter.Etl.YAM.Loaders
{

    internal class YamPixelSummaryLoader : Loader<YamPixelSummary>
    {
        public YamPixelSummaryLoader(int accountId = -1) : base(accountId)
        {
        }

        protected override int Load(List<YamPixelSummary> items)
        {
            Logger.Info(accountId, "Loading {0} YamPixelSummaries..", items.Count);
            return 0;
        }
    }
}
