using System;
using System.Collections.Generic;

namespace CakeExtracter.Etl.TradingDesk.Helpers
{
    public class EntityIdStorage<T>
    {
        private readonly Dictionary<string, int> ids = new Dictionary<string, int>();

        private readonly Func<T, int> getIdFunc;
        private readonly Func<T, string> getCompositeKeyFunc;

        public EntityIdStorage(Func<T, int> getIdFunc, Func<T, string> getCompositeKeyFunc)
        {
            this.getIdFunc = getIdFunc;
            this.getCompositeKeyFunc = getCompositeKeyFunc;
        }

        public void AddEntityIdToStorage(T item)
        {
            var key = getCompositeKeyFunc(item);
            var id = getIdFunc(item);
            if (ids.ContainsKey(key))
            {
                ids[key] = id;
            }
            else
            {
                ids.Add(key, id);
            }
        }

        public int GetEntityIdFromStorage(T item)
        {
            var key = getCompositeKeyFunc(item);
            return ids[key];
        }

        public bool IsEntityInStorage(T item)
        {
            var key = getCompositeKeyFunc(item);
            return ids.ContainsKey(key);
        }
    }
}
