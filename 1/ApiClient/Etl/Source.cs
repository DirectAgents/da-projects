using System.Collections.Generic;
using Common;
using System.Threading;

namespace ApiClient.Etl
{
    public class Source<T> : ISource<T>
    {
        public Thread Extract()
        {
            var thread = new Thread(DoExtract);
            thread.Start();
            return thread;
        }

        public virtual void DoExtract()  
        {
            Logger.Log("Override the method DoExtract() to do something useful.");
            Done = true;
        }

        protected void AddItems(T[] items)
        {
            lock (Locker)
            {
                Total = items.Length;
                this.items.AddRange(items);
            }
        }

        protected void WaitUntilNotPaused()
        {
            while (Paused)
                Thread.Sleep(1000);
        }

        List<T> items = new List<T>();
        IEnumerable<T> ISource<T>.Items { get { return items; } }

        int total = 0;
        public int Total
        {
            get
            {
                lock (Locker)
                    return total;
            }
            set
            {
                lock (Locker)
                {
                    //Logger.Log("Total to extract is {0} + {1} = {2}.", total, value, total + value);
                    total += value;
                }
            }
        }

        bool paused = false;
        public bool Paused
        {
            get
            {
                lock (Locker)
                    return paused;
            }
            set
            {
                lock (Locker)
                    paused = value;
            }
        }

        bool done = false;
        public bool Done
        {
            get
            {
                lock (Locker)
                    return done;
            }
            protected set
            {
                lock (Locker)
                {
                    done = value;
                    if (value)
                        Logger.Log("Extraction complete.");
                }
            }
        }

        object locker = new object();
        public object Locker { get { return locker; } }
    }
}
