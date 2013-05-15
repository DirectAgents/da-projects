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
        private IClientPortalRepository cpRepo;

        public GoalsController(ICakeRepository cakeRepository, IClientPortalRepository cpRepository)
        {
            this.cakeRepo = cakeRepository;
            this.cpRepo = cpRepository;
        }

        public ActionResult Index()
        {
            var advId = HomeController.GetAdvertiserId();
            if (advId == null) return null;

            var goals = cpRepo.GetGoals(advId.Value);
            var offers = cakeRepo.Offers(advId);
            var goalVMs = AccountRepository.GetGoalVMs(goals.ToList(), offers.ToList(), false);
            var model = new GoalsModel()
            {
                Goals = goalVMs
            };
            return PartialView(model);
        }

        public ActionResult List()
        {
            var advId = HomeController.GetAdvertiserId();
            if (advId == null) return null;

            var goals = cpRepo.GetGoals(advId.Value);
            var offers = cakeRepo.Offers(advId);
            var goalVMs = AccountRepository.GetGoalVMs(goals.ToList(), offers.ToList(), false);
            return PartialView(goalVMs);
        }

        public ActionResult Add()
        {
            var defaultGoal = new GoalVM();
            return DoEdit(defaultGoal);
        }

        public ActionResult Edit(int id)
        {
            var userProfile = HomeController.GetUserProfile();
            var goal = cpRepo.GetGoal(id);
            var goalVM = new GoalVM(goal, null, userProfile.Culture);
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
            cpRepo.DeleteGoal(id, advId);
            return null;
        }

        // --- repository-type methods ---

        private void SaveGoal(GoalVM goalVM)
        {
            using (var usersContext = new UsersContext())
            {
                if (goalVM.Id < 0)
                {
                    var goal = new ClientPortal.Web.Models.Goal();
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