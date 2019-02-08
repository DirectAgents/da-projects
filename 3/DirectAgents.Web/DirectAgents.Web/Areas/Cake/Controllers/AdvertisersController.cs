using System;
using System.Linq;
using System.Web.Mvc;
using CakeExtracter.CakeMarketingApi;
using CakeExtracter.Commands;
using DirectAgents.Domain.Abstract;

namespace DirectAgents.Web.Areas.Cake.Controllers
{
    public class AdvertisersController : DirectAgents.Web.Controllers.ControllerBase
    {
        public AdvertisersController(IMainRepository mainRepository)
        {
            this.daRepo = mainRepository;
        }

        public ActionResult Index(int minOffers = 0)
        {
            var advs = daRepo.GetAdvertisers();
            advs = advs.Where(x => x.Offers.Count() >= minOffers);
            return View(advs.OrderBy(x => x.AdvertiserName));
        }

        public ActionResult IndexGauge(DateTime? start, DateTime? end)
        {
            var today = DateTime.Today;
            start = start ?? new DateTime(today.Year, today.Month, 1);

            var advGaugesQuery = daRepo.GetGaugesByAdvertiser(startDateForStats: start);
            var advGauges = advGaugesQuery.OrderBy(x => x.Advertiser.AdvertiserName).ToList();
            advGauges.Add(daRepo.GetGaugeForAllAdvertisers(startDateForStats: start));
            return View(advGauges);
        }

        public ActionResult ClearCampSums(int id, bool justToday = false)
        {
            var today = DateTime.Today;
            var startDate = justToday ? today : new DateTime(today.Year, today.Month, 1); // beginning of month
            var campSums = daRepo.GetCampSums(advertiserId: id, startDate: startDate);
            daRepo.DeleteCampSums(campSums);
            return RedirectToAction("IndexGauge");
        }
        public ActionResult LoadCampSums(int id, bool justToday = false)
        {
            var today = DateTime.Today;
            var startDate = justToday ? today : new DateTime(today.Year, today.Month, 1); // beginning of month
            DASynchCampSums.RunStatic(advertiserIds: id.ToString(), startDate: startDate, endDate: today);
            return RedirectToAction("IndexGauge");
        }

        public ActionResult ClearAffSubSums(int id, bool justToday = false)
        {
            var today = DateTime.Today;
            var startDate = justToday ? today : new DateTime(today.Year, today.Month, 1); // beginning of month
            var affSubSums = daRepo.GetAffSubSummaries(advertiserId: id, startDate: startDate);
            daRepo.DeleteAffSubSummaries(affSubSums);
            return RedirectToAction("IndexGauge");
        }
        public ActionResult LoadAffSubSums(int id, bool justToday = false)
        {
            var today = DateTime.Today;
            var startDate = justToday ? today : new DateTime(today.Year, today.Month, 1); // beginning of month
            DASynchAffSubSums.RunStatic(advertiserId: id, startDate: startDate, endDate: today);
            return RedirectToAction("IndexGauge");
        }

        public ActionResult ClearConvs(int id, bool justToday = false)
        {
            var today = DateTime.Today;
            var startDate = justToday ? today : new DateTime(today.Year, today.Month, 1); // beginning of month
            var eventConvs = daRepo.GetEventConversions(advertiserId: id, startDate: startDate);
            daRepo.DeleteEventConversions(eventConvs);
            return RedirectToAction("IndexGauge");
        }
        public ActionResult LoadConvs(int id, bool justToday = false)
        {
            var today = DateTime.Today;
            var startDate = justToday ? today : new DateTime(today.Year, today.Month, 1); // beginning of month
            DASynchCakeEventConversions.RunStatic(advertiserId: id, startDate: startDate, endDate: today);
            return RedirectToAction("IndexGauge");
        }
    }
}
