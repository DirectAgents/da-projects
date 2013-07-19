using System.Collections.Generic;
using System.Data.Entity.Validation;
using CakeExtracter.CakeMarketingApi.Entities;
using CakeExtracter.Common;

namespace CakeExtracter.Etl.CakeMarketing.Loaders
{
    public class ClicksLoader : Loader<Click>
    {
        protected override int Load(List<Click> items)
        {
            var loaded = 0;
            var added = 0;
            var updated = 0;
            using (var db = new ClientPortal.Data.Contexts.ClientPortalDWContext())
            {
                foreach (var source in items)
                {
                    ClientPortal.Data.Contexts.FactClick target;
                    target = db.FactClicks.Find(source.ClickId);
                    if (target == null)
                    {
                        target = new ClientPortal.Data.Contexts.FactClick
                        {
                            ClickKey = source.ClickId,
                        };

                        source.Region = source.Region ?? new Region
                        {
                            RegionCode = "unknown",
                            RegionName = "unknown"
                        };
                        source.Region.RegionCode = string.IsNullOrWhiteSpace(source.Region.RegionCode)
                                                        ? "unknown"
                                                        : source.Region.RegionCode;
                        target.DimRegion = db.FindOrCreateByPredicate(c => c.RegionCode == source.Region.RegionCode, () => new ClientPortal.Data.Contexts.DimRegion
                        {
                            RegionCode = source.Region.RegionCode
                        });

                        target.DimCountry = db.FindOrCreateByPredicate(c => c.CountryCode == source.Country.CountryCode, () => new ClientPortal.Data.Contexts.DimCountry
                        {
                            CountryCode = source.Country.CountryCode,
                        });

                        target.DateKey = source.ClickDate.Date;

                        target.DimAdvertiser = db.FindOrCreateByKey(source.Advertiser.AdvertiserId, () => new ClientPortal.Data.Contexts.DimAdvertiser
                        {
                            AdvertiserKey = source.Advertiser.AdvertiserId,
                            AdvertiserName = source.Advertiser.AdvertiserName
                        });

                        target.DimOffer = db.FindOrCreateByKey(source.Offer.OfferId, () => new ClientPortal.Data.Contexts.DimOffer
                        {
                            OfferKey = source.Offer.OfferId,
                            OfferName = source.Offer.OfferName
                        });

                        target.DimAffiliate = db.FindOrCreateByKey(source.Affiliate.AffiliateId, () => new ClientPortal.Data.Contexts.DimAffiliate
                        {
                            AffiliateKey = source.Affiliate.AffiliateId,
                            AffiliateName = source.Affiliate.AffiliateName
                        });

                        source.Browser = source.Browser ?? new Browser
                        {
                            BrowserId = 0,
                            BrowserName = "unknown"
                        };
                        target.DimBrowser = db.FindOrCreateByKey(source.Browser.BrowserId, () => new ClientPortal.Data.Contexts.DimBrowser
                        {
                            BrowserKey = source.Browser.BrowserId,
                            BrowserName = source.Browser.BrowserName
                        });

                        db.FactClicks.Add(target);
                        added++;
                    }
                    else
                    {
                        updated++;
                    }
                    loaded++;
                }

                Logger.Info("Loading {0} FactClicks ({1} updates, {2} additions)", loaded, updated, added);

                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var entityValidationError in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in entityValidationError.ValidationErrors)
                        {
                            Logger.Warn("entity validation error for property {0}: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }

                    Logger.Error(ex);
                    throw;
                }
            }
            return loaded;
        }
    }
}
