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

        private readonly string sourceConnectionName;

        private readonly string destinationConnectioName;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseAnalyticSynchronizer"/> class.
        /// </summary>
        /// <param name="startDate">Start date to synch the analytic data.</param>
        /// <param name="endDate">End date to synch the analytic data.</param>
        protected BaseAnalyticSynchronizer(DateTime startDate, DateTime endDate)
        {
            this.startDate = startDate;
            this.endDate = endDate;
            sourceConnectionName = SynchronizerParamsProvider.GetMainDatabaseConnectionName;
            destinationConnectioName = SynchronizerParamsProvider.GetAnalyticDataConnectionName;
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
            var scriptParams = SynchronizerParamsProvider.GetCommonScriptParams(startDate, endDate);
            var scriptPath = SynchronizerParamsProvider.GetSelectAnalyticDataScriptPath(TargetAnalyticTable);
            return scriptsExecutor.ExecuteSelectScript(scriptPath, scriptParams);
        }

        private void RemoveOldData()
        {
            var scriptsExecutor = new SqlScriptsExecutor(destinationConnectioName);
            var sqlScript = SynchronizerParamsProvider.GetDeleteAnalyticDataScript(TargetAnalyticTable, startDate, endDate);
            scriptsExecutor.ExecuteSqlCommand(sqlScript);
        }

        private void SaveAnalyticData(DataTable analyticData)
        {
            var scriptsExecutor = new SqlScriptsExecutor(destinationConnectioName);
            scriptsExecutor.BulkSaveDataTable(analyticData, TargetAnalyticTable);
        }
    }
}