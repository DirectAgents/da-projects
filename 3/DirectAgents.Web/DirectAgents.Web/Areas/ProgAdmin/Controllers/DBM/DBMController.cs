using System.Linq;
using System.Web.Mvc;
using DirectAgents.Domain.SpecialPlatformsDataProviders.DBM;
using DirectAgents.Web.Areas.ProgAdmin.Models.DBM;

namespace DirectAgents.Web.Areas.ProgAdmin.Controllers.DBM
{
    /// <summary>
    /// DBM Stats Controller.
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class DbmController : Controller
    {
        private readonly IDbmWebPortalDataService dataService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbmController"/> class.
        /// </summary>
        /// <param name="dataService">The DBM web portal data service.</param>
        public DbmController(IDbmWebPortalDataService dataService)
        {
            this.dataService = dataService;
        }

        /// <summary>
        /// Endpoint for latests page.
        /// </summary>
        /// <param name="acctId">The acct identifier.</param>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns>Latests Action Result</returns>
        public ActionResult Latests()
        {
            var latests = dataService.GetAccountsLatestsInfo();
            var model = new DbmLatestsInfoVm
            {
                LatestsInfo = latests.ToList(),
            };
            return View(model);
        }
    }
}