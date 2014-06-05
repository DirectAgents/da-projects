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
        public static int RunStatic()
        {
            var cmd = new DASynchOfferBudgetStatsCommand();
            return cmd.Run();
        }

        private GmailEmailer emailer;
        private string reportingEmail;

        public override void ResetProperties()
        {
        }

        public DASynchOfferBudgetStatsCommand()
        {
            IsCommand("daSynchOfferBudgetStats", "synch OfferDailySummaries for all offers with budgets");
        }

        public override int Execute(string[] remainingArguments)
        {
            reportingEmail = ConfigurationManager.AppSettings["GmailReporting_Email"];
            var reportingPassword = ConfigurationManager.AppSettings["GmailReporting_Password"];
            emailer = new GmailEmailer(new System.Net.NetworkCredential(reportingEmail, reportingPassword));

            var today = DateTime.Today;
            var firstOfMonth = new DateTime(today.Year, today.Month, 1);
            var firstOfLastMonth = firstOfMonth.AddMonths(-1);

            var offers = GetOffersWithBudgets();
            foreach (var offer in offers)
            {
                DateTime startDate;
                if (offer.BudgetIsMonthly)
                    startDate = firstOfLastMonth;
                else
                    startDate = (offer.BudgetStart.HasValue ? offer.BudgetStart.Value : offer.DateCreated.Date);
                var dateRange = new DateRange(startDate, today.AddDays(1));

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

        private IEnumerable<Offer> GetOffersWithBudgets()
        {
            using (var repo = new DirectAgents.Domain.Concrete.MainRepository(new DAContext()))
            {
                var offers = repo.GetOffers(true, null, null, true);
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
            var adv = offer.Advertiser;
            StringBuilder text = new StringBuilder("<table>");
            string adManagerText = adv.AdManager == null ? "" : String.Format("{0} ({1})", adv.AdManager.FullName, adv.AdManager.EmailAddress);
            text.AppendFormat("<tr><td>Ad Manager:</td><td>{0}</td></tr>", adManagerText);
            string acctManagerText = adv.AccountManager == null ? "" : String.Format("{0} ({1})", adv.AccountManager.FullName, adv.AccountManager.EmailAddress);
            text.AppendFormat("<tr><td>Acct Manager:</td><td>{0}</td></tr>", acctManagerText);
            text.AppendFormat("<tr><td>OfferId:</td><td>{0}</td></tr>", offer.OfferId);
            text.AppendFormat("<tr><td>Offer:</td><td>{0}</td></tr>", offer.OfferName);
            text.AppendFormat("<tr><td>Budget:</td><td>{0:N2}</td></tr>", offer.Budget);
            text.AppendFormat("<tr><td>IsMonthly:</td><td>{0}</td></tr>", offer.BudgetIsMonthlyString);
            text.AppendFormat("<tr><td>BudgetStart:</td><td>{0}</td></tr>", offer.BudgetStart.HasValue ? offer.BudgetStart.Value.ToShortDateString() : "");
            text.AppendFormat("<tr><td>Stats:</td><td>{0} - {1}</td></tr>", offer.EarliestStatDate.HasValue ? offer.EarliestStatDate.Value.ToShortDateString() : "",
                                                    offer.LatestStatDate.HasValue ? offer.LatestStatDate.Value.ToShortDateString() : "");
            text.AppendFormat("<tr><td>Percent Used:</td><td>{0:P1}</td></tr>", offer.BudgetUsedPercent);
            text.AppendFormat("<tr><td>Budget Used:</td><td>{0:N2}</td></tr>", offer.BudgetUsed);
            text.Append("</table>");

            if (over100percent)
                text.Append(String.Format("<br/><br/>*** Reached 100% of budget. ***"));
            else
                text.Append(String.Format("<br/><br/>*** Reached 85% of budget. ***"));

            SendEmail(text.ToString());
        }

        private void SendEmail(string text)
        {
            emailer.SendEmail(reportingEmail,
                              new string[] { "kevin@directagents.com" },
                              null,
                              "Budget Alert",
                              text,
                              true);
        }
    }
}
