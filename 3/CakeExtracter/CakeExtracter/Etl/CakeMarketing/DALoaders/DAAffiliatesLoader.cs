using System.Collections.Generic;
using System.Data.Entity;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.Cake;

namespace CakeExtracter.Etl.CakeMarketing.DALoaders
{
    public class DAAffiliatesLoader : Loader<CakeMarketingApi.Entities.Affiliate>
    {
        public DAAffiliatesLoader() { }

        protected override int Load(List<CakeMarketingApi.Entities.Affiliate> items)
        {
            Logger.Info("Synching {0} affiliates...", items.Count);
            using (var db = new DAContext())
            {
                foreach (var item in items)
                {
                    var affiliate = db.Set<Affiliate>().Find(item.AffiliateId);
                    if (affiliate == null)
                    {
                        affiliate = new Affiliate
                        {
                            AffiliateId = item.AffiliateId
                        };
                        db.Affiliates.Add(affiliate);
                    }
                    else // Affiliate found (in cache or db)
                    {
                        var entry = db.Entry(affiliate);
                        if (entry.State != EntityState.Unchanged)
                        {
                            Logger.Warn("Encountered duplicate Affiliate {0} - skipping", item.AffiliateId);
                            continue;
                        }
                    }
                    affiliate.AffiliateName = item.AffiliateName;
                    SetAccountManagerForAffiliate(db, item, affiliate);
                }
                db.SaveChanges();
            }
            return items.Count;
        }

        private void SetAccountManagerForAffiliate(DbContext db, CakeMarketingApi.Entities.Affiliate item, Affiliate affiliate)
        {
            var accountManagerId = item.AccountManagers[0].ContactId;
            var contact = db.Set<Contact>().Find(accountManagerId);
            if (contact != null)
            {
                affiliate.AccountManagerId = contact.ContactId;
            }
            else
            {
                Logger.Info("Affiliate {0}'s AccountManager (ContactId {1}) doesn't exist. Leaving AccountManagerId unchanged.", affiliate.AffiliateId, accountManagerId);
            }
        }
    }
}
