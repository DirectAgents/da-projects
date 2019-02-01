using System.Web.Mvc;
using DirectAgents.Domain.Abstract;

namespace DirectAgents.Web.Areas.SearchAdmin.Controllers
{
    public class StatsGaugesController : Web.Controllers.ControllerBase
    {
        public StatsGaugesController(ICPSearchRepository cpSearchRepository)
        {
            cpSearchRepo = cpSearchRepository;
        }

        public ActionResult Index()
        {
            var gaugesByChannel = cpSearchRepo.StatsGaugesByChannel();
            return View(gaugesByChannel);
        }
    }
}