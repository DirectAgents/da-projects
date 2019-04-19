﻿using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Common;
using CakeExtracter.Etl.SocialMarketing.EntitiesLoaders;
using CakeExtracter.Etl.SocialMarketing.EntitiesStorage;
using CakeExtracter.Etl.SocialMarketing.LoadersDA.EntitiesLoaders;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg.Facebook.Ad;

namespace CakeExtracter.Etl.SocialMarketing.LoadersDA
{
    /// <summary>
    /// Facebook Ad Summary loader
    /// </summary>
    /// <seealso cref="CakeExtracter.Etl.Loader{DirectAgents.Domain.Entities.CPProg.Facebook.Ad.FbAdSummary}" />
    public class FacebookAdSummaryLoader : Loader<FbAdSummary>
    {
        private readonly FacebookAdsLoader fbAdsLoader;
        private readonly FacebookAdSetsLoader fbAdSetsLoader;
        private readonly FacebookCampaignsLoader fbCampaignsLoader;
        private readonly FacebookCreativesLoader fbCreativesLoader;
        private readonly FacebookActionTypeLoader fbActionTypeLoader;
        private readonly DateRange dateRange;

        private List<FbAdSummary> latestSummaries = new List<FbAdSummary>();
        private List<FbAdAction> latestActions = new List<FbAdAction>();

        private static object lockObj = new object();

        private const int batchSize = 1000;

        public FacebookAdSummaryLoader(int accountId, DateRange dateRange)
            : base(accountId, batchSize)
        {
            fbActionTypeLoader = new FacebookActionTypeLoader();
            fbCreativesLoader = new FacebookCreativesLoader();
            fbCampaignsLoader = new FacebookCampaignsLoader();
            fbAdSetsLoader = new FacebookAdSetsLoader(fbCampaignsLoader);
            fbAdsLoader = new FacebookAdsLoader(fbAdSetsLoader, fbCampaignsLoader, fbCreativesLoader);
            this.dateRange = dateRange;
        }

        protected override int Load(List<FbAdSummary> summaries)
        {
            EnsureAdEntitiesData(summaries);
            // sometimes from api can be pulled duplicate summaries
            var uniqueSummaries = summaries.GroupBy(s => new { s.AdId, s.Date }).Select(gr => gr.First()).ToList();
            var notProcessedSummaries = uniqueSummaries.
                 Where(s => !latestSummaries.Any(ls => s.AdId == ls.AdId && s.Date == ls.Date)).ToList();
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

        private void LoadSummaries(List<FbAdSummary> summaries)
        {
            latestSummaries.AddRange(summaries);
        }

        private void EnsureAdEntitiesData(List<FbAdSummary> items)
        {
            var fbAds = items.Select(item => item.Ad).Where(item => item != null).ToList();
            fbAdsLoader.AddUpdateDependentEntities(fbAds);
            items.ForEach(item =>
            {
                item.AdId = item.Ad.Id;
                item.Actions.ForEach(action => action.AdId = item.Ad.Id);
            });
        }

        private List<FbAdAction> PrepareActionsData(List<FbAdSummary> items)
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
                db.FbAdSummaries.Where(x => (x.Date >= dateRange.FromDate && x.Date <= dateRange.ToDate)
                    && x.Ad.AccountId == accountId).DeleteFromQuery();
            }, lockObj, "BulkDeleteByQuery");
        }

        private void LoadLatestSummariesToDb(List<FbAdSummary> summaries)
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
                db.FbAdActions.Where(x => (x.Date >= dateRange.FromDate && x.Date <= dateRange.ToDate)
                    && x.Ad.AccountId == accountId).DeleteFromQuery();
            }, lockObj, "BulkDeleteByQuery");
        }

        private void LoadLatestActionsToDb(List<FbAdAction> actions)
        {
            SafeContextWrapper.TryMakeTransactionWithLock((ClientPortalProgContext db) =>
            {
                db.BulkInsert(actions);
            }, lockObj, "BulkInsert");
        }
    }
}
