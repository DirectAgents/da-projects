using Newtonsoft.Json;
using System.Web.Mvc;

namespace LTWeb.Controllers
{
    public class JsonDotNetResult : ActionResult
    {
        private object obj { get; set; }
        public JsonDotNetResult(object obj)
        {
            this.obj = obj;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.AddHeader("content-type", "application/json");
            context.HttpContext.Response.Write(JsonConvert.SerializeObject(this.obj));
        }
    }
}