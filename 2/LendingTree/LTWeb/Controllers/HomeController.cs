using System.Web.Mvc;
using LTWeb.Service;

namespace LTWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(int? mode)
        {
            LTWeb.Session.Reset();
            PrepareLendingTreeModelForNewSession(LTWeb.Session.LTModel, mode);
            return RedirectToAction("Show", "Questions");
        }

        public static void PrepareLendingTreeModelForNewSession(ILendingTreeModel lendingTreeModel, int? v)
        {
            if (v != null)
            {
                lendingTreeModel.SsnRequired = (v == AppSettings.SsnRequiredModeValue);
            }
            lendingTreeModel.VisitorIPAddress = System.Web.HttpContext.Current.Request.UserHostAddress;
            lendingTreeModel.VisitorURL = AppSettings.VisitorUrl;
        }
    }
}
