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

        public ActionResult Pacing(DateTime? date)
        {
            if (!date.HasValue)
                date = DateTime.Today;
            var startOfMonth = new DateTime(date.Value.Year, date.Value.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            var campaigns = tdRepo.Campaigns()
                .OrderBy(c => c.Advertiser.Name).ThenBy(c => c.Name);
            var budgetStats = new List<BudgetWithStats>();
            foreach (var camp in campaigns)
            {
                var budget = camp.BudgetFor(startOfMonth);
                var tdStat = tdRepo.GetTDStat(startOfMonth, endOfMonth, campaign: camp);
                var budgetWithStats = new BudgetWithStats(budget, tdStat, (budget == null) ? camp : null);
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