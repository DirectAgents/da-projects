using System;
using System.Linq;
using EomAppModels;

namespace EomApp1.Properties
{
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase
    {
        protected override void OnSettingsLoaded(object sender, System.Configuration.SettingsLoadedEventArgs e)
        {
            // The common setting specifying the list of databases needs to be set before making any queries.
            EomAppCommon.Settings.MasterDatabaseListConnectionString = this.DAMain1ConnectionString;
            string name = this.DADatabaseName;
            var database = DADatabase.ByName(name);

            if (database != null)
            {
                DateTime effectiveDate = database.effective_date.Value;
                int year = effectiveDate.Year;
                int month = effectiveDate.Month;
                this["DADatabaseR1ConnectionString"] = database.connection_string;
                this.StatsYear = year;
                this.StatsMonth = month;
                this.StatsDaysInMonth = DateTime.DaysInMonth(year, month);
                this.PubReportSubjectLine = string.Format("{0} Report", name);
            }
            else
            {
                throw new Exception("The currently selected database named " + name + " is not found in the master list of EOM databases.");
            }

            // TODO: find and remove references
            this["DADatabaseMarch11ConnectionString"] = this["DADatabaseR1ConnectionString"];

#if DEBUG
            if (EomAppCommon.Settings.DebugEomDatabase)
            {
                var form = new System.Windows.Forms.Form();
                var grid = new System.Windows.Forms.DataGridView {
                    AutoGenerateColumns = true,
                    Dock = System.Windows.Forms.DockStyle.Fill,
                    DataSource =
                        (from c in new[] {
                            Tuple.Create("DADatabaseR1ConnectionString", this.DADatabaseR1ConnectionString),
                            Tuple.Create("StatsYear", this.StatsYear.ToString()),
                            Tuple.Create("StatsMonth", this.StatsMonth.ToString()),
                            Tuple.Create("StatsDaysInMonth", this.StatsDaysInMonth.ToString()),
                            Tuple.Create("PubReportSubjectLine", this.PubReportSubjectLine),
                        }
                         select new
                         {
                             Key = c.Item1,
                             Value = c.Item2,
                         }).ToList(),
                    AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill,
                };
                form.Controls.AddRange(new[] { grid, });
                form.ShowDialog();
            }
#endif
        }

        public int StatsYear { get; set; }

        public int StatsMonth { get; set; }

        public string PubReportSubjectLine { get; set; }

        public int StatsDaysInMonth { get; set; }
    }
}
