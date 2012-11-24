using System.Web.Mvc;

namespace LTWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(int? mode)
        {
            Settings.Reset();

            var ltModel = Settings.LTModel;

            // mode=1 --> SSN is required
            // mode=2 --> SSN is not required
            if (mode != null)
            {
                if (mode == 1)
                {
                    ltModel.SsnRequired = true;
                }
                else if (mode == 2)
                {
                    ltModel.SsnRequired = false;
                }
            }

            return RedirectToAction("Show", "Questions");
        }
    }
}
