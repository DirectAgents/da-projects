using System;

namespace CakeExtracter.CakeMarketingApi.Entities
{
    public class Click
    {
        public int ClickId { get; set; }
        public int TotalClicks { get; set; }
        public DateTime ClickDate { get; set; }
        public Affiliate Affiliate { get; set; }
        public Advertiser Advertiser { get; set; }
        public Offer Offer { get; set; }
        public Country Country { get; set; }
        public Region Region { get; set; }
        public Browser Browser { get; set; }
    }
}
