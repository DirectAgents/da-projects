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
        /// <param name="sinceDate">Since date.</param>
        /// <param name="beforeDate">Before date.</param>
        public void CleanCommissionJunctionInfo(int accountId, DateRangeType dateRangeType, DateTime sinceDate, DateTime beforeDate)
        {
            Logger.Info(accountId, "The cleaning of CommissionJunction for account ({0}) has begun (since {1} - before {2} ({3}))",
                accountId, sinceDate.ToShortDateString(), beforeDate.ToShortDateString(), dateRangeType);
            SafeContextWrapper.TryMakeTransaction((ClientPortalProgContext db) =>
            {
                switch (dateRangeType)
                {
                    case DateRangeType.Event:
                        db.CjAdvertiserCommissions.Where(x => (x.EventDate >= sinceDate && x.EventDate < beforeDate) && x.AccountId == accountId).DeleteFromQuery();
                        break;
                    case DateRangeType.Posting:
                        db.CjAdvertiserCommissions.Where(x => (x.PostingDate >= sinceDate && x.PostingDate < beforeDate) && x.AccountId == accountId).DeleteFromQuery();
                        break;
                }
            }, "DeleteFromQuery");
            Logger.Info(accountId, "The cleaning of CommissionJunction for account ({0}) is over (since {1} - before {2} ({3}))",
                accountId, sinceDate.ToShortDateString(), beforeDate.ToShortDateString(), dateRangeType);
        }
    }
}
