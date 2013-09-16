using CakeExtracter.CakeMarketingApi;
using CakeExtracter.CakeMarketingApi.Entities;

namespace CakeExtracter.Etl.CakeMarketing.Extracters
{
    public class OffersExtracter : Extracter<Offer>
    {
        protected override void Extract()
        {
            Add(CakeMarketingUtility.Offers());
            End();
        }
    }
}
