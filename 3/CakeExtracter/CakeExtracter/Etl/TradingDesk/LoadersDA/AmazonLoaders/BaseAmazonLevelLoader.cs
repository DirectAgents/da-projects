using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA.AmazonLoaders
{
    public abstract class BaseAmazonLevelLoader<TLevelEntity> : Loader<TLevelEntity>
        where TLevelEntity : DatedStatsSummary
    {
        private const int loadingBatchesSize = 500;

        protected BaseAmazonLevelLoader(int accountId)
            : base(accountId, loadingBatchesSize)
        {
        }
    }
}
