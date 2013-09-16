using CakeExtracter.CakeMarketingApi;
using CakeExtracter.CakeMarketingApi.Entities;

namespace CakeExtracter.Etl.CakeMarketing.Extracters
{
    public class AdvertisersExtracter : Extracter<Advertiser>
    {
        protected override void Extract()
        {
            Add(CakeMarketingUtility.Advertisers());
            End();
        }
    }
}
