namespace Amazon.Entities.Summaries
{
    public class AmazonStatSummary
    {
        public string CampaignId { get; set; }
        public string CampaignName { get; set; }
        public string CampaignType { get; set; }
        public string AdGroupId { get; set; }
        public string AdGroupName { get; set; }
        public decimal Cost { get; set; }
        public int Impressions { get; set; }
        public int Clicks { get; set; }
        public int AttributedConversions1D { get; set; }
        public int AttributedConversions7D { get; set; }
        public int AttributedConversions14D { get; set; }
        public int AttributedConversions30D { get; set; }
        public int AttributedConversions1DSameSku { get; set; }
        public int AttributedConversions7DSameSku { get; set; }
        public int AttributedConversions14DSameSku { get; set; }
        public int AttributedConversions30DSameSku { get; set; }
        public decimal AttributedSales1D { get; set; }
        public decimal AttributedSales7D { get; set; }
        public decimal AttributedSales14D { get; set; }
        public decimal AttributedSales30D { get; set; }
        public decimal AttributedSales1DSameSku { get; set; }
        public decimal AttributedSales7DSameSku { get; set; }
        public decimal AttributedSales14DSameSku { get; set; }
        public decimal AttributedSales30DSameSku { get; set; }
        public int AttributedUnitsOrdered1D { get; set; }
        public int AttributedUnitsOrdered7D { get; set; }
        public int AttributedUnitsOrdered14D { get; set; }
        public int AttributedUnitsOrdered30D { get; set; }

        public virtual bool AllZeros()
        {
            return Cost == 0.0M && Impressions == 0 && Clicks == 0 &&
                   AttributedSales1D == 0.0M && AttributedSales7D == 0.0M && AttributedSales14D == 0.0M && AttributedSales30D == 0.0M &&
                   AttributedSales1DSameSku == 0.0M && AttributedSales7DSameSku == 0.0M && AttributedSales14DSameSku == 0.0M && AttributedSales30DSameSku == 0.0M &&
                   AttributedConversions1D == 0 && AttributedConversions7D == 0 && AttributedConversions14D == 0 && AttributedConversions30D == 0 &&
                   AttributedConversions1DSameSku == 0 && AttributedConversions7DSameSku == 0 && AttributedConversions14DSameSku == 0 && AttributedConversions30DSameSku == 0 &&
                   AttributedUnitsOrdered1D == 0 && AttributedUnitsOrdered7D == 0 && AttributedUnitsOrdered14D == 0 && AttributedUnitsOrdered30D == 0;
        }
    }
}