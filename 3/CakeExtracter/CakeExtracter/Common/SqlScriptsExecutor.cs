using System;
using System.Data;
using CakeExtractor.SqlScriptsExecution.Core;

namespace CakeExtracter.Common
{
    /// <summary>
    /// Service for sql script execution.
    /// </summary>
    public class SqlScriptsExecutor
    {
        private SqlExecutionConfigsProvider configsProvider;

        private ScriptFileContentProvider scriptFileContentProvider;

        private SqlCommandsInvoker sqlCommandsInvoker;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlScriptsExecutor"/> class.
        /// </summary>
        /// <param name="connectionDbConnectionStringName">Name of the connection database connection string.</param>
        public SqlScriptsExecutor(string connectionDbConnectionStringName)
        {
            configsProvider = new SqlExecutionConfigsProvider();
            scriptFileContentProvider = new ScriptFileContentProvider();
            var dbConnectionString = configsProvider.GetDbConnectionString(connectionDbConnectionStringName);
            sqlCommandsInvoker = new SqlCommandsInvoker(dbConnectionString);
        }

        /// <summary>
        /// Executes the script with replaced parameters.
        /// </summary>
        /// <param name="scriptPath">The script path.</param>
        /// <param name="scriptParams">The script parameters.</param>
        public void ExecuteScriptWithParams(string scriptPath, string[] scriptParams)
        {
            var scriptContent = scriptFileContentProvider.GetSqlScriptFileContent(scriptPath, scriptParams);
            sqlCommandsInvoker.RunSqlCommands(scriptContent);
        }

        /// <summary>
        /// Executes the select script with replaced parameters and put result into DataTable.
        /// </summary>
        /// <param name="scriptPath">The script path.</param>
        /// <param name="scriptParams">The script parameters.</param>
        /// <returns>Selected data.</returns>
        public DataTable ExecuteSelectScript(string scriptPath, string[] scriptParams)
        {
            var scriptContent = scriptFileContentProvider.GetSqlScriptFileContent(scriptPath, scriptParams);
            return sqlCommandsInvoker.SelectSqlDataAsDataTable(scriptContent);
        }

        /// <summary>
        /// Executes the sql command.
        /// </summary>
        /// <param name="sqlCommand">The sql command.</param>
        public void ExecuteSqlCommand(string sqlCommand)
        {
            sqlCommandsInvoker.RunSqlCommands(sqlCommand);
        }

        /// <summary>
        /// Saves the data into the target table using bulk operation.
        /// </summary>
        /// <param name="data">The data to save.</param>
        /// <param name="targetTable">The target table name.</param>
        public void BulkSaveDataTable(DataTable data, string targetTable)
        {
            sqlCommandsInvoker.RunBulkSave(data, targetTable);
        }

        /// <summary>
        /// Executes the sql command and return a scalar result.
        /// </summary>
        /// <typeparam  name="T">Expected type of a result.</typeparam>
        /// <param name="sqlCommand">The sql command.</param>
        /// <returns>Scalar result of the query.</returns>
        public T GetScalarResult<T>(string sqlCommand)
        {
            return sqlCommandsInvoker.ExecuteAsScalar<T>(sqlCommand);
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
