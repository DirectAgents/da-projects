using System.Collections.Generic;

namespace CakeExtracter.CakeMarketingApi.Entities
{
    public class CampaignSummaryResponse
    {
        public bool Success { get; set; }
        public int RowCount { get; set; }
        public List<CampaignSummary> Campaigns { get; set; }
    }

    public class CampaignSummary
    {
        public Campaign1 Campaign { get; set; }
        public Affiliate1 Affiliate { get; set; }
        public Offer1 Offer { get; set; }
        public AccountManager1 AccountManager { get; set; }
        public Advertiser1 Advertiser { get; set; }
        public AccountManager1 AdvertiserManager { get; set; }
        // PriceFormat(e.g. CPA)
        // MediaType(e.g. Email)
        public int Views { get; set; }
        public int Clicks { get; set; }
        public int Conversions { get; set; }
        public int Paid { get; set; }
        public int Sellable { get; set; }
        public decimal Cost { get; set; }
        public decimal Revenue { get; set; }
        // ClickThruPercentage, Events...
        // Pending, Rejected, Approved, Returned, Orders...

        public void CopyStatsFrom(CampaignSummary cs)
        {
            Views = cs.Views;
            Clicks = cs.Clicks;
            Conversions = cs.Conversions;
            Paid = cs.Paid;
            Sellable = cs.Sellable;
            Cost = cs.Cost;
            Revenue = cs.Revenue;
        }
    }
}
