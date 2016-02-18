using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Etl.TradingDesk.LoadersDA;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.TD;
using FacebookAPI;
using FacebookAPI.Entities;

namespace CakeExtracter.Etl.SocialMarketing.LoadersDA
{
    public class FacebookAdSummaryLoader : Loader<FBSummary>
    {
        private readonly int accountId;
        private TDadSummaryLoader tdAdSummaryLoader;
        private Dictionary<string, int> tdAdIdLookupByFBAdId = new Dictionary<string, int>();

        public FacebookAdSummaryLoader(int accountId)
        {
            this.BatchSize = FacebookUtility.RowsPerCall; //FB API only returns 25 rows in one call
            this.accountId = accountId;
            this.tdAdSummaryLoader = new TDadSummaryLoader();
        }

        protected override int Load(List<FBSummary> items)
        {
            Logger.Info("Loading {0} Ad DailySummaries..", items.Count);
            AddUpdateDependentTDads(items);
            var sItems = items.Select(i => CreateTDadSummary(i, tdAdIdLookupByFBAdId[i.AdId])).ToList();
            var count = tdAdSummaryLoader.UpsertDailySummaries(sItems);
            return count;
        }

        public static TDadSummary CreateTDadSummary(FBSummary item, int tdAdId)
        {
            var sum = new TDadSummary
            {
                TDadId = tdAdId,
                Date = item.Date,
                Impressions = item.Impressions,
                //Clicks = item.UniqueClicks,
                Clicks = item.LinkClicks,
                PostClickConv = item.TotalActions,
                //NOTE: TotalActions- includes postclick AND postview (within 1 day)... can be configured?
                //PostViewConv = 0,
                Cost = item.Spend
            };
            return sum;
        }

        private void AddUpdateDependentTDads(List<FBSummary> items)
        {
            var tuples = items.Select(i => Tuple.Create(i.AdId, i.AdName)).Distinct();

            using (var db = new DATDContext())
            {
                foreach (var tuple in tuples)
                {
                    string fbAdId = tuple.Item1;
                    string fbAdName = tuple.Item2;

                    if (tdAdIdLookupByFBAdId.ContainsKey(fbAdId))
                        continue; // already encountered this ad

                    var tdAdsInDb = db.TDads.Where(a => a.AccountId == accountId && a.ExternalId == fbAdId);
                    if (!tdAdsInDb.Any())
                    {   // TDad doesn't exist in the db; so create it and put an entry in the lookup
                        var tdAd = new TDad
                        {
                            AccountId = accountId,
                            ExternalId = fbAdId,
                            Name = fbAdName
                            // other properties...
                        };
                        db.TDads.Add(tdAd);
                        db.SaveChanges();
                        Logger.Info("Saved new TDad: {0} ({1}), ExternalId={2}", tdAd.Name, tdAd.Id, tdAd.ExternalId);
                        tdAdIdLookupByFBAdId[fbAdId] = tdAd.Id;
                    }
                    else
                    {   // Update & put existing TDad in the lookup
                        // There should only be one matching TDad in the db, but just in case...
                        foreach (var tdAd in tdAdsInDb)
                        {
                            if (!string.IsNullOrWhiteSpace(fbAdName))
                                tdAd.Name = fbAdName;
                            // other properties...
                        }
                        int numUpdates = db.SaveChanges();
                        if (numUpdates > 0)
                        {
                            Logger.Info("Updated TDad: {0}, Eid={1}", fbAdName, fbAdId);
                            if (numUpdates > 1)
                                Logger.Warn("Multiple entities in db ({0})", numUpdates);
                        }
                        tdAdIdLookupByFBAdId[fbAdId] = tdAdsInDb.First().Id;
                    }
                }
            }
        }

    }
}
