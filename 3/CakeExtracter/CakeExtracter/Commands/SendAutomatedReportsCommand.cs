using System.ComponentModel.Composition;
using CakeExtracter.Common;
using CakeExtracter.Reports;
using ClientPortal.Data.Contexts;
using ClientPortal.Data.Services;
using System.Configuration;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class SendAutomatedReportsCommand : ConsoleCommand
    {
        public override void ResetProperties()
        {
        }

        public SendAutomatedReportsCommand()
        {
            IsCommand("sendAutomatedReports", "send automated reports");
        }

        // TODO: IoC
        public override int Execute(string[] remainingArguments)
        {
            var gmailUsername = ConfigurationManager.AppSettings["GmailEmailer_Username"];
            var gmailPassword = ConfigurationManager.AppSettings["GmailEmailer_Password"];
            var portalEmail = ConfigurationManager.AppSettings["PortalEmail"];

            using (var db = new ClientPortalContext())
            {
                var reportManager = new ReportManager(
                                            new ClientPortalRepository(db),
                                            new GmailEmailer(new System.Net.NetworkCredential(gmailUsername, gmailPassword)),
                                            portalEmail,
                                            "Direct Agents Client Portal Automated Report"
                                        );
                reportManager.CatchUp();
            }
            return 0;
        }
    }
}
