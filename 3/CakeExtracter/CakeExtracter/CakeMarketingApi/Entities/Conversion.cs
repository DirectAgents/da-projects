using System;

namespace CakeExtracter.CakeMarketingApi.Entities
{
    public class Conversion
    {
        public int ConversionId { get; set; }
        public DateTime ConversionDate { get; set; }
        public int ClickId { get; set; }
        public Affiliate Affiliate { get; set; }
        public Advertiser Advertiser { get; set; }
        public Offer Offer { get; set; }
        public Received Received { get; set; }
        public string TransactionId { get; set; }
    }
}