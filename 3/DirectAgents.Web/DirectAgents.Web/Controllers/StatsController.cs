using System;
using System.Linq;
using System.Web.Mvc;
using DirectAgents.Domain.Concrete;
using DirectAgents.Domain.Contexts;

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