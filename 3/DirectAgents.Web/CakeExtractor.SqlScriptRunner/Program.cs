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
        private static SqlExecutionConfigsProvider configsProvider = new SqlExecutionConfigsProvider();

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
                    var scriptContent = GetScriptContent(args);
                    ExecuteSqlScript(scriptContent);
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

        private static string GetScriptContent(string[] args)
        {
            var scriptFileContentProvider = new ScriptFileContentProvider();
            var scriptRelativePath = args[0];
            var scriptParams = args.ToArray().Skip(1).ToArray();
            var scriptFullPath = $"{configsProvider.GetSqlScriptsFolderPath()}{scriptRelativePath}";
            CakeExtracter.Logger.Info($"Script path - {scriptFullPath}");
            var scriptContent = scriptFileContentProvider.GetSqlScriptFileContent(scriptFullPath, scriptParams);
            CakeExtracter.Logger.Info("Script was loaded. Arguments were replaced");
            return scriptContent;
        }

        private static void ExecuteSqlScript(string scriptContent)
        {
            const string connectionName = "DataBaseConnectionString";
            var scriptsExecutor = new SqlCommandsInvoker(configsProvider.GetDbConnectionString(connectionName));
            CakeExtracter.Logger.Info("Started sql commands execution");
            var watch = System.Diagnostics.Stopwatch.StartNew();
            scriptsExecutor.RunSqlCommands(scriptContent);
            watch.Stop();
            CakeExtracter.Logger.Info($"Finished Sql commands execution. Execution time: {watch.Elapsed}");
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
