using System;
using System.Linq;
using System.Web.Mvc;
using DirectAgents.Domain.Abstract;

namespace DirectAgents.Web.Areas.Cake.Controllers
{
    public class CampsController : DirectAgents.Web.Controllers.ControllerBase
    {
        public CampsController(IMainRepository mainRepository)
        {
            this.daRepo = mainRepository;
        }

        public ActionResult Index(int? offId)
        {
            var camps = daRepo.GetCamps(offerId: offId);
            return View(camps);
        }
    }
}