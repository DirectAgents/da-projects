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

        private List<int> campIds_unableToRetrieve = new List<int>();
        public IEnumerable<int> CampIds_UnableToRetrieve
        {
            get { return campIds_unableToRetrieve.ToArray(); }
        }

        public CampaignsExtracter(int offerId = 0, IEnumerable<int> campaignIds = null)
        {
            this.offerId = offerId;
            this.campaignIds = campaignIds;
        }

        protected override void Extract()
        {
            Logger.Info("Extracting Campaigns, OffId {0}, {1} CampIds specified",
                offerId, (campaignIds == null) ? "no" : campaignIds.Count().ToString());
            if (campaignIds == null)
            {
                var campaigns = CakeMarketingUtility.Campaigns(offerId: offerId);
                Add(campaigns);
            }
            else
            {
                foreach (int campaignId in campaignIds)
                {
                    var campaigns = CakeMarketingUtility.Campaigns(offerId: offerId, campaignId: campaignId);
                    if (campaigns.Any())
                    {
                        Add(campaigns);
                    }
                    else
                    {
                        Logger.Info("Couldn't retrieve campaign {0} from Cake (offer {1})", campaignId, offerId);
                        campIds_unableToRetrieve.Add(campaignId);
                    }
                }
            }
            End();
        }
    }
}
