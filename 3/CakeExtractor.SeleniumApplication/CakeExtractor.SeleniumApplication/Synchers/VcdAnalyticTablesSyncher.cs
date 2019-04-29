using CakeExtracter;
using CakeExtracter.Common;
using CakeExtracter.Common.Constants;
using Polly;
using System;
using System.Configuration;

namespace CakeExtractor.SeleniumApplication.Synchers
{
    /// <summary>
    /// Helper for synchronization of normal and analytical tables for VCD data.
    /// </summary>
    public class VcdAnalyticTablesSyncher
    {
        private SqlScriptsExecutor sqlScriptExecutor;

        private const string AmazonAmsDbConnectionStringConfigName = "ClientPortalProgContext";

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonAmsAnalyticSyncher"/> class.
        /// </summary>
        public VcdAnalyticTablesSyncher()
        {
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
                    Logger.Warn(accountId, String.Format("Sync analytic table failed. Waiting {0} seconds before trying again.", AnlyticTablesSyncConstants.secondsToWait)))
                .Execute(() =>
                {
                    const string syncScriptPathConfigName = "VcdSyncScriptPath";
                    var scriptPath = ConfigurationManager.AppSettings[syncScriptPathConfigName];
                    sqlScriptExecutor.ExecuteScriptWithParams(scriptPath, new string[] { accountId.ToString() });
                });
            Logger.Info(accountId, "Finished sync VCD analytic table with normal tables.");
        }
    }
}
