using System;
using System.Data.Entity;
using System.Linq;

namespace CakeExtracter.Common
{
    static class EntityFrameworkExtensions
    {
        public static T FindOrCreateByKey<T>(this DbContext context, object pk, Func<T> createNew) where T : class
        {
            T result;
            var set = context.Set<T>();
            result = set.Find(pk);
            if (result == null)
            {
                result = createNew();
                set.Add(result);
            }
            return result;
        }

        public static T FindOrCreateByPredicate<T>(this ClientPortal.Data.Contexts.ClientPortalDWContext context, Func<T, bool> predicate, Func<T> createNew) where T : class
        {
            T result;
            var set = context.Set<T>();
            result = set.FirstOrDefault(predicate);
            if (result == null)
            {
                result = createNew();
                set.Add(result);
            }
            return result;
        }
    }
}
