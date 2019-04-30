using System.Collections.Generic;
using DirectAgents.Domain.Entities.CPProg.YAM.Summaries;

namespace CakeExtracter.Etl.YAM.Loaders
{
    internal class YamCampaignSummaryLoader : Loader<YamCampaignSummary>
    {
        public YamCampaignSummaryLoader(int accountId = -1) : base(accountId)
        {
        }

        protected override int Load(List<YamCampaignSummary> items)
        {
            Logger.Info(accountId, "Loading {0} YamCampaignSummaries..", items.Count);
            return 0;
        }
    }
}
