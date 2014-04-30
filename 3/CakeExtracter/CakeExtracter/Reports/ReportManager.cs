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
        private readonly string reportEmailFromAddress;
        private readonly string reportEmailSubject;

        public ReportManager(IClientPortalRepository cpRepo, GmailEmailer emailer, string reportEmailFromAddress, string reportEmailSubject)
        {
            this.cpRepo = cpRepo;
            this.emailer = emailer;
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
            var advertisers = GetAutoReportingAdvertisers();
            while (advertisers.Count() > 0)
            {
                foreach (var adv in advertisers)
                {
                    if (adv.AutomatedReportsNextSendAfter == null)
                        adv.AutomatedReportsNextSendAfter = DateTime.Today.AddDays(-1);

                    if (adv.AutomatedReportsPeriodDays == 0 || String.IsNullOrWhiteSpace(adv.AutomatedReportsDestinationEmail))
                    {
                        // Has invalid values. Disable.
                        adv.AutomatedReportsEnabled = false;
                    }
                    else
                    {
                        var report = this.SelectReport(adv);
                        this.SendReport(adv, report);
                        this.UpdateAdvertiser(adv);
                    }
                    cpRepo.SaveChanges();
                }
                advertisers = GetAutoReportingAdvertisers();
                // in case more reports need to be generated for an advertiser
            }

            Cleanup();
        }

        private IEnumerable<Advertiser> GetAutoReportingAdvertisers()
        {
            return cpRepo.Advertisers
                         .Where(a => a.AutomatedReportsEnabled)
                         .ToList()
                         .Where(a => a.AutomatedReportsNextSendAfter == null || DateTime.Now > a.AutomatedReportsNextSendAfter);
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

        // TODO: switch to a day of the week instead of a constant period
        private void UpdateAdvertiser(Advertiser advertiser)
        {
            advertiser.AutomatedReportsNextSendAfter = advertiser.AutomatedReportsNextSendAfter.Value.AddDays(advertiser.AutomatedReportsPeriodDays);
        }

        private void Cleanup()
        {
            var disabledAdvertisers = cpRepo.Advertisers.Where(a => !a.AutomatedReportsEnabled);
            foreach (var adv in disabledAdvertisers)
            {
                adv.AutomatedReportsNextSendAfter = null;
                // so that multiple reports won't be sent unexpectedly once re-enabled
            }
            cpRepo.SaveChanges();
        }

    }
}
