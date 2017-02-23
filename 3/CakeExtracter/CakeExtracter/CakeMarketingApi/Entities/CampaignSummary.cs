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
        public SourceAffiliate1 SourceAffiliate { get; set; }
        public SiteOffer1 SiteOffer { get; set; }
        public BrandAdvertiser1 BrandAdvertiser { get; set; }
        public AccountManager1 BrandAdvertiserManager { get; set; }
        // PriceFormat(e.g. CPA)
        // MediaType(e.g. Email)
        public int Views { get; set; }
        public int Clicks { get; set; }
        public decimal MacroEventConversions { get; set; }
        public decimal Paid { get; set; }
        public decimal Sellable { get; set; }
        public decimal Cost { get; set; }
        public decimal Revenue { get; set; }
        // ClickThruPercentage, Events...
        // Pending, Rejected, Approved, Returned, Orders...

        public void CopyStatsFrom(CampaignSummary cs)
        {
            Views = cs.Views;
            Clicks = cs.Clicks;
            MacroEventConversions = cs.MacroEventConversions;
            Paid = cs.Paid;
            Sellable = cs.Sellable;
            Cost = cs.Cost;
            Revenue = cs.Revenue;
        }
    }
}
