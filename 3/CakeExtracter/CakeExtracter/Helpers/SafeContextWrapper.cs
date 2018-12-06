using System;
using System.Collections.Concurrent;
using System.Data.Entity;
using System.Linq;

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

        private static readonly ConcurrentDictionary<string, object> AdSetActionLockers = new ConcurrentDictionary<string, object>();
        private static readonly ConcurrentDictionary<string, object> StrategyActionLockers = new ConcurrentDictionary<string, object>();
        private static readonly ConcurrentDictionary<string, object> DailySummariesLockers = new ConcurrentDictionary<string, object>();
        private static readonly ConcurrentDictionary<string, object> StrategySummariesLockers = new ConcurrentDictionary<string, object>();
        private static readonly ConcurrentDictionary<string, object> AdSetSummariesLockers = new ConcurrentDictionary<string, object>();
        private static readonly ConcurrentDictionary<string, object> AdSummariesLockers = new ConcurrentDictionary<string, object>();
        private static readonly ConcurrentDictionary<string, object> KeywordSummariesLockers = new ConcurrentDictionary<string, object>();
        private static readonly ConcurrentDictionary<string, object> SearchTermSummariesLockers = new ConcurrentDictionary<string, object>();

        public static int SaveChangedContext<T>(object contextLocker, Action<T> changeContextAction)
            where T : DbContext, new()
        {
            lock (contextLocker)
            {
                using (var dbContext = new T())
                {
                    changeContextAction(dbContext);
                    var numChanges = dbContext.SaveChanges();
                    return numChanges;
                }
            }
        }

        public static int SaveChangedContext<T>(object contextLocker, T dbContext, Action changeContextAction)
            where T : DbContext, new()
        {
            lock (contextLocker)
            {
                changeContextAction();
                var numChanges = dbContext.SaveChanges();
                return numChanges;
            }
        }

        public static void Lock(object contextLocker, Action changeContextAction)
        {
            lock (contextLocker)
            {
                changeContextAction();
            }
        }

        public static object GetDailySummariesLocker(int accountId, DateTime date)
        {
            var key = GetCompletedKey(accountId, date);
            return GetEntitiesLocker(DailySummariesLockers, key);
        }

        public static object GetStrategyActionLocker(int strategyId, DateTime date)
        {
            var key = GetCompletedKey(strategyId, date);
            return GetEntitiesLocker(StrategyActionLockers, key);
        }

        public static object GetStrategySummariesLocker(int strategyId, DateTime date)
        {
            var key = GetCompletedKey(strategyId, date);
            return GetEntitiesLocker(StrategySummariesLockers, key);
        }

        public static object GetAdSetActionLocker(int adSetId, DateTime date)
        {
            var key = GetCompletedKey(adSetId, date);
            return GetEntitiesLocker(AdSetActionLockers, key);
        }

        public static object GetAdSetSummariesLocker(int adSetId, DateTime date)
        {
            var key = GetCompletedKey(adSetId, date);
            return GetEntitiesLocker(AdSetSummariesLockers, key);
        }

        public static object GetAdSummariesLocker(int adId, DateTime date)
        {
            var key = GetCompletedKey(adId, date);
            return GetEntitiesLocker(AdSummariesLockers, key);
        }

        public static object GetKeywordSummariesLocker(int keywordId, DateTime date)
        {
            var key = GetCompletedKey(keywordId, date);
            return GetEntitiesLocker(KeywordSummariesLockers, key);
        }

        public static object GetSearchTermSummariesLocker(int termId, DateTime date)
        {
            var key = GetCompletedKey(termId, date);
            return GetEntitiesLocker(SearchTermSummariesLockers, key);
        }

        private static string GetCompletedKey(params object[] ids)
        {
            var idsAsStr = ids.Select(x => x.ToString());
            var key = string.Join(string.Empty, idsAsStr);
            return key;
        }

        private static object GetEntitiesLocker<TKey>(ConcurrentDictionary<TKey, object> lockers, TKey id)
        {
            return lockers.GetOrAdd(id, new object());
        }
    }
}
