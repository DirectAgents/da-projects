using System;
using CakeExtracter.Etl.TradingDesk.LoadersDA;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using FacebookAPI;
using FacebookAPI.Entities;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Common;

namespace CakeExtracter.Etl.SocialMarketing.LoadersDA
{
    public class FacebookAdSetSummaryLoader : Loader<FBSummary>
    {
        public static EntityIdStorage<ActionType> DefaultActionTypeStorage = StorageCollection.ActionTypeStorage;

        private readonly bool loadActions;
        private readonly EntityIdStorage<ActionType> actionTypeStorage;
        private readonly TDAdSetSummaryLoader adSetSummaryLoader;

        public FacebookAdSetSummaryLoader(int accountId, bool loadActions = false)
            : base(accountId)
        {
            BatchSize = FacebookUtility.RowsReturnedAtATime; //FB API only returns 25 rows at a time
            actionTypeStorage = DefaultActionTypeStorage;
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
                    if (actionTypeStorage.IsEntityInStorage(actionTypeCode))
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
                            actionTypeStorage.AddEntityIdToStorage(actionType);
                        }
                        else
                        {
                            var actionType = actionTypesInDb.First();
                            actionTypeStorage.AddEntityIdToStorage(actionType);
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
                var adSetEnumerator = adSetSummaries.GetEnumerator();
                while (itemEnumerator.MoveNext())
                {
                    adSetEnumerator.MoveNext();

                    UpsertAdSetActionSummaries(itemEnumerator.Current, adSetEnumerator.Current.AdSetId, db, progress);
                } // loop through items

                SafeContextWrapper.TrySaveChanges(db);
            }

            Logger.Info(accountId, "Saved AdSetActions ({0} updates, {1} additions, {2} deletions)", progress.UpdatedCount, progress.AddedCount, progress.DeletedCount);
        }

        /// <summary>
        /// The method updates summaries of adSet actions
        /// </summary>
        /// <param name="fbAdSetSummary"></param>
        /// <param name="adSetId"></param>
        /// <param name="db"></param>
        /// <param name="progress"></param>
        private void UpsertAdSetActionSummaries(FBSummary fbAdSetSummary, int adSetId, ClientPortalProgContext db, LoadingProgress progress)
        {
            if (fbAdSetSummary.Actions == null)
            {
                return;
            }

            var fbActionSummaries = fbAdSetSummary.Actions.Values;
            var existingActions = GetExistingAdSetActions(fbAdSetSummary.Date, adSetId, db);

            RemoveAdSetActions(db, progress, existingActions, fbActionSummaries);
            AddUpdateActionSummaries(fbAdSetSummary.Date, adSetId, fbActionSummaries, existingActions, db, progress);
        }

        /// <summary>
        /// The method adds / updates summaries of adSet actions
        /// </summary>
        /// <param name="date"></param>
        /// <param name="adSetId"></param>
        /// <param name="fbActionSummaries"></param>
        /// <param name="existingActions"></param>
        /// <param name="db"></param>
        /// <param name="progress"></param>
        private void AddUpdateActionSummaries(DateTime date, int adSetId,
            IEnumerable<FBAction> fbActionSummaries, List<AdSetAction> existingActions, ClientPortalProgContext db,
            LoadingProgress progress)
        {
            var addedAdSetActions = new List<AdSetAction>();
            foreach (var fbActionSummary in fbActionSummaries)
            {
                var actionTypeId = actionTypeStorage.GetEntityIdFromStorage(fbActionSummary.ActionType);
                var actionsOfType =
                    existingActions.Where(x => x.ActionTypeId == actionTypeId).ToList(); // should be one at most
                if (!actionsOfType.Any())
                {
                    var adSetAction = AddActionSummary(date, adSetId, actionTypeId, fbActionSummary);
                    addedAdSetActions.Add(adSetAction);
                    progress.AddedCount++;
                }
                else
                {
                    progress.UpdatedCount += UpdateActionSummary(actionsOfType, fbActionSummary);
                }
            }
            db.AdSetActions.AddRange(addedAdSetActions);
        }

        /// <summary>
        /// The method updates adSet action summary. Returns a count of updating actions
        /// </summary>
        /// <param name="actionsOfType"></param>
        /// <param name="fbActionSummary"></param>
        /// <returns>Count of updating actions</returns>
        private int UpdateActionSummary(IEnumerable<AdSetAction> actionsOfType, FBAction fbActionSummary)
        {
            var updatedCount = 0;
            foreach (var adSetAction in actionsOfType) // should be just one, but just in case
            {
                SetAdSetActionMetrics(adSetAction, fbActionSummary);
                updatedCount++;
            }

            return updatedCount;
        }

        /// <summary>
        /// The method adds adSet action summary. Returns added adSet action
        /// </summary>
        /// <param name="date"></param>
        /// <param name="adSetId"></param>
        /// <param name="actionTypeId"></param>
        /// <param name="actionSummary"></param>
        /// <returns>Added adSet action</returns>
        private AdSetAction AddActionSummary(DateTime date, int adSetId, int actionTypeId, FBAction actionSummary)
        {
            var adSetAction = new AdSetAction
            {
                Date = date,
                AdSetId = adSetId,
                ActionTypeId = actionTypeId
            };
            SetAdSetActionMetrics(adSetAction, actionSummary);
            return adSetAction;
        }

        /// <summary>
        /// The method returns a list of existing adSet actions (including those saved in the DB and not yet saved, but added to the save queue)
        /// </summary>
        /// <param name="date"></param>
        /// <param name="adSetId"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        private List<AdSetAction> GetExistingAdSetActions(DateTime date, int adSetId, ClientPortalProgContext db)
        {
            var savedInDbActions = db.AdSetActions.Where(x => x.Date == date && x.AdSetId == adSetId).ToList();
            if (savedInDbActions.Any())
            {
                return savedInDbActions;
            }

            var notSavedInDbActions = db.AdSetActions.Local
                .Where(action => action.Date == date && action.AdSetId == adSetId).ToList();
            return notSavedInDbActions;
        }

        /// <summary>
        /// The method removes existing adSet actions
        /// </summary>
        /// <param name="db"></param>
        /// <param name="progress"></param>
        /// <param name="existingActions"></param>
        /// <param name="fbActions"></param>
        private void RemoveAdSetActions(ClientPortalProgContext db, LoadingProgress progress,
            IEnumerable<AdSetAction> existingActions, IEnumerable<FBAction> fbActions)
        {
            var actionTypeIds = fbActions.Select(x => actionTypeStorage.GetEntityIdFromStorage(x.ActionType))
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
