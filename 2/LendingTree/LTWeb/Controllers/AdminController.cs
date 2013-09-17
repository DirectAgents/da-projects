using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using CsvHelper;
using KendoGridBinder;
using LTWeb.Common;
using LTWeb.DataAccess;
using LTWeb.Models;
using LTWeb.Service;

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

        FileResult CsvFile<T>(IEnumerable<T> rows, string downloadFileName)
            where T : class
        {
            return File(CsvStream(rows), "application/CSV", downloadFileName);
        }

        string DateStamp()
        {
            return DateTime.Now.ToString("yyyyMMdd");
        }

        MemoryStream CsvStream<T>(IEnumerable<T> rows)
            where T : class
        {
            var output = new MemoryStream();
            var writer = new StreamWriter(output);
            var csv = new CsvWriter(writer);
            csv.WriteRecords<T>(rows);
            writer.Flush();
            output.Position = 0;
            return output;
        }

        [HttpPost]
        public ActionResult Export(DateTime fromDate, DateTime toDate)
        {
            if (!IsAllowedAdminAccess)
                return null;

            var rows = this.context.Leads
                                   .Where(c => c.Timestamp > fromDate && c.Timestamp < toDate);

            return CsvFile(rows, string.Format("leads{0}.csv", DateStamp()));
        }

        [HttpPost]
        public ActionResult LeadsGrid(KendoGridRequest request)
        {
            if (!IsAllowedAdminAccess)
                return null;

            var grid = new KendoGrid<Lead>(request, this.context.Leads);

            return new JsonDotNetResult(grid); // default json serializer had trouble with XML content, switched to Json.NET
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

            var service = new LendingTreeService();
            service.Resend(lead);

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

                model.ToDate = DateTime.Today.AddDays(1);
                model.FromDate = model.ToDate.AddDays(-7);

                return View(model);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(AdminVM model)
        {
            if (!IsAllowedAdminAccess)
                return RedirectToAction("Index", "Home");

            using (var db = new LTWebDataContext()) // TODO: dependency injection
            {
                db.AdminSettings.AddOrUpdate(c => c.Name, new[]
                { 
                    new AdminSetting
                    {
                        Name = "Pixel", 
                        Value = model.Pixel
                    } 
                });
                db.AdminSettings.Single(c => c.Name == "Rate1").Value = model.Rate1;
                db.AdminSettings.Single(c => c.Name == "Rate2").Value = model.Rate2;
                db.SaveChanges();
            }
            SessionUility.Clear(SessionKeys.Admin); // allow changes to be visible in this session
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
