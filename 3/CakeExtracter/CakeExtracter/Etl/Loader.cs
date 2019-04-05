using System;
using System.Collections.Generic;
using System.Threading;
using CakeExtracter.Common;

namespace CakeExtracter.Etl
{
    public abstract class Loader<T>
    {
        private const int DefaultBatchSize = 100;

        protected readonly int accountId;

        public int LoadedCount;
        public int ExtractedCount;

        public event Action<Exception> ProcessEtlFailedWithoutInformation;

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
            ProcessEtlFailedWithoutInformation += e =>
            {
                var exc = new Exception($"Exception in loader: {e}", e);
                Logger.Error(exc);
            };
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

                foreach (var list in extractor.EnumerateAll().InBatches(BatchSize))
                {
                    var loadCount = Load(list);

                    Interlocked.Add(ref LoadedCount, loadCount);
                    ExtractedCount = extractor.Added;

                    Logger.Info(accountId, "Extracted: {0} Loaded: {1} Queue: {2} Done: {3}", ExtractedCount,
                        LoadedCount,extractor.Count, extractor.Done);
                }

                if (LoadedCount != ExtractedCount)
                {
                    var ex = new Exception($"Unmatched counts: loaded {LoadedCount}, extracted {ExtractedCount}");
                    Logger.Error(accountId, ex);
                    throw ex;
                }

                AfterLoad();
            }
            catch (Exception e)
            {
                ProcessEtlFailedWithoutInformation?.Invoke(e);
            }
        }

        protected abstract int Load(List<T> items);

        protected virtual void AfterLoad()
        {
        }
    }
}