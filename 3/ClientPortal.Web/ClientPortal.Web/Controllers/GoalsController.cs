using ClientPortal.Data.Contexts;
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
            var advId = HomeController.GetAdvertiserId();
            if (advId == null) return null;

            var goals = GetGoals(advId.Value);
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

            var goals = GetGoals(advId.Value);
            return PartialView(goals);
        }

        public ActionResult Add()
        {
            var defaultGoal = new GoalVM();
            return DoEdit(defaultGoal);
        }

        public ActionResult Edit(int id)
        {
            var goalVM = GetGoal(id);
            return DoEdit(goalVM);
        }

        private ActionResult DoEdit(GoalVM goalVM)
        {
            var advId = HomeController.GetAdvertiserId();

            List<CakeOffer> offers = new List<CakeOffer>();
            if (advId.HasValue)
                offers = offerRepo.CakeOffers(advId).ToList();

            ViewBag.Offers = offers;

            return PartialView("Edit", goalVM);
        }

        [HttpPost]
        public ActionResult Save(Goal goal)
        {
            var advId = HomeController.GetAdvertiserId();
            if (advId.HasValue)
            {
                goal.AdvertiserId = advId.Value;
                SaveGoal(goal);
            }
            var goalVM = new GoalVM(goal, null);
            return PartialView("Item", goalVM);
        }

        // --- repository-type methods ---

        private List<GoalVM> GetGoals(int advertiserId)
        {
            List<Goal> goals;
            using (var usersContext = new UsersContext())
            {
                goals = usersContext.Goals.Where(g => g.AdvertiserId == advertiserId).ToList();
            }
            var offers = offerRepo.CakeOffers(advertiserId).ToList();

            var goalVMs = from g in goals
                          join o in offers on g.OfferId equals o.Offer_Id // todo: left join?
                          select new GoalVM(g, o.OfferName);

            return goalVMs.ToList();
        }

        private GoalVM GetGoal(int id)
        {
            using (var usersContext = new UsersContext())
            {
                var goal = usersContext.Goals.Where(g => g.Id == id).FirstOrDefault();
                if (goal == null)
                    return null;
                else
                    return new GoalVM(goal, null);
            }
        }

        private void SaveGoal(Goal goal)
        {
            using (var usersContext = new UsersContext())
            {
                if (goal.Id < 0)
                    usersContext.Goals.Add(goal);
                else
                {
                    var existingGoal = usersContext.Goals.FirstOrDefault(g => g.Id == goal.Id);
                    if (existingGoal != null)
                        TryUpdateModel(existingGoal);
                }
                usersContext.SaveChanges();
            }
        }
    }
}