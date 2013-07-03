using CakeExtracter.CakeMarketingApi.Entities;

namespace CakeExtracter.CakeMarketingApi.Entities
{
    public class OfferDailySummary
    {
        public OfferDailySummary()
        {
        }

        public OfferDailySummary(int offerId, DailySummary dailySummary)
        {
            OfferId = offerId;
            DailySummary = dailySummary;
        }

        public int OfferId { get; set; }

        public DailySummary DailySummary { get; set; }
    }
}