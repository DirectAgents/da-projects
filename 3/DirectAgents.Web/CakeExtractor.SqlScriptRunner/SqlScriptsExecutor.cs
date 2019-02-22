using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;

namespace CakeExtractor.SqlScriptsExecutor
{
    public class SqlScriptsExecutor
    {
        private string sqlConnectionString;

        public SqlScriptsExecutor(string sqlConnectionString)
        {
            this.sqlConnectionString = sqlConnectionString;
        }

        public bool RunSqlCommands(string commandsText)
        {
            try
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
                                try
                                {
                                    command.ExecuteNonQuery();
                                }
                                catch (SqlException ex)
                                {
                                    Console.WriteLine(string.Format("Please check the SqlServer script. Error: {0}", ex.Message));
                                    return false;
                                }
                            }
                        }
                    }
                    connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        private List<string> SplitScriptOnCommands(string sourceScriptContent)
        {
           return  Regex.Split(sourceScriptContent, @"^\s*GO\s*$",
                                     RegexOptions.Multiline | RegexOptions.IgnoreCase).ToList();
        }

        private int GetCommandTimeOutInSeconds()
        {
            return 120 * 60;
        }
    }
}
