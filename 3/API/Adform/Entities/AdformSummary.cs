using System;

namespace Adform
{
    public class AdformSummary
    {
        public DateTime Date { get; set; }
        public int Impressions { get; set; }
        public int Clicks { get; set; }
        public decimal Cost { get; set; }

        public string Order { get; set; }
        public string Campaign { get; set; }
        public string LineItem { get; set; }
        public string Banner { get; set; }

        public string AdInteractionType { get; set; }
        public int ConversionsAll { get; set; }
        public int ConversionsConvType1 { get; set; }
        public int ConversionsConvType2 { get; set; }
        public int ConversionsConvType3 { get; set; }
        public decimal SalesAll { get; set; }
        public decimal SalesConvType1 { get; set; }
        public decimal SalesConvType2 { get; set; }
        public decimal SalesConvType3 { get; set; }
    }
}
