using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Etl.AmazonAttribution.Exceptions;
using CakeExtracter.Helpers;
using CakeExtracter.Logging.TimeWatchers.Amazon;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg.AmazonAttribution;

namespace CakeExtracter.Etl.AmazonAttribution.Loaders
{
    public class AmazonAttributionLoader : Loader<AttributionSummary>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonAttributionLoader"/> class.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// 
        /// /// <summary>
        /// Action for exception of failed loading.
        /// </summary>
        public event Action<AttributionFailedEtlException> ProcessFailedLoading;

        public AmazonAttributionLoader(int accountId)
            : base(accountId)
        {
        }

        /// <inheritdoc/>
        protected override int Load(List<AttributionSummary> items)
        {
            try
            {
                AmazonTimeTracker.Instance.ExecuteWithTimeTracking(
                    () =>
                        {
                            SafeContextWrapper.TryMakeTransaction(
                                (ClientPortalProgContext db) => db.BulkMerge(items),
                                "BulkMerge");
                        },
                    accountId,
                    "Attribution",
                    AmazonJobOperations.LoadSummaryItemsData);
                return items.Count;
            }
            catch (Exception e)
            {
                Logger.Error(accountId, e);
                ProcessFailedStatsLoading(e, items);
                return 0;
            }
        }

        private void ProcessFailedStatsLoading(Exception e, List<AttributionSummary> items)
        {
            Logger.Error(e);
            var exception = GetFailedStatsLoadingException(e, items);
            ProcessFailedLoading?.Invoke(exception);
        }

        private AttributionFailedEtlException GetFailedStatsLoadingException(Exception e, List<AttributionSummary> items)
        {
            var fromDate = items.Min(x => x.Date);
            var toDate = items.Max(x => x.Date);
            var fromDateArg = fromDate == default(DateTime) ? null : (DateTime?)fromDate;
            var toDateArg = toDate == default(DateTime) ? null : (DateTime?)toDate;
            var exception = new AttributionFailedEtlException(fromDateArg, toDateArg, accountId, e);
            return exception;
        }
    }
}