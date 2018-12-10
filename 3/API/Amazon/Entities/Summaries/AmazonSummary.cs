using System;

namespace Amazon.Entities.Summaries
{
    public class AmazonSummary
    {
        public DateTime Date { get; set; }
        public int Impressions { get; set; }
        public int Clicks { get; set; }
        public decimal Cost { get; set; }
        public string Campaign { get; set; }
        public string LineItem { get; set; }
        public string Banner { get; set; }
        public string AdInteractionType { get; set; }
        public int Conversions { get; set; }
        public decimal Sales { get; set; }
    }
}