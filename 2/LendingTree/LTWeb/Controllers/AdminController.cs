using System.Linq;
using System.Web.Mvc;
using LTWeb.DataAccess;
using LTWeb.Models;
using System.Data.Entity.Migrations;

namespace LTWeb.Controllers
{
    public class AdminController : Controller
    {
        bool IsAllowedAdminAccess
        {
            get { return AppSettings.AdminIps.Contains(Request.UserHostAddress); }
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
    }
}
