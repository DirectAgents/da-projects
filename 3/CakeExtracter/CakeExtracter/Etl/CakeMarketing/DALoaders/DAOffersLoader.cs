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
                var newVerticals = new List<DirectAgents.Domain.Entities.Cake.Vertical>();
                var newOfferTypes = new List<DirectAgents.Domain.Entities.Cake.OfferType>();
                var newOfferStatuses = new List<DirectAgents.Domain.Entities.Cake.OfferStatus>();

                foreach (var item in items)
                {
                    var vertical = db.Verticals.Where(v => v.VerticalId == item.Vertical.VerticalId)
                        .SingleOrFallback(() =>
                        {
                            var newVertical = newVerticals.SingleOrDefault(v => v.VerticalId == item.Vertical.VerticalId);
                            if (newVertical == null)
                            {
                                newVertical = new DirectAgents.Domain.Entities.Cake.Vertical()
                                {
                                    VerticalId = item.Vertical.VerticalId,
                                    VerticalName = item.Vertical.VerticalName
                                };
                                newVerticals.Add(newVertical);
                            }
                            return newVertical;
                        });
                    var offerType = db.OfferTypes.Where(o => o.OfferTypeId == item.OfferType.OfferTypeId)
                        .SingleOrFallback(() =>
                        {
                            var newOfferType = newOfferTypes.SingleOrDefault(o => o.OfferTypeId == item.OfferType.OfferTypeId);
                            if (newOfferType == null)
                            {
                                newOfferType = new DirectAgents.Domain.Entities.Cake.OfferType()
                                {
                                    OfferTypeId = item.OfferType.OfferTypeId,
                                    OfferTypeName = item.OfferType.OfferTypeName
                                };
                                newOfferTypes.Add(newOfferType);
                            }
                            return newOfferType;
                        });
                    var offerStatus = db.OfferStatuses.Where(o => o.OfferStatusId == item.OfferStatus.OfferStatusId)
                        .SingleOrFallback(() =>
                        {
                            var newOfferStatus = newOfferStatuses.SingleOrDefault(o => o.OfferStatusId == item.OfferStatus.OfferStatusId);
                            if (newOfferStatus == null)
                            {
                                newOfferStatus = new DirectAgents.Domain.Entities.Cake.OfferStatus()
                                {
                                    OfferStatusId = item.OfferStatus.OfferStatusId,
                                    OfferStatusName = item.OfferStatus.OfferStatusName
                                };
                                newOfferStatuses.Add(newOfferStatus);
                            }
                            return newOfferStatus;
                        });

                    //TODO: check if advertiser exists??

                    var offer = db.Offers.Where(o => o.OfferId == item.OfferId)
                        .SingleOrFallback(() =>
                        {
                            var newOffer = new DirectAgents.Domain.Entities.Cake.Offer()
                            {
                                OfferId = item.OfferId,
                                Vertical = vertical,
                                OfferType = offerType,
                                OfferStatus = offerStatus
                            };
                            db.Offers.Add(newOffer);
                            return newOffer;
                        });

                    offer.Vertical = vertical;
                    offer.OfferType = offerType;
                    offer.OfferStatus = offerStatus;

                    offer.AdvertiserId = item.Advertiser.AdvertiserId;
                    offer.OfferName = item.OfferName;
                    offer.Hidden = item.Hidden;
                    offer.CurrencyAbbr = item.Currency.CurrencyAbbr;
                    offer.DefaultPriceFormatName = (from c in item.OfferContracts
                                                    where c.OfferContractId == item.DefaultOfferContractId
                                                    select c.PriceFormat.PriceFormatName).SingleOrDefault();
                    offer.DateCreated = item.DateCreated;
                }

                Logger.Info(db.ChangeCountsAsString());

                db.SaveChanges();
            }
            return items.Count;
        }
    }
}
