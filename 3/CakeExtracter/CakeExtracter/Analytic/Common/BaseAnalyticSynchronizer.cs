using System;
using System.Data;

using CakeExtracter.Common;

namespace CakeExtracter.Analytic.Common
{
    /// <summary>
    /// Class provides common methods to synch analytic data.
    /// </summary>
    internal abstract class BaseAnalyticSynchronizer
    {
        private readonly DateTime startDate;

        private readonly DateTime endDate;

        private readonly int? accountId;

        private readonly string sourceConnectionName;

        private readonly string destinationConnectionName;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseAnalyticSynchronizer"/> class.
        /// </summary>
        /// <param name="startDate">Start date to synch the analytic data.</param>
        /// <param name="endDate">End date to synch the analytic data.</param>
        /// <param name="accountId">Account identifier to synch analytic date (null = all accounts).</param>
        protected BaseAnalyticSynchronizer(DateTime startDate, DateTime endDate, int? accountId = null)
        {
            this.startDate = startDate;
            this.endDate = endDate;
            this.accountId = accountId;
            sourceConnectionName = AnalyticParamsProvider.GetMainDatabaseConnectionName;
            destinationConnectionName = AnalyticParamsProvider.GetAnalyticDataConnectionName;
        }

        /// <summary>
        /// Gets name of the target analytic table.
        /// </summary>
        protected abstract string TargetAnalyticTable { get; }

        /// <summary>
        /// Runs the analytic data synchronization.
        /// </summary>
        public void RunSynchronizer()
        {
            var analyticDate = GetAnalyticData();
            if (analyticDate.Rows.Count > 0)
            {
                RemoveOldData();
                SaveAnalyticData(analyticDate);
            }
        }

        private DataTable GetAnalyticData()
        {
            var scriptsExecutor = new SqlScriptsExecutor(sourceConnectionName);
            var scriptParams = AnalyticParamsProvider.GetCommonScriptParams(startDate, endDate, accountId);
            var scriptPath = AnalyticParamsProvider.GetSelectAnalyticDataScriptPath(TargetAnalyticTable);
            return scriptsExecutor.ExecuteSelectScript(scriptPath, scriptParams);
        }

        private void RemoveOldData()
        {
            var scriptsExecutor = new SqlScriptsExecutor(destinationConnectionName);
            var sqlScript = AnalyticParamsProvider
                .GetDeleteAnalyticDataScript(TargetAnalyticTable, startDate, endDate, accountId);

            scriptsExecutor.ExecuteSqlCommand(sqlScript);
        }

        private void SaveAnalyticData(DataTable analyticData)
        {
            var scriptsExecutor = new SqlScriptsExecutor(destinationConnectionName);
            scriptsExecutor.BulkSaveDataTable(analyticData, TargetAnalyticTable);
        }
    }
}