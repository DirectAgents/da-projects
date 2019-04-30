using System.Collections.Generic;
using DirectAgents.Domain.Entities.CPProg.YAM.Summaries;

namespace CakeExtracter.Etl.YAM.Loaders
{
    internal class YamLineSummaryLoader : Loader<YamLineSummary>
    {
        public YamLineSummaryLoader(int accountId = -1) : base(accountId)
        {
        }

        protected override int Load(List<YamLineSummary> items)
        {
            Logger.Info(accountId, "Loading {0} YamLineSummaries..", items.Count);
            return 0;
        }
    }
}
