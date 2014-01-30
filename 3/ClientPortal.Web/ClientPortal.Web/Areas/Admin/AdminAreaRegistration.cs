using System.Web.Mvc;

namespace ClientPortal.Web.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "OfferLogo",
                "OfferLogo/{id}",
                new { controller = "Offers", action = "Logo" },
                new[] { "ClientPortal.Web.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "CreativeThumbnail",
                "CreativeThumbnail/{id}",
                new { controller = "Creatives", action = "Thumbnail" },
                new[] { "ClientPortal.Web.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { "ClientPortal.Web.Areas.Admin.Controllers" }
            );
        }
    }
}
