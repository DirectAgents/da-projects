using System;
using System.Linq;
using System.Web.Mvc;
using DirectAgents.Domain.SpecialPlatformsDataProviders.Facebook;
using DirectAgents.Web.Areas.ProgAdmin.Models;

namespace DirectAgents.Web.Areas.ProgAdmin.Controllers.Facebook
{
    /// <summary>
    /// Facebook Stats Controller.
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class FacebookController : Controller
    {
        private readonly IFacebookWebPortalDataService dataService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookController"/> class.
        /// </summary>
        /// <param name="dataService">The facebook web portal data service.</param>
        public FacebookController(IFacebookWebPortalDataService dataService)
        {
            this.dataService = dataService;
        }

        /// <summary>
        /// Endpoint for facebook latests page.
        /// </summary>
        /// <param name="acctId">The acct identifier.</param>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns>Latests Action Result</returns>
        public ActionResult Latests()
        {
            var latests = dataService.GetAccountsLatestsInfo();
            var model = new FacebookLatestsInfoVm
            {
                LatestsInfo = latests.ToList(),
            };
            return View(model);
        }

        /// <summary>
        /// Endpoint for facebook daily totals page.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns>Daily Totals action Results</returns>
        public ActionResult DailyTotals(DateTime? start, DateTime? end)
        {
            if (start.HasValue && end.HasValue)
            {
                return View("Totals", new FacebookTotalsInfoVm
                {
                    LevelName = "Daily",
                    TotalsInfo = dataService.GetDailyTotalsInfo(start.Value, end.Value).ToList(),
                });
            }
            else
            {
                return RedirectToAction("DailyTotals", "Facebook", GetCurrentMonthStartEndDateParams());
            }
        }

        /// <summary>
        /// Endpoint for facebook strategy totals page.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns>Strategy totals action results.</returns>
        public ActionResult StrategyTotals(DateTime? start, DateTime? end)
        {
            if (start.HasValue && end.HasValue)
            {
                return View("Totals", new FacebookTotalsInfoVm
                {
                    LevelName = "Strategy",
                    TotalsInfo = dataService.GetCampaignTotalsInfo(start.Value, end.Value).ToList(),
                });
            }
            else
            {
                return RedirectToAction("StrategyTotals", "Facebook", GetCurrentMonthStartEndDateParams());
            }
        }

        /// <summary>
        /// Endpoint for facebook adset totals page.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns>Adset totals action result.</returns>
        public ActionResult AdSetTotals(DateTime? start, DateTime? end)
        {
            if (start.HasValue && end.HasValue)
            {
                return View("Totals", new FacebookTotalsInfoVm
                {
                    LevelName = "AdSet",
                    TotalsInfo = dataService.GetAdsetsTotalsInfo(start.Value, end.Value).ToList(),
                });
            }
            else
            {
                return RedirectToAction("AdSetTotals", "Facebook", GetCurrentMonthStartEndDateParams());
            }
        }

        /// <summary>
        /// Endpoint for facebook creatives totals page.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns>Creatives totals action result</returns>
        public ActionResult CreativeTotals(DateTime? start, DateTime? end)
        {
            if (start.HasValue && end.HasValue)
            {
                return View("Totals", new FacebookTotalsInfoVm
                {
                    LevelName = "Ad",
                    TotalsInfo = dataService.GetAdsTotalsInfo(start.Value, end.Value).ToList(),
                });
            }
            else
            {
                return RedirectToAction("CreativeTotals", "Facebook", GetCurrentMonthStartEndDateParams());
            }
        }

        private object GetCurrentMonthStartEndDateParams()
        {
            var firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            return new
            {
                start = firstDayOfMonth,
                end = lastDayOfMonth,
            };
        }
    }
}