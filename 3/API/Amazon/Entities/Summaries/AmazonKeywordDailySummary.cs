namespace Amazon.Entities.Summaries
{
    public class AmazonKeywordDailySummary : AmazonStatSummary
    {
        public string KeywordId { get; set; }

        public string KeywordText { get; set; }

        /// <summary>
        /// The match type [ broad, exact, phrase ]. For more information, see match types in the Amazon Advertising support center.
        /// </summary>
        public string MatchType { get; set; }
    }
}