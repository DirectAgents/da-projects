using CakeExtractor.SqlScriptsExecution.Core;
using System;

namespace CakeExtracter.Common
{
    public class SqlScriptsExecutor
    {
        private SqlExecutionConfigsProvider configsProvider;

        private ScriptFileContentProvider scriptFileContentProvider;

        private SqlCommandsInvoker sqlCommandsInvoker;

        public SqlScriptsExecutor(string connectionDbConnectionStringName)
        {
            configsProvider = new SqlExecutionConfigsProvider();
            scriptFileContentProvider = new ScriptFileContentProvider();
            var dbConnectionString = configsProvider.GetDbConnectionString(connectionDbConnectionStringName);
            sqlCommandsInvoker = new SqlCommandsInvoker(dbConnectionString);
        }

        public void ExecuteScriptWithParams(string scriptPath, string[] scriptParams)
        {
            var scriptContent = scriptFileContentProvider.GetSqlScriptFileContent(scriptPath, scriptParams);
            sqlCommandsInvoker.RunSqlCommands(scriptContent);
        }

        private string GetScriptContent(string scriptFullPath, string[] scriptParams)
        {
            if (!string.IsNullOrEmpty(scriptFullPath))
            {
                Logger.Info($"Script path - {scriptFullPath}");
                var scriptContent = scriptFileContentProvider.GetSqlScriptFileContent(scriptFullPath, scriptParams);
                Logger.Info("Script was loaded. Arguments were replaced");
                return scriptContent;
            }
            else
            {
                throw new Exception("Sql Script script execution path can't be null or empty.");
            }
        }
    }
}
