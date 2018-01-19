using System;
using System.Linq;
using System.Web.Mvc;
using DirectAgents.Domain.Abstract;

namespace DirectAgents.Web.Areas.SearchAdmin.Controllers
{
    public class SearchProfilesController : DirectAgents.Web.Controllers.ControllerBase
    {
        public SearchProfilesController(ICPSearchRepository cpSearchRepository)
        {
            this.cpSearchRepo = cpSearchRepository;
        }

        public ActionResult Test()
        {
            var searchProfiles = cpSearchRepo.SearchProfiles();
            var sps = String.Join(" ", searchProfiles.Select(x => x.SearchProfileName).ToArray());
            return Content(sps);
        }

        public ActionResult Index()
        {
            var searchProfiles = cpSearchRepo.SearchProfiles();
            return View(searchProfiles);
        }
    }
}