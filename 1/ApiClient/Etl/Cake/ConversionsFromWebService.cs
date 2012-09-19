using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApiClient.Models;
using Common;
using Extensions;

namespace ApiClient.Etl.Cake
{
    public class ConversionsFromWebService : ISource<conversion>
    {
        static DateTime Today = DateTime.Now;
        static DateRange DateRange = new DateRange(Today.FirstDayOfMonth(), Today.LastDayOfMonth());
        //static DateRange DateRange = new DateRange(Today.FirstDayOfMonth(), Today.FirstDayOfMonth());
        static int ExtractBatchSize = 500;

        public ConversionsFromWebService()
        {
            Locker = new object();
            Items = new List<ApiClient.Models.conversion>();
        }

        public Thread Extract()
        {
            var thread = new Thread(DoExtract);
            thread.Start();
            return thread;
        }

        //717490
        void DoExtract()
        {
            var loop = Parallel.ForEach(DateRange.Days, day =>
            {
                int numExtracted = 0;
                int rowCount = -1;
                while (rowCount != numExtracted)
                {
                    var extracted = CakeWebService.Extract(day, numExtracted + 1, ExtractBatchSize);
                    numExtracted += extracted.conversions.Length;
                    if (rowCount == -1)
                    {
                        rowCount = extracted.row_count;
                        Total += extracted.row_count;
                    }
                    lock (Locker)
                    {
                        Items.AddRange(extracted.conversions);
                    }
                }
            });

            while (!loop.IsCompleted)
                Thread.Sleep(250);

            Done = true;
        }

        public List<conversion> Items { get; set; }

        IEnumerable<conversion> ISource<conversion>.Items { get { return Items; } }

        public object Locker { get; set; }

        private int total = 0;
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
                    total = value;
                    Logger.Log("Total conversions to extract is {0}.", value);
                }
            }
        }

        private bool done = false;
        public bool Done
        {
            get
            {
                lock (Locker)
                    return done;
            }
            private set
            {
                lock (Locker)
                {
                    done = value;
                    if (value)
                        Logger.Log("Conversion extraction complete.");
                }
            }
        }
    }
}
