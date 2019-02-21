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

        public int BatchSize { get; set; }
        public int LoadedCount;
        public int ExtractedCount;

        private Extracter<T> extractor;

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
                Logger.Error(new Exception($"Exception in loader: {e}", e));
            }
        }

        protected abstract int Load(List<T> items);

        protected virtual void AfterLoad()
        {
        }
    }
}