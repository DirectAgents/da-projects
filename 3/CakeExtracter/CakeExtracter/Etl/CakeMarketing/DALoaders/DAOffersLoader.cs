using CakeExtracter.CakeMarketingApi.Entities;
using CakeExtracter.Common;
using MoreLinq;
using System.Collections.Generic;
using System.Linq;

namespace CakeExtracter.Etl.CakeMarketing.DALoaders
{
    public class DAOffersLoader : Loader<Offer>
    {
        protected override int Load(List<Offer> items)
        {
            Logger.Info("Synching {0} offers...", items.Count);
            using (var db = new DirectAgents.Domain.Contexts.DAContext())
            {
                foreach (var item in items)
                {
                    var offer = db.Offers.Where(o => o.OfferId == item.OfferId)
                                         .SingleOrFallback(() =>
                                         {
                                             var newOffer = new DirectAgents.Domain.Entities.Cake.Offer()
                                             {
                                                 OfferId = item.OfferId
                                             };
                                             db.Offers.Add(newOffer);
                                             return newOffer;
                                         });

                    offer.AdvertiserId = item.Advertiser.AdvertiserId;
                    offer.OfferName = item.OfferName;
                    offer.CurrencyAbbr = item.Currency.CurrencyAbbr;
                    offer.DefaultPriceFormatName = (from c in item.OfferContracts
                                                    where c.OfferContractId == item.DefaultOfferContractId
                                                    select c.PriceFormat.PriceFormatName).SingleOrDefault();
                }

                Logger.Info(db.ChangeCountsAsString());

                db.SaveChanges();
            }
            return items.Count;
        }
    }
}
