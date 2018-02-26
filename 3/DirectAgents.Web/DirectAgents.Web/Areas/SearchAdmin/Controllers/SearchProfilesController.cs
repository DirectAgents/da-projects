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

        public ActionResult Index(string orderBy, int? activeSinceMonths = 1)
        {
            // use -1 for activeSinceMonths to show all profiles
            DateTime? activeSince = null;
            if (activeSinceMonths.HasValue && activeSinceMonths >= 0) // if 0, use yesterday. otherwise, X months prior to today
                activeSince = (activeSinceMonths.Value == 0) ? DateTime.Today.AddDays(-1) : DateTime.Today.AddMonths(-activeSinceMonths.Value);
            var searchProfiles = cpSearchRepo.SearchProfiles(activeSince: activeSince);

            if (orderBy != null && orderBy.ToLower() == "id")
                searchProfiles = searchProfiles.OrderBy(x => x.SearchProfileId);
            else
                searchProfiles = searchProfiles.OrderBy(x => x.SearchProfileName);

            ViewBag.OrderBy = orderBy;
            ViewBag.ShowAll = (activeSince == null);
            return View(searchProfiles);
        }
    }
}