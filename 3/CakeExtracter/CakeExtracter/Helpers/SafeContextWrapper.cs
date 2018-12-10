using System;
using System.Collections.Generic;
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
        public static object DailySummaryLocker = new object();
        public static object StrategySummaryLocker = new object();
        public static object AdSetSummaryLocker = new object();
        public static object AdSummaryLocker = new object();
        public static object KeywordSummaryLocker = new object();
        public static object SearchTermSummaryLocker = new object();

        private const int RetryCount = 5;
        private const int WaitSeconds = 3;

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

        public static object GetAdSetActionLocker(int adSetId)
        {
            return GetEntitiesLocker(AdSetActionLockers, adSetId);
        }

        public static object GetStrategyActionLocker(int adSetId)
        {
            return GetEntitiesLocker(StrategyActionLockers, adSetId);
        }

        public static int TrySaveChanges<T>(T dbContext, object contextLocker, Action<T> changeContextAction)
            where T : DbContext, new()
        {
            var numChanges = 0;
            try
            {
                numChanges = SaveChanges(dbContext);
            }
            catch (Exception exception)
            {
                if (exception is DbUpdateException)
                {
                    LogEndOfRetrying(exception);
                    return numChanges;
                }
                numChanges = TrySaveChanges(contextLocker, changeContextAction);
            }

            return numChanges;
        }

        public static int TrySaveChanges<T>(T dbContext)
            where T : DbContext, new()
        {
            try
            {
                return SaveChanges(dbContext);
            }
            catch (Exception exception)
            {
                LogEndOfRetrying(exception);
            }

            return 0;
        }

        private static int TrySaveChanges<T>(object contextLocker, Action<T> changeContextAction)
            where T : DbContext, new()
        {
            var numChanges = 0;
            try
            {
                Logger.Warn("Trying to save changes using reopened database context.");
                numChanges = SaveChanges(contextLocker, changeContextAction);
            }
            catch (Exception exc)
            {
                LogEndOfRetrying(exc);
            }

            return numChanges;
        }

        private static int SaveChanges<T>(T dbContext)
            where T : DbContext
        {
            var numChanges = Policy
                .Handle<Exception>()
                .WaitAndRetry(RetryCount,
                    retryNumber => new TimeSpan(0, 0, WaitSeconds),
                    (exc, time, context) => ProcessSaveException(exc))
                .Execute(dbContext.SaveChanges);
            return numChanges;
        }

        private static int SaveChanges<T>(object contextLocker, Action<T> changeContextAction)
            where T : DbContext, new()
        {
            var numChanges = Policy
                .Handle<Exception>()
                .Retry(RetryCount, (exc, retryCount) => ProcessSaveException(exc))
                .Execute(() => SaveChangedContextWithoutRetrying(contextLocker, changeContextAction));
            return numChanges;
        }

        private static int SaveChangedContextWithoutRetrying<T>(object contextLocker, Action<T> changeContextAction)
            where T : DbContext, new()
        {
            lock (contextLocker)
            {
                using (var dbContext = new T())
                {
                    changeContextAction(dbContext);
                    return dbContext.SaveChanges();
                }
            }
        }

        private static void LogEndOfRetrying(Exception exception)
        {
            ProcessSaveException(exception);
            Logger.Warn("Saving of changes is failed.");
        }

        private static void ProcessSaveException(Exception exception)
        {
            LogException(exception);
            switch (exception)
            {
                case DbUpdateException updateException:
                    ProcessDbUpdateException(updateException);
                    break;
            }
        }

        private static void LogException(Exception exception)
        {
            var excForLog = exception;
            while (excForLog.InnerException != null)
            {
                excForLog = excForLog.InnerException;
            }
            Logger.Error(excForLog);
        }

        private static void ProcessDbUpdateException(DbUpdateException exception)
        {
            var notSavedEntriesInfo = exception.Entries.Select(GetEntryInfo);
            var message = string.Join("\n", notSavedEntriesInfo);
            Logger.Warn(message);
        }

        private static string GetEntryInfo(DbEntityEntry entry)
        {
            const string valuesTemplate = "\n{0}:\n\t{1}";
            var currentValuesInfo = GetPropertiesInfo(entry.CurrentValues);
            var message = string.Format(valuesTemplate, "Current values", currentValuesInfo);
            if (entry.State != EntityState.Modified)
            {
                return message;
            }
            var originalValuesInfo = GetPropertiesInfo(entry.OriginalValues);
            message = message + string.Format(valuesTemplate, "Original values", originalValuesInfo);
            return message;
        }

        private static string GetPropertiesInfo(DbPropertyValues properties)
        {
            var propertiesInfo = properties.PropertyNames.Select(name => $"{name} = {properties.GetValue<object>(name)}");
            var message = string.Join(", ", propertiesInfo);
            return message;
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
