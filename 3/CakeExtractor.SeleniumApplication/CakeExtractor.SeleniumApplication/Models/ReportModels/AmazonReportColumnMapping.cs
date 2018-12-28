using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtractor.SeleniumApplication.Models.ReportModels
{
    internal class AmazonReportColumnMapping : ColumnMapping
    {
        public string DetailPageViews { get; set; }
        public string UnitsSold { get; set; }
        public string AttributedSales14D { get; set; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Date = "\"Date\"";
            Impressions = "Total Impressions";
            Clicks = "Total Clicks";
            Cost = "Total Spend";
            DetailPageViews = "Detail Page Views";
            UnitsSold = "Units Sold";
            AttributedSales14D = "Total Sales";
        }
    }
}
