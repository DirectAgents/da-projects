using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;

namespace CakeExtracter.Etl.TradingDesk.Helpers
{
    public class EntityIdStorage<T>
    {
        private readonly Dictionary<string, int> ids = new Dictionary<string, int>();

        private readonly Func<T, int> getIdFunc;
        private readonly Func<T, string>[] getCompositeKeyFunctions;

        public EntityIdStorage(Func<T, int> getIdFunc, params Func<T, string>[] getCompositeKeyFunctions)
        {
            this.getIdFunc = getIdFunc;
            this.getCompositeKeyFunctions = getCompositeKeyFunctions;
        }

        public void AddEntityIdToStorage(T item)
        {
            var id = getIdFunc(item);
            getCompositeKeyFunctions.ForEach(f => AddEntityIdToStorage(id, f(item)));
        }

        public int GetEntityIdFromStorage(T item)
        {
            var key = getCompositeKeyFunctions
                .Select(f => f(item))
                .FirstOrDefault(x => x != null && ids.ContainsKey(x));
            return key == null ? 0 : ids[key];
        }

        public bool IsEntityInStorage(T item)
        {
            return item != null && getCompositeKeyFunctions.Any(func => IsEntityInStorage(func(item)));
        }

        private void AddEntityIdToStorage(int id, string key)
        {
            if (ids.ContainsKey(key))
            {
                ids[key] = id;
            }
            else
            {
                ids.Add(key, id);
            }
        }

        private int GetEntityIdFromStorage(string key)
        {
            return !ids.ContainsKey(key) ? 0 : ids[key];
        }

        private bool IsEntityInStorage(string key)
        {
            return key != null && ids.ContainsKey(key);
        }
    }
}
