using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CakeExtracter.Etl.TradingDesk.LoadersDA;
using CakeExtracter.Helpers;
using CakeExtracter.Logging.TimeWatchers.Amazon;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.Amazon.Loaders.EntitiesLoaders
{
    public class AmazonStrategyLoader : AmazonBaseEntityLoader<Strategy>
    {
        protected override object LockerObject => SafeContextWrapper.StrategyLocker;

        protected override string LevelName => AmazonJobLevels.Strategy;

        private readonly TDStrategySummaryLoader summaryItemsLoader;

        public AmazonStrategyLoader(int accountId)
            : base(accountId)
        {
            summaryItemsLoader = new TDStrategySummaryLoader(accountId);
        }

        public void MergeDependentStrategies(List<StrategySummary> items)
        {
            PrepareData(items);
            AddUpdateDependentStrategies(items);
            items.ForEach(SetSummaryParents);
        }

        public void SetSummaryParents(StrategySummary summary)
        {
            summary.StrategyId = summary.Strategy.Id;
        }

        private void PrepareData(List<StrategySummary> items)
        {
            items.ForEach(x =>
            {
                if (x.Strategy == null)
                {
                    x.Strategy = new Strategy();
                }
                x.Strategy.AccountId = accountId;
                Mapper.Map(x, x.Strategy);
            });
        }

        private void AddUpdateDependentStrategies(IEnumerable<StrategySummary> items)
        {
            var strategies = items.Select(x => x.Strategy).ToList();
            AddUpdateDependentStrategies(strategies);
        }

        private void AddUpdateDependentStrategies(IEnumerable<Strategy> items)
        {
            var strategies = GetUniqueEntities(items);
            summaryItemsLoader.AddDependentStrategyTypes(new ClientPortalProgContext(), strategies);
            MergeItems(strategies, options => options.ColumnPrimaryKeyExpression = x => x.ExternalId);
            SetEntityDatabaseIds(items, strategies);
            LogMergedEntities(items);
        }

        private IEnumerable<Strategy> GetUniqueEntities(IEnumerable<Strategy> items)
        {
            var notNullableItems = items.Where(x => x != null).ToList();
            notNullableItems.ForEach(x => x.AccountId = accountId);
            var entities = notNullableItems
                .Where(x => !string.IsNullOrWhiteSpace(x.Name) || !string.IsNullOrWhiteSpace(x.ExternalId))
                .GroupBy(x => new { x.AccountId, x.Name, x.ExternalId })
                .Select(x => x.First())
                .ToList();
            return entities;
        }

        private static void SetEntityDatabaseIds(IEnumerable<Strategy> items, IEnumerable<Strategy> dbEntities)
        {
            foreach (var item in items)
            {
                var dbEntity = dbEntities.FirstOrDefault(x => item.ExternalId == x.ExternalId);
                if (dbEntity != null)
                {
                    item.Id = dbEntity.Id;
                }
            }
        }
    }
}
