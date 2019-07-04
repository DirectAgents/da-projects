using System;
using Amazon.Entities.Summaries;

namespace CakeExtracter.Etl.AmazonSelenium.PDA.Models
{
    public class AmazonPdaCampaignSummary : AmazonStatSummary
    {
        public DateTime Date { get; set; }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string TargetingType { get; set; }

        public string Status { get; set; }

        public double StartDate { get; set; }

        public double EndDate { get; set; }

        public double CreationDate { get; set; }

        public long Orders { get; set; }

        public int DetailPageViews { get; set; }

        public int UnitsSold { get; set; }

        public override bool AllZeros()
        {
            return base.AllZeros() && Orders == 0 && DetailPageViews == 0 && UnitsSold == 0;
        }
    }
}
