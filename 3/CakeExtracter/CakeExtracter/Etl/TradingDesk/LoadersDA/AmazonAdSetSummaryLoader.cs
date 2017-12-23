using Amazon.Entities;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA
{
    public class AmazonAdSetSummaryLoader : Loader<AmazonKeywordMetric>
    {
        private readonly int accountId;
        private Dictionary<string, int> _adSetIdLookup = new Dictionary<string, int>(); // by AdsetEid and DB Id
        public AmazonAdSetSummaryLoader(int accountId = -1)
        {
            this.accountId = accountId;
        }
        protected override int Load(List<AmazonKeywordMetric> items)
        {
            Logger.Info("Loading {0} Amazon AdSet Summary data:", items.Count);
            LoadAdSets();            
            var count = UpsertAdSetSummaries(items);
            return items.Count;
        }
        private void LoadAdSets()
        {
            using (var db = new ClientPortalProgContext())
            {
                _adSetIdLookup = db.AdSets.ToDictionary(y => y.ExternalId, x => x.Id);
            }
        }
        private int UpsertAdSetSummaries(List<AmazonKeywordMetric> items)
        {
            var addedCount = 0;
            var updatedCount = 0;
            var duplicateCount = 0;
            var skippedCount = 0;
            var deletedCount = 0;
            var alreadyDeletedCount = 0;
            var itemCount = 0;
            using (var db = new ClientPortalProgContext())
            {
                foreach (var item in items)
                {
                    if (_adSetIdLookup.ContainsKey(item.KeywordId))
                    {
                        int adSetId = _adSetIdLookup[item.KeywordId];
                        if (!item.AllZeros(includeProspects: false))
                        {
                            var source = new AdSetSummary
                            {
                                Date = item.Date,
                                AdSetId = adSetId,
                                Impressions = item.Impressions,
                                Clicks = item.Clicks,
                                Cost = (decimal)item.Cost,
                            };
                            var target = db.Set<AdSetSummary>().Find(item.Date, adSetId);
                            if (target == null)
                            { // add new
                                db.AdSetSummaries.Add(source);
                                addedCount++;
                            }
                            else
                            { // update existing
                                var entry = db.Entry(target);
                                if (entry.State == EntityState.Unchanged)
                                {
                                    entry.State = EntityState.Detached;
                                    AutoMapper.Mapper.Map(source, target);
                                    entry.State = EntityState.Modified;
                                    updatedCount++;
                                }
                                else
                                {
                                    duplicateCount++;
                                }
                            }
                        }
                        else // AllZeros...
                        {
                            var existing = db.Set<AdSetSummary>().Find(item.Date, adSetId);
                            if (existing == null)
                                alreadyDeletedCount++;
                            else
                            {
                                db.AdSetSummaries.Remove(existing);
                                deletedCount++;
                            }
                        }
                    }
                    else // the eid was not in the Ad dictionary
                    {
                        skippedCount++;
                    }
                    itemCount++;
                }
                Logger.Info("Saving {0} TDadSummaries ({1} updates, {2} additions, {3} deletions, {4} already-deleted, {5} duplicates, {6} skipped)", itemCount, updatedCount, addedCount, deletedCount, alreadyDeletedCount, duplicateCount, skippedCount);
                int numChanges = db.SaveChanges();
            }
            return itemCount;
        }

    }
}
