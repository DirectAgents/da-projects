﻿using System;
using System.Collections.Generic;
using System.Data.Entity;

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
        public static object DailySummaryLocker = new object();
        public static object StrategySummaryLocker = new object();
        public static object AdSetSummaryLocker = new object();
        public static object AdSummaryLocker = new object();
        public static object KeywordSummaryLocker = new object();
        public static object SearchTermSummaryLocker = new object();

        private static readonly Dictionary<int, object> AdSetActionLockers = new Dictionary<int, object>();
        private static readonly Dictionary<int, object> StrategyActionLockers = new Dictionary<int, object>();

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

        public static object GetAdSetActionLocker(int adSetId)
        {
            return GetEntitiesLocker(AdSetActionLockers, adSetId);
        }

        public static object GetStrategyActionLocker(int adSetId)
        {
            return GetEntitiesLocker(StrategyActionLockers, adSetId);
        }

        private static object GetEntitiesLocker(Dictionary<int, object> lockers, int id)
        {
            if (!lockers.ContainsKey(id))
            {
                lockers.Add(id, new object());
            }

            return lockers[id];
        }
    }
}
