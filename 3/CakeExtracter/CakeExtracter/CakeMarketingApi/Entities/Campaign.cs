namespace CakeExtracter.CakeMarketingApi.Entities
{
    public class Campaign
    {
        //NAME: Offer.OfferName [- OfferContract.OfferContractName] - OfferContract.PriceFormat.PriceFormatName - Payout.FormattedAmount
        // "Payout.FormattedAmount" could instead use: Currency.CurrencySymbol and Payout.Amount (X.XX) ?

        public int CampaignId { get; set; }
        //CampaignType
        public Affiliate1 Affiliate { get; set; }
        public Offer1 Offer { get; set; }
        public OfferContractInfo OfferContract { get; set; }
        //etc
        public Payout Payout { get; set; }
        public Currency Currency { get; set; }
        //etc
    }
}
