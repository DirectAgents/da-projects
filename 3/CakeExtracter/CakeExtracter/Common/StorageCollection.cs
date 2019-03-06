using CakeExtracter.Helpers;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Common
{
    internal static class StorageCollection
    {
        public static EntityIdStorage<EntityType> TypeStorage = new EntityIdStorage<EntityType>(x => x.Id, 
            x => x.Name);

        public static EntityIdStorage<ActionType> ActionTypeStorage = new EntityIdStorage<ActionType>(x => x.Id,
            x => x.Code);

        public static EntityIdStorage<MetricType> MetricTypeStorage = new EntityIdStorage<MetricType>(x => x.Id, 
            x => $"{x.Name} {x.DaysInterval}");

        public static EntityIdStorage<Strategy> StrategyWithEidStorage = new EntityIdStorage<Strategy>(x => x.Id,
            x => $"{x.AccountId} {x.Name} {x.ExternalId}",
            x => $"{x.AccountId} {x.ExternalId}");

        public static EntityIdStorage<Strategy> StrategyWithoutEidStorage = new EntityIdStorage<Strategy>(x => x.Id,
            x => (x.ExternalId == null && x.Name == null) ? null : $"{x.AccountId} {x.Name} {x.ExternalId}",
            x => (x.ExternalId == null) ? null : $"{x.AccountId} {x.ExternalId}",
            x => (x.Name == null) ? null : $"{x.AccountId} {x.Name}");

        public static EntityIdStorage<AdSet> AdSetWithEidStorage = new EntityIdStorage<AdSet>(x => x.Id,
            x => $"{x.AccountId} {x.StrategyId} {x.Name} {x.ExternalId}",
            x => $"{x.AccountId} {x.Name} {x.ExternalId}");

        public static EntityIdStorage<AdSet> AdSetWithoutEidStorage = new EntityIdStorage<AdSet>(x => x.Id,
            x => (x.ExternalId == null && x.Name == null) ? null : $"{x.AccountId} {x.StrategyId} {x.Name} {x.ExternalId}",
            x => (x.ExternalId == null) ? null : $"{x.AccountId} {x.StrategyId} {x.ExternalId}",
            x => (x.Name == null) ? null : $"{x.AccountId} {x.StrategyId} {x.Name}");

        public static EntityIdStorage<TDad> TDadStorage = new EntityIdStorage<TDad>(x => x.Id,
            x => $"{x.AccountId} {x.AdSetId} {x.Name} {x.ExternalId}", 
            x => $"{x.AccountId} {x.Name} {x.ExternalId}");

        public static EntityIdStorage<Keyword> KeywordStorage = new EntityIdStorage<Keyword>(x => x.Id, 
            x => $"{x.AccountId} {x.AdSetId} {x.StrategyId} {x.Name} {x.ExternalId}", 
            x => $"{x.AccountId} {x.Name} {x.ExternalId}");

        public static EntityIdStorage<SearchTerm> SearchTermStorage = new EntityIdStorage<SearchTerm>(x => x.Id, 
            x => $"{x.AccountId} {x.Name} {x.KeywordId}");
    }
}
