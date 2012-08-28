using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EomToolWeb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: null,
            //    url: "Page{page}",
            //    defaults: new { controller = "Publishers", action = "List" }
            //);

            routes.MapRoute(
                name: null,
                url: "Payouts/Summary/{mode}",
                defaults: new { controller = "Payouts", action = "Summary", mode = UrlParameter.Optional }
            );

            //routes.MapRoute(
            //    name: null,
            //    url: "Payouts/List/{mode}",
            //    defaults: new { controller = "Payouts", action = "List", mode = UrlParameter.Optional }
            //);

            routes.MapRoute(
                name: null,
                url: "Payouts/ListPartial/{mode}",
                defaults: new { controller = "Payouts", action = "ListPartial", mode = UrlParameter.Optional }
            );

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);
        }
    }
}