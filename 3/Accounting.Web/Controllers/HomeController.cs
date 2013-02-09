using Accounting.Web.Models;
using KendoGridBinder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accounting.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Grid(KendoGridRequest request)
        {
            using (var db = new KendoDataContext())
            {
                var grid = new KendoGrid<Employee>(request, db.Employees.ToList());
                return Json(grid);
            }
        }

    }
}
