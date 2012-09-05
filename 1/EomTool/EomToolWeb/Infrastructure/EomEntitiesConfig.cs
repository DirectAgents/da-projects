using System;
using System.Data.SqlClient;
using System.Web;
using System.Web.Configuration;
using EomTool.Domain.Entities;

namespace EomToolWeb.Infrastructure
{
    public class EomEntitiesConfig : IEomEntitiesConfig
    {
        public string ConnectionString
        {
            get
            {
                string eomDateSessionString = "EomDate";

                var session = HttpContext.Current.Session;

                DateTime eomDate = session[eomDateSessionString] == null
                    ? DateTime.Now.FirstDayOfMonth()
                    : (DateTime)session[eomDateSessionString];

                session.Set(eomDateSessionString, eomDate);

                return ConnectionStringByDate(eomDate);
            }
        }

        private string ConnectionStringByDate(DateTime eomDate)
        {

            using (var con = WebConfigurationManager.OpenWebConfiguration(null).GetSqlConnection("DAMain1"))
            using (var cmd = new SqlCommand("select top 1 connection_string from DADatabase where effective_date = @eomDate", con))
            {
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@eomDate",
                    SqlDbType = System.Data.SqlDbType.DateTime,
                    Value = eomDate
                });

                string connectionString = (string)cmd.ExecuteScalar();

                return connectionString;
            }
        }
    }
}