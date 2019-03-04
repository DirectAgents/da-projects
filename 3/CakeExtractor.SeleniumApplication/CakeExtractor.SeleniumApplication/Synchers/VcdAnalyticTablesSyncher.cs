using CakeExtracter;
using CakeExtracter.Common;
using System.Configuration;

namespace CakeExtractor.SeleniumApplication.Synchers
{
    /// <summary>
    /// Syncher of normal and analytical tables for vcd data.
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
            Logger.Info("Started sync vcd analytic table with normal tables", accountId);
            const string syncScriptPathConfigName = "VcdSyncScriptPath";
            var scriptPath = ConfigurationManager.AppSettings[syncScriptPathConfigName];
            sqlScriptExecutor.ExecuteScriptWithParams(scriptPath, new string[] { accountId.ToString() });
            Logger.Info("Finished sync vcd analytic table with normal tables", accountId);
        }
    }
}
