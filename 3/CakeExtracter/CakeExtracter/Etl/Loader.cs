using System;
using System.Collections.Generic;
using System.Threading;
using CakeExtracter.Common;

namespace CakeExtracter.Etl
{
    public abstract class Loader<T>
    {
        private Extracter<T> extracter;

        public int BatchSize { get; set; }
        public int LoadedCount;
        public int ExtractedCount;

        protected readonly int accountId;

        private const int defaultBatchSize = 100;

        protected Loader()
        {
            BatchSize = defaultBatchSize;
        }

        protected Loader(int accountId, int batchSize = defaultBatchSize)
            : this()
        {
            this.accountId = accountId;
            this.BatchSize = batchSize;
        }

       

        public Thread Start(Extracter<T> source)
        {
            extracter = source;
            var thread = new Thread(DoLoad);
            thread.Start();
            return thread;
        }

        private void DoLoad()
        {
            LoadedCount = 0;
            ExtractedCount = 0;

            foreach (var list in extracter.EnumerateAll().InBatches(BatchSize))
            {
                int loadCount = Load(list);

                Interlocked.Add(ref LoadedCount, loadCount);
                ExtractedCount = extracter.Added;

                Logger.Info(accountId, "Extracted: {0} Loaded: {1} Queue: {2} Done: {3}", ExtractedCount, LoadedCount,
                            extracter.Count, extracter.Done);
            }

            if (LoadedCount != ExtractedCount)
            {
                var ex = new Exception(string.Format("Unmatched counts: loaded {0}, extracted {1}", LoadedCount, ExtractedCount));
                Logger.Error(accountId, ex);
                throw ex;
            }

            AfterLoad();
        }

        protected abstract int Load(List<T> items);

        protected virtual void AfterLoad()
        {
        }

        //public virtual string Status()
        //{
        //    return string.Format("Loaded {0} items", LoadedCount);
        //}
    }
}