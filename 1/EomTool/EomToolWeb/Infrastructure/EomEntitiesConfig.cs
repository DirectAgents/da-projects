using System;
using System.Web;
using System.Web.Configuration;
using DAgents.Common;
using EomTool.Domain.Entities;

namespace EomToolWeb.Infrastructure
{
    public class EomEntitiesConfig : EomEntitiesConfigBase, IEomEntitiesConfig
    {
        public override DateTime CurrentEomDate
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
                // valid eom dates are between Aug 2012 and last month
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
    }

    public class EomEntitiesConfigBase : IEomEntitiesConfig
    {
        public virtual DateTime CurrentEomDate { get; set; }

        public string ConnectionString
        {
            get
            {
                var dateTime = CurrentEomDate;
                if (!DatabaseExistsForDate(dateTime))
                {   // try the previous month, in case it's the first of the month and the db hasn't been created for the month that just ended
                    dateTime = dateTime.AddMonths(-1);
                    CurrentEomDate = dateTime;
                }
                var connString = ConnectionStringByDate(dateTime);
                return connString;
            }
        }

        public bool DatabaseExistsForDate(DateTime eomDate)
        {
            var eomToolConfig = EomToolWebConfigSection.GetConfigSection();
            if (eomToolConfig.DebugMode) return true;

            string query = "select top 1 connection_string from DADatabase where effective_date = @eomDate";
            var connectionString = SqlUtility.ExecuteScalar<string>(eomToolConfig.MasterConnectionString, query, eomDate);
            return (connectionString != null);
        }

        public string ConnectionStringByDate(DateTime eomDate)
        {
            var eomToolConfig = EomToolWebConfigSection.GetConfigSection();
            string connectionString;

            if (eomToolConfig.DebugMode)
            {
                var year = eomDate.Year;
                var month = eomDate.ToString("MMM");
                connectionString = @"data source=biz2\da;initial catalog=zDADatabase" + month + year + "Test;Integrated Security=True";
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