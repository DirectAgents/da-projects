using ClientPortal.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeExtracter.Etl.SearchMarketing.Loaders
{
    public class CallDailySummaryLoader : Loader<CallDailySummary>
    {
        private readonly SearchProfile searchProfile;

        public CallDailySummaryLoader(SearchProfile searchProfile)
        {
            this.searchProfile = searchProfile;
        }

        protected override int Load(List<CallDailySummary> items)
        {
            Logger.Info("Loading {0} CallDailySummaries..", items.Count);
            //AddUpdateDependentSearchCampaigns(items);
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
                foreach (var item in items)
                {
                    var searchCampaign = searchProfile.SearchAccounts.SelectMany(sa => sa.SearchCampaigns).SingleOrDefault(sc => sc.LCcmpid == item.LCcmpid);
                    //...
                }
                Logger.Info("Saving {0} CallDailySummaries ({1} updates, {2} additions, {3} skipped)", itemCount, updatedCount, addedCount, skippedCount);
                db.SaveChanges();
            }
            return itemCount;
        }
    }
}
