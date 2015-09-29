using System.Linq;
using System.Web.Mvc;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Entities.TD;

namespace DirectAgents.Web.Areas.TD.Controllers
{
    public class PlatformsController : DirectAgents.Web.Controllers.ControllerBase
    {
        public PlatformsController(ITDRepository tdRepository)
        {
            this.tdRepo = tdRepository;
        }

        public ActionResult Index()
        {
            var platforms = tdRepo.Platforms()
                .OrderBy(p => p.Name);
            return View(platforms);
        }

        public ActionResult Maintenance(int id)
        {
            var platform = tdRepo.Platform(id);
            if (platform == null)
                return HttpNotFound();

            return View(platform);
        }
	}
}