using CakeExtracter.CakeMarketingApi;
using CakeExtracter.CakeMarketingApi.Entities;

namespace CakeExtracter.Etl.CakeMarketing.Extracters
{
    public class CampaignsExtracter : Extracter<Campaign>
    {
        private readonly int offerId;

        public CampaignsExtracter(int offerId)
        {
            this.offerId = offerId;
        }

        protected override void Extract()
        {
            Add(CakeMarketingUtility.Campaigns(offerId));
            End();
        }
    }
}
