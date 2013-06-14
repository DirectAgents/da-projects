using ClientPortal.Data.Contexts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace ClientPortal.Web.Models
{
//    public enum ReportFrequencyEnum { Custom = 0, Daily, Weekly, Monthly };
    public enum ReportFrequencyEnum { Daily = 1, Weekly, Monthly };

    public class ScheduledReportVM //: IValidatableObject
    {
        public int Id { get; set; }

        [Display(Name="Report Type")]
        public ReportType ReportType { get; set; }

        public string ReportTypeString {
            get { return SplitCamelCase(Enum.GetName(typeof(ReportType), ReportType)); }
        }

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

        [Display(Name="Cumulative for Month?")]
        public bool IsCumulative { get; set; }

        [Display(Name="Email Recipients")]
        public string[] Recipients { get; set; }


        // Constructors
        public ScheduledReportVM()
        { // defaults
            this.Id = -1;
            this.ReportType = ReportType.OfferSummary;
            this.Months = 0;
            this.Days = 1;
            this.Recipients = new string[] { };
        }
        public ScheduledReportVM(ScheduledReport rep)
        {
            this.Id = rep.Id;

            this.ReportType = rep.ReportType;

            this.Months = rep.Months;
            this.Days = rep.Days;
            this.IsCumulative = rep.IsCumulative;

            this.Recipients = rep.ScheduledReportRecipients.Select(r => r.EmailAddress).ToArray();
        }

        public void SetEntityProperties(ScheduledReport reportEntity, out List<ScheduledReportRecipient> recipientsToDelete)
        {
            reportEntity.ReportType = this.ReportType;
            reportEntity.Months = this.Months;
            reportEntity.Days = this.Days;
            reportEntity.IsCumulative = this.IsCumulative;

            recipientsToDelete = new List<ScheduledReportRecipient>();

            // Loop through "new" recipients. See what's changed
            int i;
            for (i = 0; i < Recipients.Length; i++)
            {
                if (i >= reportEntity.ScheduledReportRecipients.Count)
                {
                    var scheduledReportRecipient = new ScheduledReportRecipient() { EmailAddress = Recipients[i] };
                    reportEntity.ScheduledReportRecipients.Add(scheduledReportRecipient);
                }
                else
                {
                    if (Recipients[i] != reportEntity.ScheduledReportRecipients.ElementAt(i).EmailAddress)
                    { // remove all from here on, in the existing entity
                        for (int j = reportEntity.ScheduledReportRecipients.Count - 1; j >= i; j--)
                        {
                            var recipient = reportEntity.ScheduledReportRecipients.ElementAt(j);
                            recipientsToDelete.Add(recipient);
                            reportEntity.ScheduledReportRecipients.Remove(recipient);
                        }
                        var scheduledReportRecipient = new ScheduledReportRecipient() { EmailAddress = Recipients[i] };
                        reportEntity.ScheduledReportRecipients.Add(scheduledReportRecipient);
                    }
                }
            }
            // See if existing recipients need to be removed
            if (i < reportEntity.ScheduledReportRecipients.Count)
            {
                // TODO: consolidate duplicate code
                for (int j = reportEntity.ScheduledReportRecipients.Count - 1; j >= i; j--)
                {
                    var recipient = reportEntity.ScheduledReportRecipients.ElementAt(j);
                    recipientsToDelete.Add(recipient);
                    reportEntity.ScheduledReportRecipients.Remove(recipient);
                }
            }
        }

        public static string SplitCamelCase(string input)
        {
            return Regex.Replace(input, "([A-Z])", " $1", RegexOptions.Compiled).Trim();
        }

        public static IEnumerable<SelectListItem> CamelEnumSelectList(Type enumType)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var names = Enum.GetNames(enumType);
            foreach (var name in names)
            {
                list.Add(new SelectListItem() { Value = name, Text = SplitCamelCase(name) });
            }
            return list;
        }
    }
}