using System;
using System.Configuration;

using SeleniumDataBrowser.Helpers;

namespace CakeExtracter.Analytic.Common
{
    /// <summary>
    /// Provides formatted parameters for synchronizer.
    /// </summary>
    internal static class SynchronizerParamsProvider
    {
        private const string SelectAnalyticDataScriptNamePattern = "{0}_SelectAnalyticData.sql";

        private const string BaseScriptPathParam = "AnalyticScriptsPath";

        private const string DeleteAnalyticDataScriptTemplate = "DELETE FROM {0} WHERE Date >= '{1}' AND Date <= '{2}'";

        private const string DateFormat = "yyyy-MM-dd";

        /// <summary>
        /// Gets the connection string to the AnalyticData database.
        /// </summary>
        public static string GetAnalyticDataConnectionName => "AnalyticData";

        /// <summary>
        /// Gets the connection string to the main database.
        /// </summary>
        public static string GetMainDatabaseConnectionName => "ClientPortalProgContext";

        /// <summary>
        /// Gets the sql script path to select analytic data depending on the target table.
        /// </summary>
        /// <param name="targetTable">The target table name to select data.</param>
        /// <returns>Sql script path.</returns>
        public static string GetSelectAnalyticDataScriptPath(string targetTable)
        {
            var scriptName = string.Format(SelectAnalyticDataScriptNamePattern, targetTable);
            var baseScriptPath = ConfigurationManager.AppSettings[BaseScriptPathParam];
            return PathToFileDirectoryHelper.GetAssemblyRelativePath(PathToFileDirectoryHelper.CombinePath(baseScriptPath, scriptName));
        }

        /// <summary>
        /// Gets common script params formatted as strings array.
        /// </summary>
        /// <param name="startDate">The start date parameter to synch data.</param>
        /// <param name="endDate">The end date parameter to synch data.</param>
        /// <param name="accountId">The account identifier to synch data (null = all accounts).</param>
        /// <returns>Common script params formatted as strings array.</returns>
        public static string[] GetCommonScriptParams(DateTime startDate, DateTime endDate, int? accountId = null)
        {
            if (accountId != null)
            {
                return new[] { accountId.ToString(), startDate.ToString(DateFormat), endDate.ToString(DateFormat) };
            }

            return new[] { startDate.ToString(DateFormat), endDate.ToString(DateFormat) };
        }

        /// <summary>
        /// Gets sql command to delete analytic data depending on the start date, end date and target table.
        /// </summary>
        /// <param name="targetTable">The name of the target table.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="accountId">The account identifier (null = all accounts).</param>
        /// <returns>Sql command.</returns>
        public static string GetDeleteAnalyticDataScript(
            string targetTable,
            DateTime startDate,
            DateTime endDate,
            int? accountId = null)
        {
            var deleteDataScript = string.Format(
                DeleteAnalyticDataScriptTemplate,
                targetTable,
                startDate.ToString(DateFormat),
                endDate.ToString(DateFormat));

            if (accountId != null)
            {
                deleteDataScript += $"AND AccountId = {accountId}";
            }

            return deleteDataScript;
        }
    }
}