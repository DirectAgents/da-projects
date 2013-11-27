namespace CakeExtracter.CakeMarketingApi.Entities
{
    public class Campaign
    {
        public int CampaignId { get; set; }
        //CampaignType
        public Affiliate Affiliate { get; set; }
        public Offer Offer { get; set; }
        public OfferContractInfo OfferContract { get; set; }
        //etc
        public Currency Currency { get; set; }
        //etc
    }
}
