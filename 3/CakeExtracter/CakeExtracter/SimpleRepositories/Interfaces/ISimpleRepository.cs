using System.Collections.Generic;
using System.Data.Entity;
using CakeExtracter.Helpers;

namespace CakeExtracter.SimpleRepositories.Interfaces
{
    public interface ISimpleRepository<T>
    {
        EntityIdStorage<T> IdStorage { get; }

        void AddItems<TContext>(TContext db, IEnumerable<T> items)
            where TContext: DbContext, new();
    }
}
