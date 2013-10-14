using System.ComponentModel.Composition;
using CakeExtracter.Common;
using CakeExtracter.Reports;
using ClientPortal.Data.Contexts;
using ClientPortal.Data.Services;

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
            using (var db = new ClientPortalContext())
            {
                // TODO: put credential into in config/IoC
                var reportManager = new ReportManager(
                                            new ClientPortalRepository(db),
                                            new GmailEmailer(new System.Net.NetworkCredential("portal@directagents.com", "dAp9rt@l")),
                                            "portal@directagents.com",
                                            "Direct Agents Client Portal Automated Report"
                                        );
                reportManager.CatchUp();
            }
            return 0;
        }
    }
}
