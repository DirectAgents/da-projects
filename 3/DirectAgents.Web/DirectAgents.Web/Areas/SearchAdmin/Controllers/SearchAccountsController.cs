using System;
using System.Linq;
using System.Web.Mvc;
using DirectAgents.Domain.Abstract;

namespace DirectAgents.Web.Areas.SearchAdmin.Controllers
{
    public class SearchAccountsController : DirectAgents.Web.Controllers.ControllerBase
    {
        public SearchAccountsController(ICPSearchRepository cpSearchRepository)
        {
            this.cpSearchRepo = cpSearchRepository;
        }

        public ActionResult Index(int? spId)
        {
            var searchAccounts = cpSearchRepo.SearchAccounts(spId: spId);
            return View(searchAccounts.OrderBy(x => x.SearchProfileId));
        }

        public ActionResult IndexGauge(int? spId)
        {
            var searchAccounts = cpSearchRepo.SearchAccounts(spId: spId, includeGauges: true);
            return View(searchAccounts.OrderBy(x => x.SearchProfileId));
        }
    }
}