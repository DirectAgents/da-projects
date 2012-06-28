using System;
using EomAppModels;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace EomApp1.Properties
{
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase
    {
        protected override void OnSettingsLoaded(object sender, System.Configuration.SettingsLoadedEventArgs e)
        {
            // The common setting for the laster database list needs to be set before making any queries.
            EomAppCommon.Settings.MasterDatabaseListConnectionString = this.DAMain1ConnectionString;
            //EomAppCommon.Settings.DebugEomDatabase = true;

            string databaseName = this.DADatabaseName;
            var daDatabase = DADatabase.ByName(databaseName);

            if (daDatabase != null)
            {
                DateTime effectiveDate = daDatabase.effective_date.Value;
                int year = effectiveDate.Year;
                int month = effectiveDate.Month;

                this["DADatabaseR1ConnectionString"] = daDatabase.connection_string;
                this.StatsYear = year;
                this.StatsMonth = month;
                this.StatsDaysInMonth = DateTime.DaysInMonth(year, month);
                this.PubReportSubjectLine = string.Format("{0} Report", databaseName);
            }
            else
            {
                throw new Exception("The currently selected database named " + databaseName + " is not found in the master list of EOM databases.");
            }

            this["DADatabaseMarch11ConnectionString"] = this["DADatabaseR1ConnectionString"];

            if (EomAppCommon.Settings.DebugEomDatabase)
            {
                var form = new System.Windows.Forms.Form();
                var grid = new System.Windows.Forms.DataGridView {
                    AutoGenerateColumns = true,
                    Dock = System.Windows.Forms.DockStyle.Fill,
                    DataSource = 
                        (from c in new [] {
                            Tuple.Create("DADatabaseR1ConnectionString", this.DADatabaseR1ConnectionString),
                            Tuple.Create("StatsYear", this.StatsYear.ToString()),
                            Tuple.Create("StatsMonth", this.StatsMonth.ToString()),
                            Tuple.Create("StatsDaysInMonth", this.StatsDaysInMonth.ToString()),
                            Tuple.Create("PubReportSubjectLine", this.PubReportSubjectLine),
                        } select new {
                            Key = c.Item1,
                            Value = c.Item2,
                        }).ToList(),
                    AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill,
                };
                form.Controls.AddRange(new[] { grid, });
                form.ShowDialog();
            }
        }
        public int StatsYear { get; set; }

        public int StatsMonth { get; set; }

        public string PubReportSubjectLine { get; set; }

        public int StatsDaysInMonth { get; set; }
    }
}
