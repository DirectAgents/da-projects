using System;
using System.Web.SessionState;
using System.Web.Configuration;
using System.Configuration;
using System.Data.SqlClient;

namespace EomToolWeb
{
    public static class Extensions
    {
        public static DateTime FirstDayOfMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }

        public static void Set(this HttpSessionState session, string key, object value)
        {
            object currentValue = session[key];
            if (currentValue != null && !currentValue.Equals(value))
                session[key] = value;
        }

        public static string GetConnectionString(this Configuration config, string connectionStringName)
        {
            ConnectionStringSettings connectionString;
            if (config.ConnectionStrings.ConnectionStrings.Count > 0)
            {
                connectionString = config.ConnectionStrings.ConnectionStrings[connectionStringName];
                if (connectionString != null)
                    return connectionString.ConnectionString;
            }
            throw new Exception("Failed to obtain connection string for " + connectionStringName);
        }

        public static SqlConnection GetSqlConnection(this Configuration config, string connectionStringName)
        {
            return new SqlConnection(config.GetConnectionString(connectionStringName));
        }
    }
}