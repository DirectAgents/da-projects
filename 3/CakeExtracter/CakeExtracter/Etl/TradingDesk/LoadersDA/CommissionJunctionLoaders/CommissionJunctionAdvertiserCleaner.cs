using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using System;
using System.Linq;
using CommissionJunction.Enums;

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
        public void CleanCommissionJunctionInfo(int accountId, DateRangeType dateRangeType, DateTime fromDate, DateTime toDate)
        {
            Logger.Info(accountId, "The cleaning of ComissionJunction for account ({0}) has begun - {1} - {2} ({3})", accountId, fromDate, toDate, dateRangeType);
            SafeContextWrapper.TryMakeTransaction((ClientPortalProgContext db) =>
            {
                switch (dateRangeType)
                {
                    case DateRangeType.Event:
                        db.CjAdvertiserCommissions.Where(x => (x.EventDate >= fromDate && x.EventDate <= toDate) && x.AccountId == accountId).DeleteFromQuery();
                        break;
                    case DateRangeType.Posting:
                        db.CjAdvertiserCommissions.Where(x => (x.PostingDate >= fromDate && x.PostingDate <= toDate) && x.AccountId == accountId).DeleteFromQuery();
                        break;
                }
            }, "DeleteFromQuery");
            Logger.Info(accountId, "The cleaning of ComissionJunction for account ({0}) is over - {1} - {2} ({3})", accountId, fromDate, toDate, dateRangeType);
        }
    }
}
