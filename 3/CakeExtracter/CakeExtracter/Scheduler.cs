using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace CakeExtracter
{
    class Scheduler
    {
        private readonly object _locker = new object();
        private bool _inUse;
        private Timer _timer;
        private readonly Syncher _syncher = new Syncher();
        private readonly List<SyncherRun> _synchItems;

        public Scheduler()
        {
            _synchItems = new List<SyncherRun>()
                {
                    new SyncherRun(_syncher, 278, 60, 0),
                    new SyncherRun(_syncher, 278, 240, -1),

                    new SyncherRun(_syncher, 455, 60, 0),
                    new SyncherRun(_syncher, 455, 240, -1),

                    new SyncherRun(_syncher, 250, 60, 0),
                    new SyncherRun(_syncher, 250, 240, -1),

                    new SyncherRun(_syncher, 207, 60, 0),
                    new SyncherRun(_syncher, 207, 240, -1),
                };
        }

        public void Run()
        {
            _timer = new Timer(TimerMethod, null, 1000, 60000); // wake up every minute
            Console.WriteLine("Press ENTER to stop the scheduler..");
            Console.ReadLine();
        }

        public void TimerMethod(object state)
        {
            lock (_locker)
            {
                if (_inUse)
                    return;
            }
            try
            {
                lock (_locker)
                {
                    _inUse = true;
                }

                var now = DateTime.Now;

                var pastDue = _synchItems.Where(c => c.Next < now);

                if (pastDue.Any())
                {
                    pastDue.First().Run();
                }
                else
                {
                    Console.WriteLine("Nothing to synch at {0}.", DateTime.Now.ToString());
                }
            }
            finally
            {
                lock (_locker)
                {
                    _inUse = false;
                }
            }
        }
    }
}