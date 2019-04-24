using System;

namespace DBM.Parsers.Models
{
    public class DbmBaseReportRow
    {
        public DateTime Date { get; set; }

        public string AdvertiserId { get; set; }

        public string AdvertiserName { get; set; }

        public string AdvertiserCurrency { get; set; }

        public string CampaignId { get; set; }

        public string CampaignName { get; set; }

        public string InsertionOrderId { get; set; }

        public string InsertionOrderName { get; set; }

        public string LineItemId { get; set; }

        public string LineItemName { get; set; }

        public decimal Revenue { get; set; }

        public decimal Impressions { get; set; }

        public decimal Clicks { get; set; }

        public decimal PostClickConv { get; set; }

        public decimal PostViewConv { get; set; }

        public decimal CMPostClickRevenue { get; set; }

        public decimal CMPostViewRevenue { get; set; }
    }
}
