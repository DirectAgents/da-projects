using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Common;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA.AdformLoaders
{
    /// <summary>
    /// Line item summary loader for Adform ETL:
    /// uses AdformAdSetWithoutEidStorage and AdformStrategyWithoutEidStorage, because Adform entities do not have portal IDs.
    /// </summary>
    internal class AdformLineItemSummaryLoader : TDAdSetSummaryLoader
    {
        public AdformLineItemSummaryLoader(int accountId = -1)
            : base(accountId, StorageCollection.AdformAdSetWithoutEidStorage, StorageCollection.AdformStrategyWithoutEidStorage)
        {

        }

        protected override List<AdSet> GetAdSets(ClientPortalProgContext db, AdSet adSet)
        {
            var adSetsInDb = db.AdSets.Where(x => x.AccountId == adSet.AccountId && x.Name == adSet.Name);
            return adSetsInDb.ToList();
        }
    }
}
