using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DirectAgents.Domain.Abstract;

namespace EomToolWeb.Controllers
{
    public class AdminController : Controller
    {
        IAdmin admin;

        public AdminController(IAdmin admin)
        {
            this.admin = admin;
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
    }
}
