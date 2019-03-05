using CakeExtracter.Common;
using System.Configuration;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA.AmazonLoaders.AnalyticTablesSync
{
    /// <summary>
    /// Syncher from amazon ams normal tables to analytic tables.
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
            Logger.Info("Started syncing asin analytic table with normal tables", accountId);
            const string syncScriptPathConfigName = "AmazonAmsAsinSyncScriptPath";
            var scriptPath = ConfigurationManager.AppSettings[syncScriptPathConfigName];
            sqlScriptExecutor.ExecuteScriptWithParams(scriptPath, new string[] { accountId.ToString() });
            Logger.Info("Finished syncing asin analytic table with normal tables", accountId);
        }
    }
}
