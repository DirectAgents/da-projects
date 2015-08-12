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
        public Affiliate1 Affiliate { get; set; }
    }
}
