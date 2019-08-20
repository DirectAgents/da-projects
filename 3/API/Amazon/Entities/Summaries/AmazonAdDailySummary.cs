namespace Amazon.Entities.Summaries
{
    public class AmazonAdDailySummary : AmazonStatSummary
    {
        public string AdId { get; set; }

        public string Asin { get; set; }

        public string Sku { get; set; }
    }
}