﻿using System.Linq;
using System.Web.Mvc;
using LTWeb.DataAccess;
using LTWeb.Models;

namespace LTWeb.Controllers
{
    public class AdminController : Controller
    {

        string[] allowedHostAddresses = new string[]
        {
            "::1"
        };

        bool AllowedAdminAccess
        {
            get { return allowedHostAddresses.Contains(Request.UserHostAddress); }
        }

        public ActionResult Index()
        {
            if (!AllowedAdminAccess)
                return RedirectToAction("Index", "Home");

            using (var repo = new Repository(new LTWebDataContext())) // TODO: dependency injection
            {
                var model = new AdminVM();

                model.Rate1 = repo.Single<AdminSetting>(c => c.Name == "Rate1").Value;
                model.Rate2 = repo.Single<AdminSetting>(c => c.Name == "Rate2").Value;

                return View(model);
            }
        }

        [HttpPost]
        public ActionResult Index(AdminVM model)
        {
            if (!AllowedAdminAccess)
                return RedirectToAction("Index", "Home");

            using (var db = new LTWebDataContext()) // TODO: dependency injection
            {
                db.AdminSettings.Single(c => c.Name == "Rate1").Value = model.Rate1;
                db.AdminSettings.Single(c => c.Name == "Rate2").Value = model.Rate2;

                db.SaveChanges();
            }

            return View(model);
        }
    }
}
