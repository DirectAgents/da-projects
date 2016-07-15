using System.Web.Mvc;
using ClientPortal.Data.Contracts;
using ClientPortal.Web.Models;

namespace ClientPortal.Web.Controllers
{
    [Authorize]
    public class SearchController : CPController
    {
        public SearchController(IClientPortalRepository cpRepository)
        {
            this.cpRepo = cpRepository;
        }

        public ActionResult Weekly()
        {
            var userInfo = GetUserInfo();
            if (!userInfo.HasSearch)
                return RedirectToAction("Index", "Home");

            var model = new GenericModel
            {
                UserInfo = userInfo
            };
            return View(model);
        }
	}
}