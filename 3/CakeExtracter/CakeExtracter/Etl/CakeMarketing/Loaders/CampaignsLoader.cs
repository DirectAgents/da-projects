using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CakeExtracter.CakeMarketingApi.Entities;
using CakeExtracter.Common;

namespace CakeExtracter.Etl.CakeMarketing.Loaders
{
    public class CampaignsLoader : Loader<Campaign>
    {
        protected override int Load(List<Campaign> items)
        {
            Logger.Info("Loading/Synching {0} campaigns...", items.Count);
            using (var db = new ClientPortal.Data.Contexts.ClientPortalContext())
            {
                List<ClientPortal.Data.Contexts.Offer> newOffers = new List<ClientPortal.Data.Contexts.Offer>();
                List<ClientPortal.Data.Contexts.Affiliate> newAffiliates = new List<ClientPortal.Data.Contexts.Affiliate>();

                foreach (var item in items)
                {
                    var offer = db.Offers.Where(o => o.OfferId == item.Offer.OfferId).SingleOrDefault();
                    if (offer == null)
                        offer = newOffers.SingleOrDefault(o => o.OfferId == item.Offer.OfferId);
                    if (offer == null)
                    {
                        Logger.Info("Adding new offer: {0} ({1})", item.Offer.OfferName, item.Offer.OfferId);
                        offer = new ClientPortal.Data.Contexts.Offer
                        {
                            OfferId = item.Offer.OfferId,
                            OfferName = item.Offer.OfferName
                        };
                        newOffers.Add(offer);
                    }
                    //note: offer properties are not updated here

                    var affiliate = db.Affiliates.Where(a => a.AffiliateId == item.Affiliate.AffiliateId).SingleOrDefault();
                    if (affiliate == null)
                        affiliate = newAffiliates.Where(a => a.AffiliateId == item.Affiliate.AffiliateId).SingleOrDefault();
                    if (affiliate == null)
                    {
                        Logger.Info("Adding new affiliate: {0} ({1})", item.Affiliate.AffiliateName, item.Affiliate.AffiliateId);
                        affiliate = new ClientPortal.Data.Contexts.Affiliate
                        {
                            AffiliateId = item.Affiliate.AffiliateId,
                            AffiliateName = item.Affiliate.AffiliateName
                        };
                        newAffiliates.Add(affiliate);
                    }
                    if (affiliate.AffiliateName != item.Affiliate.AffiliateName)
                        affiliate.AffiliateName = item.Affiliate.AffiliateName;

                    var campaign = db.Campaigns.Where(c => c.CampaignId == item.CampaignId).SingleOrDefault();
                    if (campaign == null)
                    {
                        campaign = new ClientPortal.Data.Contexts.Campaign();
                        campaign.CampaignId = item.CampaignId;
                        db.Campaigns.Add(campaign);
                    }
                    campaign.Offer = offer;
                    campaign.Affiliate = affiliate;

                    StringBuilder campaignNameSB = new StringBuilder(offer.OfferName);
                    if (item.OfferContract != null)
                    {
                        if (!String.IsNullOrWhiteSpace(item.OfferContract.OfferContractName))
                            campaignNameSB.Append(" - " + item.OfferContract.OfferContractName);
                        if (item.OfferContract.PriceFormat != null)
                        {
                            campaignNameSB.Append(" - " + item.OfferContract.PriceFormat.PriceFormatName);

                            campaign.PriceFormatName = item.OfferContract.PriceFormat.PriceFormatName;
                        }
                    }
                    if (item.Payout != null)
                        campaignNameSB.Append(" - " + item.Payout.FormattedAmount);

                    campaign.CampaignName = campaignNameSB.ToString();

                    //offer.Currency = item.Currency.CurrencyAbbr;

                    //offer.DefaultPriceFormat = (from c in item.OfferContracts
                    //                            where c.OfferContractId == item.DefaultOfferContractId
                    //                            select c.PriceFormat.PriceFormatName).SingleOrDefault();
                }

                Logger.Info("Campaigns/Offers/Affiliates: " + db.ChangeCountsAsString());

                db.SaveChanges();
            }
            return items.Count;
        }
    }
}
