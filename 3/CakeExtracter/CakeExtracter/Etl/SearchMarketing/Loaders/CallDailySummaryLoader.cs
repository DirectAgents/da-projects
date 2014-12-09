using ClientPortal.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeExtracter.Etl.SearchMarketing.Loaders
{
    public class CallDailySummaryLoader : Loader<CallDailySummary>
    {
        private readonly int searchProfileId;

        public CallDailySummaryLoader(int searchProfileId)
        {
            this.searchProfileId = searchProfileId;
        }

        protected override int Load(List<CallDailySummary> items)
        {
            Logger.Info("Loading {0} CallDailySummaries..", items.Count);
            AddUpdateDependentSearchCampaigns(items);
            var count = UpsertCallDailySummaries(items);
            return count;
        }

        private int UpsertCallDailySummaries(List<CallDailySummary> items)
        {
            var addedCount = 0;
            var updatedCount = 0;
            var skippedCount = 0;
            var itemCount = 0;
            using (var db = new ClientPortalContext())
            {
                var searchCampaigns = db.SearchProfiles.Find(searchProfileId).SearchAccounts.SelectMany(sa => sa.SearchCampaigns);
                foreach (var item in items)
                {
                    var searchCampaign = searchCampaigns.SingleOrDefault(sc => sc.LCcmpid == item.LCcmpid);
                    if (searchCampaign != null)
                    {
                        item.SearchCampaignId = searchCampaign.SearchCampaignId;

                        var target = db.Set<CallDailySummary>().Find(item.SearchCampaignId, item.Date);
                        if (target == null)
                        {
                            db.CallDailySummaries.Add(item);
                            addedCount++;
                        }
                        else
                        {
                            AutoMapper.Mapper.Map(item, target);
                            db.Entry(target).State = EntityState.Modified;
                            updatedCount++;
                        }
                    }
                    else
                    {
                        Logger.Warn("SearchCampaign {0} not found (SearchProfile {1}); skipping load of item", item.LCcmpid, this.searchProfileId);
                        skippedCount++;
                    }
                    itemCount++;
                }
                Logger.Info("Saving {0} CallDailySummaries ({1} updates, {2} additions, {3} skipped)", itemCount, updatedCount, addedCount, skippedCount);
                db.SaveChanges();
            }
            return itemCount;
        }

        // Find any items whose LCcmpid doesn't exist in any SearchCampaign. If any, create dummy campaigns for them.
        private void AddUpdateDependentSearchCampaigns(List<CallDailySummary> items)
        {
            using (var db = new ClientPortalContext())
            {
                var searchProfile = db.SearchProfiles.Find(searchProfileId);
                var searchCampaigns = searchProfile.SearchAccounts.SelectMany(sa => sa.SearchCampaigns);

                foreach (var lccmpid in items.Select(i => i.LCcmpid).Distinct())
                {
                    var existing = searchCampaigns.Where(sc => sc.LCcmpid == lccmpid).OrderBy(sc => sc.SearchCampaignId).FirstOrDefault();
                    if (existing == null)
                    {
                        var searchAccount = searchProfile.SearchAccounts.OrderBy(sa => sa.SearchAccountId).FirstOrDefault();
                        if (searchAccount == null)
                        {
                            Logger.Warn("Cannot add SearchCampaign because SearchProfile {0} has no SearchAccounts", searchProfile.SearchProfileId);
                        }
                        else
                        {
                            searchAccount.SearchCampaigns.Add(new SearchCampaign
                            {
                                SearchCampaignName = "(temp)",
                                LCcmpid = lccmpid
                            });
                            Logger.Info("Creating (temp) SearchCampaign: {0}", lccmpid);
                            db.SaveChanges();
                        }
                    }
                }
            }
        }
    }
}
