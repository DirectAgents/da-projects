using System.Collections.Generic;
using System.Web.Mvc;
using ClientPortal.Web.Models;

namespace ClientPortal.Web.Controllers
{
    public class GoalsController : Controller
    {
        public ActionResult Index()
        {
            var goalTypes = GetGoalTypes();
            var metrics = GetMetrics();

            var goals = new List<GoalVM>
            {
                new GoalVM
                {
                    Name = "goal name 1",
                    Type = goalTypes[1],
                    TypeId = goalTypes[1].Id,
                    Offer = "offer name 1",
                    Metric = metrics[0],
                    MetricId = metrics[0].Id,
                    Target = 50
                },
                new GoalVM
                {
                    Name = "goal name 2",
                    Type = goalTypes[0],
                    TypeId = goalTypes[0].Id,
                    Offer = "offer name 2",
                    Metric = metrics[1],
                    MetricId = metrics[1].Id,
                    Target = 1000
                },
            };

            var model = new GoalsModel()
            {
                Goals = goals
            };

            return PartialView(model);
        }

        public ActionResult Add()
        {
            var defaultGoal = new GoalVM
            {
                TypeId = GoalTypeEnum.Percent,
                MetricId = MetricEnum.Revenue
            };

            return PartialView(defaultGoal);
        }

        [HttpPost]
        public ActionResult Add(GoalVM goalVM)
        {
            return PartialView("Item", goalVM);
        }


        private List<GoalTypeVM> GetGoalTypes()
        {
            var goalTypes = new List<GoalTypeVM> {
                new GoalTypeVM() { Id = GoalTypeEnum.Absolute, Name = "Absolute" },
                new GoalTypeVM() { Id = GoalTypeEnum.Percent, Name = "Percent" }
            };
            return goalTypes;
        }

        private List<MetricVM> GetMetrics()
        {
            var metrics = new List<MetricVM> {
                new MetricVM() { Id = MetricEnum.Conversions, Name = "Conversions" },
                new MetricVM() { Id = MetricEnum.Revenue, Name = "Revenue"}
            };
            return metrics;
        }

    }
}