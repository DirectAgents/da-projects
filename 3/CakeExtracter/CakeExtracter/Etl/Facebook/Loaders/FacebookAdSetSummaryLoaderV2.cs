using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Commands;
using CakeExtracter.Common;
using CakeExtracter.Etl.Facebook.Exceptions;
using CakeExtracter.Etl.Facebook.Interfaces;
using CakeExtracter.Etl.Facebook.Loaders.EntitiesLoaders;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg.Facebook.AdSet;

namespace CakeExtracter.Etl.Facebook.Loaders
{
    /// <summary>
    /// Facebook ADset Summary loader.
    /// </summary>
    /// <seealso cref="CakeExtracter.Etl.Loader{DirectAgents.Domain.Entities.CPProg.Facebook.AdSet.FbAdSetSummary}" />
    public class FacebookAdSetSummaryLoaderV2 : Loader<FbAdSetSummary>, IFacebookLoadingErrorHandler
    {
        private readonly FacebookAdSetsLoader fbAdSetsLoader;
        private readonly FacebookCampaignsLoader fbCampaignsLoader;
        private readonly FacebookActionTypeLoader fbActionTypeLoader;
        private readonly DateRange dateRange;

        private List<FbAdSetSummary> latestSummaries = new List<FbAdSetSummary>();
        private List<FbAdSetAction> latestActions = new List<FbAdSetAction>();

        private const int batchSize = 1000;

        private static object lockObj = new object();

        /// <inheritdoc/>
        public event Action<FacebookFailedEtlException> ProcessFailedLoading;

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookAdSetSummaryLoaderV2"/> class.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <param name="dateRange">The date range.</param>
        public FacebookAdSetSummaryLoaderV2(int accountId, DateRange dateRange)
            : base(accountId, batchSize)
        {
            fbActionTypeLoader = new FacebookActionTypeLoader();
            fbCampaignsLoader = new FacebookCampaignsLoader();
            fbAdSetsLoader = new FacebookAdSetsLoader(fbCampaignsLoader);
            this.dateRange = dateRange;
        }

        /// <summary>
        /// Loads the specified summaries.
        /// </summary>
        /// <param name="summaries">The summaries.</param>
        /// <returns></returns>
        protected override int Load(List<FbAdSetSummary> summaries)
        {
            try
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
            catch (Exception ex)
            {
                OnProcessFailedLoading(ex, summaries);
                return 0;
            }
        }

        /// <summary>
        /// Afters the load action.
        /// </summary>
        protected override void AfterLoadAction()
        {
            try
            {
                DeleteOldSummariesFromDb();
                LoadLatestSummariesToDb(latestSummaries);
                DeleteOldActionsFromDb();
                LoadLatestActionsToDb(latestActions);
            }
            catch (Exception ex)
            {
                Logger.Error(accountId, ex);
            }
        }

        private void EnsureAdSetEntitiesData(List<FbAdSetSummary> items)
        {
            var fbAdSets = items.Select(item => item.AdSet).Where(item => item != null).ToList();
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
            Logger.Info(accountId, $"Started loading adsets summaries data for {dateRange.ToString()}");
            SafeContextWrapper.TryMakeTransactionWithLock((ClientPortalProgContext db) =>
            {
                db.BulkInsert(summaries);
            }, lockObj, "BulkInsert");
        }

        private void DeleteOldActionsFromDb()
        {
            Logger.Info(accountId, $"Started cleaning old adsets data for {dateRange.ToString()}");
            SafeContextWrapper.TryMakeTransactionWithLock((ClientPortalProgContext db) =>
            {
                db.FbAdSetActions.Where(x => (x.Date >= dateRange.FromDate && x.Date <= dateRange.ToDate)
                    && x.AdSet.AccountId == accountId).DeleteFromQuery();
            }, lockObj, "BulkDeleteByQuery");
        }

        private void LoadLatestActionsToDb(List<FbAdSetAction> actions)
        {
            Logger.Info(accountId, $"Started loading adsets actions data for {dateRange.ToString()}");
            SafeContextWrapper.TryMakeTransactionWithLock((ClientPortalProgContext db) =>
            {
                db.BulkInsert(actions);
            }, lockObj, "BulkInsert");
        }

        private void OnProcessFailedLoading(Exception e, List<FbAdSetSummary> items)
        {
            Logger.Error(accountId, e);
            var exception = GetFailedLoadingException(e, items);
            ProcessFailedLoading?.Invoke(exception);
        }

        private FacebookFailedEtlException GetFailedLoadingException(Exception e, List<FbAdSetSummary> items)
        {
            var fromDate = items.Min(x => x.Date);
            var toDate = items.Max(x => x.Date);
            var fromDateArg = fromDate == default(DateTime) ? null : (DateTime?)fromDate;
            var toDateArg = toDate == default(DateTime) ? null : (DateTime?)toDate;
            var exception = new FacebookFailedEtlException(fromDateArg, toDateArg, accountId, StatsTypeAgg.AdSetArg, e);
            return exception;
        }
    }
}
