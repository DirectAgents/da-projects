using System.Collections.Generic;
using DirectAgents.Domain.Entities.CPProg.YAM.Summaries;

namespace CakeExtracter.Etl.YAM.Loaders
{
    internal class YamAdSummaryLoader : Loader<YamAdSummary>
    {
        public YamAdSummaryLoader(int accountId = -1) : base(accountId)
        {
        }

        protected override int Load(List<YamAdSummary> items)
        {
            Logger.Info(accountId, "Loading {0} YamAdSummaries..", items.Count);
            return 0;
        }
    }
}
