using System;
using System.Configuration;
using Amazon.Helpers;
using CakeExtracter.Common;
using CakeExtracter.Common.Constants;
using Polly;

namespace CakeExtracter.Etl.Amazon.Loaders.AnalyticTablesSync
{
    /// <summary>
    /// Helper for synchronization data from amazon ams normal tables to analytic tables.
    /// </summary>
    public class AmazonAmsAnalyticSyncher
    {
        private const string AmazonAmsDbConnectionStringConfigName = "ClientPortalProgContext";

        private readonly SqlScriptsExecutor sqlScriptExecutor;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonAmsAnalyticSyncher"/> class.
        /// </summary>
        public AmazonAmsAnalyticSyncher()
        {
            sqlScriptExecutor = new SqlScriptsExecutor(AmazonAmsDbConnectionStringConfigName);
        }

        /// <summary>
        /// Synchronizes normal tables data to analytical tables data for the asin level for account.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        public void SyncAsinLevelForAccount(int accountId)
        {
            Logger.Info(accountId, "Started syncing asin analytic table with normal tables");
            Policy
                .Handle<Exception>()
                .Retry(AnlyticTablesSyncConstants.maxRetryAttempts, (exception, retryCount, context) =>
                    Logger.Warn(accountId, $"Sync analytic table failed. Waiting {AnlyticTablesSyncConstants.secondsToWait} seconds before trying again."))
                .Execute(() =>
                {
                    var syncScriptPath = GetSyncScriptPath();
                    sqlScriptExecutor.ExecuteScriptWithParams(syncScriptPath, new[] { accountId.ToString() });
                });
            Logger.Info(accountId, "Finished syncing asin analytic table with normal tables");
        }

        /// <summary>
        /// Gets the full path to sync SQL script.
        /// </summary>
        /// <returns>Full path to sync SQL script.</returns>
        private string GetSyncScriptPath()
        {
            const string syncScriptPathConfigurationKey = "AmazonAmsAsinSyncScriptPath";
            const string syncScriptNameConfigurationKey = "AmazonAmsAsinSyncScriptName";
            var path = ConfigurationManager.AppSettings[syncScriptPathConfigurationKey];
            var name = ConfigurationManager.AppSettings[syncScriptNameConfigurationKey];
            return FileManager.GetAssemblyRelativePath(FileManager.CombinePath(path, name));
        }
    }
}
