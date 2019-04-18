using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Common;
using CakeExtracter.Etl.SocialMarketing.EntitiesLoaders;
using CakeExtracter.Etl.SocialMarketing.LoadersDA.EntitiesLoaders;
using DirectAgents.Domain.Entities.CPProg.Facebook.AdSet;

namespace CakeExtracter.Etl.SocialMarketing.LoadersDA
{
    public class FacebookAdSetSummaryLoader : Loader<FbAdSetSummary>
    {
        private readonly FacebookAdSetsLoader fbAdSetsLoader;
        private readonly FacebookCampaignsLoader fbCampaignsLoader;
        private readonly FacebookActionTypeLoader fbActionTypeLoader;
        private readonly DateRange dateRange;

        private List<FbAdSetSummary> latestSummaries = new List<FbAdSetSummary>();
        private List<FbAdSetAction> latestActions = new List<FbAdSetAction>();

        public FacebookAdSetSummaryLoader(int accountId, DateRange dateRange)
            : base(accountId)
        {
            fbActionTypeLoader = new FacebookActionTypeLoader();
            fbCampaignsLoader = new FacebookCampaignsLoader();
            fbAdSetsLoader = new FacebookAdSetsLoader(fbCampaignsLoader);
            this.dateRange = dateRange;
        }

        protected override int Load(List<FbAdSetSummary> items)
        {
            EnsureAdSetEntitiesData(items);
            latestSummaries.AddRange(items);
            var actions = PrepareActionsData(items);
            latestActions.AddRange(actions);
            return items.Count;
        }

        protected override void AfterLoadAction()
        {
            DeleteOldSummariesFromDb();
            LoadLatestSummariesToDb(latestSummaries);
            DeleteOldActionsFromDb();
            LoadLatestActionsToDb(latestActions);
        }

        private void LoadSummaries(List<FbAdSetSummary> summaries)
        {
            latestSummaries.AddRange(summaries);
        }

        private void EnsureAdSetEntitiesData(List<FbAdSetSummary> items)
        {
            var fbAdSets = items.Select(item => item.AdSet).ToList();
            fbAdSetsLoader.AddUpdateDependentEntities(fbAdSets);
            items.ForEach(item =>
            {
                item.AdSetId = item.AdSet.Id;
                item.Actions.ForEach(action => action.AdSetId = item.AdSet.Id);
            });
        }

        private List<FbAdSetAction> PrepareActionsData(List<FbAdSetSummary> items)
        {
            var actions = items.SelectMany(item => item.Actions).ToList();
            var actionTypes = actions.Select(action => action.ActionType).ToList();
            fbActionTypeLoader.EnsureActionType(actionTypes);
            actions.ForEach(action => action.ActionTypeId = action.ActionType.Id);
            return actions;
        }

        private void DeleteOldSummariesFromDb()
        {
            SafeContextWrapper.TryMakeTransaction((ClientPortalProgContext db) =>
            {
                db.FbAdSetSummaries.Where(x => (x.Date >= dateRange.FromDate && x.Date <= dateRange.ToDate)
                    && x.AdSet.AccountId == accountId).DeleteFromQuery();
            }, "BulkDeleteByQuery");
        }

        private void LoadLatestSummariesToDb(List<FbAdSetSummary> summaries)
        {
            SafeContextWrapper.TryMakeTransaction((ClientPortalProgContext db) =>
            {
                db.BulkInsert(summaries);
            }, "BulkInsert");
        }

        private void DeleteOldActionsFromDb()
        {
            SafeContextWrapper.TryMakeTransaction((ClientPortalProgContext db) =>
            {
                db.FbAdSetActions.Where(x => (x.Date >= dateRange.FromDate && x.Date <= dateRange.ToDate)
                    && x.AdSet.AccountId == accountId).DeleteFromQuery();
            }, "BulkDeleteByQuery");
        }

        private void LoadLatestActionsToDb(List<FbAdSetAction> actions)
        {
            SafeContextWrapper.TryMakeTransaction((ClientPortalProgContext db) =>
            {
                db.BulkInsert(actions);
            }, "BulkInsert");
        }
    }
}
