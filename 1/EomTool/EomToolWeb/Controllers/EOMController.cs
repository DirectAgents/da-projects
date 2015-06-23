using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using EomTool.Domain.Abstract;
using EomTool.Domain.Concrete;
using EomTool.Domain.Entities;
using EomToolWeb.Infrastructure;

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

        protected IMainRepository CreateMainRepository(DateTime? dateTime = null)
        {
            if (dateTime == null) // default: current month
            {
                var today = DateTime.Today;
                dateTime = new DateTime(today.Year, today.Month, 1);
            }
            var config = new EomEntitiesConfigBase()
            {
                CurrentEomDate = dateTime.Value
            };

            IMainRepository repo = null;
            if (config.DatabaseExistsForDate(dateTime.Value))
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

    public class JsonpResult : System.Web.Mvc.JsonResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            HttpResponseBase response = context.HttpContext.Response;

            if (!String.IsNullOrEmpty(ContentType))
            {
                response.ContentType = ContentType;
            }
            else
            {
                response.ContentType = "application/javascript";
            }
            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }
            if (Data != null)
            {
                // The JavaScriptSerializer type was marked as obsolete prior to .NET Framework 3.5 SP1
#pragma warning disable 0618
                HttpRequestBase request = context.HttpContext.Request;

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                var callbackName = request.Params["jsoncallback"] ?? "mycallback";
                response.Write(callbackName + "(" + serializer.Serialize(Data) + ")");
#pragma warning restore 0618
            }
        }
    }

    public static class JsonResultExtensions
    {
        public static JsonpResult ToJsonp(this JsonResult json)
        {
            return new JsonpResult { ContentEncoding = json.ContentEncoding, ContentType = json.ContentType, Data = json.Data, JsonRequestBehavior = json.JsonRequestBehavior };
        }
    }
}