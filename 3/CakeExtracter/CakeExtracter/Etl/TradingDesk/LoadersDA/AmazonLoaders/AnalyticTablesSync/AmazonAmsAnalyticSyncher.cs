using CakeExtracter.Common;
using CakeExtracter.Common.Constants;
using Polly;
using System;
using System.Configuration;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA.AmazonLoaders.AnalyticTablesSync
{
    /// <summary>
    /// Helper for synchronization data from amazon ams normal tables to analytic tables.
    /// </summary>
    public class AmazonAmsAnalyticSyncher
    {
        private SqlScriptsExecutor sqlScriptExecutor;

        private const string AmazonAmsDbConnectionStringConfigName = "ClientPortalProgContext";

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
                    Logger.Warn(accountId, String.Format("Sync analytic table failed. Waiting {0} seconds before trying again.", AnlyticTablesSyncConstants.secondsToWait)))
                .Execute(() =>
                {
                    const string syncScriptPathConfigName = "AmazonAmsAsinSyncScriptPath";
                    var scriptPath = ConfigurationManager.AppSettings[syncScriptPathConfigName];
                    sqlScriptExecutor.ExecuteScriptWithParams(scriptPath, new string[] { accountId.ToString() });
                });
            Logger.Info(accountId, "Finished syncing asin analytic table with normal tables");
        }
    }
}
