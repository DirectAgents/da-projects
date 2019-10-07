using System;
using System.Linq;
using System.Web.Mvc;
using CakeExtracter.CakeMarketingApi;
using CakeExtracter.Commands;
using CakeExtracter.Commands.DA;
using DirectAgents.Domain.Abstract;
using DirectAgents.Web.Areas.Cake.Models;

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

        public ActionResult IndexGauge() //(bool all = false)
        {
            var today = DateTime.Today;
            var monthStart = new DateTime(today.Year, today.Month, 1);
            var advGaugesQuery = daRepo.GetGaugesByAdvertiser(startDateForStats: monthStart);
            var advGauges = advGaugesQuery.OrderBy(x => x.Advertiser.AdvertiserName).ToList();
            advGauges.Add(daRepo.GetGaugeForAllAdvertisers(startDateForStats: monthStart));
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

        public ActionResult ShowStats(int id, DateTime? start, DateTime? end)
        {
            var adv = daRepo.GetAdvertiser(id);
            if (adv == null)
                return HttpNotFound();

            var today = DateTime.Today;
            start = start ?? new DateTime(today.Year, today.Month, 1);
            end = end ?? today;

            var campSums = adv.Offers.AsQueryable().SelectMany(o => o.Camps).SelectMany(c => c.CampSums)
                .Where(cs => cs.Date >= start.Value && cs.Date <= end.Value);
            var eventConvs = daRepo.GetEventConversions(advertiserId: id, startDate: start, endDate: end);

            var model = new AdvertiserStatsVM
            {
                Advertiser = adv,
                Start = start.Value,
                End = end.Value,
                NumOffers = adv.Offers.Count(),
                CampSum_Convs = campSums.Sum(cs => (decimal?)cs.Conversions) ?? 0,
                CampSum_Paid = campSums.Sum(cs => (decimal?)cs.Paid) ?? 0,
                EventConvs_Count = eventConvs.Count(),
            };
            if (model.EventConvs_Count > 0)
            {
                model.EventConvs_Earliest = eventConvs.Select(x => x.ConvDate).Min();
                model.EventConvs_Latest = eventConvs.Select(x => x.ConvDate).Max();
            }
            return View(model);
        }

        public ActionResult ClearCampSumsCustom(int id, DateTime start, DateTime end)
        {
            var campSums = daRepo.GetCampSums(advertiserId: id, startDate: start, endDate: end);
            daRepo.DeleteCampSums(campSums);

            return Redirect(ShowStatsUrl(id, start, end));
        }

        public ActionResult LoadCampSumsCustom(int id, DateTime start, DateTime end)
        {
            DASynchCampSums.RunStatic(advertiserIds: id.ToString(), startDate: start, endDate: end);

            return Redirect(ShowStatsUrl(id, start, end));
        }

        public ActionResult ClearEventConvsCustom(int id, DateTime start, DateTime end)
        {
            var eventConvs = daRepo.GetEventConversions(advertiserId: id, startDate: start, endDate: end);
            daRepo.DeleteEventConversions(eventConvs);

            return Redirect(ShowStatsUrl(id, start, end));
        }

        public ActionResult LoadEventConvsCustom(int id, DateTime start, DateTime end)
        {
            DASynchCakeEventConversions.RunStatic(advertiserId: id, startDate: start, endDate: end);

            return Redirect(ShowStatsUrl(id, start, end));
        }

        private string ShowStatsUrl(int id, DateTime start, DateTime end)
        {
            var url = Url.Action("ShowStats", new { id = id }) + "?start=" + start.ToShortDateString() + "&end=" + end.ToShortDateString();
            return url;
        }
    }
}
