namespace Amazon.Entities.Summaries
{
    public class AmazonAsinSummaries : AmazonStatSummary
    {
        public string KeywordId { get; set; }

        public string KeywordText { get; set; }

        public string MatchType { get; set; }

        public string Asin { get; set; }

        public string OtherAsin { get; set; }

        public string Sku { get; set; }

        public int AttributedUnitsOrdered1DOtherSku { get; set; }

        public int AttributedUnitsOrdered7DOtherSku { get; set; }

        public int AttributedUnitsOrdered14DOtherSku { get; set; }

        public int AttributedUnitsOrdered30DOtherSku { get; set; }

        public decimal AttributedSales1DOtherSku { get; set; }

        public decimal AttributedSales7DOtherSku { get; set; }

        public decimal AttributedSales14DOtherSku { get; set; }

        public decimal AttributedSales30DOtherSku { get; set; }

        public override bool AllZeros()
        {
            return AttributedUnitsOrdered1DOtherSku == 0 && AttributedUnitsOrdered7DOtherSku == 0 &&
                   AttributedUnitsOrdered14DOtherSku == 0 && AttributedUnitsOrdered30DOtherSku == 0 &&
                   AttributedSales1DOtherSku == 0 && AttributedSales7DOtherSku == 0 &&
                   AttributedSales14DOtherSku == 0 && AttributedSales30DOtherSku == 0;
        }
    }
}
