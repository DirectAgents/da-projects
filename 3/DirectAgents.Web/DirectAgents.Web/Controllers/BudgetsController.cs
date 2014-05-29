using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Concrete;
using DirectAgents.Domain.Contexts;
using System.Linq;
using System.Web.Mvc;

namespace DirectAgents.Web.Controllers
{
    public class BudgetsController : Controller
    {
        private IMainRepository mainRepo;

        public BudgetsController()
        {
            this.mainRepo = new MainRepository(new DAContext());
        }

        public ActionResult Advertisers()
        {
            var advertisers = mainRepo.GetAdvertisers();

            return View(advertisers.OrderBy(a => a.AdvertiserName));
        }

        public ActionResult Index()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            mainRepo.Dispose();
            base.Dispose(disposing);
        }
    }
}
