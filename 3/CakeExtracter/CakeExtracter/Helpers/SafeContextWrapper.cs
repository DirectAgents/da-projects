using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Polly;

namespace CakeExtracter.Helpers
{
    public static class SafeContextWrapper
    {
        public static object ActionTypeLocker = new object();
        public static object MetricTypeLocker = new object();
        public static object EntityTypeLocker = new object();
        public static object StrategyLocker = new object();
        public static object AdSetLocker = new object();
        public static object AdLocker = new object();
        public static object KeywordLocker = new object();
        public static object SearchTermLocker = new object();
        public static object DailyLocker = new object();

        private const int RetryCount = 3;
        private const int WaitSeconds = 3;

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
                    LogEndOfRetrying(exception, "SaveChanges");
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
                LogEndOfRetrying(exception, "SaveChanges");
            }

            return 0;
        }

        public static void TryBulkInsert<T, TItem>(object contextLocker, List<TItem> items)
            where T : DbContext, new()
            where TItem: class
        {
            try
            {
                TryToMakeTransactionManyTimes(false, contextLocker, (T db) => db.BulkInsert(items));
            }
            catch (Exception exception)
            {
                LogEndOfRetrying(exception, "BulkInsert");
            }
        }

        public static void TryBulkInsert<T, TItem>(List<TItem> items)
            where T : DbContext, new()
            where TItem : class
        {
            try
            {
                TryToMakeTransactionManyTimes(false, (T db) => db.BulkInsert(items));
            }
            catch (Exception exception)
            {
                LogEndOfRetrying(exception, "BulkInsert");
            }
        }

        public static void TryMakeTransaction<T>(Action<T> transactionAction, string level)
            where T : DbContext, new()
        {
            try
            {
                TryToMakeTransactionManyTimes(false, transactionAction);
            }
            catch (Exception exception)
            {
                LogEndOfRetrying(exception, level);
            }
        }

        public static void TryMakeTransactionWithLock<T>(Action<T> transactionAction, object contextLocker, string level)
           where T : DbContext, new()
        {
            try
            {
                TryToMakeTransactionManyTimes(false, contextLocker, transactionAction);
            }
            catch (Exception exception)
            {
                LogEndOfRetrying(exception, level);
            }
        }

        private static int TrySaveChanges<T>(object contextLocker, Action<T> changeContextAction)
            where T : DbContext, new()
        {
            var numChanges = 0;
            try
            {
                Logger.Warn("Trying to save changes using reopened database context.");
                TryToMakeTransactionManyTimes(false, contextLocker, (T dbContext) =>
                {
                    changeContextAction(dbContext);
                    numChanges = dbContext.SaveChanges();
                });
            }
            catch (Exception exc)
            {
                LogEndOfRetrying(exc, "SaveChanges");
            }

            return numChanges;
        }

        private static int SaveChanges<T>(T dbContext)
            where T : DbContext
        {
            var numChanges = 0;
            TryToMakeActionManyTimes(true, () => numChanges = dbContext.SaveChanges());
            return numChanges;
        }
        
        public static void TryToMakeTransactionManyTimes<T>(bool needToWait, object contextLocker, Action<T> transactionAction)
            where T : DbContext, new()
        {
            TryToMakeActionManyTimes(needToWait, () =>
            {
                lock (contextLocker)
                {
                    using (var db = new T())
                    {
                        transactionAction(db);
                    }
                }
            });
        }

        public static void TryToMakeTransactionManyTimes<T>(bool needToWait, Action<T> transactionAction)
            where T : DbContext, new()
        {
            TryToMakeActionManyTimes(needToWait, () =>
            {
                using (var db = new T())
                {
                    transactionAction(db);
                }
            });
        }

        private static void TryToMakeActionManyTimes(bool needToWait, Action transactionAction)
        {
            var policyBuilder = Policy.Handle<Exception>();
            var retryPolicy = needToWait
                ? policyBuilder.WaitAndRetry(RetryCount, retryNumber => new TimeSpan(0, 0, WaitSeconds),
                    (exc, time, context) => ProcessTransactionException(exc))
                : policyBuilder.Retry(RetryCount, (exc, retryCount) => ProcessTransactionException(exc));
            retryPolicy.Execute(transactionAction);
        }

        private static void LogEndOfRetrying(Exception exception, string level)
        {
            ProcessTransactionException(exception);
            var highException = new Exception($"End of retrying, transaction is failed. Level - {level}", exception);
            Logger.Error(highException);
        }

        private static void ProcessTransactionException(Exception exception)
        {
            Logger.Warn(exception.Message);
            switch (exception)
            {
                case DbUpdateException updateException:
                    ProcessDbUpdateException(updateException);
                    break;
            }
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
    }
}
