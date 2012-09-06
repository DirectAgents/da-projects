using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Collections;

namespace DAgents.Common
{
    public static class SqlUtility
    {
        public static void ExecuteNonQuery(string sql)
        {
            using (var con = new SqlConnection(EomAppCommon.EomAppSettings.ConnStr))
            using (var cmd = new SqlCommand(sql, con))
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public static T ExecuteScalar<T>(string connStr, string query, params object[] queryParams)
        {
            using (var con = new SqlConnection(connStr))
            using (var cmd = new SqlCommand(query, con))
            {
                var queue = new Queue(queryParams);
                var matches = Regex.Matches(query, @"@\w+");
                foreach (Match match in matches)
                    cmd.Parameters.AddWithValue(match.Value, queue.Dequeue());
                con.Open();
                T result = (T)cmd.ExecuteScalar();
                return result;
            }
        }
    }
}
