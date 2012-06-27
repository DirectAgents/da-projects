using System;
namespace EomApp1.Properties
{
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase
    {
        protected override void OnSettingsLoaded(object sender, System.Configuration.SettingsLoadedEventArgs e)
        {
            bool b = false;
            if (this.DADatabaseName == "Dec 10")
            {
                b = true;
                this["DADatabaseR1ConnectionString"] =
                    @"Data Source=biz2\da;Initial Catalog=DADatabaseR1;Integrated Security=True";
                this.StatsYear = 2010;
                this.StatsMonth = 12;
                this.StatsDaysInMonth = 31;
                this.PubReportSubjectLine = "Dec 10 Report";
            }
            else if (this.DADatabaseName == "Jan 11")
            {
                b = true;
                this["DADatabaseR1ConnectionString"] =
                    @"Data Source=biz2\da;Initial Catalog=DADatabaseR2;Integrated Security=True";
                this.StatsYear = 2011;
                this.StatsMonth = 1;
                this.StatsDaysInMonth = 31;
                this.PubReportSubjectLine = "Jan 11 Report";
            }
            else if (this.DADatabaseName == "Feb 11")
            {
                b = true;
                this["DADatabaseR1ConnectionString"] =
                    @"Data Source=biz2\da;Initial Catalog=DADatabaseR3;Integrated Security=True";
                this.StatsYear = 2011;
                this.StatsMonth = 2;
                this.StatsDaysInMonth = 28;
                this.PubReportSubjectLine = "Feb 11 Report";
            }
            else if (this.DADatabaseName == "Mar 11")
            {
                b = true;
                this["DADatabaseR1ConnectionString"] =
                    @"Data Source=biz2\da;Initial Catalog=DADatabaseMarch11;Integrated Security=True";
                this.StatsYear = 2011;
                this.StatsMonth = 3;
                this.StatsDaysInMonth = 31;
                this.PubReportSubjectLine = "Mar 11 Report";
            }
            else if (this.DADatabaseName == "Apr 11")
            {
                b = true;
                this["DADatabaseR1ConnectionString"] =
                    @"Data Source=biz2\da;Initial Catalog=DADatabaseApr11;Integrated Security=True";
                this.StatsYear = 2011;
                this.StatsMonth = 4;
                this.StatsDaysInMonth = 30;
                this.PubReportSubjectLine = "Apr 11 Report";
            }
            else if (this.DADatabaseName == "May 11")
            {
                b = true;
                this["DADatabaseR1ConnectionString"] =
                    @"Data Source=biz2\da;Initial Catalog=DADatabaseMay11;Integrated Security=True";
                this.StatsYear = 2011;
                this.StatsMonth = 5;
                this.StatsDaysInMonth = 31;
                this.PubReportSubjectLine = "May 11 Report";
            }
            else if (this.DADatabaseName == "Jun 11")
            {
                b = true;
                this["DADatabaseR1ConnectionString"] =
                    @"Data Source=biz2\da;Initial Catalog=DADatabaseJun11;Integrated Security=True";
                this.StatsYear = 2011;
                this.StatsMonth = 6;
                this.StatsDaysInMonth = 30;
                this.PubReportSubjectLine = "Jun 11 Report";
            }
            else if (this.DADatabaseName == "Jul 11")
            {
                b = true;
                this["DADatabaseR1ConnectionString"] =
                    @"Data Source=biz2\da;Initial Catalog=DADatabaseJul11;Integrated Security=True";
                this.StatsYear = 2011;
                this.StatsMonth = 7;
                this.StatsDaysInMonth = 31;
                this.PubReportSubjectLine = "Jul 11 Report";
            }
            else if (this.DADatabaseName == "Aug 11")
            {
                b = true;
                this["DADatabaseR1ConnectionString"] =
                    @"Data Source=biz2\da;Initial Catalog=DADatabaseAug11;Integrated Security=True";
                this.StatsYear = 2011;
                this.StatsMonth = 8;
                this.StatsDaysInMonth = 31;
                this.PubReportSubjectLine = "Aug 11 Report";
            }
            else if (this.DADatabaseName == "Sep 11")
            {
                b = true;
                this["DADatabaseR1ConnectionString"] =
                    @"Data Source=biz2\da;Initial Catalog=DADatabaseSep11;Integrated Security=True";
                this.StatsYear = 2011;
                this.StatsMonth = 9;
                this.StatsDaysInMonth = 30;
                this.PubReportSubjectLine = "Sep 11 Report";
            }
            else if (this.DADatabaseName == "Oct 11")
            {
                b = true;
                this["DADatabaseR1ConnectionString"] = @"Data Source=biz2\da;Initial Catalog=DADatabaseOct11;Integrated Security=True";
                this.StatsYear = 2011;
                this.StatsMonth = 10;
                this.StatsDaysInMonth = 31;
                this.PubReportSubjectLine = "Oct 11 Report";
            }
            else if (this.DADatabaseName == "Nov 11")
            {
                b = true;
                this["DADatabaseR1ConnectionString"] = @"Data Source=biz2\da;Initial Catalog=DADatabaseNov11;Integrated Security=True";
                this.StatsYear = 2011;
                this.StatsMonth = 11;
                this.StatsDaysInMonth = 30;
                this.PubReportSubjectLine = "Nov 11 Report";
            }
            else if (this.DADatabaseName == "Dec 11")
            {
                b = true; // this flag prevents horrible tradgedy
                this["DADatabaseR1ConnectionString"] = @"Data Source=biz2\da;Initial Catalog=DADatabaseDec2011;Integrated Security=True";
                this.StatsYear = 2011;
                this.StatsMonth = 12;
                this.StatsDaysInMonth = 31;
                this.PubReportSubjectLine = "Dec 11 Report";
            }
            else if (this.DADatabaseName == "Jan 2012")
            {
                b = true; // this flag prevents horrible tradgedy
                this["DADatabaseR1ConnectionString"] = @"Data Source=biz2\da;Initial Catalog=DADatabaseJan2012;Integrated Security=True";
                this.StatsYear = 2012;
                this.StatsMonth = 1;
                this.StatsDaysInMonth = 31;
                this.PubReportSubjectLine = "Jan 2012 Report";
            }
            else if (this.DADatabaseName == "Feb 2012")
            {
                b = true; // this flag prevents horrible tradgedy
                this["DADatabaseR1ConnectionString"] = @"Data Source=biz2\da;Initial Catalog=DADatabaseFeb2012;Integrated Security=True";
                this.StatsYear = 2012;
                this.StatsMonth = 2;
                this.StatsDaysInMonth = 29;
                this.PubReportSubjectLine = "Feb 2012 Report";
            }
            else if (this.DADatabaseName == "Mar 2012")
            {
                b = true; // this flag prevents horrible tradgedy
                this["DADatabaseR1ConnectionString"] = @"Data Source=biz2\da;Initial Catalog=DADatabaseMarch2012;Integrated Security=True";
                this.StatsYear = 2012;
                this.StatsMonth = 3;
                this.StatsDaysInMonth = 31;
                this.PubReportSubjectLine = "Mar 2012 Report";
            }
            else if (this.DADatabaseName == "Apr 2012 (before Cake)")
            {
                b = true; // this flag prevents horrible tradgedy
                this["DADatabaseR1ConnectionString"] = @"Data Source=biz2\da;Initial Catalog=DADatabaseApril2012_before_cake;Integrated Security=True";
                this.StatsYear = 2012;
                this.StatsMonth = 4;
                this.StatsDaysInMonth = 30;
                this.PubReportSubjectLine = "Apr 2012 Report";
            }
            else if (this.DADatabaseName == "May 2012")
            {
                b = true; // this flag prevents horrible tradgedy
                this["DADatabaseR1ConnectionString"] = @"Data Source=biz2\da;Initial Catalog=DADatabaseMay2012Restored;Integrated Security=True";
                this.StatsYear = 2012;
                this.StatsMonth = 5;
                this.StatsDaysInMonth = 31;
                this.PubReportSubjectLine = "May 2012 Report";
            }
            else if (this.DADatabaseName == "Jun 2012")
            {
                b = true; // this flag prevents horrible tradgedy
                this["DADatabaseR1ConnectionString"] = @"Data Source=biz2\da;Initial Catalog=DADatabaseJune2012;Integrated Security=True";
                this.StatsYear = 2012;
                this.StatsMonth = 6;
                this.StatsDaysInMonth = 30;
                this.PubReportSubjectLine = "Jun 2012 Report";
            }
            if (!b)
            {
                throw new Exception("database configuration incomplete");
            }
            this["DADatabaseMarch11ConnectionString"] = this["DADatabaseR1ConnectionString"];
        }
        public int StatsYear { get; set; }
        public int StatsMonth { get; set; }
        public string PubReportSubjectLine { get; set; }
        public int StatsDaysInMonth { get; set; }
    }
}
