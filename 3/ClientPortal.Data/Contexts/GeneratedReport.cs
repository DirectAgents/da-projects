//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClientPortal.Data.Contexts
{
    using System;
    using System.Collections.Generic;
    
    public partial class GeneratedReport
    {
        public GeneratedReport()
        {
            this.EmailedReports = new HashSet<EmailedReport>();
        }
    
        public int Id { get; set; }
        public ReportType ReportType { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public ReportStatus ReportStatus { get; set; }
        public Nullable<System.DateTime> ReportStatusUpdated { get; set; }
        public string Content { get; set; }
        public int ScheduledReportId { get; set; }
    
        public virtual ScheduledReport ScheduledReport { get; set; }
        public virtual ICollection<EmailedReport> EmailedReports { get; set; }
    }
}
