using System.Collections.Generic;
using System.Linq;
using AdRoll.Entities;
using DirectAgents.Domain.Contexts;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA
{
    public class AdRollAdLoader : Loader<Ad>
    {
        private readonly int accountId;

        public AdRollAdLoader(int acctId)
        {
            this.accountId = acctId;
        }

        protected override int Load(List<Ad> items)
        {
            Logger.Info("Updating {0} Ads..", items.Count);
            using (var db = new DATDContext())
            {
                var dbAds = db.TDads.Where(a => a.AccountId == this.accountId);

                foreach (var itemAd in items)
                {
                    // should be just one, but...
                    var adMatch = dbAds.Where(a => a.ExternalId == itemAd.eid);
                    if (adMatch.Any())
                    {
                        foreach (var dbAd in adMatch)
                        {
                            dbAd.Name = itemAd.name;
                            dbAd.Width = itemAd.width;
                            dbAd.Height = itemAd.height;
                            dbAd.Url = itemAd.src;
                            // status, created_date, updated_date...
                        }
                    }
                    else
                    {
                        Logger.Warn("Ad {0} not found. Skipping update.", itemAd.eid);
                    }
                }
                db.SaveChanges();
            }
            return items.Count;
        }
    }
}
