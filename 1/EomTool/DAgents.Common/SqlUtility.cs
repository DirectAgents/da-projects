using System.Data.SqlClient;

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
    }
}
