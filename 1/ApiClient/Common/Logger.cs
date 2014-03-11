using System;

namespace Common
{
    public class Logger
    {
        public static void InitializeLogging()
        {
            DirectAgents.Common.Logging.Logger.InitializeLogging();
        }

        public static void Log(string messageFormat, params object[] formatArgs)
        {
            DirectAgents.Common.Logging.Logger.Info(messageFormat, formatArgs);
        }

        public static void Warn(string messageFormat, params object[] formatArgs)
        {
            DirectAgents.Common.Logging.Logger.Warn(messageFormat, formatArgs);
        }

        public static void Progress()
        {
            //Console.Write(".");
        }

        internal static void Done()
        {
            //Console.WriteLine();
        }
    }
}
