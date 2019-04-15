using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CakeExtracter.Helpers;
using CakeExtracter.SimpleRepositories.RepositoriesWithStorage.Interfaces;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.SimpleRepositories.RepositoriesWithStorage
{
    /// <inheritdoc />
    /// <summary>
    /// The repository for working with Type objects in the database.
    /// </summary>
    public class TypeRepositoryWithStorage : IRepositoryWithStorage<EntityType, ClientPortalProgContext>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="TypeRepositoryWithStorage"/>.
        /// </summary>
        /// <param name="storage">The storage for entity IDs.</param>
        public TypeRepositoryWithStorage(EntityIdStorage<EntityType> storage)
        {
            IdStorage = storage;
        }

        /// <inheritdoc />
        public EntityIdStorage<EntityType> IdStorage { get; }

        /// <inheritdoc />
        public List<EntityType> GetItems(ClientPortalProgContext db, EntityType itemToCompare)
        {
            var items = db.Set<EntityType>().Where(x => x.Name == itemToCompare.Name);
            return items.ToList();
        }

        /// <inheritdoc />
        public EntityType AddItem(ClientPortalProgContext db, EntityType sourceItem)
        {
            db.Set<EntityType>().Add(sourceItem);
            var numChanges = SafeContextWrapper.TrySaveChanges(db);
            if (numChanges == 0)
            {
                return null;
            }

            IdStorage.AddEntityIdToStorage(sourceItem);
            return sourceItem;
        }

        /// <inheritdoc />
        public int UpdateItem(ClientPortalProgContext db, EntityType sourceItem, EntityType targetItemInDb)
        {
            targetItemInDb.Name = sourceItem.Name;
            var numChanges = SafeContextWrapper.TrySaveChanges(db);
            if (numChanges > 0)
            {
                IdStorage.AddEntityIdToStorage(targetItemInDb);
            }

            return numChanges;
        }

        /// <inheritdoc />
        public void AddItems(IEnumerable<EntityType> items)
        {
            using (var db = new ClientPortalProgContext())
            {
                AddItems(db, items);
            }
        }

        /// <inheritdoc />
        public void AddItems(ClientPortalProgContext db, IEnumerable<EntityType> items)
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

            newTypes.ForEach(IdStorage.AddEntityIdToStorage);
        }

        private void AddNewDbTypeToList(DbContext db, EntityType sourceType, List<EntityType> newTypes)
        {
            if (IdStorage.IsEntityInStorage(sourceType))
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
                IdStorage.AddEntityIdToStorage(typeInDb);
            }
        }
    }
}
