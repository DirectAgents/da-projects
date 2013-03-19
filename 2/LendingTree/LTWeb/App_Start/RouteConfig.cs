using System.ServiceModel.Activation;
using System.Web.Mvc;
using System.Web.Routing;

namespace LTWeb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                constraints:  new { controller = "^(?!ws).*" }
            );

            routes.Add(new ServiceRoute("ws/LeadService", new ServiceHostFactory(), typeof(ws.LeadService)));
        }
    }
}