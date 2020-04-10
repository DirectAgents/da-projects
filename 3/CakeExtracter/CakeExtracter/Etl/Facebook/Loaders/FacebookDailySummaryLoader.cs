using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Commands;
using CakeExtracter.Common;
using CakeExtracter.Etl.Facebook.Exceptions;
using CakeExtracter.Etl.Facebook.Interfaces;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg.Facebook.Daily;

namespace CakeExtracter.Etl.Facebook.Loaders
{
    /// <summary>
    /// Facebook Daily summary loader
    /// </summary>
    /// <seealso cref="FbDailySummary" />
    public class FacebookDailySummaryLoader : Loader<FbDailySummary>, IFacebookLoadingErrorHandler
    {
        private readonly DateRange dateRange;

        private static object lockObj = new object();

        private List<FbDailySummary> latestSummaries = new List<FbDailySummary>();

        /// <inheritdoc/>
        public event Action<FacebookFailedEtlException> ProcessFailedLoading;

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookDailySummaryLoader"/> class.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <param name="dateRange">The date range.</param>
        public FacebookDailySummaryLoader(int accountId, DateRange dateRange)
            : base(accountId)
        {
            this.dateRange = dateRange;
        }

        /// <summary>
        /// Loads the specified items.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        protected override int Load(List<FbDailySummary> items)
        {
            try
            {
                latestSummaries.AddRange(items);
                return items.Count;
            }
            catch (Exception ex)
            {
                OnProcessFailedLoading(ex, items);
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
                if (!latestSummaries.Any())
                {
                    return;
                }
                DeleteOldSummariesFromDb();
                LoadLatestSummariesToDb(latestSummaries);
            }
            catch (Exception ex)
            {
                Logger.Error(accountId, ex);
            }
        }

        private void LoadSummaries(List<FbDailySummary> summaries)
        {
            latestSummaries.AddRange(summaries);
        }

        private void DeleteOldSummariesFromDb()
        {
            Logger.Info(accountId, $"Started cleaning old daily data for {dateRange.ToString()}");
            SafeContextWrapper.TryMakeTransactionWithLock((ClientPortalProgContext db) =>
            {
                db.FbDailySummaries.Where(x => (x.Date >= dateRange.FromDate && x.Date <= dateRange.ToDate)
                    && x.AccountId == accountId).DeleteFromQuery();
            }, lockObj, "BulkDeleteByQuery");
        }

        private void LoadLatestSummariesToDb(List<FbDailySummary> summaries)
        {
            Logger.Info(accountId, $"Started loading daily summaries data for {dateRange.ToString()}");
            SafeContextWrapper.TryMakeTransactionWithLock((ClientPortalProgContext db) =>
            {
                db.BulkInsert(summaries);
            }, lockObj, "BulkInsert");
        }

        private void OnProcessFailedLoading(Exception e, List<FbDailySummary> items)
        {
            Logger.Error(accountId, e);
            var exception = GetFailedLoadingException(e, items);
            ProcessFailedLoading?.Invoke(exception);
        }

        private FacebookFailedEtlException GetFailedLoadingException(Exception e, List<FbDailySummary> items)
        {
            var fromDate = items.Min(x => x.Date);
            var toDate = items.Max(x => x.Date);
            var fromDateArg = fromDate == default(DateTime) ? null : (DateTime?)fromDate;
            var toDateArg = toDate == default(DateTime) ? null : (DateTime?)toDate;
            var exception = new FacebookFailedEtlException(fromDateArg, toDateArg, accountId, FbStatsTypeAgg.DailyArg, e);
            return exception;
        }
    }
}
