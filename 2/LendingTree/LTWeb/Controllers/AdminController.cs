using KendoGridBinder;
using LTWeb.DataAccess;
using LTWeb.Models;
using System;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Mvc;

namespace LTWeb.Controllers
{
    public class AdminController : Controller
    {
        Lazy<LTWebDataContext> _context = new Lazy<LTWebDataContext>();
        LTWebDataContext context { get { return _context.Value; } }

        bool IsAllowedAdminAccess
        {
            get { return AppSettings.AdminIps.Contains(Request.UserHostAddress); }
        }

        [HttpPost]
        public ActionResult LeadsGrid(KendoGridRequest request)
        {
            if (!IsAllowedAdminAccess)
                return null;

            var grid = new KendoGrid<Lead>(request, this.context.Leads);
            return new JsonDotNetResult(grid);
        }

        public ActionResult Fix(string appID)
        {
            if (!IsAllowedAdminAccess)
                return null;

            return View(this.context.Leads.First(c => c.AppId == appID));
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Fix(Lead lead)
        {
            if (!IsAllowedAdminAccess)
                return null;

            lead.ResponseContent = "OK";

            return View(lead);
        }

        public ActionResult Index()
        {
            if (!IsAllowedAdminAccess)
                return RedirectToAction("Index", "Home");

            using (var repo = new Repository(new LTWebDataContext())) // TODO: dependency injection
            {
                var model = new AdminVM();

                model.Rate1 = repo.Single<AdminSetting>(c => c.Name == "Rate1").Value;

                model.Rate2 = repo.Single<AdminSetting>(c => c.Name == "Rate2").Value;

                var setting = repo.Single<AdminSetting>(c => c.Name == "Pixel");
                model.Pixel = setting == null ? string.Empty : setting.Value;

                return View(model);
            }
        }

        [HttpPost]
        public ActionResult Index(AdminVM model)
        {
            if (!IsAllowedAdminAccess)
                return RedirectToAction("Index", "Home");

            using (var db = new LTWebDataContext()) // TODO: dependency injection
            {
                db.AdminSettings.AddOrUpdate(c => c.Name, new[] { new AdminSetting { Name = "Pixel", Value = model.Pixel } });

                db.AdminSettings.Single(c => c.Name == "Rate1").Value = model.Rate1;

                db.AdminSettings.Single(c => c.Name == "Rate2").Value = model.Rate2;

                db.SaveChanges();
            }

            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _context.IsValueCreated)
                this.context.Dispose();

            base.Dispose(disposing);
        }
    }
}
