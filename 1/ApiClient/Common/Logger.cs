using System;
using System.Threading;

namespace Common
{
    public class Logger
    {
        public static void Log(string messageFormat, params object[] formatArgs)
        {
            Console.WriteLine(messageFormat, formatArgs);
        }
    }
}
