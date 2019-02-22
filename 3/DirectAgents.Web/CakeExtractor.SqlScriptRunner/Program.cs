using CakeExtracter.Logging.Loggers;
using System;
using System.Configuration;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace CakeExtractor.SqlScriptsExecutor
{
    class Program
    {
        static void Main(string[] args)
        {
            InitializeLogging();
            ProcessScriptExecution(args);
        }

        private static void ProcessScriptExecution(string[] args)
        {
            try
            {
                if (args.Length > 0)
                {
                    var scriptFileContentProvider = new ScriptFileContentProvider();
                    var scriptsExecutor = new SqlScriptsExecutor(GetDbConnectionString());
                    var scriptRelativePath = args[0];
                    var scriptParams = args.ToArray().Skip(1).ToArray();
                    var scriptContent = scriptFileContentProvider.GetSqlScriptFileContent(scriptRelativePath, scriptParams);
                    scriptsExecutor.RunSqlCommands(scriptContent);
                }
                else
                {
                   throw new Exception("First cmd line argument with execution script relative file path should be provided");
                }
            }
            catch (Exception ex)
            {
                CakeExtracter.Logger.Error(ex);
            }
        }

        private static string GetDbConnectionString()
        {
            const string ConnectionStringConfigurationName = "DataBaseConnectionString";
            if (!string.IsNullOrEmpty(ConfigurationManager.ConnectionStrings[ConnectionStringConfigurationName].ConnectionString))
            {
                return ConfigurationManager.ConnectionStrings[ConnectionStringConfigurationName].ConnectionString;
            }
            else
            {
                throw new Exception("Db connection string should specified in app config. See key DataBaseConnectionString");
            }
        }

        private static void InitializeLogging()
        {
            var configurationSource = ConfigurationSourceFactory.Create();
            var logWriterFactory = new LogWriterFactory(configurationSource);
            Logger.SetLogWriter(logWriterFactory.Create());
            CakeExtracter.Logger.Instance = new EnterpriseLibraryLogger("SqlCommandsRunner");
        }
    }
}
