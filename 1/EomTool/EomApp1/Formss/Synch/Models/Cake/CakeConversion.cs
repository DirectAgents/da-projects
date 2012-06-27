namespace EomApp1.Formss.Synch.Models.Cake
{
    public partial class CakeConversion
    {
        public void Update(EomApp1.Cake.WebServices._4.Reports.conversion source)
        {
            var target = this;
            target.ConversionDate = source.conversion_date;
            target.Affiliate_Id = source.affiliate.affiliate_id;
            target.Offer_Id = source.offer.offer_id;
            target.Advertiser_Id = source.advertiser.advertiser_id;
            target.Campaign_Id = source.campaign_id;
            target.Creative_Id = source.creative.creative_id;
            target.CreativeName = source.creative.creative_name;
            target.Subid1 = source.sub_id_1;
            target.ConversionType = source.conversion_type;
            target.PricePaid = source.paid.amount;
            target.PricePaidFormattedAmount = source.paid.formatted_amount;
            target.PricePaidCurrencyId = source.paid.currency_id;
            target.PriceReceived = source.received.amount;
            target.PriceReceivedFormattedAmount = source.received.formatted_amount;
            target.PriceReceivedCurrencyId = source.paid.currency_id;
        }
    }
}
