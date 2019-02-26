using System;
using System.Configuration;

namespace CakeExtractor.SqlScriptsExecutor
{
    public class ConfigsProvider
    {
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

        public string GetDbConnectionString()
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
    }
}
