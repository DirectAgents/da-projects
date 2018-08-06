using System.Linq;
using System.Web.Mvc;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Entities.CPSearch;

namespace DirectAgents.Web.Areas.SearchAdmin.Controllers
{
    public class SearchConvTypesController : DirectAgents.Web.Controllers.ControllerBase
    {
        public SearchConvTypesController(ICPSearchRepository cpSearchRepository)
        {
            this.cpSearchRepo = cpSearchRepository;
        }

        public ActionResult IndexGauge(int? spId, int? saId, int? scId)
        {
            var convTypes = cpSearchRepo.GetConvTypes(spId: spId, searchAccountId: saId, searchCampaignId: scId, includeGauges: true);
            return View(convTypes.OrderBy(x => x.Alias).ThenBy(x => x.SearchConvTypeId));
        }
    }
}