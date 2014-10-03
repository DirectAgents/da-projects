using ClientPortal.Data.Contexts;
using ClientPortal.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CakeExtracter.Reports
{
    public class SimpleReportManager
    {
        private const int DefaultPeriodDays = 7;
        private const DayOfWeek DefaultStartDayOfWeek = DayOfWeek.Monday; //TODO: make app setting

        private readonly IClientPortalRepository cpRepo;
        private readonly GmailEmailer emailer;

        public SimpleReportManager(IClientPortalRepository cpRepo, GmailEmailer emailer)
        {
            this.cpRepo = cpRepo;
            this.emailer = emailer;
        }

        // returns # of reports sent
        public int CatchUp()
        {
            int totalReportsSent = 0;
            var reportsToSend = GetReportsToSend();
            while (reportsToSend.Count() > 0)
            {
                foreach (var simpleReport in reportsToSend)
                {
                    CheckInitialize(simpleReport);

                    bool sentSomething = false;
                    if (AbleToSend(simpleReport))
                    {
                        var iReports = GetIReports(simpleReport);
                        foreach (var iReport in iReports)
                        {
                            if (SendReport(simpleReport, iReport))
                            {
                                sentSomething = true;
                                totalReportsSent++;
                            }
                        }
                        if (sentSomething)
                            UpdateLastAndNextSend(simpleReport);
                    }
                    if (!sentSomething)
                    {
                        // Couldn't send. Disable.
                        simpleReport.Enabled = false;
                        cpRepo.SaveChanges();
                    }
                }
                // see if any more reports need to be sent
                reportsToSend = GetReportsToSend();
            }
            Cleanup();
            return totalReportsSent;
        }

        private IEnumerable<SimpleReport> GetReportsToSend()
        {
            var now = DateTime.Now;
            var reportsToSend = cpRepo.SimpleReports.Where(r => r.Enabled && (r.NextSend == null || now >= r.NextSend));
            return reportsToSend.ToList();
        }

        // Set NextSend to the day after the end of the last stats period.
        // Also initializes PeriodDays if PeriodMonths and PeriodDays are both 0.
        private void CheckInitialize(SimpleReport simpleReport)
        {
            if (simpleReport.NextSend == null)
            {
                var today = DateTime.Today;
                if (simpleReport.PeriodMonths > 0)
                {
                    // Set to the 1st of this month, so last month's stats will be sent
                    simpleReport.NextSend = new DateTime(today.Year, today.Month, 1);
                }
                else
                {
                    simpleReport.NextSend = today;

                    if (simpleReport.PeriodDays <= 0)
                        simpleReport.PeriodDays = DefaultPeriodDays;

                    DayOfWeek? startDayOfWeek = simpleReport.GetStartDayOfWeek();

                    if (startDayOfWeek.HasValue && simpleReport.PeriodDays == 7)
                    {   // Set to the StartDayOfWeek that's today or most recently passed
                        while (simpleReport.NextSend.Value.DayOfWeek != startDayOfWeek.Value)
                        {
                            simpleReport.NextSend = simpleReport.NextSend.Value.AddDays(-1);
                        }
                    }
                }
                cpRepo.SaveChanges();
            }
        }

        private bool AbleToSend(SimpleReport simpleReport)
        {
            if (String.IsNullOrWhiteSpace(simpleReport.Email))
                return false;

            return true;
        }

        // the IReports are the objects used to generate the report text
        private IEnumerable<IReport> GetIReports(SimpleReport simpleReport)
        {
            List<IReport> iReports = new List<IReport>();

            if (simpleReport.Advertiser != null)
            {
                iReports.Add(new CakeReport(cpRepo, simpleReport.Advertiser, simpleReport.NextSend.Value.AddDays(-1), simpleReport.PeriodDays));
            }
            if (simpleReport.SearchProfile != null)
            {
                iReports.Add(new SearchReport(cpRepo, simpleReport));
            }
            return iReports;
        }

        private bool SendReport(SimpleReport simpleReport, IReport report)
        {
            try
            {
                var reportString = report.Generate();
                this.emailer.SendEmail(
                                emailer.Credential.UserName,
                                new[] { simpleReport.Email },
                                simpleReport.EmailCC == null ? null : new[] { simpleReport.EmailCC },
                                report.Subject,
                                reportString,
                                true);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return false;
            }
        }

        private void UpdateLastAndNextSend(SimpleReport rep)
        {
            var today = DateTime.Today;
            rep.LastSend = DateTime.Now;

            rep.LastStatsDate = rep.NextSend ?? today;
            rep.LastStatsDate = rep.LastStatsDate.Value.AddDays(-1);

            rep.NextSend = rep.NextSend ?? today; // to ensure it's not null
            if (rep.PeriodMonths > 0)
            {
                rep.NextSend = rep.NextSend.Value.AddMonths(rep.PeriodMonths);
            }
            else
            {
                int days = rep.PeriodDays;
                if (days <= 0)
                    days = DefaultPeriodDays;
                rep.NextSend = rep.NextSend.Value.AddDays(days);
            }
            cpRepo.SaveChanges();
        }

        private void Cleanup()
        {
            var today = DateTime.Today;
            var disabledReportsInThePast = cpRepo.SimpleReports.Where(sr => !sr.Enabled && sr.NextSend < today);
            foreach (var rep in disabledReportsInThePast)
            {
                rep.NextSend = null;
                // so that multiple reports won't be sent unexpectedly once re-enabled
            }
            cpRepo.SaveChanges();
        }
    }
}
