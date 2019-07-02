using System;
using System.Collections.Generic;
using System.Threading;
using CakeExtracter.Common;
using CakeExtracter.Common.JobExecutionManagement;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Exceptions;

namespace CakeExtracter.Etl
{
    public abstract class Loader<T>
    {
        private const int DefaultBatchSize = 100;

        protected readonly int accountId;

        public int LoadedCount;
        public int ExtractedCount;

        public event Action<FailedEtlException> ProcessEtlFailedWithoutInformation;

        private Extracter<T> extractor;

        public int BatchSize { get; set; }

        protected Loader()
        {
            BatchSize = DefaultBatchSize;
        }

        protected Loader(int accountId, int batchSize = DefaultBatchSize)
            : this()
        {
            this.accountId = accountId;
            BatchSize = batchSize;
        }

        public Thread Start(Extracter<T> source)
        {
            extractor = source;
            var thread = new Thread(DoLoad);
            thread.Start();
            return thread;
        }

        private void DoLoad()
        {
            try
            {
                LoadedCount = 0;
                ExtractedCount = 0;
                PreLoadAction();
                foreach (var list in extractor.EnumerateAll().InBatches(BatchSize))
                {
                    var loadCount = Load(list);

                    Interlocked.Add(ref LoadedCount, loadCount);
                    ExtractedCount = extractor.Added;

                    Logger.Info(accountId, "Extracted: {0} Loaded: {1} Queue: {2} Done: {3}", ExtractedCount,
                        LoadedCount, extractor.Count, extractor.Done);
                }

                if (LoadedCount != ExtractedCount)
                {
                    var ex = new Exception($"Unmatched counts: loaded {LoadedCount}, extracted {ExtractedCount}");
                    Logger.Error(accountId, ex);
                    throw ex;
                }
                AfterLoadAction();
            }
            catch (Exception e)
            {
                var exception = new FailedEtlException(null, null, accountId, e);
                LogExceptionInLoader(exception);
                ProcessEtlFailedWithoutInformation?.Invoke(exception);
            }
        }

        protected abstract int Load(List<T> items);

        protected virtual void AfterLoadAction()
        {
        }

        protected virtual void PreLoadAction()
        {
        }

        private void LogExceptionInLoader(FailedEtlException exc)
        {
            var exception = new Exception($"Exception in loader: {exc}", exc);
            if (exc.AccountId.HasValue)
            {
                Logger.Error(exc.AccountId.Value, exception);
            }
            else
            {
                Logger.Error(exception);
            }
        }
    }
}