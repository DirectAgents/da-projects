using System;
using CakeExtracter.Common;
using CakeExtracter.Logging.TimeWatchers;
using CakeExtracter.Logging.TimeWatchers.Amazon;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Etl.Amazon.Exceptions;
using CakeExtracter.Helpers;

namespace CakeExtracter.Etl.TradingDesk.Extracters.AmazonExtractors.AmazonApiExtractors
{
    /// <summary>
    /// Daily data extractor for amazon job based on database keywords data.
    /// </summary>
    /// <seealso cref="CakeExtracter.Etl.TradingDesk.Extracters.DatabaseKeywordsToDailySummaryExtracter" />
    public class AmazonDatabaseKeywordsToDailySummaryExtracter : DatabaseKeywordsToDailySummaryExtractor
    {

        public event Action<FailedStatsLoadingException> ProcessFailedExtraction;

        public AmazonDatabaseKeywordsToDailySummaryExtracter(DateRange dateRange, int accountId)
            :base(dateRange, accountId)
        {
        }

        public virtual void RemoveOldData(DateRange dateRange)
        {
            Logger.Info(accountId, "The cleaning of DailySummaries for account ({0}) has begun - {1}.", accountId, dateRange);
            AmazonTimeTracker.Instance.ExecuteWithTimeTracking(() =>
            {
                SafeContextWrapper.TryMakeTransaction((ClientPortalProgContext db) =>
                {
                    db.DailySummaryMetrics.Where(x => x.EntityId == accountId && (x.Date >= dateRange.FromDate && x.Date <= dateRange.ToDate)).DeleteFromQuery();
                    db.DailySummaries.Where(x => x.AccountId == accountId && (x.Date >= dateRange.FromDate && x.Date <= dateRange.ToDate)).DeleteFromQuery();
                }, "DeleteFromQuery");
            }, accountId, AmazonJobLevels.account, AmazonJobOperations.cleanExistingData);
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
                RemoveOldData(dateRange);
                IEnumerable<DailySummary> items = null;
                AmazonTimeTracker.Instance.ExecuteWithTimeTracking(() => { items = GetDailySummaryDataFromDataBase(); },
                    accountId, AmazonJobLevels.account, AmazonJobOperations.reportExtracting);
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
