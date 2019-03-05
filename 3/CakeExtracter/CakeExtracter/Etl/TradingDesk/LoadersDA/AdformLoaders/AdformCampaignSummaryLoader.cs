using CakeExtracter.Common;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA.AdformLoaders
{
    /// <summary>
    /// Campaign (strategy) summary loader for Adform ETL:
    /// uses StrategyWithoutEidStorage instead of the StrategyWithEidStorage, because Adform entities do not have portal IDs.
    /// </summary>
    internal class AdformCampaignSummaryLoader : TDStrategySummaryLoader
    {
        public AdformCampaignSummaryLoader(int accountId = -1)
            : base(accountId, StorageCollection.StrategyWithoutEidStorage)
        {

        }
    }
}
