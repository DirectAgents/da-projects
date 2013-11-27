using System.Collections.Generic;
using System.Linq;
using CakeExtracter.CakeMarketingApi.Entities;
using CakeExtracter.Common;
using MoreLinq;

namespace CakeExtracter.Etl.CakeMarketing.Loaders
{
    public class CampaignsLoader : Loader<Campaign>
    {
        protected override int Load(List<Campaign> items)
        {
            Logger.Info("Synching {0} campaigns...", items.Count);
            using (var db = new ClientPortal.Data.Contexts.ClientPortalContext())
            {
                foreach (var item in items)
                {
                    var offer = db.Offers.Where(o => o.OfferId == item.Offer.OfferId)
                                    .SingleOrFallback(() =>
                                        {
                                            Logger.Info("Adding new offer: {0} ({1})", item.Offer.OfferName, item.Offer.OfferId);
                                            var newOffer = new ClientPortal.Data.Contexts.Offer
                                            {
                                                OfferId = item.Offer.OfferId,
                                                OfferName = item.Offer.OfferName
                                            };
                                            return newOffer;
                                        });

                    var affiliate = db.Affiliates.Where(a => a.AffiliateId == item.Affiliate.AffiliateId)
                                        .SingleOrFallback(() =>
                                            {
                                                Logger.Info("Adding new affiliate: {0} ({1})", item.Affiliate.AffiliateName, item.Affiliate.AffiliateId);
                                                var newAffiliate = new ClientPortal.Data.Contexts.Affiliate
                                                {
                                                    AffiliateId = item.Affiliate.AffiliateId,
                                                    AffiliateName = item.Affiliate.AffiliateName
                                                };
                                                return newAffiliate;
                                            });

                    var campaign = db.Campaigns.Where(c => c.CampaignId == item.CampaignId)
                                           .SingleOrFallback(() =>
                                               {
                                                   var newCampaign = new ClientPortal.Data.Contexts.Campaign();
                                                   newCampaign.CampaignId = item.CampaignId;
                                                   db.Campaigns.Add(newCampaign);
                                                   return newCampaign;
                                               });

                    campaign.Offer = offer;
                    campaign.Affiliate = affiliate;

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
