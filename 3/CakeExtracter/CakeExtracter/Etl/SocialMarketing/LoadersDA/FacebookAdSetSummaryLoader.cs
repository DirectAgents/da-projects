using CakeExtracter.Etl.TradingDesk.LoadersDA;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using FacebookAPI;
using FacebookAPI.Entities;
using System.Collections.Generic;
using System.Linq;

namespace CakeExtracter.Etl.SocialMarketing.LoadersDA
{
    public class FacebookAdSetSummaryLoader : Loader<FBSummary>
    {
        public static EntityIdStorage<ActionType> ActionTypeStorage;
        private readonly bool LoadActions;
        private readonly TDAdSetSummaryLoader adsetSummaryLoader;

        static FacebookAdSetSummaryLoader()
        {
            ActionTypeStorage = new EntityIdStorage<ActionType>(x => x.Id, x => x.Code);
        }

        public FacebookAdSetSummaryLoader(int accountId, bool loadActions = false)
            : base(accountId)
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
                AdSet = new AdSet
                {
                    ExternalId = item.AdSetId,
                    Name = item.AdSetName,
                    Strategy = new Strategy
                    {
                        Name = item.CampaignName,
                        ExternalId = item.CampaignId,
                    }
                },
                Impressions = item.Impressions,
                AllClicks = item.AllClicks,
                Clicks = item.LinkClicks,
                PostClickConv = item.Conversions_click,
                PostViewConv = item.Conversions_view,
                PostClickRev = item.ConVal_click,
                PostViewRev = item.ConVal_view,
                Cost = item.Spend
            };
            return sum;
        }

        public void AddUpdateDependentActionTypes(List<FBSummary> items, int accountId)
        {
            var actionTypeCodes = items.Where(i => i.Actions != null).SelectMany(i => i.Actions.Keys).Distinct();

            using (var db = new ClientPortalProgContext())
            {
                SafeContextWrapper.Lock(SafeContextWrapper.ActionTypeLocker, () =>
                {
                    foreach (var actionTypeCode in actionTypeCodes)
                    {
                        if (ActionTypeStorage.IsEntityInStorage(actionTypeCode))
                        {
                            continue;
                        }

                        var actionTypesInDb = db.ActionTypes.Where(x => x.Code == actionTypeCode).ToList();
                        if (!actionTypesInDb.Any())
                        {
                            var actionType = AddActionType(db, actionTypeCode);
                            Logger.Info(accountId, "Saved new ActionType: {0}", actionType.Code);
                            ActionTypeStorage.AddEntityIdToStorage(actionType);
                        }
                        else
                        {
                            var actionType = actionTypesInDb.First();
                            ActionTypeStorage.AddEntityIdToStorage(actionType);
                        }
                    }
                });
            }
        }
        
        private void AddUpdateDependentActionTypes(List<FBSummary> items)
        {
            AddUpdateDependentActionTypes(items, accountId);
        }

        private ActionType AddActionType(ClientPortalProgContext db, string actionTypeCode)
        {
            var actionType = new ActionType
            {
                Code = actionTypeCode
            };
            db.ActionTypes.Add(actionType);
            db.SaveChanges();
            return actionType;
        }

        //Note: get the actions from the items(FBSummaries); get the adsetId from the adsetSummaries
        private void UpsertAdSetActions(List<FBSummary> items, List<AdSetSummary> adsetSummaries)
        {
            var progress = new LoadingProgress();
            SafeContextWrapper.SaveChangedContext<ClientPortalProgContext>(
                SafeContextWrapper.AdSetActionLocker, db =>
                {
                    // The items and adsetSummaries have a 1-to-1 correspondence because of how the latter were instantiated above
                    var itemEnumerator = items.GetEnumerator();
                    var asEnumerator = adsetSummaries.GetEnumerator();
                    while (itemEnumerator.MoveNext())
                    {
                        asEnumerator.MoveNext();
                        if (itemEnumerator.Current.Actions == null)
                        {
                            continue;
                        }

                        var date = itemEnumerator.Current.Date;
                        var adsetId = asEnumerator.Current.AdSetId;
                        var fbActions = itemEnumerator.Current.Actions.Values;

                        var actionTypeIds = fbActions.Select(x => ActionTypeStorage.GetEntityIdFromStorage(x.ActionType)).ToArray();
                        var existingActions = db.AdSetActions.Where(x => x.Date == date && x.AdSetId == adsetId);

                        //Delete actions that no longer have stats for the date/adset
                        foreach (var adsetAction in existingActions.Where(x => !actionTypeIds.Contains(x.ActionTypeId)))
                        {
                            db.AdSetActions.Remove(adsetAction);
                            progress.DeletedCount++;
                        }

                        //Add/update the rest
                        foreach (var fbAction in fbActions)
                        {
                            var actionTypeId = ActionTypeStorage.GetEntityIdFromStorage(fbAction.ActionType);
                            var actionsOfType =
                                existingActions.Where(x => x.ActionTypeId == actionTypeId); // should be one at most
                            if (!actionsOfType.Any())
                            {
                                // Create new
                                var adsetAction = new AdSetAction
                                {
                                    Date = date,
                                    AdSetId = adsetId,
                                    ActionTypeId = actionTypeId,
                                    PostClick = fbAction.Num_click ?? 0,
                                    PostView = fbAction.Num_view ?? 0,
                                    PostClickVal = fbAction.Val_click ?? 0,
                                    PostViewVal = fbAction.Val_view ?? 0
                                };
                                db.AdSetActions.Add(adsetAction);
                                progress.AddedCount++;
                            }
                            else
                            {
                                foreach (var adsetAction in actionsOfType) // should be just one, but just in case
                                {
                                    // Update
                                    adsetAction.PostClick = fbAction.Num_click ?? 0;
                                    adsetAction.PostView = fbAction.Num_view ?? 0;
                                    adsetAction.PostClickVal = fbAction.Val_click ?? 0;
                                    adsetAction.PostViewVal = fbAction.Val_view ?? 0;
                                    progress.UpdatedCount++;
                                }
                            }
                        }
                    } // loop through items
                }
            );

            Logger.Info(accountId, "Saved AdSetActions ({0} updates, {1} additions, {2} deletions)", progress.UpdatedCount, progress.AddedCount, progress.DeletedCount);
        }

    }
}
