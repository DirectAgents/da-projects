using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;

namespace DAgents.Common
{
    public static class SqlBatchUtil
    {
        static readonly ILogger Logger = new ConsoleLogger();

        /// <summary>
        /// Uses default separator: "GO"
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="batch"></param>
        public static void Execute(string connectionString, string batch)
        {
            Run(connectionString, batch, "GO");
        }

        /// <summary>
        /// Uses default separator: "GO"
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="files"></param>
        public static void ExecuteFiles(string connectionString, string[] files)
        {
            foreach (var file in files)
                ExecuteFile(connectionString, file);
        }

        /// <summary>
        /// Uses default separator: "GO"
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="file"></param>
        public static void ExecuteFile(string connectionString, string file)
        {
            Run(connectionString, File.ReadAllText(file), "GO");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="file"></param>
        /// <param name="separator"></param>
        public static void ExecuteFile(string connectionString, string file, string separator)
        {
            Run(connectionString, File.ReadAllText(file), separator);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="batch"></param>
        /// <param name="separator"></param>
        static void Run(string connectionString, string batch, string separator)
        {
            using (var conection = new SqlConnection(connectionString))
            {
                conection.Open();
                Run(batch, conection, separator);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="script"></param>
        /// <param name="sqlConnection"></param>
        /// <param name="separator"></param>
        static void Run(string script, SqlConnection sqlConnection, string separator)
        {
            var parts = Regex.Split(script, separator);

            foreach (var part in parts)
            {
                string sql = part.Trim();

                if (sql.Length < 1) continue;

                SqlCommand cmd = sqlConnection.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;

                Logger.Log("Executing SQL" + sql);

                int rc = cmd.ExecuteNonQuery();

                Logger.Log("Execution result is " + rc);
            }
        }
    }
}
