using System;

namespace Amazon.Entities.Summaries
{
    public class AmazonSearchTermDailySummary : AmazonKeywordDailySummary
    {
        public string Query { get; set; }
        public DateTime Date { get; set; }
    }
}