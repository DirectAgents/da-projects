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

        public static void PrepareLendingTreeModelForNewSession(ILendingTreeModel lendingTreeModel, int? mode)
        {
            if (mode != null)
            {
                lendingTreeModel.SsnRequired = (mode == AppSettings.SsnRequiredModeValue);
            }
            lendingTreeModel.VisitorIPAddress = System.Web.HttpContext.Current.Request.UserHostAddress;
            lendingTreeModel.VisitorURL = AppSettings.VisitorUrl;

            // TODO!!!: add DOB question
            lendingTreeModel.DOB = "10/10/1960";
        }

    }
}
