using System;
using System.Configuration;

namespace CakeExtractor.SqlScriptsExecution.Core
{
    /// <summary>
    /// Provider for sql script execution related configs.
    /// </summary>
    public class SqlExecutionConfigsProvider
    {
        /// <summary>
        /// Gets the SQL scripts location.
        /// </summary>
        /// <returns>Path to the folder where sql scripts located</returns>
        public string GetSqlScriptsFolderPath()
        {
            const string DefaultSqlFolderPath = "./SQL/";
            const string SqlFolderPathConfigKey = "SqlScriptsFolderPath";
            try
            {
                return !string.IsNullOrEmpty(ConfigurationManager.AppSettings[SqlFolderPathConfigKey])
                ? ConfigurationManager.AppSettings[SqlFolderPathConfigKey]
                : DefaultSqlFolderPath;
            }
            catch
            {
                return DefaultSqlFolderPath;
            }
        }

        /// <summary>
        /// Gets the database connection string.
        /// </summary>
        /// <param name="connectionName">Name of the connection.</param>
        /// <returns>Database connection string with specified connection name. </returns>
        /// <exception cref="Exception">Db connection string should specified in app config. See key {connectionName}</exception>
        public string GetDbConnectionString(string connectionName)
        {
            if (!string.IsNullOrEmpty(ConfigurationManager.ConnectionStrings[connectionName].ConnectionString))
            {
                return ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
            }
            else
            {
                throw new Exception($"Db connection string should specified in app config. See key {connectionName}");
            }
        }
    }
}
