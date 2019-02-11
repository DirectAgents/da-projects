using CakeExtracter.Helpers;
using DirectAgents.Domain.Entities.CPProg;
using CakeExtracter.SimpleRepositories;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA
{
    /// <summary>
    /// Campaign (strategy) summary loader for Adform ETL:
    /// uses Adform Strategy Repository instead of the simple Strategy Repository,
    /// because it needs other keys in Entity Storage
    /// </summary>
    class AdformCampaignSummaryLoader : TDStrategySummaryLoader
    {
        public static readonly EntityIdStorage<Strategy> StrategyStorage;
        
        static AdformCampaignSummaryLoader()
        {
            StrategyStorage = new AdformStrategyRepository().IdStorage;
        }

        public AdformCampaignSummaryLoader(int accountId = -1)
            : base(accountId, new AdformStrategyRepository())
        {
        }
    }
}
