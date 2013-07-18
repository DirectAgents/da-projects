using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CakeExtracter.Common;

namespace CakeExtracter.Etl
{
    public interface ILoader<T>
    {
        int Load(List<T> items);
    }

    public class MultiLoader<T>
    {
        private Extracter<T> extracter;
        private ILoader<T>[] destinations;

        public MultiLoader(ILoader<T>[] destinations)
        {
            this.destinations = destinations;
            BatchSize = 100;
        }

        public Thread Start(Extracter<T> source)
        {
            extracter = source;
            var thread = new Thread(DoLoad);
            thread.Start();
            return thread;
        }

        class Worker
        {
            public Worker(ILoader<T> loader, int batchSize)
            {
                Loader = loader;
                Collection = new BlockingCollection<T>(5000);
            }

            public void Start()
            {
                Task = new Task<int>(() =>
                {
                    var loadedCount = 0;
                    foreach (var list in Collection.GetConsumingEnumerable().InBatches(BatchSize))
                    {
                        loadedCount += Loader.Load(list);
                    }
                    return loadedCount;
                });
            }

            public Task<int> Task { get; set; }

            public BlockingCollection<T> Collection { get; set; }

            public ILoader<T> Loader { get; set; }

            public int BatchSize { get; set; }
        }

        private void DoLoad()
        {
            var workers = new List<Worker>();

            // Create workers
            foreach (var loader in destinations)
            {
                workers.Add(new Worker(loader, BatchSize));
            }

            // Start workers
            foreach (var worker in workers)
            {
                worker.Start();
            }

            // Wait for all workers to complete
            Task.WaitAll(workers.Select(c => c.Task).ToArray());

            // Check item counts
            var counts = workers.Select(c => c.Task.Result);
            if (counts.Any(c => c != extracter.Added))
            {
                var ex = new Exception(string.Format("Unmatched counts: expecting {0}", extracter.Added));
                Logger.Error(ex);
                throw ex;
            }
        }

        public int BatchSize { get; set; }
    }
}
