using System.Collections.Generic;
using System.Data.Entity;
using CakeExtracter.Helpers;

namespace CakeExtracter.SimpleRepositories.Interfaces
{
    public interface ISimpleRepository<T>
    {
        EntityIdStorage<T> IdStorage { get; }

        List<T> GetItems<TContext>(TContext db, T itemToCompare)
            where TContext : DbContext, new();

        T AddItem<TContext>(TContext db, T sourceItem)
            where TContext : DbContext, new();

        int UpdateItem<TContext>(TContext db, T sourceItem, T targetItemInDb)
            where TContext : DbContext, new();

        void AddItems(IEnumerable<T> items);

        void AddItems<TContext>(TContext db, IEnumerable<T> items)
            where TContext: DbContext, new();
    }
}
