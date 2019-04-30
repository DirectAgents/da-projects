using System.Collections.Generic;
using DirectAgents.Domain.Entities.CPProg.YAM.Summaries;

namespace CakeExtracter.Etl.YAM.Loaders
{
    internal class YamDailySummaryLoader : Loader<YamDailySummary>
    {
        public YamDailySummaryLoader(int accountId = -1) : base(accountId)
        {
        }

        protected override int Load(List<YamDailySummary> items)
        {
            Logger.Info(accountId, "Loading {0} YamDailySummaries..", items.Count);
            return 0;
        }
    }
}
