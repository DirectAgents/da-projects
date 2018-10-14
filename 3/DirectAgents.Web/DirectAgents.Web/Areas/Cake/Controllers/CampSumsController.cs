using System;
using System.Linq;
using System.Web.Mvc;
using DirectAgents.Domain.Abstract;

namespace DirectAgents.Web.Areas.Cake.Controllers
{
    public class CampSumsController : DirectAgents.Web.Controllers.ControllerBase
    {
        public CampSumsController(IMainRepository mainRepository)
        {
            this.daRepo = mainRepository;
        }

        public ActionResult Index(int? campId)
        {
            var campSums = daRepo.GetCampSums(campId: campId);
            if (campSums.Any())
            {
                var firstCampSum = campSums.First();
                if (campId.HasValue)
                    ViewBag.Camp = firstCampSum.Camp;
            }
            return View(campSums.OrderBy(x => x.Date));
        }
    }
}