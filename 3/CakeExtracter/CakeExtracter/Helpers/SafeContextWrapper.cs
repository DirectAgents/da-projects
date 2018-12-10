using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Polly;

namespace CakeExtracter.Helpers
{
    internal static class SafeContextWrapper
    {
        public static object ActionTypeLocker = new object();
        public static object MetricTypeLocker = new object();
        public static object EntityTypeLocker = new object();
        public static object StrategyLocker = new object();
        public static object AdSetLocker = new object();
        public static object AdLocker = new object();
        public static object KeywordLocker = new object();
        public static object SearchTermLocker = new object();

        public static int SaveChangedContext<T>(object contextLocker, Action<T> changeContextAction)
            where T : DbContext, new()
        {
            lock (contextLocker)
            {
                using (var dbContext = new T())
                {
                    changeContextAction(dbContext);
                    return TrySaveChanges(dbContext, contextLocker, changeContextAction);
                }
            }
        }

        public static int SaveChangedContext<T>(object contextLocker, T dbContext, Action changeContextAction)
            where T : DbContext, new()
        {
            lock (contextLocker)
            {
                changeContextAction();
                return TrySaveChanges(dbContext, contextLocker, db =>
                {
                    changeContextAction();
                });
            }
        }

        public static void Lock(object contextLocker, Action changeContextAction)
        {
            lock (contextLocker)
            {
                changeContextAction();
            }
        }
    }
}
