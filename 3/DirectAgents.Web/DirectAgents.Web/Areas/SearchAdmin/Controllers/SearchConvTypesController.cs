using System.Linq;
using System.Web.Mvc;
using DirectAgents.Domain.Abstract;

namespace DirectAgents.Web.Areas.SearchAdmin.Controllers
{
    public class SearchConvTypesController : Web.Controllers.ControllerBase
    {
        public SearchConvTypesController(ICPSearchRepository cpSearchRepository)
        {
            cpSearchRepo = cpSearchRepository;
        }

        public ActionResult IndexGauge(int? spId, int? saId, int? scId)
        {
            var convTypes = cpSearchRepo
                .GetConvTypes(spId: spId, searchAccountId: saId, searchCampaignId: scId, includeGauges: true)
                .OrderBy(x => x.Alias).ThenBy(x => x.SearchConvTypeId).ToList();
            return View(convTypes);
        }
    }
}