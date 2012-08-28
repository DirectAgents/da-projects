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
                url: "Payouts/Summary/{mode}",
                defaults: new { controller = "Payouts", action = "Summary", mode = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: null,
                url: "Payouts/ListPartial/{mode}",
                defaults: new { controller = "Payouts", action = "ListPartial", mode = UrlParameter.Optional }
            );
        }
    }
}