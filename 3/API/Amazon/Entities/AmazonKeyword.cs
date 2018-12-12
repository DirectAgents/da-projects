namespace Amazon.Entities
{
    public class AmazonKeyword
    {
        public long KeywordId { get; set; }
        public long AdGroupId { get; set; }
        public long CampaignId { get; set; }
        public string KeywordText { get; set; }
        public string MatchType { get; set; }
        public string State { get; set; }        
    }
}
