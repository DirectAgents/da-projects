using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.SimpleRepositories.BaseRepositories.Interfaces;
using DirectAgents.Domain.Entities.CPProg.Adform;
using Z.EntityFramework.Extensions;

namespace CakeExtracter.Etl.Adform.Loaders
{
    internal class AdfMediaTypeLoader //: Loader<AdfMediaType>
    {
        private readonly int accountId;
        private readonly IBaseRepository<AdfMediaType> mediaTypeRepository;

        public AdfMediaTypeLoader(int accountId, IBaseRepository<AdfMediaType> mediaTypeRepository)
           // : base(accountId)
        {
            this.accountId = accountId;
            this.mediaTypeRepository = mediaTypeRepository;
        }

        public bool MergeDependentEntitiesWithExisted(IEnumerable<AdfMediaType> items)
        {
            return MergeDependentEntitiesWithExisted(items, options =>
            {
                options.ColumnPrimaryKeyExpression = x => x.ExternalId;
            });
        }

        public bool MergeDependentEntitiesWithExisted(
            IEnumerable<AdfMediaType> items,
            Action<EntityBulkOperation<AdfMediaType>> entityBulkOptionsAction)
        {
            var entities = GetUniqueEntities(items);
            var result = mediaTypeRepository.MergeItems(entities, entityBulkOptionsAction);
            SetEntityDatabaseIds(items, entities);
            LogMergedEntities(entities, mediaTypeRepository.EntityName);
            return result;
        }

        private static IEnumerable<AdfMediaType> GetUniqueEntities(IEnumerable<AdfMediaType> items)
        {
            var notNullableItems = items.Where(x => x != null).ToList();
            var entities = notNullableItems
                .GroupBy(x => new { x.Name, x.ExternalId })
                .Select(x => x.First())
                .ToList();
            return entities;
        }

        private static void SetEntityDatabaseIds(IEnumerable<AdfMediaType> items, IEnumerable<AdfMediaType> dbEntities)
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

        private void LogMergedEntities(IEnumerable<AdfMediaType> items, string entitiesName)
        {
            Logger.Info(accountId, $"{entitiesName} were merged: {items.Count()}.");
        }
    }
}
