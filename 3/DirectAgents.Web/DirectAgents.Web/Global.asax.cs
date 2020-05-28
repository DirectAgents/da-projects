using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using CakeExtracter;

using KendoGridBinder.ModelBinder.Mvc;

namespace DirectAgents.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            AutoMapperConfig.CreateMaps();
            ModelBinders.Binders.Add(typeof(KendoGridMvcRequest), new KendoGridMvcModelBinder());
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            if (exception != null)
            {
                Logger.Error(exception);
                Server.ClearError();
            }
            Response.Redirect("~/Error");
        }
    }
}