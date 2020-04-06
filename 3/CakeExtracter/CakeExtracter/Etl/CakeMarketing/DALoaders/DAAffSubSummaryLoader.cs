using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Etl.CakeMarketing.Exceptions;
using CakeExtracter.CakeMarketingApi.Entities;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.Cake;

namespace CakeExtracter.Etl.CakeMarketing.DALoaders
{
    public class DAAffSubSummaryLoader : Loader<SubIdSummary>
    {
        private readonly Func<SubIdSummary, bool> KeepFunc;
        private Dictionary<int, Dictionary<string, int>> affSubIdLookupByName = new Dictionary<int, Dictionary<string, int>>();
        // (outer dictionary by affId, inner dictionary by AffSub.Name)

        public event Action<CakeAffSubSumsFailedEtlException> ProcessFailedLoading;

        public DAAffSubSummaryLoader()
        {
            this.KeepFunc = sum => !sum.AllZeros();
        }

        protected override int Load(List<SubIdSummary> items)
        {
            try
            {
                Logger.Info("Loading {0} SubIdSums..", items.Count);
                AddMissingAffSubs(items);
                var count = UpsertAffSubSums(items);
                return count;
            }
            catch (Exception e)
            {
                ProcessFailedStatsLoading(e, items);
                return items.Count;
            }
        }

        private int UpsertAffSubSums(IEnumerable<SubIdSummary> items)
        {
            var progress = new LoadingProgress();
            using (var db = new DAContext())
            {
                foreach (var item in items)
                {
                    var toDelete = !KeepFunc(item);

                    if (item.SubIdName == null)
                    {
                        item.SubIdName = "";
                    }
                    // ?what if there's one item with SubIdName=="" and one with SubIdName==null? TODO: handle this

                    var subIdLookup = affSubIdLookupByName[item.affiliateId];
                    var pk1 = subIdLookup[item.SubIdName];
                    var pk2 = item.offerId;
                    var pk3 = item.Date;
                    var target = db.AffSubSummaries.Find(pk1, pk2, pk3);

                    if (toDelete)
                    {
                        if (target == null)
                        {
                            progress.AlreadyDeletedCount++;
                        }
                        else
                        {
                            db.AffSubSummaries.Remove(target);
                            progress.DeletedCount++;
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
                            progress.AddedCount++;
                        }
                        else
                        {
                            // update:
                            item.CopyValuesTo(target);
                            progress.UpdatedCount++;
                        }
                    }

                    progress.ItemCount++;
                }

                Logger.Info("Processing {0} AffSubSummaries ({1} updates, {2} additions, {3} deletions, {4} already-deleted)",
                    progress.ItemCount, progress.UpdatedCount, progress.AddedCount, progress.DeletedCount, progress.AlreadyDeletedCount);
                SafeContextWrapper.TrySaveChanges(db);
            }

            return progress.ItemCount;
        }

        // Also update the affSub lookup...
        private void AddMissingAffSubs(IEnumerable<SubIdSummary> items)
        {
            using (var db = new DAContext())
            {
                var affGroups = items.GroupBy(x => x.affiliateId);
                foreach (var grp in affGroups) // Looping through the affiliates...
                {
                    var affId = grp.Key;
                    if (!affSubIdLookupByName.ContainsKey(affId))
                    {
                        AddAffiliateSubsToLookup(db, affId);
                    }
                    var subIdLookup = affSubIdLookupByName[affId];
                    var namesInLookup = subIdLookup.Keys;

                    //change nulls to empty strings
                    var affSubNamesToAdd = grp.Select(x => x.SubIdName ?? "").Distinct()
                        .Where(name => !namesInLookup.Contains(name)).ToList();
                    foreach (var nameToAdd in affSubNamesToAdd)
                    {
                        var affSub = new AffSub
                        {
                            AffiliateId = affId,
                            Name = nameToAdd
                        };
                        db.AffSubs.Add(affSub);
                        SafeContextWrapper.TrySaveChanges(db);
                        Logger.Info("Saved new AffSub for affiliate {0}: {1}", affId, nameToAdd);
                        subIdLookup[nameToAdd] = affSub.Id;
                    }
                }
            }
        }

        private void AddAffiliateSubsToLookup(DAContext db, int affId)
        {
            var affSubs = db.AffSubs.Where(x => x.AffiliateId == affId).ToList();
            affSubIdLookupByName[affId] = affSubs.GroupBy(x => x.Name).ToDictionary(x => x.Key, x => x.First().Id);
        }

        private void ProcessFailedStatsLoading(Exception e, List<SubIdSummary> items)
        {
            Logger.Error(e);
            var exception = GetFailedLoadingException(e, items);
            ProcessFailedLoading?.Invoke(exception);
        }

        protected virtual CakeAffSubSumsFailedEtlException GetFailedLoadingException(Exception e, List<SubIdSummary> items)
        {
            var fromDate = items.Min(x => x.Date);
            var toDate = items.Max(x => x.Date);
            var fromDateArg = fromDate == default(DateTime) ? null : (DateTime?)fromDate;
            var toDateArg = toDate == default(DateTime) ? null : (DateTime?)toDate;
            var exception = new CakeAffSubSumsFailedEtlException(fromDateArg, toDateArg, null, null, null, e);
            return exception;
        }
    }
}
