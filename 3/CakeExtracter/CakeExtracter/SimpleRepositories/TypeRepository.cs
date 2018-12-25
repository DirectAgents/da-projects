using CakeExtracter.Helpers;
using CakeExtracter.SimpleRepositories.Interfaces;
using DirectAgents.Domain.Entities.CPProg;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

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
