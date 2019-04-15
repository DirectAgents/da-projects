using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using DirectAgents.Domain.Abstract;
using KendoGridBinder;

namespace DirectAgents.Web.Controllers
{
    public class ControllerBase : Controller
    {
        protected IMainRepository daRepo;
        protected ICPProgRepository cpProgRepo;
        protected ICPSearchRepository cpSearchRepo;
        protected ITDRepository tdRepo;
        protected IRevTrackRepository rtRepo;
        protected IABRepository abRepo;
        protected ISecurityRepository securityRepo;
        protected ISpecialPlatformRepository specialPlatformRepo;
        
        protected ClientPortal.Data.Contracts.IClientPortalRepository cpRepo;

        // TODO: Make SecurityRepo disposable and dispose here:

        protected override void Dispose(bool disposing)
        {
            daRepo?.Dispose();
            cpProgRepo?.Dispose();
            cpSearchRepo?.Dispose();
            tdRepo?.Dispose();
            rtRepo?.Dispose();
            abRepo?.Dispose();
            cpRepo?.Dispose();
            specialPlatformRepo?.Dispose();
            base.Dispose(disposing);
        }

        private IClientRepository _clientRepo;
        protected IClientRepository clientRepo
        {
            get { return _clientRepo; }
            set
            {   //NOTE: Be sure to setup the required underlying repositories first.
                _clientRepo = value;
                _clientRepo.SetRepositories(cpProgRepo, cpSearchRepo);
            }
        }

        private ISuperRepository _superRepo;
        protected ISuperRepository superRepo
        {
            get { return _superRepo; }
            set
            {   //NOTE: Be sure to setup the required underlying repositories first.
                _superRepo = value;
                _superRepo.SetRepositories(daRepo, rtRepo, abRepo);
            }
        }

        // --- Getting/Setting the Current Month (i.e. Accounting Period) ---

        // Pass in null for month to use "CurrentMonth" cookie (for the specified area)
        // Returns the selected month
        protected DateTime SetChooseMonthViewData(string area, DateTime? month = null)
        {
            if (!month.HasValue)
                month = GetCurrentMonth(area);
            else if (month.Value.Day > 1) // make sure passed-in value is the 1st of a month
                month = new DateTime(month.Value.Year, month.Value.Month, 1);

            ViewBag.ChooseMonthSelectList = ChooseMonthSelectList(month.Value);
            return month.Value;
        }

        // Passing in null for month will not use the cookie (like the previous method does)
        protected DateTime SetChooseMonthViewData_NonCookie(DateTime? month = null)
        {
            if (!month.HasValue)
                month = DateTime.Today.AddDays(-1); // if it's the 1st, use last month
            return SetChooseMonthViewData(null, month); // area not needed b/c month always has a value
        }

        protected SelectList ChooseMonthSelectList(DateTime selMonth, bool includeNextMonth = false)
        {
            var slItems = new List<SelectListItem>();
            var iMonth = DateTime.Today.AddDays(-1); // If it's the 1st, the "current month" is still last month.
            iMonth = new DateTime(iMonth.Year, iMonth.Month, 1);
            if (includeNextMonth)
                iMonth = iMonth.AddMonths(1);
            int numMonths = (includeNextMonth ? 13 : 12);
            for (int i = 0; i < numMonths; i++)
            {
                slItems.Add(new SelectListItem { Text = iMonth.ToString("MMM yyyy"), Value = iMonth.ToShortDateString()});
                iMonth = iMonth.AddMonths(-1);
            }
            return new SelectList(slItems, "Value", "Text", selMonth.ToShortDateString());
        }

        // First check for a cookie. If none, set it to a default value. Either way, return the "current month".
        protected DateTime GetCurrentMonth(string area)
        {
            var currMonthCookie = Request.Cookies["CurrentMonth" + area];
            DateTime currMonth;
            if (currMonthCookie == null || !DateTime.TryParse(currMonthCookie.Value, out currMonth))
            {
                currMonth = DateTime.Today.AddDays(-1); // default to last month if it's the 1st
                currMonth = new DateTime(currMonth.Year, currMonth.Month, 1);
                SetCurrentMonth(area, currMonth);
            }
            return currMonth;
        }
        protected void SetCurrentMonth(string area, DateTime value)
        {
            HttpCookie cookie = new HttpCookie("CurrentMonth" + area);
            cookie.Value = value.ToString("d");
            Response.Cookies.Add(cookie);
        }

        public static bool AreDatesCustom(DateTime? start, DateTime? end)
        {
            if (!start.HasValue || !end.HasValue)
                return true;
            return (start.Value.Day != 1) || (start.Value.AddMonths(1).AddDays(-1) != end.Value);
        }

        // ---

        protected JsonResult CreateJsonResult<T>(KendoGrid<T> kgrid, object aggregates, bool allowGet = false)
        {
            var kg = new KG<T>();
            kg.data = kgrid.Data;
            kg.total = kgrid.Total;
            kg.aggregates = aggregates;

            var json = Json(kg, allowGet ? JsonRequestBehavior.AllowGet : JsonRequestBehavior.DenyGet);
            return json;
        }
    }

    class KG<T>
    {
        public IEnumerable<T> data { get; set; }
        public int total { get; set; }
        public object aggregates { get; set; }
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