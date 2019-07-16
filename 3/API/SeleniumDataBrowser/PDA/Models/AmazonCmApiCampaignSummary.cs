using System;

namespace SeleniumDataBrowser.PDA.Models
{
    public class AmazonCmApiCampaignSummary
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

        public int Impressions { get; set; }

        public int Clicks { get; set; }

        public decimal Cost { get; set; }

        public decimal AttributedSales14D { get; set; }
    }
}