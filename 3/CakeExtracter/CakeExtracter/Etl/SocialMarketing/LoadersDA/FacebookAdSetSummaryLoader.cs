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

        private static object lockObj = new object();

        public FacebookAdSetSummaryLoader(int accountId, DateRange dateRange)
            : base(accountId)
        {
            fbActionTypeLoader = new FacebookActionTypeLoader();
            fbCampaignsLoader = new FacebookCampaignsLoader();
            fbAdSetsLoader = new FacebookAdSetsLoader(fbCampaignsLoader);
            this.dateRange = dateRange;
        }

        protected override int Load(List<FbAdSetSummary> summaries)
        {
            EnsureAdSetEntitiesData(summaries);
            // sometimes from api can be pulled duplicate summaries
            var uniqueSummaries = summaries.GroupBy(s => new { s.AdSetId, s.Date }).Select(gr => gr.First()).ToList();
            var notProcessedSummaries = uniqueSummaries.
                Where(s => !latestSummaries.Any(ls => s.AdSetId == ls.AdSetId && s.Date == ls.Date)).ToList();
            latestSummaries.AddRange(notProcessedSummaries);
            var actions = PrepareActionsData(notProcessedSummaries);
            latestActions.AddRange(actions);
            return summaries.Count;
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
            SafeContextWrapper.TryMakeTransactionWithLock((ClientPortalProgContext db) =>
            {
                db.FbAdSetSummaries.Where(x => (x.Date >= dateRange.FromDate && x.Date <= dateRange.ToDate)
                    && x.AdSet.AccountId == accountId).DeleteFromQuery();
            }, lockObj, "BulkDeleteByQuery");
        }

        private void LoadLatestSummariesToDb(List<FbAdSetSummary> summaries)
        {
            SafeContextWrapper.TryMakeTransactionWithLock((ClientPortalProgContext db) =>
            {
                db.BulkInsert(summaries);
            }, lockObj, "BulkInsert");
        }

        private void DeleteOldActionsFromDb()
        {
            SafeContextWrapper.TryMakeTransactionWithLock((ClientPortalProgContext db) =>
            {
                db.FbAdSetActions.Where(x => (x.Date >= dateRange.FromDate && x.Date <= dateRange.ToDate)
                    && x.AdSet.AccountId == accountId).DeleteFromQuery();
            }, lockObj, "BulkDeleteByQuery");
        }

        private void LoadLatestActionsToDb(List<FbAdSetAction> actions)
        {
            SafeContextWrapper.TryMakeTransactionWithLock((ClientPortalProgContext db) =>
            {
                db.BulkInsert(actions);
            }, lockObj, "BulkInsert");
        }
    }
}
