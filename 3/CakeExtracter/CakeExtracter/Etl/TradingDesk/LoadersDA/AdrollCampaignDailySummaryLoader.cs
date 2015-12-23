using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AdRoll.Entities;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.AdRoll;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA
{
    public class AdrollCampaignDailySummaryLoader : Loader<CampaignSummary>
    {
        private readonly int advertisableId;
        private Dictionary<string, int> campIdLookupByEid = new Dictionary<string, int>();

        public AdrollCampaignDailySummaryLoader(int advertisableId)
        {
            this.advertisableId = advertisableId;
        }

        protected override int Load(List<CampaignSummary> items)
        {
            Logger.Info("Loading {0} Adroll CampaignDailySummaries..", items.Count);
            //AddUpdateDependentAds(items);
            //var count = UpsertAdDailySummaries(items);
            //return count;
            return items.Count;
        }
    }
}
