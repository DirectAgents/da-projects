using CakeExtractor.SqlScriptsExecution.Core;
using System;
using System.Configuration;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA.AmazonLoaders.AnalyticTablesSync
{
    /// <summary>
    /// Syncher from amazon ams normal tables to analytic tables.
    /// </summary>
    public class AmazonAmsAnalyticSyncher
    {
        private SqlExecutionConfigsProvider configsProvider;

        private ScriptFileContentProvider scriptFileContentProvider;

        private SqlCommandsInvoker sqlCommandsInvoker;

        private const string AmazonAmsDbConnectionString = "ClientPortalProgContext";

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonAmsAnalyticSyncher"/> class.
        /// </summary>
        public AmazonAmsAnalyticSyncher()
        {
            configsProvider = new SqlExecutionConfigsProvider();
            scriptFileContentProvider = new ScriptFileContentProvider();
            var dbConnectionString = configsProvider.GetDbConnectionString(AmazonAmsDbConnectionString);
            sqlCommandsInvoker = new SqlCommandsInvoker(dbConnectionString);
        }

        /// <summary>
        /// Synchronizes normal tables data to analytical tables data for the asin level for account.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        public void SyncAsinLevelForAccount(int accountId)
        {
            Logger.Info("Started syncing asin analytic table with normal tales", accountId);
            const string syncScriptPathConfigName = "AmazonAmsAsinSyncScriptPath";
            var scriptPath = ConfigurationManager.AppSettings[syncScriptPathConfigName];
            ExecuteScriptWithParams(scriptPath, new string[] {accountId.ToString() });
            Logger.Info("Finished syncing asin analytic table with normal tales", accountId);
        }

        private void ExecuteScriptWithParams(string scriptPath, string[] scriptParams)
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
