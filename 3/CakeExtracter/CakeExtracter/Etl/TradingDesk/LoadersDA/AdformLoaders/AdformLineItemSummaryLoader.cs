using CakeExtracter.Common;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA.AdformLoaders
{
    /// <summary>
    /// Line item summary loader for Adform ETL:
    /// uses AdSetWithoutEidStorage and StrategyWithoutEidStorage, because Adform entities do not have portal IDs.
    /// </summary>
    internal class AdformLineItemSummaryLoader : TDAdSetSummaryLoader
    {
        public AdformLineItemSummaryLoader(int accountId = -1)
            : base(accountId, StorageCollection.AdSetWithoutEidStorage, StorageCollection.StrategyWithoutEidStorage)
        {

        }
    }
}
