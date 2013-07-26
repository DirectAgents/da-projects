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

        public int BatchSize { get; set; }

        public Thread Start(Extracter<T> source)
        {
            extracter = source;
            var thread = new Thread(DoLoad);
            thread.Start();
            return thread;
        }

        private void DoLoad()
        {
            var loadedCount = 0;
            var extractedCount = 0;

            foreach (var list in extracter.EnumerateAll().InBatches(BatchSize))
            {
                int loadCount = Load(list);

                Interlocked.Add(ref loadedCount, loadCount);
                extractedCount = extracter.Added;

                Logger.Info("Extracted: {0} Loaded: {1} Queue: {2} Done: {3}", extractedCount, loadedCount,
                            extracter.Count, extracter.Done);
            }

            if (loadedCount != extractedCount)
            {
                var ex = new Exception(string.Format("Unmatched counts: loaded {0}, extracted {1}", loadedCount, extractedCount));
                Logger.Error(ex);
                throw ex;
            }

            AfterLoad();
        }

        protected abstract int Load(List<T> items);

        protected virtual void AfterLoad()
        {
        }
    }
}