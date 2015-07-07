using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Concrete;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.Cake;
using DirectAgents.Web.Models;

namespace DirectAgents.Web.Controllers
{
    public class StatsController : ControllerBase
    {
        public StatsController()
        {
            this.mainRepo = new MainRepository(new DAContext()); // TODO: injection
            //this.securityRepo = new SecurityRepository();
        }

        // ---

        public ActionResult Links()
        {
            return View();
        }

        // Set numdays=-1 for MTD, 0 for today (so far)
        // Set take=0 for all
        public ActionResult CakeAdv(string sort, int numdays = -1, int take = 0, bool jsonp = false)
        {
            var advStats = GetAdvStats(sort, numdays, take);
            if (jsonp)
            {
                var json = Json(advStats, JsonRequestBehavior.AllowGet);
                return json.ToJsonp();
            }
            else
            {
                var model = new ScreensVM { AdvStats = advStats };
                return View(model);
            }
        }
        private IEnumerable<StatsSummary> GetAdvStats(string sort, int numdays = -1, int take = 0)
        {
            var today = DateTime.Today;
            DateTime startDate;
            if (numdays < 1) // MTD
                startDate = new DateTime(today.Year, today.Month, 1);
            else
                startDate = today.AddDays(-numdays);

            var ods = mainRepo.GetOfferDailySummaries(null, startDate);

            IEnumerable<StatsSummary> advStats = new List<StatsSummary>();
            var advertisers = mainRepo.GetAdvertisers();
            foreach (var adv in advertisers)
            {
                var offerIds = adv.Offers.Select(o => o.OfferId).ToArray();
                var advDailySummaries = ods.Where(ds => offerIds.Contains(ds.OfferId));
                if (advDailySummaries.Any())
                {
                    var stats = new StatsSummary
                    {
                        Name = adv.AdvertiserName,
                        Views = advDailySummaries.Sum(ds => ds.Views),
                        Clicks = advDailySummaries.Sum(ds => ds.Clicks),
                        Conversions = advDailySummaries.Sum(ds => ds.Conversions),
                        Paid = advDailySummaries.Sum(ds => ds.Paid),
                        Sellable = advDailySummaries.Sum(ds => ds.Sellable),
                        Revenue = advDailySummaries.Sum(ds => ds.Revenue),
                        Cost = advDailySummaries.Sum(ds => ds.Cost)
                    };
                    ((List<StatsSummary>)advStats).Add(stats);
                }
            }
            if (sort == "margin")
                advStats = advStats.OrderByDescending(a => a.Margin);
            else if (sort == "cost")
                advStats = advStats.OrderByDescending(a => a.Cost);
            else // default: by revenue
                advStats = advStats.OrderByDescending(a => a.Revenue);

            if (take > 0)
                advStats = advStats.Take(take);

            return advStats;
        }

        public JsonResult CakeSummary(int numdays = -1)
        {
            var today = DateTime.Today;
            DateTime startDate;
            if (numdays < 1) // default: MTD
                startDate = new DateTime(today.Year, today.Month, 1);
            else
                startDate = today.AddDays(-numdays);
            var stats = mainRepo.GetStatsSummary(null, startDate, null);

            var json = Json(stats, JsonRequestBehavior.AllowGet);
            return json.ToJsonp();
        }

        public ActionResult AdRoll()
        {
            var today = DateTime.Today;
            var startOfMonth = new DateTime(today.Year, today.Month, 1);

            var advertisables = mainRepo.Advertisables();
            foreach (var adv in advertisables)
            {
                mainRepo.FillStats(adv, startOfMonth, null); // MTD
            }
            var model = advertisables.OrderBy(a => a.Name).ToList();
            return View(model);
        }
    }
}