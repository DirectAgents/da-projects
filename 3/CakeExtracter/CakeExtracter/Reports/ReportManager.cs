using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ClientPortal.Data.Contexts;
using ClientPortal.Data.Contracts;

// TODO: separate project
namespace CakeExtracter.Reports
{
    public class ReportManager
    {
        private readonly IClientPortalRepository cpRepo;
        private readonly GmailEmailer emailer;
        private readonly DbContext dbContext;
        private readonly string reportEmailFromAddress;
        private readonly string reportEmailSubject;

        public ReportManager(DbContext db, IClientPortalRepository cpRepo, GmailEmailer emailer, string reportEmailFromAddress, string reportEmailSubject)
        {
            this.cpRepo = cpRepo;
            this.emailer = emailer;
            this.dbContext = db;
            this.reportEmailFromAddress = reportEmailFromAddress;
            this.reportEmailSubject = reportEmailSubject;
        }

        /// <summary>
        /// Go through all advertisers with automated reports enabled and check if they are due
        /// to receive automated reports.  If so, generate and send them.  Finally, update
        /// the next time to send them.
        /// </summary>
        public void CatchUp()
        {
            foreach (var advertiser in GetAutoReportingAdvertisers())
            {
                var report = this.SelectReport(advertiser);
                this.SendReport(advertiser, report);
                this.UpdateAdvertiser(advertiser);
            }
        }

        private IEnumerable<Advertiser> GetAutoReportingAdvertisers()
        {
            return cpRepo.Advertisers
                         .Where(c => c.AutomatedReportsEnabled)
                         .ToList()
                         .Where(c => DateTime.Now > c.AutomatedReportsNextSendAfter);
        }

        // TODO: switch to a day of the week instead of a constant period
        private void UpdateAdvertiser(Advertiser advertiser)
        {
            advertiser.AutomatedReportsNextSendAfter = advertiser.AutomatedReportsNextSendAfter.Value.AddDays(advertiser.AutomatedReportsPeriodDays);
            this.dbContext.SaveChanges();
        }

        private void SendReport(Advertiser advertiser, IReport report)
        {
            this.emailer.SendEmail(
                            this.reportEmailFromAddress,
                            new[] { advertiser.AutomatedReportsDestinationEmail },
                            null,
                            this.reportEmailSubject,
                            report.Generate(),
                            true);
        }

        private IReport SelectReport(Advertiser advertiser)
        {
            IReport report = null;
            if (advertiser.HasSearch)
            {
                report = new SearchReport(cpRepo, advertiser);
            }
            else
            {
                report = new CakeReport(cpRepo, advertiser);
            }
            return report;
        }
    }
}
