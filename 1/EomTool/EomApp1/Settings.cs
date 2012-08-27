using System;
using System.Linq;
using DAgents.Common;
using EomAppModels;

namespace EomApp1.Properties
{
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase
    {
        protected override void OnSettingsLoaded(object sender, System.Configuration.SettingsLoadedEventArgs e)
        {
            EomAppCommon.EomAppSettings.MasterDatabaseListConnectionString = this.DAMain1ConnectionString;
            EomAppCommon.EomAppSettings.SettingsConnectionString = this.SettingsConnectionString;

            if (IsTestMode())
            {
                string name = "Test";
                SetSettings(this.OverrideConnectionString, this.OverrideDate, name);
                this.DADatabaseName = name;
                this["EomToolSecurityConnectionString"] = this.OverrideEomToolSecurityConnectionString;
            }
            else
            {
                if (this.DADatabaseName == "Test")
                    this.DADatabaseName = "Dec 10";

                string name = this.DADatabaseName;
                var database = DADatabase.ByName(name);

                if (database != null)
                {
                    SetSettings(database.connection_string, database.effective_date.Value, name);
                }
                else
                {
                    throw new Exception("The currently selected database named " + name + " is not found in the master list of EOM databases.");
                }
            }

            // TODO: find and remove references
            this["DADatabaseMarch11ConnectionString"] = this["DADatabaseR1ConnectionString"];
        }

        private bool IsTestMode()
        {
            return (OverrideConnectionHasValue() && IsValidTestUser() && this.TestMode);
        }

        private bool OverrideConnectionHasValue()
        {
            return !String.IsNullOrWhiteSpace(this.OverrideConnectionString);
        }

        private bool IsValidTestUser()
        {
            var testUsers = this.TestUserIdentities.Split(',');
            string identity = WindowsIdentityHelper.GetWindowsIdentityNameLower();
            bool found = testUsers.Any(c => c == identity);
            return found;
        }

        private void SetSettings(string connectionString, DateTime effectiveDate, string reportName)
        {
            int year = effectiveDate.Year;
            int month = effectiveDate.Month;
            this["DADatabaseR1ConnectionString"] = connectionString;
            this.StatsYear = year;
            this.StatsMonth = month;
            this.StatsDaysInMonth = DateTime.DaysInMonth(year, month);
            this.PubReportSubjectLine = string.Format("{0} Report", reportName);
        }

        public int StatsYear { get; set; }

        public int StatsMonth { get; set; }

        public string PubReportSubjectLine { get; set; }

        public int StatsDaysInMonth { get; set; }
    }
}
