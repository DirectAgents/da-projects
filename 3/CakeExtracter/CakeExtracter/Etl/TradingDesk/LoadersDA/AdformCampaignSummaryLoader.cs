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
        public AdformCampaignSummaryLoader(int accountId = -1)
            : base(accountId, new AdformStrategyRepository())
        {

        }
    }
}
