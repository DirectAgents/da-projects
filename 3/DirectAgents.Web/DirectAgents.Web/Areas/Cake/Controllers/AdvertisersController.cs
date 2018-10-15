using System;
using System.Linq;
using System.Web.Mvc;
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

        public ActionResult IndexGauge(bool all = false)
        {
            var advGauges = daRepo.GetGaugesByAdvertiser();
            return View(advGauges.OrderBy(x => x.Advertiser.AdvertiserName));
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