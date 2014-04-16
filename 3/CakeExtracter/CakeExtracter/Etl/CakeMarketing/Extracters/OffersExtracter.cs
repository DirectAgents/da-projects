using CakeExtracter.CakeMarketingApi;
using CakeExtracter.CakeMarketingApi.Entities;

namespace CakeExtracter.Etl.CakeMarketing.Extracters
{
    public class OffersExtracter : Extracter<Offer>
    {
        private readonly int advertiserId; // 0 for all advertisers

        public OffersExtracter(int advertiserId = 0)
        {
            this.advertiserId = advertiserId;
        }
        protected override void Extract()
        {
            Add(CakeMarketingUtility.Offers(advertiserId));
            End();
        }
    }
}
