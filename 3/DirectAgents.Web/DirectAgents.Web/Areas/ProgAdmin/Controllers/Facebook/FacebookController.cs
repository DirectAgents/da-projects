using DirectAgents.Domain.SpecialPlatformsDataProviders.Facebook;
using DirectAgents.Web.Areas.ProgAdmin.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace DirectAgents.Web.Areas.ProgAdmin.Controllers.Facebook
{
    /// <summary>
    /// Facebook Stats Controller
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class FacebookController : Controller
    {
        private readonly IFacebookWebPortalDataService facebookWebPortalDataService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookController"/> class.
        /// </summary>
        /// <param name="facebookWebPortalDataService">The facebook web portal data service.</param>
        public FacebookController(IFacebookWebPortalDataService facebookWebPortalDataService)
        {
            this.facebookWebPortalDataService = facebookWebPortalDataService;
        }

        /// <summary>
        /// Endpoint for facebook latests page.
        /// </summary>
        /// <param name="acctId">The acct identifier.</param>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns></returns>
        public ActionResult Latests()
        {
            var latests = facebookWebPortalDataService.GetAccountsLatestsInfo();
            var model = new FacebookLatestsInfoVm
            {
                LatestsInfo = latests.ToList()
            };
            return View(model);
        }

        /// <summary>
        /// Endpoint for facebook daily totals page.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns></returns>
        public ActionResult DailyTotals(DateTime? start, DateTime? end)
        {
            var defaultFromDate = DateTime.Now.AddDays(-10);
            var defaultToDate = DateTime.Now;
            var totals = facebookWebPortalDataService.GetDailyTotalsInfo(defaultFromDate, defaultToDate);
            var model = new FacebookTotalsInfoVm
            {
                LevelName = "Daily",
                TotalsInfo = totals.ToList()
            };
            return View("Totals", model);
        }

        /// <summary>
        /// Endpoint for facebook strategy totals page.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns></returns>
        public ActionResult StrategyTotals(DateTime? start, DateTime? end)
        {
            var defaultFromDate = DateTime.Now.AddDays(-10);
            var defaultToDate = DateTime.Now;
            var totals = facebookWebPortalDataService.GetCampaignTotalsInfo(defaultFromDate, defaultToDate);
            var model = new FacebookTotalsInfoVm
            {
                LevelName = "Strategy",
                TotalsInfo = totals.ToList()
            };
            return View("Totals", model);
        }

        /// <summary>
        /// Endpoint for facebook adset totals page.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns></returns>
        public ActionResult AdSetTotals(DateTime? start, DateTime? end)
        {
            var defaultFromDate = DateTime.Now.AddDays(-10);
            var defaultToDate = DateTime.Now;
            var totals = facebookWebPortalDataService.GetAdsetsTotalsInfo(defaultFromDate, defaultToDate);
            var model = new FacebookTotalsInfoVm
            {
                LevelName = "AdSet",
                TotalsInfo = totals.ToList()
            };
            return View("Totals", model);
        }

        /// <summary>
        /// Endpoint for facebook creatives totals page.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns></returns>
        public ActionResult CreativeTotals(DateTime? start, DateTime? end)
        {
            var defaultFromDate = DateTime.Now.AddDays(-10);
            var defaultToDate = DateTime.Now;
            var totals = facebookWebPortalDataService.GetAdsTotalsInfo(defaultFromDate, defaultToDate);
            var model = new FacebookTotalsInfoVm
            {
                LevelName = "Ad",
                TotalsInfo = totals.ToList()
            };
            return View("Totals", model);
        }
    }
}