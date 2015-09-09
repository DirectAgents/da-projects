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

        public ActionResult Pacing(DateTime? date, int? campId, bool showPerfStats = false)
        {
            if (!date.HasValue)
                date = DateTime.Today;
            var startOfMonth = new DateTime(date.Value.Year, date.Value.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            var campaigns = tdRepo.Campaigns();
            if (campId.HasValue)
                campaigns = campaigns.Where(c => c.Id == campId.Value);

            var budgetStats = new List<TDStatWithBudget>();
            foreach (var camp in campaigns.OrderBy(c => c.Advertiser.Name).ThenBy(c => c.Name))
            {
                var budgetInfo = camp.BudgetInfoFor(startOfMonth);
                var tdStat = tdRepo.GetTDStat(startOfMonth, endOfMonth, campaign: camp, marginFees: budgetInfo);
                var statWithBudget = new TDStatWithBudget(tdStat, budgetInfo);
                if (budgetInfo == null)
                    statWithBudget.Campaign = camp;

                // If we're viewing one particular campaign, show its external accounts
                if (campId.HasValue)
                {
                    var extAccountStats = new List<TDStat>();
                    var extAccounts = camp.ExtAccounts.OrderBy(a => a.Platform.Code).ThenBy(a => a.Name);
                    foreach (var extAccount in extAccounts)
                    {   //Note: Multiple Active Record Sets used here
                        var extAcctStat = tdRepo.GetTDStatWithAccount(startOfMonth, endOfMonth, extAccount: extAccount, marginFees: budgetInfo);
                        var extAcctBudgetStat = new TDStatWithBudget(extAcctStat, budgetInfo);
                        extAccountStats.Add(extAcctBudgetStat);
                    }
                    statWithBudget.ExtAccountStats = extAccountStats;
                }

                budgetStats.Add(statWithBudget);
            }
            var model = new CampaignPacingVM
            {
                CampaignBudgetStats = budgetStats,
                ShowPerfStats = showPerfStats
            };
            return View(model);
        }
	}
}