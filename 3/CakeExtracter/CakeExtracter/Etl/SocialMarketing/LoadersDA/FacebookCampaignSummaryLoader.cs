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
    public class FacebookCampaignSummaryLoader : Loader<FBSummary>
    {
        private static readonly EntityIdStorage<ActionType> ActionTypeStorage;
        private readonly bool loadActions;
        private readonly FacebookAdSetSummaryLoader fbAdSetLoader;
        private readonly TDStrategySummaryLoader strategySummaryLoader;

        static FacebookCampaignSummaryLoader()
        {
            ActionTypeStorage = FacebookAdSetSummaryLoader.ActionTypeStorage;
        }

        public FacebookCampaignSummaryLoader(int accountId, bool loadActions = false)
            : base(accountId)
        {
            BatchSize = FacebookUtility.RowsReturnedAtATime; //FB API only returns 25 rows at a time
            strategySummaryLoader = new TDStrategySummaryLoader(accountId);
            fbAdSetLoader = new FacebookAdSetSummaryLoader(accountId);
            this.loadActions = loadActions;
        }

        protected override int Load(List<FBSummary> items)
        {
            var dbItems = items.Select(CreateStrategySummary).ToList();
            strategySummaryLoader.AddUpdateDependentStrategies(dbItems);
            strategySummaryLoader.AssignStrategyIdToItems(dbItems);
            var count = strategySummaryLoader.UpsertDailySummaries(dbItems);

            if (!loadActions)
            {
                return count;
            }

            AddUpdateDependentActionTypes(items);
            UpsertStrategyActions(items, dbItems);
            return count;
        }

        public StrategySummary CreateStrategySummary(FBSummary item)
        {
            var sum = new StrategySummary
            {
                Date = item.Date,
                Strategy = new Strategy
                {
                    Name = item.CampaignName,
                    ExternalId = item.CampaignId
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

        private void AddUpdateDependentActionTypes(List<FBSummary> items)
        {
            fbAdSetLoader.AddUpdateDependentActionTypes(items, accountId);
        }

        //Note: get the actions from the items(FBSummaries); get the strategyId from the strategySummaries
        private void UpsertStrategyActions(List<FBSummary> items, List<StrategySummary> strategySummaries)
        {
            var progress = new LoadingProgress();
            using (var db = new ClientPortalProgContext())
            {
                var itemEnumerator = items.GetEnumerator();
                var ssEnumerator = strategySummaries.GetEnumerator();
                while (itemEnumerator.MoveNext())
                {
                    ssEnumerator.MoveNext();
                    if (itemEnumerator.Current.Actions == null)
                    {
                        continue;
                    }

                    var date = itemEnumerator.Current.Date;
                    var strategyId = ssEnumerator.Current.StrategyId;
                    var fbActions = itemEnumerator.Current.Actions.Values;

                    var existingActions = db.StrategyActions.Where(x => x.Date == date && x.StrategyId == strategyId);
                    RemoveStrategyActions(db, progress, existingActions, fbActions);

                    //Add/update the rest
                    var addedStrategyActions = new List<StrategyAction>();
                    foreach (var fbAction in fbActions)
                    {
                        var actionTypeId = ActionTypeStorage.GetEntityIdFromStorage(fbAction.ActionType);
                        var actionsOfType = existingActions.Where(x => x.ActionTypeId == actionTypeId); // should be one at most
                        if (!actionsOfType.Any())
                        {
                            var strategyAction = new StrategyAction
                            {
                                Date = date,
                                StrategyId = strategyId,
                                ActionTypeId = actionTypeId
                            };
                            SetStrategyActionMetrics(strategyAction, fbAction);
                            addedStrategyActions.Add(strategyAction);
                            progress.AddedCount++;
                        }
                        else
                        {
                            foreach (var strategyAction in actionsOfType) // should be just one, but just in case
                            {
                                SetStrategyActionMetrics(strategyAction, fbAction);
                                progress.UpdatedCount++;
                            }
                        }
                    }

                    db.StrategyActions.AddRange(addedStrategyActions);
                } // loop through items

                SafeContextWrapper.TrySaveChanges(db);
            }

            Logger.Info(accountId, "Saved StrategyActions ({0} updates, {1} additions, {2} deletions)", progress.UpdatedCount, progress.AddedCount, progress.DeletedCount);
        }

        private void RemoveStrategyActions(ClientPortalProgContext db, LoadingProgress progress,
                IEnumerable<StrategyAction> existingActions, IEnumerable<FBAction> fbActions)
        {
            var actionTypeIds = fbActions.Select(x => ActionTypeStorage.GetEntityIdFromStorage(x.ActionType))
                .ToArray();
            //Delete actions that no longer have stats for the date/adset
            var actionsForRemoving = existingActions.Where(x => !actionTypeIds.Contains(x.ActionTypeId)).ToList();
            db.StrategyActions.RemoveRange(actionsForRemoving);
            progress.DeletedCount += actionsForRemoving.Count;
        }

        private void SetStrategyActionMetrics(StrategyAction strategyAction, FBAction fbAction)
        {
            strategyAction.PostClick = fbAction.Num_click ?? 0;
            strategyAction.PostView = fbAction.Num_view ?? 0;
        }
    }
}
