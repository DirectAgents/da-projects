using CakeExtracter.Logging.Loggers;
using System;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using CakeExtractor.SqlScriptsExecution.Core;

namespace CakeExtractor.SqlScriptsExecutor
{
    /// <summary>
    /// Entry point of SQl script runner application
    /// </summary>
    class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        static void Main(string[] args)
        {
            InitializeLogging();
            ProcessScriptExecution(args);
        }

        /// <summary>
        /// Checks commands arguments. Processes the script execution.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <exception cref="Exception">First cmd line argument with execution script relative file path should be provided</exception>
        private static void ProcessScriptExecution(string[] args)
        {
            try
            {
                if (args.Length > 0)
                {
                    var configsProvider = new ConfigsProvider();
                    var scriptFileContentProvider = new ScriptFileContentProvider();
                    var scriptsExecutor = new SqlCommandsInvoker(configsProvider.GetDbConnectionString());
                    var scriptRelativePath = args[0];
                    var scriptParams = args.ToArray().Skip(1).ToArray();
                    var scriptFullPath = $"{configsProvider.GetSqlScriptsFolderPath()}{scriptRelativePath}";
                    var scriptContent = scriptFileContentProvider.GetSqlScriptFileContent(scriptFullPath, scriptParams);
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

        /// <summary>
        /// Initializes the logging.
        /// </summary>
        private static void InitializeLogging()
        {
            var configurationSource = ConfigurationSourceFactory.Create();
            var logWriterFactory = new LogWriterFactory(configurationSource);
            Logger.SetLogWriter(logWriterFactory.Create());
            CakeExtracter.Logger.Instance = new EnterpriseLibraryLogger("SqlCommandsRunner");
        }
    }
}
