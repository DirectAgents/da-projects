using System;
using System.Linq;
using System.Timers;
using DAgents.Common;

namespace EomApp1.Formss.Synch
{
    public class RemoteLogPollerBase
    {
        private object stateLocker = new object();
        private RemoteLogPollerState state = RemoteLogPollerState.Stopped;

        protected enum RemoteLogPollerState
        {
            Stopped,
            Starting,
            Stopping,
            Started,
        }

        protected RemoteLogPollerState State
        {
            get 
            {
                lock (this.stateLocker)
                {
                    return this.state;
                }
            }
            set
            {
                lock (this.stateLocker)
                {
                    this.state = value;
                }
            }
        }
    }

    public class RemoteLogPoller : RemoteLogPollerBase
    {
        private int maxID;
        private Timer timer;
        private readonly Guid guid;
        private readonly double interval;
        private readonly ILogger logger;

        private RemoteLogPoller(ILogger logger)
        {
            this.logger = logger;
            this.interval = 1000;
            this.guid = Guid.NewGuid();
        }

        public static void Run(ILogger logger, Action<Guid> action)
        {
            var poller = new RemoteLogPoller(logger);

            poller.Start();

            try
            {
                action(poller.guid);
            }
            finally
            {
                poller.Finish();
            }
        }

        private void Start()
        {
            this.State = RemoteLogPollerState.Starting;

            this.maxID = 0;

            this.timer = new Timer(this.interval);
            timer.Elapsed += new ElapsedEventHandler(OnTimer);
            timer.Enabled = true;

            this.State = RemoteLogPollerState.Started;
        }

        private void Finish()
        {
            this.State = RemoteLogPollerState.Stopping;
            int i = 0;
            while (this.State != RemoteLogPollerState.Stopped || i > 50) // spin while waiting for stop to complete for max of 5 seconds
            {
                System.Threading.Thread.Sleep(100);
                i++;
            }
        }

        private void OnTimer(object sender, ElapsedEventArgs e)
        {
            if (this.State == RemoteLogPollerState.Started)
            {
                this.LogEntries();
            }

            if (this.State == RemoteLogPollerState.Stopping)
            {
                this.timer.Stop();

                this.State = RemoteLogPollerState.Stopped;
            }
        }

        private void LogEntries()
        {
            using (var context = new __defunct.CakeSynchEntities())
            {
                var log = context.Logs.Where(c => c.Name == this.guid).FirstOrDefault();

                if (log != null)
                {
                    var entries = log.LogEntries.Where(c => c.Id > this.maxID).ToList();

                    foreach (var logEntry in entries.OrderBy(c => c.Id))
                    {
                        this.logger.Log(logEntry.Message);
                    }

                    this.maxID += entries.Count();
                }
            }
        }
    }
}
