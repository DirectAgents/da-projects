using ClientPortal.Data.Contexts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClientPortal.Web.Models
{
//    public enum ReportFrequencyEnum { Custom = 0, Daily, Weekly, Monthly };
    public enum ReportFrequencyEnum { Daily = 1, Weekly, Monthly };

    public class ScheduledReportVM //: IValidatableObject
    {
        public int Id { get; set; }

        [Display(Name="Type")]
        public ReportType ReportType { get; set; }

        public int Months { get; set; }
        public int Days { get; set; }
        public ReportFrequencyEnum Frequency
        {
            get
            {
//                var frequency = ReportFrequencyEnum.Custom;
                var frequency = ReportFrequencyEnum.Daily;
                if (Months == 0)
                {
                    if (Days == 1)
                        frequency = ReportFrequencyEnum.Daily;
                    else if (Days == 7)
                        frequency = ReportFrequencyEnum.Weekly;
                }
                else if (Months == 1 && Days == 0)
                    frequency = ReportFrequencyEnum.Monthly;

                return frequency;
            }
            set
            {
                switch (value)
                {
                    case ReportFrequencyEnum.Monthly:
                        Months = 1;
                        Days = 0;
                        break;
                    case ReportFrequencyEnum.Weekly:
                        Months = 0;
                        Days = 7;
                        break;
                    default:
                        Months = 0;
                        Days = 1;
                        break;
                }
            }
        }

        [Display(Name="Cumulative?")]
        public bool IsCumulative { get; set; }


        // Constructors
        public ScheduledReportVM()
        { // defaults
            this.Id = -1;
            this.ReportType = ReportType.OfferSummary;
            this.Months = 0;
            this.Days = 1;
        }
        public ScheduledReportVM(ScheduledReport rep)
        {
            this.Id = rep.Id;

//            this.ReportType = rep.ReportType;
            this.ReportType = ReportType.OfferSummary; // temp!

            this.Months = rep.Months;
            this.Days = rep.Days;
            this.IsCumulative = rep.IsCumulative;
        }

        public void SetEntityProperties(ScheduledReport rep)
        {
//            rep.ReportType = this.ReportType;
            rep.Months = this.Months;
            rep.Days = this.Days;
            rep.IsCumulative = this.IsCumulative;
        }
    }
}