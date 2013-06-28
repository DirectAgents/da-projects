using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace CakeExtracter.Etl
{
    public abstract class Extracter<T> : IExtracter<T>
    {
        private int total;
        private bool done;
        private readonly List<T> extractedItems = new List<T>();

        protected Extracter()
        {
            Locker = new object();
        }

        public object Locker { get; set; }

        public Thread BeginExtracting()
        {
            var thread = new Thread(Extract);
            thread.Start();
            return thread;
        }

        public int TotalExtracted
        {
            get { lock (Locker) return total; }
            set { lock (Locker) total = value; }
        }

        public bool IsComplete
        {
            get { lock (Locker) return done; }
            set { lock (Locker) done = value; }
        }

        public IEnumerable<T> ExtractedItems
        {
            get { lock (Locker) return extractedItems; }
        }

        protected void AddExtracted(IEnumerable<T> extracted)
        {
            lock (Locker)
            {
                var extractedArray = extracted as T[] ?? extracted.ToArray();
                TotalExtracted += extractedArray.Count();
                extractedItems.AddRange(extractedArray);
            }
        }

        protected abstract void Extract();
    }
}
