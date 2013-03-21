using System.Collections.Generic;
using System.Web.Mvc;
using ClientPortal.Web.Models;

namespace ClientPortal.Web.Controllers
{
    public class GoalsController : Controller
    {
        public ActionResult Index()
        {
            var goals = new List<GoalVM>
            {
                new GoalVM
                {
                    Name = "goal name 1",
                    Type = GoalTypeEnum.Percent,
                    Offer = "offer name 1",
                    Metric = MetricTypeEnum.Conversions,
                    Target = 50
                },
                new GoalVM
                {
                    Name = "goal name 2",
                    Type = GoalTypeEnum.Absolute,
                    Offer = "offer name 2",
                    Metric = MetricTypeEnum.Revenue,
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
                Type = GoalTypeEnum.Absolute,
                Metric = MetricTypeEnum.Conversions
            };

            return PartialView(defaultGoal);
        }

        [HttpPost]
        public ActionResult Add(GoalVM goalVM)
        {
            return PartialView("Item", goalVM);
        }

/*
        private List<GoalTypeVM> GetGoalTypes()
        {
            var goalTypes = new List<GoalTypeVM> {
                new GoalTypeVM() { Id = 1, Name = "Absolute" },
                new GoalTypeVM() { Id = 2, Name = "Percent" }
            };
            return goalTypes;
        }

        private List<MetricTypeVM> GetMetricTypes()
        {
            var metricTypes = new List<MetricTypeVM> {
                new MetricTypeVM() { Id = 1, Name = "Conversions" },
                new MetricTypeVM() { Id = 2, Name = "Revenue"}
            };
            return metricTypes;
        }

*/
    }
}