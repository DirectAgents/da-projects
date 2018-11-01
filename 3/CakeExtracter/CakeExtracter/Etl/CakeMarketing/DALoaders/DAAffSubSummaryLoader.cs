using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.CakeMarketingApi.Entities;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.Cake;

namespace CakeExtracter.Etl.CakeMarketing.DALoaders
{
    public class DAAffSubSummaryLoader : Loader<SubIdSummary>
    {
        private readonly Func<SubIdSummary, bool> KeepFunc;
        private Dictionary<int, Dictionary<string, int>> affSubIdLookupByName = new Dictionary<int, Dictionary<string, int>>();
        // (outer dictionary by affId, inner dictionary by AffSub.Name)

        public DAAffSubSummaryLoader()
        {
            this.KeepFunc = sum => !sum.AllZeros();
        }

        protected override int Load(List<SubIdSummary> items)
        {
            Logger.Info("Loading {0} SubIdSums..", items.Count);
            AddMissingAffSubs(items);
            var count = UpsertAffSubSums(items);
            return count;
        }

        private int UpsertAffSubSums(List<SubIdSummary> items)
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
                    toDelete = !KeepFunc(item);

                    if (item.SubIdName == null)
                        item.SubIdName = "";
                    // ?what if there's one item with SubIdName=="" and one with SubIdName==null? TODO: handle this

                    var subIdLookup = affSubIdLookupByName[item.affiliateId];
                    var pk1 = subIdLookup[item.SubIdName];
                    var pk2 = item.offerId;
                    var pk3 = item.Date;
                    var target = db.AffSubSummaries.Find(pk1, pk2, pk3);

                    if (toDelete)
                    {
                        if (target == null)
                            alreadyDeleted++;
                        else
                        {
                            db.AffSubSummaries.Remove(target);
                            deleted++;
                        }
                    }
                    else //to add/update
                    {
                        if (target == null)
                        {
                            target = new AffSubSummary
                            {
                                AffSubId = pk1,
                                OfferId = item.offerId,
                                Date = item.Date
                            };
                            item.CopyValuesTo(target);
                            db.AffSubSummaries.Add(target);
                            added++;
                        }
                        else
                        {   // update:
                            item.CopyValuesTo(target);
                            updated++;
                        }
                    }
                    loaded++;
                }
                Logger.Info("Processing {0} AffSubSummaries ({1} updates, {2} additions, {3} deletions, {4} already-deleted)", loaded, updated, added, deleted, alreadyDeleted);
                db.SaveChanges();
            }
            return loaded;
        }

        // Also update the affSub lookup...
        private void AddMissingAffSubs(List<SubIdSummary> items)
        {
            using (var db = new DAContext())
            {
                var affGroups = items.GroupBy(x => x.affiliateId);
                foreach (var grp in affGroups) // Looping through the affiliates...
                {
                    var affId = grp.Key;
                    if (!affSubIdLookupByName.ContainsKey(affId))
                    {
                        var affSubs = db.AffSubs.Where(x => x.AffiliateId == affId);
                        affSubIdLookupByName[affId] = affSubs.GroupBy(x => x.Name).ToDictionary(x => x.Key, x => x.First().Id);
                    }
                    var subIdLookup = affSubIdLookupByName[affId];
                    var namesInLookup = subIdLookup.Keys;

                                                //change nulls to empty strings
                    var affSubNamesToAdd = grp.Select(x => x.SubIdName == null ? "" : x.SubIdName).Distinct().Where(name => !namesInLookup.Contains(name));
                    foreach (var nameToAdd in affSubNamesToAdd)
                    {
                        var affSub = new AffSub
                        {
                            AffiliateId = affId,
                            Name = nameToAdd
                        };
                        db.AffSubs.Add(affSub);
                        db.SaveChanges(); // to get the id
                        Logger.Info("Saved new AffSub for affiliate {0}: {1}", affId, nameToAdd);
                        subIdLookup[nameToAdd] = affSub.Id;
                    }
                }
            }
        }
    }
}
