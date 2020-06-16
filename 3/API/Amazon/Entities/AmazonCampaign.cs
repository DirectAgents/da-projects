using System.Collections.Generic;

namespace Amazon.Entities
{
    public class AmazonCampaign
    {
        public string CampaignId { get; set; }
        public string Name { get; set; }
        public string CampaignType { get; set; }
        public string TargetingType { get; set; }
        public bool PremiumBidAdjustment { get; set; }
        public decimal DailyBudget { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string State { get; set; }
        public List<string> Networks { get; set; }
    }
}