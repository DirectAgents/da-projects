using ClientPortal.Data.Contracts;
using ClientPortal.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ClientPortal.Web.Controllers
{
    public class GoalsController : Controller
    {
        private IOfferRepository offerRepo;

        public GoalsController(IOfferRepository offerRepository)
        {
            this.offerRepo = offerRepository;
        }

        public ActionResult Index()
        {
            var goals = GetGoals();
            var model = new GoalsModel()
            {
                Goals = goals
            };
            return PartialView(model);
        }

        public ActionResult List()
        {
            var goals = GetGoals();
            return PartialView(goals);
        }

        public ActionResult Add()
        {
            var defaultGoal = new GoalVM();
            return PartialView(defaultGoal);
        }

        [HttpPost]
        public ActionResult Add(Goal goal)
        {
            var advId = HomeController.GetAdvertiserId();
            if (advId.HasValue)
            {
                goal.AdvertiserId = advId.Value;
                SaveGoal(goal);
            }
            var goalVM = new GoalVM(goal);
            return PartialView("Item", goalVM);
        }

        // --- repository-type methods ---

        private List<GoalVM> GetGoals()
        {
            using (var usersContext = new UsersContext())
            {
                var goals = usersContext.Goals.ToList();
                var goalVMs = goals.Select(g => new GoalVM(g)).ToList();
                return goalVMs;
            }
        }

        private void SaveGoal(Goal goal)
        {
            using (var usersContext = new UsersContext())
            {
                usersContext.Goals.Add(goal);
                usersContext.SaveChanges();
            }
        }
    }
}