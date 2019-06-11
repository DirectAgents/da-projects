using System.Collections.Generic;
using System.Linq;
using CakeExtracter.CakeMarketingApi;
using CakeExtracter.CakeMarketingApi.Entities;

namespace CakeExtracter.Etl.CakeMarketing.Extracters
{
    public class CampaignsExtracter : Extracter<Campaign>
    {
        private readonly int offerId;
        private readonly IEnumerable<int> campaignIds;

        private readonly List<int> campIdsUnableToRetrieve = new List<int>();

        public IEnumerable<int> CampIdsUnableToRetrieve => campIdsUnableToRetrieve.ToArray();

        public CampaignsExtracter(int offerId = 0, IEnumerable<int> campaignIds = null)
        {
            this.offerId = offerId;
            this.campaignIds = campaignIds;
        }

        protected override void Extract()
        {
            Logger.Info(
                "Extracting Campaigns, OffId {0}, {1} CampIds specified",
                offerId,
                campaignIds == null ? "no" : campaignIds.Count().ToString());
            if (campaignIds == null)
            {
                var campaigns = CakeMarketingUtility.Campaigns(offerId: offerId);
                Add(campaigns);
            }
            else
            {
                foreach (var campaignId in campaignIds)
                {
                    var campaigns = CakeMarketingUtility.Campaigns(offerId: offerId, campaignId: campaignId);
                    if (campaigns.Any())
                    {
                        Add(campaigns);
                    }
                    else
                    {
                        Logger.Warn("Couldn't retrieve campaign {0} from Cake (offer {1})", campaignId, offerId);
                        campIdsUnableToRetrieve.Add(campaignId);
                    }
                }
            }
            End();
        }
    }
}
