using System.Web.Mvc;
using ClientPortal.Data.Contracts;
using ClientPortal.Web.Areas.TD.Models;
using ClientPortal.Web.Controllers;

namespace ClientPortal.Web.Areas.TD.Controllers
{
    [Authorize]
    public class HomeController : CPController
    {
        public HomeController(ITDRepository tdRepository, IClientPortalRepository cpRepository)
        {
            cptdRepo = tdRepository;
            cpRepo = cpRepository;
        }

        public ActionResult Index()
        {
            var userInfo = GetUserInfo();

            var result = CheckLogoutTD(userInfo);
            if (result != null)
                return result;

            var model = new TDHomeModel(userInfo);
            return View(model);
        }

        public ActionResult Test()
        {
            var userInfo = GetUserInfo();

            var model = new TDHomeModel(userInfo);
            return View(model);
        }
    }
}
