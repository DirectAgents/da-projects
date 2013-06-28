using System;
using System.Diagnostics;

namespace CakeExtracter.Common
{
    public class LogElapsedTime : IDisposable
    {
        private readonly Stopwatch stopWatch;

        public LogElapsedTime()
        {
            stopWatch = new Stopwatch();
            stopWatch.Start();
        }

        public void Dispose()
        {
            stopWatch.Stop();

            TimeSpan ts = stopWatch.Elapsed;

            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                                               ts.Hours, ts.Minutes, ts.Seconds,
                                               ts.Milliseconds / 10);

            Logger.Info("Elapsed Time: {0}", elapsedTime);
        }
    }
}