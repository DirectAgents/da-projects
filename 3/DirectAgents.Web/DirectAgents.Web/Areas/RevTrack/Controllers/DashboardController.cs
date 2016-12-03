using System;
using System.Web.Mvc;
using DirectAgents.Domain.Abstract;
using DirectAgents.Web.Areas.RevTrack.Models;

namespace DirectAgents.Web.Areas.RevTrack.Controllers
{
    public class DashboardController : DirectAgents.Web.Controllers.ControllerBase
    {
        public DashboardController(IMainRepository daRepository, IRevTrackRepository revTrackRepository, ISuperRepository superRepository)
        {
            this.daRepo = daRepository;
            this.rtRepo = revTrackRepository;
            this.superRepo = superRepository;
        }

        public ActionResult Index()
        {
            var monthStart = new DateTime(2015, 5, 1); //TESTING

            var clientStats = superRepo.StatsByClient(monthStart);

            var model = new DashboardVM
            {
                ABStats = clientStats
            };
            return View(model);
        }
	}
}