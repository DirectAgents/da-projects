using ClientPortal.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientPortal.Web.Models
{
    public class AccountRepository
    {
        public static List<GoalVM> GetGoals(int advertiserId, int? offerId, ICakeRepository cakeRepo)
        {
            List<Goal> goals;
            using (var usersContext = new UsersContext())
            {
                goals = usersContext.Goals.Where(g => g.AdvertiserId == advertiserId).ToList();
            }
            var offers = cakeRepo.Offers(advertiserId);
            if (offerId.HasValue)
                offers = offers.Where(o => o.Offer_Id == offerId.Value);

            var goalVMs = from g in goals
                          join o in offers.ToList() on g.OfferId equals o.Offer_Id // todo: left join?
                          select new GoalVM(g, o.OfferName, o.Currency);

            return goalVMs.ToList();
        }

        public static GoalVM GetGoal(int id)
        {
            using (var usersContext = new UsersContext())
            {
                var goal = usersContext.Goals.Where(g => g.Id == id).FirstOrDefault();
                if (goal == null)
                    return null;
                else
                    return new GoalVM(goal, null, null);
            }
        }

        // returns whether goal was deleted successfully
        // if an advertiserId is passed in, the goal will only be deleted if its advertiserId matches
        public static bool DeleteGoal(int id, int? advertiserId)
        {
            bool deleted = false;
            using (var usersContext = new UsersContext())
            {
                var goal = usersContext.Goals.Where(g => g.Id == id).FirstOrDefault();
                if (goal != null && (advertiserId == null || goal.AdvertiserId == advertiserId.Value))
                {
                    usersContext.Goals.Remove(goal);
                    usersContext.SaveChanges();
                    deleted = true;
                }
            }
            return deleted;
        }

    }
}