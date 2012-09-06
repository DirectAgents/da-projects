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

        public string ConnectionString
        {
            get
            {
                var session = HttpContext.Current.Session;
                var eomDate = session[EomDateSessionKey] == null ? DateTime.Now.FirstDayOfMonth(-1) : (DateTime)session[EomDateSessionKey];
                session.Set(EomDateSessionKey, eomDate);
                return ConnectionStringByDate(eomDate);
            }
        }

        string ConnectionStringByDate(DateTime eomDate)
        {
            var config = WebConfigurationManager.OpenWebConfiguration("/EomToolWeb");
            var eomToolConfig = config.GetSection<EomToolWebConfigSection>();
            
            string query = "select top 1 connection_string from DADatabase where effective_date = @eomDate";
            string connectionString = SqlUtility.ExecuteScalar<string>(eomToolConfig.MasterConnectionString, query, eomDate);
            
            if (connectionString == null)
                throw new Exception("Cannot determine EOM database connection string for date " + eomDate.ToString());
            
            if (eomToolConfig.ReplaceIntegratedSecurityWithSALogin)
                connectionString = connectionString.Replace("Integrated Security=True", string.Format("User=sa;Password={0}", eomToolConfig.SAPassword));
            
            return connectionString;
        }
    }
}