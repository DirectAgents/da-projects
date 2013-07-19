﻿using System.Collections.Generic;
using System.Linq;
using CakeExtracter.CakeMarketingApi.Entities;
using CakeExtracter.Common;
using MoreLinq;

namespace CakeExtracter.Etl.CakeMarketing.Loaders
{
    public class OffersLoader : Loader<Offer>
    {
        protected override int Load(List<Offer> items)
        {
            using (var db = new ClientPortal.Data.Contexts.ClientPortalContext())
            {
                foreach (var item in items)
                {
                    var offer = db.Offers.Where(o => o.Offer_Id == item.OfferId)
                                         .SingleOrFallback(() =>
                                         {
                                             var newOffer = new ClientPortal.Data.Contexts.Offer();
                                             db.Offers.Add(newOffer);
                                             return newOffer;
                                         });

                    offer.Offer_Id = item.OfferId;

                    offer.OfferName = item.OfferName;

                    offer.Currency = item.Currency.CurrencyAbbr;

                    offer.DefaultPriceFormat = (from c in item.OfferContracts
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
