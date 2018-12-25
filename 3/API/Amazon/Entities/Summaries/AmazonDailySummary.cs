namespace Amazon.Entities.Summaries
{
    public class AmazonDailySummary : AmazonStatSummary
    {
        public string CampaignId { get; set; }
        public string CampaignName { get; set; }
        public string CampaignType { get; set; }
    }
}
