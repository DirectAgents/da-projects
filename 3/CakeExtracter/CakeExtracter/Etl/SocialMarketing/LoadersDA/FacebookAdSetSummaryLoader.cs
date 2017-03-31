using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Etl.TradingDesk.LoadersDA;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using FacebookAPI;
using FacebookAPI.Entities;

namespace CakeExtracter.Etl.SocialMarketing.LoadersDA
{
    public class FacebookAdSetSummaryLoader : Loader<FBSummary>
    {
        private readonly bool LoadActions;
        private TDAdSetSummaryLoader adsetSummaryLoader;
        private Dictionary<string, int> actionTypeIdLookupByCode = new Dictionary<string, int>();

        public FacebookAdSetSummaryLoader(int accountId, bool loadActions = false)
        {
            this.BatchSize = FacebookUtility.RowsReturnedAtATime; //FB API only returns 25 rows at a time
            this.adsetSummaryLoader = new TDAdSetSummaryLoader(accountId);
            this.LoadActions = loadActions;
        }

        protected override int Load(List<FBSummary> items)
        {
            var dbItems = items.Select(i => CreateAdSetSummary(i)).ToList();
            adsetSummaryLoader.AddUpdateDependentStrategies(dbItems);
            adsetSummaryLoader.AddUpdateDependentAdSets(dbItems);
            adsetSummaryLoader.AssignAdSetIdToItems(dbItems);
            var count = adsetSummaryLoader.UpsertDailySummaries(dbItems);

            if (LoadActions)
            {
                AddUpdateDependentActionTypes(items);
                UpsertAdSetActions(items, dbItems);
            }
            return count;
        }

        public static AdSetSummary CreateAdSetSummary(FBSummary item)
        {
            var sum = new AdSetSummary
            {
                Date = item.Date,
                AdSetName = item.AdSetName,
                AdSetEid = item.AdSetId,
                StrategyName = item.CampaignName,
                StrategyEid = item.CampaignId,
                Impressions = item.Impressions,
                AllClicks = item.AllClicks,
                Clicks = item.LinkClicks,
                PostClickConv = item.Conversions_28d_click,
                PostViewConv = item.Conversions_1d_view,
                PostClickRev = item.ConVal_28d_click,
                PostViewRev = item.ConVal_1d_view,
                Cost = item.Spend
            };
            return sum;
        }

        private void AddUpdateDependentActionTypes(List<FBSummary> items)
        {
            FacebookCampaignSummaryLoader.AddUpdateDependentActionTypes(items, this.actionTypeIdLookupByCode);
        }

        //Note: get the actions from the items(FBSummaries); get the adsetId from the adsetSummaries
        private void UpsertAdSetActions(List<FBSummary> items, List<AdSetSummary> adsetSummaries)
        {
            int addedCount = 0;
            int updatedCount = 0;
            int deletedCount = 0;
            using (var db = new ClientPortalProgContext())
            {
                var itemEnumerator = items.GetEnumerator();
                var asEnumerator = adsetSummaries.GetEnumerator();
                while (itemEnumerator.MoveNext())
                {
                    asEnumerator.MoveNext();
                    if (itemEnumerator.Current.Actions == null)
                        continue;
                    var date = itemEnumerator.Current.Date;
                    var adsetId = asEnumerator.Current.AdSetId;
                    var fbActions = itemEnumerator.Current.Actions;

                    var actionTypeIds = fbActions.Select(x => actionTypeIdLookupByCode[x.ActionType]).ToArray();
                    var existingActions = db.AdSetActions.Where(x => x.Date == date && x.AdSetId == adsetId);

                    //Delete actions that no longer have stats for the date/adset
                    foreach (var adsetAction in existingActions.Where(x => !actionTypeIds.Contains(x.ActionTypeId)))
                    {
                        db.AdSetActions.Remove(adsetAction);
                        deletedCount++;
                    }

                    //Add/update the rest
                    foreach (var fbAction in fbActions)
                    {
                        int actionTypeId = actionTypeIdLookupByCode[fbAction.ActionType];
                        var actionsOfType = existingActions.Where(x => x.ActionTypeId == actionTypeId); // should be one at most
                        if (!actionsOfType.Any())
                        { // Create new
                            var adsetAction = new AdSetAction
                            {
                                Date = date,
                                AdSetId = adsetId,
                                ActionTypeId = actionTypeId,
                                PostClick = fbAction.Num_28d_click ?? 0,
                                PostView = fbAction.Num_1d_view ?? 0
                            };
                            db.AdSetActions.Add(adsetAction);
                            addedCount++;
                        }
                        else foreach (var adsetAction in actionsOfType) // should be just one, but just in case
                            { // Update
                                adsetAction.PostClick = fbAction.Num_28d_click ?? 0;
                                adsetAction.PostView = fbAction.Num_1d_view ?? 0;
                                updatedCount++;
                            }
                    }
                    db.SaveChanges();
                } // loop through items
                Logger.Info("Saved AdSetActions ({0} updates, {1} additions, {2} deletions)", updatedCount, addedCount, deletedCount);
            } // using db
        }

    }
}
