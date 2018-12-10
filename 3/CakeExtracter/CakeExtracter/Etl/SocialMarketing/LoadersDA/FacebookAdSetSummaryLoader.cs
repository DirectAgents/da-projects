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
        private readonly bool loadActions;
        private readonly TDAdSetSummaryLoader adSetSummaryLoader;

        static FacebookAdSetSummaryLoader()
        {
            ActionTypeStorage = new EntityIdStorage<ActionType>(x => x.Id, x => x.Code);
        }

        public FacebookAdSetSummaryLoader(int accountId, bool loadActions = false)
            : base(accountId)
        {
            BatchSize = FacebookUtility.RowsReturnedAtATime; //FB API only returns 25 rows at a time
            adSetSummaryLoader = new TDAdSetSummaryLoader(accountId);
            this.loadActions = loadActions;
        }

        protected override int Load(List<FBSummary> items)
        {
            var dbItems = items.Select(CreateAdSetSummary).ToList();
            adSetSummaryLoader.AddUpdateDependentStrategies(dbItems);
            adSetSummaryLoader.AddUpdateDependentAdSets(dbItems);
            adSetSummaryLoader.AssignAdSetIdToItems(dbItems);
            var count = adSetSummaryLoader.UpsertDailySummaries(dbItems);

            if (!loadActions)
            {
                return count;
            }

            AddUpdateDependentActionTypes(items);
            UpsertAdSetActions(items, dbItems);
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
                foreach (var actionTypeCode in actionTypeCodes)
                {
                    if (ActionTypeStorage.IsEntityInStorage(actionTypeCode))
                    {
                        continue;
                    }

                    SafeContextWrapper.Lock(SafeContextWrapper.ActionTypeLocker, () =>
                    {
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
                    });
                }
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
            SafeContextWrapper.TrySaveChanges(db);
            return actionType;
        }

        //Note: get the actions from the items(FBSummaries); get the adsetId from the adsetSummaries
        private void UpsertAdSetActions(List<FBSummary> items, List<AdSetSummary> adSetSummaries)
        {
            var progress = new LoadingProgress();
            using (var db = new ClientPortalProgContext())
            {
                // The items and adsetSummaries have a 1-to-1 correspondence because of how the latter were instantiated above
                var itemEnumerator = items.GetEnumerator();
                var asEnumerator = adSetSummaries.GetEnumerator();
                while (itemEnumerator.MoveNext())
                {
                    asEnumerator.MoveNext();
                    if (itemEnumerator.Current.Actions == null)
                    {
                        continue;
                    }

                    var date = itemEnumerator.Current.Date;
                    var adSetId = asEnumerator.Current.AdSetId;
                    var fbActions = itemEnumerator.Current.Actions.Values;

                    var existingActions = db.AdSetActions.Where(x => x.Date == date && x.AdSetId == adSetId);
                    RemoveAdSetActions(db, progress, existingActions, fbActions);

                    //Add/update the rest
                    var addedAdSetActions = new List<AdSetAction>();
                    foreach (var fbAction in fbActions)
                    {
                        var actionTypeId = ActionTypeStorage.GetEntityIdFromStorage(fbAction.ActionType);
                        var actionsOfType =
                            existingActions.Where(x => x.ActionTypeId == actionTypeId); // should be one at most
                        if (!actionsOfType.Any())
                        {
                            var adSetAction = new AdSetAction
                            {
                                Date = date,
                                AdSetId = adSetId,
                                ActionTypeId = actionTypeId
                            };
                            SetAdSetActionMetrics(adSetAction, fbAction);
                            addedAdSetActions.Add(adSetAction);
                            progress.AddedCount++;
                        }
                        else
                        {
                            foreach (var adSetAction in actionsOfType) // should be just one, but just in case
                            {
                                SetAdSetActionMetrics(adSetAction, fbAction);
                                progress.UpdatedCount++;
                            }
                        }
                    }

                    db.AdSetActions.AddRange(addedAdSetActions);
                } // loop through items

                db.SaveChanges();
            }

            Logger.Info(accountId, "Saved AdSetActions ({0} updates, {1} additions, {2} deletions)", progress.UpdatedCount, progress.AddedCount, progress.DeletedCount);
        }

        private void RemoveAdSetActions(ClientPortalProgContext db, LoadingProgress progress,
            IEnumerable<AdSetAction> existingActions, IEnumerable<FBAction> fbActions)
        {
            var actionTypeIds = fbActions.Select(x => ActionTypeStorage.GetEntityIdFromStorage(x.ActionType))
                .ToArray();
            //Delete actions that no longer have stats for the date/adset
            var actionsForRemoving = existingActions.Where(x => !actionTypeIds.Contains(x.ActionTypeId)).ToList();
            db.AdSetActions.RemoveRange(actionsForRemoving);
            progress.DeletedCount += actionsForRemoving.Count;
        }

        private void SetAdSetActionMetrics(ActionStatsWithVals adSetAction, FBAction fbAction)
        {
            adSetAction.PostClick = fbAction.Num_click ?? 0;
            adSetAction.PostView = fbAction.Num_view ?? 0;
            adSetAction.PostClickVal = fbAction.Val_click ?? 0;
            adSetAction.PostViewVal = fbAction.Val_view ?? 0;
        }
    }
}
