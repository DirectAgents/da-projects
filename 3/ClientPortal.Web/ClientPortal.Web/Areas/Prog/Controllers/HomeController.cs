using System.Web.Mvc;
using ClientPortal.Web.Controllers;
using DirectAgents.Domain.Abstract;

namespace ClientPortal.Web.Areas.Prog.Controllers
{
    public class HomeController : CPController
    {
        public HomeController(ITDRepository datdRepository, ClientPortal.Data.Contracts.IClientPortalRepository cpRepository)
        {
            this.datdRepo = datdRepository;
            this.cpRepo = cpRepository;
        }

        public ActionResult Index()
        {
            var userInfo = GetUserInfo();
            return View(userInfo);
        }
    }
}