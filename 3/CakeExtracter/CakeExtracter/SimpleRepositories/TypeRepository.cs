using CakeExtracter.Helpers;
using CakeExtracter.SimpleRepositories.Interfaces;
using DirectAgents.Domain.Entities.CPProg;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DirectAgents.Domain.Contexts;

namespace CakeExtracter.SimpleRepositories
{
    public class TypeRepository : ISimpleRepository<EntityType>
    {
        private static readonly EntityIdStorage<EntityType> TypeStorage;

        static TypeRepository()
        {
            TypeStorage = new EntityIdStorage<EntityType>(x => x.Id, x => x.Name);
        }

        public EntityIdStorage<EntityType> IdStorage => TypeStorage;

        public List<EntityType> GetItems<TContext>(TContext db, EntityType itemToCompare) where TContext : DbContext, new()
        {
            var items = db.Set<EntityType>().Where(x => x.Name == itemToCompare.Name);
            return items.ToList();
        }

        public EntityType AddItem<TContext>(TContext db, EntityType sourceItem) where TContext : DbContext, new()
        {
            db.Set<EntityType>().Add(sourceItem);
            var numChanges = SafeContextWrapper.TrySaveChanges(db);
            if (numChanges == 0)
            {
                return null;
            }

            TypeStorage.AddEntityIdToStorage(sourceItem);
            return sourceItem;
        }

        public int UpdateItem<TContext>(TContext db, EntityType sourceItem, EntityType targetItemInDb) where TContext : DbContext, new()
        {
            targetItemInDb.Name = sourceItem.Name;
            var numChanges = SafeContextWrapper.TrySaveChanges(db);
            if (numChanges > 0)
            {
                TypeStorage.AddEntityIdToStorage(targetItemInDb);
            }

            return numChanges;
        }

        public void AddItems(IEnumerable<EntityType> items)
        {
            using (var db = new ClientPortalProgContext())
            {
                AddItems(db, items);
            }
        }

        public void AddItems<TContext>(TContext db, IEnumerable<EntityType> items) 
            where TContext : DbContext, new()
        {
            var newTypes = new List<EntityType>();

            SafeContextWrapper.SaveChangedContext(
                SafeContextWrapper.EntityTypeLocker, db, () =>
                {
                    foreach (var type in items)
                    {
                        AddNewDbTypeToList(db, type, newTypes);
                    }

                    db.Set<EntityType>().AddRange(newTypes);
                }
            );

            newTypes.ForEach(TypeStorage.AddEntityIdToStorage);
        }

        private void AddNewDbTypeToList(DbContext db, EntityType sourceType, List<EntityType> newTypes)
        {
            if (TypeStorage.IsEntityInStorage(sourceType))
            {
                return;
            }

            var typeInDb = db.Set<EntityType>().FirstOrDefault(x => sourceType.Name == x.Name);
            if (typeInDb == null)
            {
                newTypes.Add(sourceType);
            }
            else
            {
                TypeStorage.AddEntityIdToStorage(typeInDb);
            }
        }
    }
}
