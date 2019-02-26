using CakeExtracter;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;

namespace CakeExtractor.SqlScriptsExecution.Core
{
    public class SqlCommandsInvoker
    {
        private string sqlConnectionString;

        public SqlCommandsInvoker(string sqlConnectionString)
        {
            this.sqlConnectionString = sqlConnectionString;
        }

        public bool RunSqlCommands(string commandsText)
        {
            Logger.Info("Started sql command execution");
            try
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();
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
                watch.Stop();
                Logger.Info($"Finished Sql command execution. Execution time: {watch.Elapsed}");
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return false;
            }
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
