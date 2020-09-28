using System;
using System.Collections.Generic;

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
                return 0;
            }
        }
    }
}