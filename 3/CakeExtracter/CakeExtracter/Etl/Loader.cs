using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CakeExtracter.Common;

namespace CakeExtracter.Etl
{
    public abstract class Loader<T> : ILoader<T>
    {
        private IExtracter<T> extracter;

        public int TotalLoaded { get; set; }

        public int LoadBatchSize { get; set; }

        protected Loader()
        {
            LoadBatchSize = 100;
        }

        public Thread BeginLoading(IExtracter<T> source)
        {
            extracter = source;
            var thread = new Thread(Load);
            thread.Start();
            return thread;
        }

        private void Load()
        {
            while ((!extracter.IsComplete) || (extracter.TotalExtracted != TotalLoaded))
            {
                IEnumerable<T> itemsToLoad = null;

                Thread.Sleep(250);

                lock (extracter.Locker)
                {
                    if (extracter.TotalExtracted > TotalLoaded)
                    {
                        itemsToLoad = extracter.ExtractedItems.Skip(TotalLoaded).ToList();
                    }
                }

                if (itemsToLoad != null)
                {
                    foreach (var batch in itemsToLoad.InSetsOf(LoadBatchSize))
                    {
                        TotalLoaded += LoadItems(batch);
                        Logger.Info("{0}/{1}, Done: {2}", TotalLoaded, extracter.TotalExtracted, extracter.IsComplete);
                    }
                }
            }
        }

        protected abstract int LoadItems(List<T> items);
    }
}