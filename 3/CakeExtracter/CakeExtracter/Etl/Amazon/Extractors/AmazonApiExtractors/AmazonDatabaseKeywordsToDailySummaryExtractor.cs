using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Common;
using CakeExtracter.Etl.Amazon.Exceptions;
using CakeExtracter.Etl.TradingDesk.Extracters;
using CakeExtracter.Helpers;
using CakeExtracter.Logging.TimeWatchers.Amazon;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.Amazon.Extractors.AmazonApiExtractors
{
    /// <inheritdoc />
    /// <summary>
    /// Daily data extractor for amazon job based on database keywords data.
    /// </summary>
    /// <seealso cref="DatabaseKeywordsToDailySummaryExtractor" />
    public class AmazonDatabaseKeywordsToDailySummaryExtractor : DatabaseKeywordsToDailySummaryExtractor
    {
        public event Action<FailedStatsLoadingException> ProcessFailedExtraction;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonDatabaseKeywordsToDailySummaryExtractor"/> class.
        /// </summary>
        /// <param name="dateRange">Extraction date range.</param>
        /// <param name="account">Account ID.</param>
        public AmazonDatabaseKeywordsToDailySummaryExtractor(DateRange dateRange, int accountId)
            : base(dateRange, accountId)
        {
        }

        public virtual void RemoveOldData(DateRange dateRange)
        {
            Logger.Info(accountId, "The cleaning of DailySummaries for account ({0}) has begun - {1}.", accountId, dateRange);
            AmazonTimeTracker.Instance.ExecuteWithTimeTracking(
                () =>
                {
                    SafeContextWrapper.TryMakeTransaction(
                        (ClientPortalProgContext db) =>
                        {
                            db.DailySummaryMetrics
                                .Where(x => x.EntityId == accountId && x.Date >= dateRange.FromDate && x.Date <= dateRange.ToDate)
                                .DeleteFromQuery();
                            db.DailySummaries
                                .Where(x => x.AccountId == accountId && x.Date >= dateRange.FromDate && x.Date <= dateRange.ToDate)
                                .DeleteFromQuery();
                        }, "DeleteFromQuery");
                }, accountId,
                AmazonJobLevels.Account,
                AmazonJobOperations.CleanExistingData);
            Logger.Info(accountId, "The cleaning of DailySummaries for account ({0}) is over - {1}. ", accountId, dateRange);
        }

        /// <summary>
        /// The derived class implements this method, which calls Add() for each item
        /// extracted and then calls End() when complete.
        /// </summary>
        protected override void Extract()
        {
            try
            {
                IEnumerable<DailySummary> items = null;
                AmazonTimeTracker.Instance.ExecuteWithTimeTracking(
                    () =>
                    {
                        items = GetDailySummaryDataFromDataBase();
                    },
                    accountId,
                    AmazonJobLevels.Account,
                    AmazonJobOperations.ReportExtracting);
                Add(items);
            }
            catch (Exception e)
            {
                ProcessFailedStatsExtraction(e);
            }

            End();
        }

        private void ProcessFailedStatsExtraction(Exception e)
        {
            Logger.Error(accountId, e);
            var exception = new FailedStatsLoadingException(dateRange.FromDate, dateRange.ToDate, accountId, e, byDaily: true);
            ProcessFailedExtraction?.Invoke(exception);
        }
    }
}