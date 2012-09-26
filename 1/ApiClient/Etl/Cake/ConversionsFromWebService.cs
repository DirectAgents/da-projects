using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApiClient.Models.Cake;
using Common;
using Extensions;

namespace ApiClient.Etl.Cake
{
    public class ConversionsFromWebService : ISource<conversion>
    {
        static DateTime Today = DateTime.Now;
        static DateRange DateRange = new DateRange(Today.FirstDayOfMonth(), Today.LastDayOfMonth());
        //static DateRange DateRange = new DateRange(Today.FirstDayOfMonth(), Today.FirstDayOfMonth().AddDays(1));
        static int ExtractBatchSize = 500;

        public Thread Extract()
        {
            var thread = new Thread(DoExtract);
            thread.Start();
            return thread;
        }

        void DoExtract()
        {
            var loop1 = Parallel.ForEach(DateRange.Days, day =>
            {
                int rowCount =  CakeWebService.Conversions(day, 1, 1).row_count;
                if (rowCount > 0)
                {
                    Total = rowCount;
                    var range = Tuple.Create(1, rowCount);
                    foreach (var item in range.InSetIndiciesOf(ExtractBatchSize))
                    {
                        var extracted = CakeWebService.Conversions(day, item.Item1, item.Item2);
                        AddItems(extracted.conversions);
                    }
                }
            });

            while (!loop1.IsCompleted)
                Thread.Sleep(250);

            Done = true;
        }

        void AddItems(conversion[] conversions)
        {
            lock (Locker)
                items.AddRange(conversions);
        }

        List<conversion> items = new List<conversion>();
        IEnumerable<conversion> ISource<conversion>.Items { get { return items; } }

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
                    Logger.Log("Total conversions to extract is {0} + {1} = {2}.", total, value, total + value);
                    total += value;
                }
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

        object locker = new object();
        public object Locker { get { return locker; } }
    }
}
