using System.Linq;
using System.Web.Mvc;
using DirectAgents.Domain.SpecialPlatformsDataProviders.YAM;
using DirectAgents.Web.Areas.ProgAdmin.Models.YAM;

namespace DirectAgents.Web.Areas.ProgAdmin.Controllers.YAM
{
    /// <summary>
    /// YAM Stats Controller.
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class YamController : Controller
    {
        private readonly IYamWebPortalDataService dataService;

        /// <summary>
        /// Initializes a new instance of the <see cref="YamController"/> class.
        /// </summary>
        /// <param name="dataService">The YAM web portal data service.</param>
        public YamController(YamWebPortalDataService dataService)
        {
            this.dataService = dataService;
        }

        /// <summary>
        /// Endpoint for latests page.
        /// </summary>
        /// <returns>Latests Action Result.</returns>
        public ActionResult Latests()
        {
            var latests = dataService.GetAccountsLatestsInfo();
            var model = new YamLatestsInfoVm
            {
                LatestsInfo = latests.ToList(),
            };
            return View(model);
        }
    }
}