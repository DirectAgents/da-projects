using System.Web.Mvc;
using LTWeb.Service;

namespace LTWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(int? v)
        {
            LTWeb.Session.Reset();
            PrepareLendingTreeModelForNewSession(LTWeb.Session.LTModel, v);
            return RedirectToAction("Show", "Questions");
        }

        public static void PrepareLendingTreeModelForNewSession(ILendingTreeModel lendingTreeModel, int? v)
        {
            lendingTreeModel.SsnRequired = ((v ?? AppSettings.SsnModeDefaultValue) == AppSettings.SsnRequiredModeValue);
            lendingTreeModel.VisitorIPAddress = System.Web.HttpContext.Current.Request.UserHostAddress;
            lendingTreeModel.VisitorURL = AppSettings.VisitorUrl;
        }
    }
}
