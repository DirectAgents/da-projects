using System.Web.Mvc;

namespace ClientPortal.Web.Areas.TD
{
    public class TDAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "TD";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "TD_default",
                "TD/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
//                ,new[] { "ClientPortal.Web.Areas.TD.Controllers" }
            );
        }
    }
}
