using System;
using System.Web;
using System.Web.Configuration;
using DAgents.Common;
using EomTool.Domain.Entities;

namespace EomToolWeb.Infrastructure
{
    public class EomEntitiesConfig : IEomEntitiesConfig
    {
        static string EomDateSessionKey = "EomDate";

        public static DateTime EomDate
        {
            get
            {
                var session = HttpContext.Current.Session;
                if (session[EomDateSessionKey] == null)
                {
                    session[EomDateSessionKey] = DateTime.Now.FirstDayOfMonth(-1);
                }
                return (DateTime)session[EomDateSessionKey];
            }
        }

        public string ConnectionString
        {
            get
            {
                return ConnectionStringByDate(EomDate);
            }
        }

        string ConnectionStringByDate(DateTime eomDate)
        {
            var config = WebConfigurationManager.OpenWebConfiguration("/EomToolWeb");
            var eomToolConfig = config.GetSection<EomToolWebConfigSection>();
            string connectionString;

            if (eomToolConfig.DebugMode)
            {
                connectionString = @"data source=biz2\da;initial catalog=zDADatabaseJuly2012Test2;Integrated Security=True";
            }
            else
            {
                string query = "select top 1 connection_string from DADatabase where effective_date = @eomDate";
                connectionString = SqlUtility.ExecuteScalar<string>(eomToolConfig.MasterConnectionString, query, eomDate);
            }

            if (connectionString == null)
                throw new Exception("Cannot determine EOM database connection string for date " + eomDate.ToString());

            if (eomToolConfig.ReplaceIntegratedSecurityWithSALogin)
                connectionString = connectionString.Replace("Integrated Security=True", string.Format("User=sa;Password={0}", eomToolConfig.SAPassword));

            return connectionString;
        }
    }
}