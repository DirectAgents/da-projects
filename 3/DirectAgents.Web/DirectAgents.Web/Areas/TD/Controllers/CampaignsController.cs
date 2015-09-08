using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Entities.TD;
using DirectAgents.Web.Areas.TD.Models;

namespace DirectAgents.Web.Areas.TD.Controllers
{
    public class CampaignsController : DirectAgents.Web.Controllers.ControllerBase
    {
        public CampaignsController(ITDRepository tdRepository)
        {
            this.tdRepo = tdRepository;
        }

        public ActionResult Index(int? advId)
        {
            var campaigns = tdRepo.Campaigns(advId: advId)
                .OrderBy(c => c.Advertiser.Name).ThenBy(c => c.Name);

            return View(campaigns);
        }

        public ActionResult Pacing(DateTime? date, int? campId)
        {
            if (!date.HasValue)
                date = DateTime.Today;
            var startOfMonth = new DateTime(date.Value.Year, date.Value.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            var campaigns = tdRepo.Campaigns();
            if (campId.HasValue)
                campaigns = campaigns.Where(c => c.Id == campId.Value);

            var budgetStats = new List<BudgetWithStats>();
            foreach (var camp in campaigns.OrderBy(c => c.Advertiser.Name).ThenBy(c => c.Name))
            {
                var budgetInfo = camp.BudgetInfoFor(startOfMonth);
                var tdStat = tdRepo.GetTDStat(startOfMonth, endOfMonth, campaign: camp);
                tdStat.SetMultipliers(budgetInfo);
                var budgetWithStats = new BudgetWithStats(budgetInfo, tdStat, (budgetInfo == null) ? camp : null);

                // If we're viewing one particular campaign, show it's external accounts
                if (campId.HasValue)
                {
                    var budgetStatsByExtAccount = new List<BudgetWithStats>();
                    var extAccounts = camp.ExtAccounts.OrderBy(a => a.Platform.Code).ThenBy(a => a.Name);
                    foreach (var extAccount in extAccounts)
                    {   //Note: Multiple Active Record Sets used here
                        var extAcctStat = tdRepo.GetTDStatWithAccount(startOfMonth, endOfMonth, extAccount: extAccount);
                        extAcctStat.SetMultipliers(budgetInfo);
                        var extAcctBudgetStat = new BudgetWithStats(budgetInfo, extAcctStat);
                        budgetStatsByExtAccount.Add(extAcctBudgetStat);
                    }
                    budgetWithStats.BudgetStatsByExtAccount = budgetStatsByExtAccount;
                }

                budgetStats.Add(budgetWithStats);
            }
            var model = new CampaignPacingVM
            {
                CampaignStats = budgetStats
            };
            return View(model);
        }
	}
}