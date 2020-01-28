using System.Linq;
using System.Web.Mvc;
using DirectAgents.Domain.SpecialPlatformsDataProviders.Adform;
using DirectAgents.Web.Areas.ProgAdmin.Models.Adform;

namespace DirectAgents.Web.Areas.ProgAdmin.Controllers.Adform
{
    /// <summary>
    /// Adform Stats Controller.
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class AdformController : Controller
    {
        private readonly IAdformWebPortalDataService dataService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdformController"/> class.
        /// </summary>
        /// <param name="dataService">The Adform web portal data service.</param>
        public AdformController(IAdformWebPortalDataService dataService)
        {
            this.dataService = dataService;
        }

        /// <summary>
        /// Endpoint for latests page.
        /// </summary>
        /// <returns>Latests Action Result</returns>
        public ActionResult Latests()
        {
            var latests = dataService.GetAccountsLatestsInfo();
            var model = new AdformLatestsInfoVm
            {
                LatestsInfo = latests.ToList(),
            };
            return View(model);
        }
    }
}