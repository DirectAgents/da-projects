using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using System;
using System.Linq;

namespace CakeExtracter.Etl.Kochava.Loaders
{
    /// <summary>
    /// Kochava cleaner.
    /// </summary>
    public class KochavaCleaner
    {
        /// <summary>
        /// Cleans the kochava data for date range and account.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        public void CleanKochavaDataForDateRange(int accountId, DateTime fromDate, DateTime toDate)
        {
            Logger.Info(accountId, "The cleaning of Kochava Items for account ({0}) has begun - {1} - {2}", accountId, fromDate, toDate);
            SafeContextWrapper.TryMakeTransaction((ClientPortalProgContext db) =>
            {
                db.KochavaItems.Where(x => (x.Date >= fromDate && x.Date <= toDate) && x.AccountId == accountId)
                .DeleteFromQuery();
            }, "DeleteFromQuery");
            Logger.Info(accountId, "The cleaning of Kochava Items for account ({0}) is over - {1} - {2}", accountId, fromDate, toDate);
        }
    }
}
