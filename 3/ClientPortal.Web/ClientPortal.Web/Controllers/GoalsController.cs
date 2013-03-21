using System.Collections.Generic;
using System.Web.Mvc;
using ClientPortal.Web.Models;

namespace ClientPortal.Web.Controllers
{
    public class GoalsController : Controller
    {
        //
        // GET: /Goals/

        public ActionResult Index()
        {
            return PartialView(new List<GoalVM>
                {
                    new GoalVM
                        {
                            Name = "goal name",
                            Type = "Absolute|Percent",
                            Offer = "offer name",
                            Metric = "Conversions|Revenue",
                            Target = 50
                        }
                });
        }

        public ActionResult Add()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult Add(GoalVM goalVM)
        {
            return PartialView("Item", goalVM);
        }
    }
}