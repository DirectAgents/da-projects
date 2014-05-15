using DAgents.Common;
using EomTool.Domain.Abstract;
using System;
using System.Web;

namespace EomToolWeb.Infrastructure
{
    public class EomEntitiesConfig : EomEntitiesConfigBase, IEomEntitiesConfig
    {
        public override DateTime CurrentEomDate
        {
            get
            {
                DateTime thisMonth = DateTime.Now.FirstDayOfMonth();

                var userEomDateFromCookie = HttpContext.Current.Request.Cookies["UserEomDate"];
                DateTime eomDate;
                if (userEomDateFromCookie == null || !DateTime.TryParse(userEomDateFromCookie.Value, out eomDate))
                {
                    eomDate = thisMonth.AddMonths(-1); // default to last month
                    HttpCookie cookie = new HttpCookie("UserEomDate");
                    cookie.Value = eomDate.ToString();
                    HttpContext.Current.Response.Cookies.Add(cookie);
                }
                // valid eom dates are between Aug 2012 and this month
                if ((eomDate < new DateTime(2012, 8, 1)) || (eomDate > thisMonth))
                    eomDate = thisMonth;
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

        public string CurrentEomDateString
        {
            get { return CurrentEomDate.ToString("MMMyyyy") + DebugModeString; }
        }

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

        public bool DebugMode
        {
            get { return EomToolWebConfigSection.GetConfigSection().DebugMode; }
        }
        private string DebugModeString
        {
            get { return (DebugMode ? " [DEBUG MODE]" : String.Empty); }
        }

        public bool DatabaseExistsForDate(DateTime eomDate)
        {
            var eomToolConfig = EomToolWebConfigSection.GetConfigSection();
            if (eomToolConfig.DebugMode) return true;

            string query = "select top 1 connection_string from DADatabase where effective_date = @eomDate and initialized=1";
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
                connectionString = @"data source=biz\sqlexpress;initial catalog=zDADatabase" + month + year + "Test;Integrated Security=True";
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