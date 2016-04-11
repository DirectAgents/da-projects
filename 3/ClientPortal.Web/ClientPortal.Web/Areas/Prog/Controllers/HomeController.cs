using System.Web.Mvc;
using ClientPortal.Web.Controllers;
using DirectAgents.Domain.Abstract;

namespace ClientPortal.Web.Areas.Prog.Controllers
{
    public class HomeController : CPController
    {
        public HomeController(ITDRepository progRepository, ClientPortal.Data.Contracts.IClientPortalRepository cpRepository)
        {
            this.progRepo = progRepository;
            this.cpRepo = cpRepository;
        }

        public ActionResult Index()
        {
            var userInfo = GetUserInfo();
            return View(userInfo);
        }
    }
}