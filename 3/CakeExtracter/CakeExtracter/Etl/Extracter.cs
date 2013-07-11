using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace CakeExtracter.Etl
{
    public abstract class Extracter<T>
    {
        private int added;
        private readonly BlockingCollection<T> items = new BlockingCollection<T>(5000);
        private readonly object locker = new object();

        public  Thread Start()
        {
            var thread = new Thread(Extract);
            thread.Start();
            return thread;
        }

        public void End()
        {
            items.CompleteAdding();
        }

        public bool Done
        {
            get { return items.IsAddingCompleted; }
        }

        public IEnumerable<T> EnumerateAll()
        {
            return items.GetConsumingEnumerable();
        }

        public int Count
        {
            get { return items.Count; }
        }

        public int Added
        {
            get { lock (locker) return added; }
        }

        protected void Add(IEnumerable<T> extracted)
        {
            foreach (var item in extracted)
            {
                items.Add(item);
                lock (locker) added++;
            }
        }

        /// <summary>
        /// The derived class implements this method, which calls Add() for each item
        /// extracted and then calls End() when complete.
        /// </summary>
        protected abstract void Extract();
    }
}
