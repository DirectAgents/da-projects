using CakeExtracter.Helpers;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.SimpleRepositories
{
    /// <summary>
    /// Adform Strategy Repository for Adform ETL:
    /// uses instead of the simple Strategy Repository,
    /// because it needs other keys when working with Entity Storage
    /// </summary>
    class AdformStrategyRepository : StrategyRepository
    {
        internal AdformStrategyRepository()
        {
            StrategyStorage = new EntityIdStorage<Strategy>(x => x.Id,
                x => (x.ExternalId == null && x.Name == null) ? null : $"{x.AccountId} {x.Name} {x.ExternalId}",
                x => (x.ExternalId == null) ? null : $"{x.AccountId} {x.ExternalId}",
                x => (x.Name == null) ? null : $"{x.AccountId} {x.Name}");
        }
    }
}
