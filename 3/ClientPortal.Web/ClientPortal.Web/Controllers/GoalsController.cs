using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ClientPortal.Data.Contexts;
using ClientPortal.Data.Contracts;
using ClientPortal.Web.Models;

namespace ClientPortal.Web.Controllers
{
    [Authorize]
    public class GoalsController : Controller
    {
        private ICakeRepository cakeRepo;

        public GoalsController(ICakeRepository cakeRepository)
        {
            this.cakeRepo = cakeRepository;
        }

        public ActionResult Index()
        {
            var advId = HomeController.GetAdvertiserId();
            if (advId == null) return null;

            var goals = AccountRepository.GetGoals(advId.Value, null, false, cakeRepo);
            var model = new GoalsModel()
            {
                Goals = goals
            };
            return PartialView(model);
        }

        public ActionResult List()
        {
            var advId = HomeController.GetAdvertiserId();
            if (advId == null) return null;

            var goals = AccountRepository.GetGoals(advId.Value, null, false, cakeRepo);
            return PartialView(goals);
        }

        public ActionResult Add()
        {
            var defaultGoal = new GoalVM();
            return DoEdit(defaultGoal);
        }

        public ActionResult Edit(int id)
        {
            var userProfile = HomeController.GetUserProfile();
            var goalVM = AccountRepository.GetGoal(id, userProfile.Culture);
            return DoEdit(goalVM);
        }

        private ActionResult DoEdit(GoalVM goalVM)
        {
            var advId = HomeController.GetAdvertiserId();

            List<CakeOffer> offers = new List<CakeOffer>();
            if (advId.HasValue)
                offers = cakeRepo.Offers(advId).ToList();

            ViewBag.Offers = offers;

            return PartialView("Edit", goalVM);
        }

        [HttpPost]
        public ActionResult Save(GoalVM goal)
        {
            var userProfile = HomeController.GetUserProfile();
            if (ModelState.IsValid)
            {
                var advId = userProfile.CakeAdvertiserId;
                if (advId.HasValue)
                {
                    goal.AdvertiserId = advId.Value;
                    SaveGoal(goal);
                }
                return Json(new { success = true, OfferId = goal.OfferId, GoalId = goal.Id });
            }
            else
            {
                return DoEdit(goal);
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var advId = HomeController.GetAdvertiserId();
            AccountRepository.DeleteGoal(id, advId);
            return null;
        }

        // --- repository-type methods ---

        private void SaveGoal(GoalVM goalVM)
        {
            using (var usersContext = new UsersContext())
            {
                if (goalVM.Id < 0)
                {
                    Goal goal = new Goal();
                    goalVM.SetGoalEntityProperties(goal);
                    usersContext.Goals.Add(goal);
                }
                else
                {
                    var existingGoal = usersContext.Goals.FirstOrDefault(g => g.Id == goalVM.Id);
                    if (existingGoal != null)
                        goalVM.SetGoalEntityProperties(existingGoal);
                }
                usersContext.SaveChanges();
            }
        }

    }
}