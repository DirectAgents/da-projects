using System.Collections.Generic;
using System.Linq;
using CakeExtracter.CakeMarketingApi.Entities;
using CakeExtracter.Common;
using MoreLinq;

namespace CakeExtracter.Etl.CakeMarketing.Loaders
{
    public class AdvertisersLoader : Loader<Advertiser>
    {
        protected override int Load(List<Advertiser> items)
        {
            using (var db = new ClientPortal.Data.Contexts.ClientPortalContext())
            {
                foreach (var item in items)
                {
                    var advertiser = db.Advertisers
                        .Where(a => a.AdvertiserId == item.AdvertiserId)
                        .SingleOrFallback(() =>
                        {
                            var newAdvertiser = new ClientPortal.Data.Contexts.Advertiser
                            {
                                AdvertiserId = item.AdvertiserId,
                                Culture = "en-US",
                                ShowCPMRep = false,
                                ShowConversionData = false,
                                ConversionValueName = null,
                                ConversionValueIsNumber = false,
                                HasSearch = false
                            };
                            db.Advertisers.Add(newAdvertiser);
                            return newAdvertiser;
                        });

                    advertiser.AdvertiserName = item.AdvertiserName;
                }

                Logger.Info(db.ChangeCountsAsString());

                db.SaveChanges();
            }
            return items.Count;
        }
    }
}
