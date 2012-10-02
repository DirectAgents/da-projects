using System;
using System.Web;
using System.Web.Configuration;
using DAgents.Common;
using EomTool.Domain.Entities;

namespace EomToolWeb.Infrastructure
{
    public class EomEntitiesConfig : IEomEntitiesConfig
    {
        public DateTime CurrentEomDate
        {
            get
            {
                var userEomDateFromCookie = HttpContext.Current.Request.Cookies["UserEomDate"];
                DateTime lastMonth = DateTime.Now.FirstDayOfMonth(-1);
                DateTime eomDate;
                if (userEomDateFromCookie == null || !DateTime.TryParse(userEomDateFromCookie.Value, out eomDate))
                {
                    eomDate = lastMonth;
                    HttpCookie cookie = new HttpCookie("UserEomDate");
                    cookie.Value = eomDate.ToString();
                    HttpContext.Current.Response.Cookies.Add(cookie);
                }
                // valid eom dates are between Aud 2012 and last month
                if ((eomDate < new DateTime(2012, 8, 1)) || (eomDate > lastMonth))
                    eomDate = lastMonth;
                return eomDate;
            }
            set
            {
                HttpCookie cookie = new HttpCookie("UserEomDate");
                cookie.Value = value.ToString();
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        public string ConnectionStringByDate(DateTime eomDate)
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