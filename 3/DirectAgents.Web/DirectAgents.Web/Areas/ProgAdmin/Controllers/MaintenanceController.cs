using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DirectAgents.Domain.Abstract;

namespace DirectAgents.Web.Areas.ProgAdmin.Controllers
{
    public class MaintenanceController : DirectAgents.Web.Controllers.ControllerBase
    {

        public ActionResult Index()
        {
            return View();
        }
    }
}