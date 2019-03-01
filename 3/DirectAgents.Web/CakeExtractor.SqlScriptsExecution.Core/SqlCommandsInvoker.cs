using System.Collections.Generic;
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

        private List<string> SplitScriptOnCommands(string sourceScriptContent)
        {
            return Regex.Split(sourceScriptContent, @"^\s*GO\s*$",
                                      RegexOptions.Multiline | RegexOptions.IgnoreCase).ToList();
        }

        private int GetCommandTimeOutInSeconds()
        {
            return 120 * 60; //120 min timeout
        }
    }
}
