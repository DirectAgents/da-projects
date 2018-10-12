using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
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
    }
}