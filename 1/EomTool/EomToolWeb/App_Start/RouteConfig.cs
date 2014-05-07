using System.Web.Mvc;
using System.Web.Routing;

namespace EomToolWeb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: null,
                url: "Campaigns",
                defaults: new { controller = "Campaigns", action = "List2" }
            );

            routes.MapRoute(
                name: null,
                url: "Payouts/Summary/{mode}",
                defaults: new { controller = "Payouts", action = "Summary", mode = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}