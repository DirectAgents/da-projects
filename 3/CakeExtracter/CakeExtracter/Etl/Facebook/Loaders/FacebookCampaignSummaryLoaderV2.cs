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
using DirectAgents.Domain.Entities.CPProg.Facebook.Campaign;

namespace CakeExtracter.Etl.Facebook.Loaders
{
    /// <summary>
    /// Facebook Campaigns Summary Loader.
    /// </summary>
    /// <seealso cref="FbCampaignSummary" />
    public class FacebookCampaignSummaryLoaderV2 : Loader<FbCampaignSummary>, IFacebookLoadingErrorHandler
    {
        private readonly FacebookCampaignsLoader fbCampaignsLoader;
        private readonly DateRange dateRange;

        private static object lockObj = new object();

        private List<FbCampaignSummary> latestSummaries = new List<FbCampaignSummary>();

        /// <inheritdoc/>
        public event Action<FacebookFailedEtlException> ProcessFailedLoading;

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookCampaignSummaryLoaderV2"/> class.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <param name="dateRange">The date range.</param>
        public FacebookCampaignSummaryLoaderV2(int accountId, DateRange dateRange)
            : base(accountId)
        {
            fbCampaignsLoader = new FacebookCampaignsLoader();
            this.dateRange = dateRange;
        }

        /// <summary>
        /// Loads the specified summaries.
        /// </summary>
        /// <param name="summaries">The summaries.</param>
        /// <returns></returns>
        protected override int Load(List<FbCampaignSummary> summaries)
        {

            try
            {
                EnsureCampaignsEntitiesData(summaries);
                // sometimes from api can be pulled duplicate summaries
                var uniqueSummaries = summaries.GroupBy(s => new { s.CampaignId, s.Date }).Select(gr => gr.First()).ToList();
                var notProcessedSummaries = uniqueSummaries.
                     Where(s => !latestSummaries.Any(ls => s.CampaignId == ls.CampaignId && s.Date == ls.Date)).ToList();
                latestSummaries.AddRange(notProcessedSummaries);
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
            }
            catch (Exception ex)
            {
                Logger.Error(accountId, ex);
            }
        }

        private void LoadSummaries(List<FbCampaignSummary> summaries)
        {
            latestSummaries.AddRange(summaries);
        }

        private void EnsureCampaignsEntitiesData(List<FbCampaignSummary> items)
        {
            var fbCampaigns = items.Select(item => item.Campaign).Where(item => item != null).ToList();
            fbCampaignsLoader.AddUpdateDependentEntities(fbCampaigns);
            items.ForEach(item =>
            {
                item.CampaignId = item.Campaign.Id;
            });
        }

        private void DeleteOldSummariesFromDb()
        {
            Logger.Info(accountId, $"Started cleaning old campaigns data for {dateRange.ToString()}");
            SafeContextWrapper.TryMakeTransactionWithLock((ClientPortalProgContext db) =>
            {
                db.FbCampaignSummaries.Where(x => (x.Date >= dateRange.FromDate && x.Date <= dateRange.ToDate)
                    && x.Campaign.AccountId == accountId).DeleteFromQuery();
            }, lockObj, "BulkDeleteByQuery");
        }

        private void LoadLatestSummariesToDb(List<FbCampaignSummary> summaries)
        {
            Logger.Info(accountId, $"Started loading campaigns summaries data for {dateRange.ToString()}");
            SafeContextWrapper.TryMakeTransactionWithLock((ClientPortalProgContext db) =>
            {
                db.BulkInsert(summaries);
            }, lockObj, "BulkInsert");
        }

        private void OnProcessFailedLoading(Exception e, List<FbCampaignSummary> items)
        {
            Logger.Error(accountId, e);
            var exception = GetFailedLoadingException(e, items);
            ProcessFailedLoading?.Invoke(exception);
        }

        private FacebookFailedEtlException GetFailedLoadingException(Exception e, List<FbCampaignSummary> items)
        {
            var fromDate = items.Min(x => x.Date);
            var toDate = items.Max(x => x.Date);
            var fromDateArg = fromDate == default(DateTime) ? null : (DateTime?)fromDate;
            var toDateArg = toDate == default(DateTime) ? null : (DateTime?)toDate;
            var exception = new FacebookFailedEtlException(fromDateArg, toDateArg, accountId, FbStatsTypeAgg.StrategyArg, e);
            return exception;
        }
    }
}
