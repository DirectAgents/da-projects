using CakeExtracter.Common;
using CakeExtracter.Etl.CakeMarketing.DALoaders;
using CakeExtracter.Etl.CakeMarketing.Extracters;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.Cake;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using CakeExtracter.Reports;
using System.Text;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class DASynchOfferBudgetStatsCommand : ConsoleCommand
    {
        public static int RunStatic(int? offerId)
        {
            var cmd = new DASynchOfferBudgetStatsCommand()
            {
                OfferId = offerId
            };
            return cmd.Run();
        }

        public int? OfferId { get; set; }
        private GmailEmailer emailer;
        private string reportingEmail;
        private string testEmail, budgetAlertsEmail;

        public override void ResetProperties()
        {
            OfferId = null;
        }

        public DASynchOfferBudgetStatsCommand()
        {
            IsCommand("daSynchOfferBudgetStats", "synch OfferDailySummaries for all offers with budgets");
            HasOption<int>("o|offerId=", "Offer Id (default = all with budgets)", c => OfferId = c);
        }

        public override int Execute(string[] remainingArguments)
        {
            reportingEmail = ConfigurationManager.AppSettings["GmailReporting_Email"];
            var reportingPassword = ConfigurationManager.AppSettings["GmailReporting_Password"];
            emailer = new GmailEmailer(new System.Net.NetworkCredential(reportingEmail, reportingPassword));

            testEmail = ConfigurationManager.AppSettings["BudgetAlerts_TestEmail"];
            budgetAlertsEmail = ConfigurationManager.AppSettings["BudgetAlerts_Email"];

            var offers = GetOffers();
            foreach (var offer in offers) //TODO: do several in parallel
            {
                DateTime startDate = offer.DateCreated.Date;
                DateTime endDate = DateTime.Today;
                if (offer.HasBudget)
                {
                    if (offer.BudgetStart.HasValue)
                        startDate = offer.BudgetStart.Value;
                    if (offer.BudgetEnd.HasValue)
                        endDate = offer.BudgetEnd.Value;
                }
                var dateRange = new DateRange(startDate, endDate.AddDays(1));
                //TODO: is there a way to know if it's "complete" and we don't need to up this one anymore?

                var extracter = new OfferDailySummariesExtracter(dateRange, 0, offer.OfferId);
                var loader = new DAOfferDailySummariesLoader();
                var extracterThread = extracter.Start();
                var loaderThread = loader.Start(extracter);
                extracterThread.Join();
                loaderThread.Join();

                CheckOfferBudgetAlerts(offer);
            }
            return 0;
        }

        private IEnumerable<Offer> GetOffers()
        {
            using (var repo = new DirectAgents.Domain.Concrete.MainRepository(new DAContext()))
            {
                IEnumerable<Offer> offers = new List<Offer>();
                if (OfferId.HasValue)
                {
                    var offer = repo.GetOffer(OfferId.Value, true, false);
                    if (offer != null)
                        ((List<Offer>)offers).Add(offer);
                }
                else
                    offers = repo.GetOffers(true, null, null, true, false, null); // active offers with budget

                foreach (var offer in offers)
                {
                    repo.FillOfferBudgetStats(offer);
                }
                return offers.ToList();
            }
        }

        private void CheckOfferBudgetAlerts(Offer offer)
        {
            if (!offer.BudgetUsedPercent.HasValue || offer.BudgetUsedPercent.Value >= 1)
                return; // null or already over 100%
            decimal oldPercentUsed = offer.BudgetUsedPercent.Value;

            using (var repo = new DirectAgents.Domain.Concrete.MainRepository(new DAContext()))
            {
                repo.FillOfferBudgetStats(offer);
            }
            if (!offer.BudgetUsedPercent.HasValue)
                return; // this should never happen

            decimal newPercentUsed = offer.BudgetUsedPercent.Value;
            if (newPercentUsed >= 1)
            {
                // went over 100%
                Logger.Info("Offer {0} reached 100% of budget", offer.OfferId);
                SendAlertEmail(offer, true);
            }
            else if (oldPercentUsed < .85m && newPercentUsed >= .85m)
            {
                // went over 85%
                Logger.Info("Offer {0} reached 85% of budget", offer.OfferId);
                SendAlertEmail(offer, false);
            }
        }

        private void SendAlertEmail(Offer offer, bool over100percent)
        {
            string subject = String.Format("Budget Alert: {0}% reached for: {1}", (over100percent ? "100" : "85"), offer.OfferName);

            var adv = offer.Advertiser;
            StringBuilder bodyText = new StringBuilder();
            if (over100percent)
                bodyText.Append("*** Reached 100% of budget. ***<br/>Please pause all partners.<br/><br/>");
            else
                bodyText.Append("*** Reached 85% of budget. ***<br/>Please do not set any new partners live at this time.<br/><br/>");

            bodyText.Append("<table>");
            bodyText.AppendFormat("<tr><td>Offer:</td><td>{0}</td></tr>", offer.OfferName);
            bodyText.AppendFormat("<tr><td>OfferId:</td><td>{0}</td></tr>", offer.OfferId);
            bodyText.AppendFormat("<tr><td>Budget:</td><td>${0:N2}</td></tr>", offer.Budget);
            bodyText.AppendFormat("<tr><td>Budget Used:</td><td>${0:N2}</td></tr>", offer.BudgetUsed);
            bodyText.AppendFormat("<tr><td>Percent Used:</td><td>{0:P1}</td></tr>", offer.BudgetUsedPercent);
            bodyText.AppendFormat("<tr><td>Stats Range:</td><td>{0} - {1}</td></tr>",
                                  offer.EarliestStatDate.HasValue ? offer.EarliestStatDate.Value.ToShortDateString() : "",
                                  offer.LatestStatDate.HasValue ? offer.LatestStatDate.Value.ToShortDateString() : "");
            bodyText.AppendFormat("<tr><td>BudgetStart:</td><td>{0}</td></tr>", offer.BudgetStart.HasValue ? offer.BudgetStart.Value.ToShortDateString() : "");
            bodyText.AppendFormat("<tr><td>BudgetEnd:</td><td>{0}</td></tr>", offer.BudgetEnd.HasValue ? offer.BudgetEnd.Value.ToShortDateString() : "");

            string adManagerText = adv.AdManager == null ? "" : String.Format("{0} ({1})", adv.AdManager.FullName, adv.AdManager.EmailAddress);
            bodyText.AppendFormat("<tr><td>Ad Manager:</td><td>{0}</td></tr>", adManagerText);
            string acctManagerText = adv.AccountManager == null ? "" : String.Format("{0} ({1})", adv.AccountManager.FullName, adv.AccountManager.EmailAddress);
            bodyText.AppendFormat("<tr><td>Acct Manager:</td><td>{0}</td></tr>", acctManagerText);
            bodyText.Append("</table>");

            string toAddress;
            if (String.IsNullOrWhiteSpace(testEmail))
            {
                toAddress = budgetAlertsEmail + "," + (adv.AdManager != null ? adv.AdManager.EmailAddress : "")
                    + "," + (adv.AccountManager != null ? adv.AccountManager.EmailAddress : "");
            }
            else
            {
                toAddress = testEmail;
            }
            emailer.SendEmail(reportingEmail,
                              new string[] { toAddress },
                              null,
                              subject,
                              bodyText.ToString(),
                              true);
        }
    }
}
