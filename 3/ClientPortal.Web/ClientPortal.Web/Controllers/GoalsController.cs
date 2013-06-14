using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ClientPortal.Data.Contexts;
using ClientPortal.Data.Contracts;
using ClientPortal.Web.Models;

namespace ClientPortal.Web.Controllers
{
    [Authorize]
    public class GoalsController : CPController
    {
        public GoalsController(IClientPortalRepository cpRepository)
        {
            this.cpRepo = cpRepository;
        }

        public ActionResult Index()
        {
            var advId = GetAdvertiserId();
            if (advId == null) return null;

            var goalVMs = cpRepo.GetGoals(advId.Value).ToList().Select(g => new GoalVM(g));
            var model = new GoalsModel()
            {
                Goals = goalVMs.ToList()
            };
            return PartialView(model);
        }

        public ActionResult List()
        {
            var advId = GetAdvertiserId();
            if (advId == null) return null;

            var goalVMs = cpRepo.GetGoals(advId.Value).ToList().Select(g => new GoalVM(g));
            return PartialView(goalVMs);
        }

        public ActionResult Add()
        {
            var defaultGoal = new GoalVM();
            return DoEdit(defaultGoal);
        }

        public ActionResult Edit(int id)
        {
            var goal = cpRepo.GetGoal(id);
            var goalVM = new GoalVM(goal);
            return DoEdit(goalVM);
        }

        private ActionResult DoEdit(GoalVM goalVM)
        {
            var advId = GetAdvertiserId();

            List<Offer> offers = new List<Offer>();
            if (advId.HasValue)
                offers = cpRepo.Offers(advId).ToList();

            ViewBag.Offers = offers;

            return PartialView("Edit", goalVM);
        }

        [HttpPost]
        public ActionResult Save(GoalVM goalVM)
        {
            bool success = false;
            var advId = GetAdvertiserId();

            if (ModelState.IsValid)
            {
                Goal goal;
                if (goalVM.Id < 0)
                {
                    goal = new Goal() { AdvertiserId = advId.HasValue ? advId.Value : 0 };
                    goalVM.SetEntityProperties(goal);
                    cpRepo.AddGoal(goal, true);
                    success = true;
                }
                else
                {
                    var existingGoal = cpRepo.GetGoal(goalVM.Id);
                    if (existingGoal != null && (advId == null || advId.Value == existingGoal.AdvertiserId))
                    {
                        goalVM.SetEntityProperties(existingGoal);
                        cpRepo.SaveChanges();
                        success = true;
                    }
                }
                return Json(new { success = success, OfferId = goalVM.OfferId, GoalId = goalVM.Id });
            }
            else
            {
                return DoEdit(goalVM);
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var advId = GetAdvertiserId();
            cpRepo.DeleteGoal(id, advId);
            return null;
        }
    }
}