using System;
using System.Collections.Generic;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg.CJ;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA.CommissionJunctionLoaders
{
    /// <summary>
    /// Commission junction advertiser loader.
    /// </summary>
    /// <seealso cref="CakeExtracter.Etl.Loader{DirectAgents.Domain.Entities.CPProg.CJ.CjAdvertiserCommission}" />
    internal class CommissionJunctionAdvertiserLoader : Loader<CjAdvertiserCommission>
    {
        private static object CjLocker = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="CommissionJunctionAdvertiserLoader"/> class.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        public CommissionJunctionAdvertiserLoader(int accountId) : base(accountId)
        {
        }

        /// <summary>
        /// Loads the specified items.
        /// </summary>
        /// <param name="items">Count of loaded items.</param>
        /// <returns></returns>
        protected override int Load(List<CjAdvertiserCommission> items)
        {
            try
            {
                SafeContextWrapper.TryMakeTransactionWithLock<ClientPortalProgContext>((dbContext) =>
                {
                    dbContext.BulkInsert(items, options => options.IncludeGraph = true); // includes loading child(CjAdvertiserCommissionItem) items.
                }, CjLocker, "BulkInserting");
                return items.Count;
            }
            catch (Exception ex)
            {
                Logger.Error(accountId, ex);
                return 0;
            }
        }
    }
}
