using DirectAgents.Common.Logging.Loggers;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System;

namespace DirectAgents.Common.Logging
{
    public static class Logger
    {
        public static ILogger Instance = new NullLogger();

        public static void Info(string format, params object[] args)
        {
            Instance.Info(format, args);
        }

        public static void Warn(string format, params object[] args)
        {
            Instance.Warn(format, args);
        }

        public static void Error(Exception exception)
        {
            Instance.Error(exception);
        }

        //TODO: Do this with bootstrappers/MEF
        public static void InitializeLogging()
        {
            IConfigurationSource configurationSource = ConfigurationSourceFactory.Create();
            LogWriterFactory logWriterFactory = new LogWriterFactory(configurationSource);
            Microsoft.Practices.EnterpriseLibrary.Logging.Logger.SetLogWriter(logWriterFactory.Create());

            Instance = new EnterpriseLibraryLogger();
        }
    }
}
