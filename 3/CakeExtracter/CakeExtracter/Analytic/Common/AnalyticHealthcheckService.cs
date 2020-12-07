using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Common;

namespace CakeExtracter.Analytic.Common
{
    /// <summary>
    /// Service to maintain analytic tables.
    /// </summary>
    public class AnalyticHealthCheckService
    {
        private const int ExpectedDelayInDays = 1;

        private readonly SqlScriptsExecutor sqlScriptsExecutor;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnalyticHealthCheckService"/> class.
        /// </summary>
        public AnalyticHealthCheckService()
        {
            var connectionName = AnalyticParamsProvider.GetAnalyticDataConnectionName;
            sqlScriptsExecutor = new SqlScriptsExecutor(connectionName);
        }

        /// <summary>
        /// Gets a IEnumerable collection of not updated tables.
        /// </summary>
        /// <returns>IEnumerable collection of not updated tables.</returns>
        public IEnumerable<string> GetNotUpdatedTables()
        {
            return from table in AnalyticParamsProvider.GetAnalyticTables
                let script = AnalyticParamsProvider.SelectMaxAvailableDateScript(table)
                let lastUpdateDate = sqlScriptsExecutor.GetScalarResult<DateTime>(script)
                where lastUpdateDate.Subtract(DateTime.Today).Days > ExpectedDelayInDays
                select table;
        }

    }
}