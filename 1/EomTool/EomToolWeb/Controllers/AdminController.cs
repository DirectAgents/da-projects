using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DirectAgents.Domain.Abstract;
using System.Diagnostics;

namespace EomToolWeb.Controllers
{
    public class AdminController : Controller
    {
        IAdmin admin;

        public AdminController(IAdmin admin)
        {
            this.admin = admin;
            this.admin.LogHandler += admin_LogHandler;
        }

        public ActionResult Test()
        {
            var text = admin.Test();
            return Content(text);
        }
        
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

        public ActionResult LoadCampaigns()
        {
            admin.LoadCampaigns();
            return Content("Campaigns loaded");
        }

        public ActionResult LoadSummaries()
        {
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
