using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DirectAgents.Web.Areas.MatchPortal.Models;

namespace DirectAgents.Web.Areas.MatchPortal.Controllers
{
    public class ProductMatchingController : Controller
    {
        // GET: MatchPortal/ProductMatching
        public ActionResult Index()
        {
            return View();
        }

    }
}