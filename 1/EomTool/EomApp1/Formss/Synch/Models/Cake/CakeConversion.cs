﻿namespace EomApp1.Formss.Synch.Models.Cake
{
    public partial class CakeConversion
    {
        public void Update(EomApp1.Cake.WebServices._4.Reports.conversion source)
        {
            this.ConversionDate = source.conversion_date;
            this.Affiliate_Id = source.affiliate.affiliate_id;
            this.Offer_Id = source.offer.offer_id;
            this.Advertiser_Id = source.advertiser.advertiser_id;
            this.Campaign_Id = source.campaign_id;
            this.Creative_Id = source.creative.creative_id;
            this.CreativeName = source.creative.creative_name;
            this.Subid1 = source.sub_id_1;
            this.ConversionType = source.conversion_type;
            this.PricePaid = source.paid.amount;
            this.PricePaidFormattedAmount = source.paid.formatted_amount;
            this.PricePaidCurrencyId = source.paid.currency_id;
            this.PriceReceived = source.received.amount;
            this.PriceReceivedFormattedAmount = source.received.formatted_amount;
            this.PriceReceivedCurrencyId = source.paid.currency_id;
        }
    }
}
