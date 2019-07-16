using System;
using CakeExtracter.Common;
using CakeExtracter.Common.Constants;
using Polly;

namespace CakeExtracter.Etl.AmazonSelenium.VCD.Synchers
{
    /// <summary>
    /// Helper for synchronization of normal and analytical tables for VCD data.
    /// </summary>
    public class VcdAnalyticTablesSyncher
    {
        private const string AmazonAmsDbConnectionStringConfigName = "ClientPortalProgContext";

        private readonly SqlScriptsExecutor sqlScriptExecutor;
        private readonly string scriptPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="VcdAnalyticTablesSyncher"/> class.
        /// </summary>
        /// <param name="scriptPath">Path to sync script.</param>
        public VcdAnalyticTablesSyncher(string scriptPath)
        {
            this.scriptPath = scriptPath;
            sqlScriptExecutor = new SqlScriptsExecutor(AmazonAmsDbConnectionStringConfigName);
        }

        /// <summary>
        /// Synchronizes normal tables data to analytical tables data for the vcd data.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        public void SyncData(int accountId)
        {
            Logger.Info(accountId, "Started sync VCD analytic table with normal tables");
            Policy
                .Handle<Exception>()
                .Retry(AnlyticTablesSyncConstants.maxRetryAttempts, (exception, retryCount, context) =>
                    Logger.Warn(accountId, $"Sync analytic table failed. Waiting {AnlyticTablesSyncConstants.secondsToWait} seconds before trying again."))
                .Execute(() =>
                {
                    sqlScriptExecutor.ExecuteScriptWithParams(scriptPath, new[] { accountId.ToString() });
                });
            Logger.Info(accountId, "Finished sync VCD analytic table with normal tables.");
        }
    }
}