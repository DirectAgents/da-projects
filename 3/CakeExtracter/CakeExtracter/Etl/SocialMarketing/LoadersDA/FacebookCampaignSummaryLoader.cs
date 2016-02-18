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
    public class FacebookCampaignSummaryLoader : Loader<FBSummary>
    {
        private readonly int accountId;
        private TDStrategySummaryLoader strategySummaryLoader;
        private Dictionary<string, int> strategyIdLookupByCampaignId = new Dictionary<string, int>();

        public FacebookCampaignSummaryLoader(int accountId)
        {
            this.BatchSize = FacebookUtility.RowsPerCall; //FB API only returns 25 rows in one call
            this.accountId = accountId;
            this.strategySummaryLoader = new TDStrategySummaryLoader();
        }

        protected override int Load(List<FBSummary> items)
        {
            Logger.Info("Loading {0} Campaign DailySummaries..", items.Count);
            AddUpdateDependentStrategies(items);
            var sItems = items.Select(i => CreateStrategySummary(i, strategyIdLookupByCampaignId[i.CampaignId])).ToList();
            var count = strategySummaryLoader.UpsertDailySummaries(sItems);
            return count;
        }

        public static StrategySummary CreateStrategySummary(FBSummary item, int strategyId)
        {
            var sum = new StrategySummary
            {
                StrategyId = strategyId,
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

        private void AddUpdateDependentStrategies(List<FBSummary> items)
        {
            var tuples = items.Select(i => Tuple.Create(i.CampaignId, i.CampaignName)).Distinct();

            using (var db = new DATDContext())
            {
                foreach (var tuple in tuples)
                {
                    string campaignId = tuple.Item1;
                    string campaignName = tuple.Item2;

                    if (strategyIdLookupByCampaignId.ContainsKey(campaignId))
                        continue; // already encountered this campaign

                    var stratsInDb = db.Strategies.Where(a => a.AccountId == accountId && a.ExternalId == campaignId);
                    if (!stratsInDb.Any())
                    {   // Strategy doesn't exist in the db; so create it and put an entry in the lookup
                        var strategy = new Strategy
                        {
                            AccountId = accountId,
                            ExternalId = campaignId,
                            Name = campaignName
                            // other properties...
                        };
                        db.Strategies.Add(strategy);
                        db.SaveChanges();
                        Logger.Info("Saved new Strategy: {0} ({1}), ExternalId={2}", strategy.Name, strategy.Id, strategy.ExternalId);
                        strategyIdLookupByCampaignId[campaignId] = strategy.Id;
                    }
                    else
                    {   // Update & put existing Strategy in the lookup
                        // There should only be one matching Strategy in the db, but just in case...
                        foreach (var strat in stratsInDb)
                        {
                            if (!string.IsNullOrWhiteSpace(campaignName))
                                strat.Name = campaignName;
                            // other properties...
                        }
                        int numUpdates = db.SaveChanges();
                        if (numUpdates > 0)
                        {
                            Logger.Info("Updated Strategy: {0}, Eid={1}", campaignName, campaignId);
                            if (numUpdates > 1)
                                Logger.Warn("Multiple entities in db ({0})", numUpdates);
                        }
                        strategyIdLookupByCampaignId[campaignId] = stratsInDb.First().Id;
                    }
                }
            }
        }

    }
}
