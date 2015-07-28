using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using DirectAgents.Domain.Abstract;

namespace DirectAgents.Web.Controllers
{
    public class ControllerBase : Controller
    {
        protected IMainRepository daRepo;
        protected ITDRepository tdRepo;
        protected ISecurityRepository securityRepo;

        // TODO: Make SecurityRepo disposable and dispose here:

        protected override void Dispose(bool disposing)
        {
            daRepo.Dispose();
            tdRepo.Dispose();
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