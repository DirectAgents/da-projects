using System.Web.Mvc;
using LTWeb.Service;

namespace LTWeb.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// When a new visitor arrives at the site this is default action method that will execute.
        /// </summary>
        /// <param name="a">Affiliate ID</param>
        /// <param name="v">Mode</param>
        /// <returns></returns>
        public ActionResult Index(int? a, int? v)
        {
            LTWeb.Session.Reset();
            PrepareLendingTreeModelForNewSession(LTWeb.Session.LTModel, a, v);
            return RedirectToAction("Show", "Questions");
        }

        /// <summary>
        /// Initializes the model for the session.
        /// </summary>
        /// <param name="lendingTreeModel"></param>
        /// <param name="a">Affiliate ID</param>
        /// <param name="v">
        /// There are two app settings: 
        ///   1. SsnModeDefaultValue - the default SSN mode
        ///   2. SsnRequiredModeValue - the value that means SSN is required
        ///   
        /// The value of <paramref name="v"/> is compared with SsnRequiredModeValue and the boolean result of this
        /// comparison dictates whether or not this session have SSN be required or optional.
        /// 
        /// If <paramref name="v"/> is not specified then SsnModeDefaultValue is compared with SsnRequiredModeValue 
        /// and the boolean result of this comparison dictates whether or not this session have SSN be required or optional.
        /// </param>
        public static void PrepareLendingTreeModelForNewSession(ILendingTreeModel lendingTreeModel, int? a, int? v)
        {
            lendingTreeModel.AffiliateSiteID = (a ?? -1).ToString();
            lendingTreeModel.SsnRequired = ((v ?? AppSettings.SsnModeDefaultValue) == AppSettings.SsnRequiredModeValue);
            lendingTreeModel.VisitorIPAddress = System.Web.HttpContext.Current.Request.UserHostAddress;
            lendingTreeModel.VisitorURL = AppSettings.VisitorUrl;
        }
    }
}
