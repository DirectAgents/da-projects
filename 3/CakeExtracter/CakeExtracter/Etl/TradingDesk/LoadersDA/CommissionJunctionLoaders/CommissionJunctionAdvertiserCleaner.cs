using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using System;
using System.Linq;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA.CommissionJunctionLoaders
{
    /// <summary>
    /// Commission Junction Info cleaner
    /// </summary>
    public class CommissionJunctionAdvertiserCleaner
    {
        /// <summary>
        /// Cleans the commission junction information from DataBase.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        public void CleanCommissionJunctionInfo(int accountId, DateTime fromDate, DateTime toDate)
        {
            Logger.Info(accountId, "The cleaning of ComissionJunction for account ({0}) has begun - {1} - {2}", accountId, fromDate, toDate);
            SafeContextWrapper.TryMakeTransaction((ClientPortalProgContext db) =>
            {
                db.CjAdvertiserCommissions.Where(x => (x.EventDate >= fromDate && x.EventDate <= toDate) && x.AccountId == accountId)
                .DeleteFromQuery();
            }, "DeleteFromQuery");
            Logger.Info(accountId, "The cleaning of ComissionJunction for account ({0}) is over - {1} - {2}", accountId, fromDate, toDate);
        }
    }
}
