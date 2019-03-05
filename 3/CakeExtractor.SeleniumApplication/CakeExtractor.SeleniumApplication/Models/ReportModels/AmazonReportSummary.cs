using System;
using Amazon.Entities.Summaries;

namespace CakeExtractor.SeleniumApplication.Models.ReportModels
{
    public class AmazonReportSummary : AmazonStatSummary
    {
        public DateTime Date { get; set; }
        public int DetailPageViews { get; set; }
        public int UnitsSold { get; set; }

        public override bool AllZeros()
        {
            return base.AllZeros() && DetailPageViews == 0 && UnitsSold == 0;
        }
    }
}
