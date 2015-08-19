using System.Web.Mvc;

namespace DirectAgents.Web.Areas.TD
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
                new { action = "Index", id = UrlParameter.Optional }
                //new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}