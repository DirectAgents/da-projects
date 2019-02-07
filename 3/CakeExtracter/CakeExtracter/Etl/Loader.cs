using System;
using System.Collections.Generic;
using System.Threading;
using CakeExtracter.Common;

namespace CakeExtracter.Etl
{
    public abstract class Loader<T>
    {
        private Extracter<T> extracter;

        protected Loader()
        {
            BatchSize = 100;
        }

        protected Loader(int accountId)
            : this()
        {
            this.accountId = accountId;
        }

        public int BatchSize { get; set; }
        public int LoadedCount;
        public int ExtractedCount;

        protected readonly int accountId;

        public Thread Start(Extracter<T> source)
        {
            extracter = source;
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

                foreach (var list in extracter.EnumerateAll().InBatches(BatchSize))
                {
                    var loadCount = Load(list);

                    Interlocked.Add(ref LoadedCount, loadCount);
                    ExtractedCount = extracter.Added;

                    Logger.Info(accountId, "Extracted: {0} Loaded: {1} Queue: {2} Done: {3}", ExtractedCount,
                        LoadedCount,extracter.Count, extracter.Done);
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

        //public virtual string Status()
        //{
        //    return string.Format("Loaded {0} items", LoadedCount);
        //}
    }
}