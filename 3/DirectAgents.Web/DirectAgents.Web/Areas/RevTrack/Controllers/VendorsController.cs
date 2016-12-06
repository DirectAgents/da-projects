using System.Web.Mvc;
using DirectAgents.Domain.Abstract;

namespace DirectAgents.Web.Areas.RevTrack.Controllers
{
    public class VendorsController : DirectAgents.Web.Controllers.ControllerBase
    {
        public VendorsController(IRevTrackRepository revTrackRepository)
        {
            this.rtRepo = revTrackRepository;
        }

        public ActionResult Index()
        {
            return View();
        }
	}
}