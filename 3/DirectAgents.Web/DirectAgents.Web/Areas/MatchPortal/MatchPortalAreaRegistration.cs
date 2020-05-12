using System.Web.Mvc;

namespace DirectAgents.Web.Areas.MatchPortal
{
    public class MatchPortalAreaRegistration : AreaRegistration
    {
        public override string AreaName => "MatchPortal";

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "MatchPortal_default",
                "MatchPortal/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}