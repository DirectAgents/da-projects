using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;

namespace CakeExtractor.SqlScriptsExecution.Core
{
    /// <summary>
    /// Sql commands invoker.
    /// </summary>
    public class SqlCommandsInvoker
    {
        private const string ScriptSplittingPattern = @"^\s*GO\s*$";

        private const int DefaultBatchSize = 10000;

        private string sqlConnectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlCommandsInvoker"/> class.
        /// </summary>
        /// <param name="sqlConnectionString">The SQL connection string.</param>
        public SqlCommandsInvoker(string sqlConnectionString)
        {
            this.sqlConnectionString = sqlConnectionString;
        }

        /// <summary>
        /// Runs the SQL commands.
        /// </summary>
        /// <param name="commandsText">The commands text.</param>
        /// <returns>Flag whether sql commands were executed successfully.</returns>
        public bool RunSqlCommands(string commandsText)
        {
            using (SqlConnection connection = new SqlConnection(sqlConnectionString))
            {
                connection.Open();
                var sqlCommands = SplitScriptOnCommands(commandsText);
                foreach (string commandString in sqlCommands)
                {
                    if (commandString.Trim() != "")
                    {
                        using (var command = new SqlCommand(commandString, connection))
                        {
                            command.CommandTimeout = GetCommandTimeOutInSeconds();
                            command.ExecuteNonQuery();
                        }
                    }
                }
                connection.Close();
            }
            return true;
        }

        /// <summary>
        /// Runs sql query and put result into DataTable.
        /// </summary>
        /// <param name="commandText">Sql query.</param>
        /// <returns>Selected data.</returns>
        public DataTable SelectSqlDataAsDataTable(string commandText)
        {
            var dataTable = new DataTable();
            using (var connection = new SqlConnection(sqlConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(commandText, connection))
                {
                    command.CommandTimeout = GetCommandTimeOutInSeconds();
                    var dataReader = command.ExecuteReader();
                    dataTable.Load(dataReader);
                }
                connection.Close();
            }

            return dataTable;
        }

        /// <summary>
        /// Runs sql query and return a scalar result.
        /// </summary>
        /// <param name="commandText">Sql query.</param>
        /// <typeparam  name="T">Expected type of a result.</typeparam>
        /// <returns>Scalar result of the query.</returns>
        public T ExecuteAsScalar<T>(string commandText)
        {
            T result;
            using (var connection = new SqlConnection(sqlConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(commandText, connection))
                {
                    result = (T)command.ExecuteScalar();
                }
                connection.Close();
            }

            return result;
        }

        /// <summary>
        /// Runs the data saving using bulk operation.
        /// </summary>
        /// <param name="data">The data to save.</param>
        /// <param name="targetTable">The target table name.</param>
        public void RunBulkSave(DataTable data, string targetTable)
        {
            using (var bulkOperation = new SqlBulkCopy(sqlConnectionString))
            {
                bulkOperation.BatchSize = DefaultBatchSize;
                bulkOperation.BulkCopyTimeout = GetCommandTimeOutInSeconds();
                bulkOperation.DestinationTableName = targetTable;
                bulkOperation.WriteToServer(data);
            }
        }

        private static List<string> SplitScriptOnCommands(string sourceScriptContent)
        {
            return Regex.Split(
                    sourceScriptContent,
                    ScriptSplittingPattern,
                    RegexOptions.Multiline | RegexOptions.IgnoreCase)
                .ToList();
        }

        private static int GetCommandTimeOutInSeconds()
        {
            return 120 * 60; //120 min timeout
        }
    }
}
