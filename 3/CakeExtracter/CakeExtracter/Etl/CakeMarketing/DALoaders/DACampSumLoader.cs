using System;
using System.Collections.Generic;
using CakeExtracter.CakeMarketingApi.Entities;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.Cake;

namespace CakeExtracter.Etl.CakeMarketing.DALoaders
{
    public class DACampSumLoader : Loader<CampaignSummary>
    {
        private readonly DateTime date;
        public DACampSumLoader(DateTime date)
        {
            this.date = date;
        }

        private List<int> campIdsSaved = new List<int>();
        public IEnumerable<int> CampIdsSaved
        {
            get { return campIdsSaved; }
        }

        protected override int Load(List<CampaignSummary> items)
        {
            var loaded = 0;
            var added = 0;
            var updated = 0;
            var deleted = 0;
            var alreadyDeleted = 0;
            using (var db = new DAContext())
            {
                bool toDelete;
                foreach (var item in items)
                {
                    //TODO: test this...
                    //toDelete = (item.Paid == 0);
                    toDelete = false;

                    var pk1 = item.Campaign.CampaignId;
                    var pk2 = this.date;
                    var target = db.Set<CampSum>().Find(pk1, pk2);
                    if (target == null)
                    {
                        if (toDelete)
                            alreadyDeleted++;
                        else
                        {
                            target = new CampSum
                            {
                                CampId = pk1,
                                Date = pk2
                            };
                            item.CopyValuesTo(target);
                            db.CampSums.Add(target);
                            added++;
                            campIdsSaved.Add(target.CampId);
                        }
                    }
                    else // exists in db
                    {
                        if (toDelete)
                        {
                            db.CampSums.Remove(target);
                            deleted++;
                        }
                        else
                        {
                            item.CopyValuesTo(target);
                            updated++;
                            campIdsSaved.Add(target.CampId);
                        }
                    }
                    loaded++;
                }
                Logger.Info("Loading {0} CampSums ({1} updates, {2} additions, {3} deletions, {4} already-deleted)", loaded, updated, added, deleted, alreadyDeleted);
                db.SaveChanges();
            }
            return loaded;
        }
    }
}
