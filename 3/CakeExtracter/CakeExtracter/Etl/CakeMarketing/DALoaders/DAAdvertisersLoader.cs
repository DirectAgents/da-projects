using CakeExtracter.CakeMarketingApi.Entities;
using CakeExtracter.Common;
using MoreLinq;
using System.Collections.Generic;
using System.Linq;

namespace CakeExtracter.Etl.CakeMarketing.DALoaders
{
    public class DAAdvertisersLoader : Loader<Advertiser>
    {
        protected override int Load(List<Advertiser> items)
        {
            Logger.Info("Synching {0} advertisers...", items.Count);
            using (var db = new DirectAgents.Domain.Contexts.DAContext())
            {
//                var newCakeRoles = new List<ClientPortal.Data.Contexts.CakeRole>();

                foreach (var item in items)
                {
                    var advertiser = db.Advertisers
                        .Where(a => a.AdvertiserId == item.AdvertiserId)
                        .SingleOrFallback(() =>
                        {
                            var newAdvertiser = new DirectAgents.Domain.Entities.Cake.Advertiser
                            {
                                AdvertiserId = item.AdvertiserId
                            };
                            db.Advertisers.Add(newAdvertiser);
                            return newAdvertiser;
                        });

                    advertiser.AdvertiserName = item.AdvertiserName;
                }
                Logger.Info("Advertisers/CakeContacts/CakeRoles: " + db.ChangeCountsAsString());
                db.SaveChanges();
            }
            return items.Count;
        }
    }
}
