using DirectAgents.Domain.Abstract;
using System;
using System.Diagnostics;
using System.Web.Mvc;

namespace EomToolWeb.Controllers
{
    public class WikiAdminController : Controller
    {
        IAdmin admin;
        ISecurityRepository securityRepo;

        public WikiAdminController(IAdmin admin, ISecurityRepository securityRepository)
        {
            this.admin = admin;
            this.admin.LogHandler += admin_LogHandler;

            this.securityRepo = securityRepository;
        }

        public ActionResult Test()
        {
            var text = admin.Test();
            return Content(text);
        }

        public ActionResult Test2()
        {
            var text = admin.Test2();
            return Content(text);
        }
 /*
        public ActionResult CreateDB()
        {
            admin.CreateDatabaseIfNotExists();
            return Content("Database created");
        }

        public ActionResult ReCreateDB()
        {
            admin.ReCreateDatabase();
            return Content("Database re-created");
        }
 */
        public ActionResult LoadCampaigns()
        {
            if (!securityRepo.IsAccountantOrAdmin(User))
                return Content("unauthorized");

            admin.LoadCampaigns();
            return Content("Campaigns loaded");
        }

        public ActionResult LoadSummaries()
        {
            if (!securityRepo.IsAccountantOrAdmin(User))
                return Content("unauthorized");

            admin.LoadSummaries();
            return Content("Summaries loaded");
        }

        static void admin_LogHandler(object sender, TraceEventType severity, string messageFormat, params object[] formatArgs)
        {
            var msg = String.Format(messageFormat, formatArgs);
            bool wantNewLine = msg.Contains("done");
            Debugger.Log(0, null, wantNewLine ? msg + Environment.NewLine : msg);
            //TODO: incorporate severity - e.g. as 'level' ?
        }
    }
}
