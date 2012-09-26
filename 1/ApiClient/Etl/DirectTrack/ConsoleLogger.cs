using System;

namespace DirectTrack
{
    public class ConsoleLogger : ILogger
    {
        public void Log(string p)
        {
            Console.WriteLine(p);
        }

        public void LogError(string p)
        {
            Console.WriteLine("ERROR: " + p);
        }
    }
}
