using ClientPortal.Data.Contexts;
using ClientPortal.Data.Contracts;
using ClientPortal.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientPortal.Web.Models
{
    public class AccountRepository
    {
        public static List<GoalVM> GetGoalVMs(IList<Data.Contexts.Goal> goals, IList<CakeOffer> offers, bool monthlyOnly)
        {
            var goalVMs = from g in goals
                          join o in offers on g.OfferId equals o.Offer_Id // todo: left join?
                          select new GoalVM(g, o.OfferName, OfferInfo.CurrencyToCulture(o.Currency));

            if (monthlyOnly)
                goalVMs = goalVMs.Where(g => g.IsMonthly);

            return goalVMs.ToList();
        }

    }
}