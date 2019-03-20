using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg.Kochava;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CakeExtracter.Etl.Kochava.Loaders
{
    /// <summary>
    /// Performes operations with kochava items in database.
    /// </summary>
    public class KochavaItemsDbService
    {
        /// <summary>
        /// Bulk inserts kochava items into db table.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <param name="dbEntries">The database entries.</param>
        public virtual void BulkInsertItems(int accountId, List<KochavaItem> dbEntries)
        {
            SafeContextWrapper.TryMakeTransaction<ClientPortalProgContext>((dbContext) =>
            {
                dbContext.BulkInsert(dbEntries);
            }, "BulkInserting");
        }

        /// <summary>
        /// Bulk deletes items from kochava db table.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        public virtual void BulkDeleteItems(int accountId, DateTime fromDate, DateTime toDate)
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