using System.Web.Mvc;
using ClientPortal.Web.Controllers;
using DirectAgents.Domain.Abstract;

namespace ClientPortal.Web.Areas.Admin.Controllers
{
    [Authorize(Users = "admin")]
    public class ProgAdminController : CPController
    {
        public ProgAdminController(ITDRepository datdRepository)
        {
            datdRepo = datdRepository;
        }

        public ActionResult Advertisers()
        {
            return View();
        }
	}
}