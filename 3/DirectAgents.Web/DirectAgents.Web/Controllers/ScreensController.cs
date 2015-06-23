using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DirectAgents.Domain.Concrete;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.Cake;
using DirectAgents.Web.Models;

namespace DirectAgents.Web.Controllers
{
    public class ScreensController : ControllerBase
    {
        public ScreensController()
        {
            this.mainRepo = new MainRepository(new DAContext());
            //this.securityRepo = new SecurityRepository();
        }

        // ---

        public ActionResult Test()
        {
            var today = DateTime.Today;
            var startOfMonth = new DateTime(today.Year, today.Month, 1);
            var ods = mainRepo.GetOfferDailySummaries(null, startOfMonth);

            var advList = new List<Advertiser>();
            var advertisers = mainRepo.GetAdvertisers();
            foreach (var adv in advertisers)
            {
                var offerIds = adv.Offers.Select(o => o.OfferId).ToArray();
                var advDailySummaries = ods.Where(ds => offerIds.Contains(ds.OfferId));
                if (advDailySummaries.Any())
                {
                    adv.Stats = new StatsSummary
                    {
                        Views = advDailySummaries.Sum(ds => ds.Views),
                        Clicks = advDailySummaries.Sum(ds => ds.Clicks),
                        Conversions = advDailySummaries.Sum(ds => ds.Conversions),
                        Paid = advDailySummaries.Sum(ds => ds.Paid),
                        Sellable = advDailySummaries.Sum(ds => ds.Sellable),
                        Revenue = advDailySummaries.Sum(ds => ds.Revenue),
                        Cost = advDailySummaries.Sum(ds => ds.Cost)
                    };
                    advList.Add(adv);
                }
            }
            var model = new ScreensVM
            {
                Advertisers = advList.OrderByDescending(a => a.Stats.Revenue)
            };
            return View(model);
        }

        public JsonResult CakeStats() //MTD...
        {
            var today = DateTime.Today;
            var monthStart = new DateTime(today.Year, today.Month, 1);
            var stats = mainRepo.GetStatsSummary(null, monthStart, null);

            var json = Json(stats, JsonRequestBehavior.AllowGet);
            return json.ToJsonp();
        }

    }
}
