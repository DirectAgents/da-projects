using EomTool.Domain.Abstract;
using EomTool.Domain.Concrete;
using EomTool.Domain.Entities;
using EomToolWeb.Infrastructure;
using System;
using System.IO;
using System.Web.Mvc;

namespace EomToolWeb.Controllers
{
    public class EOMController : Controller
    {
        protected IMainRepository mainRepo;
        protected ISecurityRepository securityRepo;
        protected IDAMain1Repository daMain1Repo;
        protected IEomEntitiesConfig eomEntitiesConfig;

        protected void SetAccountingPeriodViewData(DateTime? minDate = null)
        {
            if (!minDate.HasValue)
                minDate = new DateTime(2014, 1, 1); // default minDate

            ViewBag.ChooseMonthSelectList = daMain1Repo.ChooseMonthSelectList(eomEntitiesConfig, minDate.Value);
            ViewBag.DebugMode = eomEntitiesConfig.DebugMode;
        }

        protected IMainRepository CreateMainRepository(DateTime dateTime)
        {
            var config = new EomEntitiesConfigBase()
            {
                CurrentEomDate = dateTime
            };

            IMainRepository repo = null;
            if (config.DatabaseExistsForDate(dateTime))
            {
                var eomEntities = new EomEntities(config);
                repo = new MainRepository(eomEntities);
            }
            return repo;
        }

        // ---

        protected string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

        // ---

        protected override void Dispose(bool disposing)
        {
            if (mainRepo != null)
                mainRepo.Dispose();
            if (daMain1Repo != null)
                daMain1Repo.Dispose();
            //TODO: make other repos disposable

            base.Dispose(disposing);
        }
    }
}