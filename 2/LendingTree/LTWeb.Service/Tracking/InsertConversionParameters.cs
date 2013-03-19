using System;

namespace LTWeb.Service.Tracking
{
    public struct InsertConversionParameters
    {
        [UrlParam("conversion_date")]
        public DateTime ConversionDate { get; set; }

        [UrlParam("affiliate_id")]
        public int AffiliateId { get; set; }

        [UrlParam("campaign_id")]
        public int CampaignId { get; set; }

        [UrlParam("sub_affiliate")]
        public string SubAffiliate { get; set; }

        [UrlParam("creative_id")]
        public int CreativeId { get; set; }

        [UrlParam("total_to_insert")]
        public int TotalToInsert { get; set; }

        [UrlParam("payout")]
        public decimal Payout { get; set; }

        [UrlParam("received")]
        public decimal Received { get; set; }

        [UrlParam("note")]
        public string Note { get; set; }

        [UrlParam("billing_option")]
        public BillingOption BillingOption { get; set; }
    }
}
