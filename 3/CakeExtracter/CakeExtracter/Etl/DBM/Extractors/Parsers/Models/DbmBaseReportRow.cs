using System;

namespace CakeExtracter.Etl.DBM.Extractors.Parsers.Models
{
    public class DbmBaseReportRow
    {
        public DateTime Date { get; set; }

        public string AdvertiserId { get; set; }

        public string AdvertiserName { get; set; }

        public string AdvertiserCurrencyCode { get; set; }

        public string CampaignId { get; set; }

        public string CampaignName { get; set; }

        public string InsertionOrderId { get; set; }

        public string InsertionOrderName { get; set; }

        public decimal Revenue { get; set; }

        public int Impressions { get; set; }

        public int Clicks { get; set; }

        public int PostClickConversions { get; set; }

        public int PostViewConversions { get; set; }

        public decimal CMPostClickRevenue { get; set; }

        public decimal CMPostViewRevenue { get; set; }
    }
}
