using ClientPortal.Web.Areas.Admin.Models;
using System.Web.Mvc;

namespace ClientPortal.Web.Areas.Admin.Controllers
{
    [Authorize(Users="admin")]
    public class HomeController : Controller
    {
        public ActionResult Index(bool? search, bool? td)
        {
            var model = new AdminModel
            {
                Search = search.HasValue && search.Value,
                TradingDesk = td.HasValue && td.Value
            };
            return View(model);
        }

    }
}
