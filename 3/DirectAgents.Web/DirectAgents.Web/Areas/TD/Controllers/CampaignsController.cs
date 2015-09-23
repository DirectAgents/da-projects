using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.DTO;
using DirectAgents.Domain.Entities.TD;
using DirectAgents.Web.Areas.TD.Models;
using KendoGridBinderEx;
using KendoGridBinderEx.ModelBinder.Mvc;

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

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var campaign = tdRepo.Campaign(id);
            if (campaign == null)
                return HttpNotFound();
            SetupForEdit(id);
            return View(campaign);
        }
        [HttpPost]
        public ActionResult Edit(Campaign camp)
        {
            if (ModelState.IsValid)
            {
                if (tdRepo.SaveCampaign(camp))
                    return RedirectToAction("Index");
                ModelState.AddModelError("", "Campaign could not be saved.");
            }
            tdRepo.FillExtended(camp);
            SetupForEdit(camp.Id);
            return View(camp);
        }
        private void SetupForEdit(int campId)
        {
            ViewBag.ExtAccounts = tdRepo.ExtAccountsNotInCampaign(campId)
                .OrderBy(a => a.Platform.Name).ThenBy(a => a.Name).ThenBy(a => a.ExternalId);
        }

        public ActionResult AddAccount(int id, int acctId)
        {
            tdRepo.AddExtAccountToCampaign(id, acctId);
            return RedirectToAction("Edit", new { id = id });
        }
        public ActionResult RemoveAccount(int id, int acctId)
        {
            tdRepo.RemoveExtAccountFromCampaign(id, acctId);
            return RedirectToAction("Edit", new { id = id });
        }

        // Non-Kendo version
        public ActionResult Pacing(DateTime? date, int? campId, bool showPerfStats = false)
        {
            if (!date.HasValue)
                date = DateTime.Today;
            var startOfMonth = new DateTime(date.Value.Year, date.Value.Month, 1);

            var budgetStats = GetCampaignStatsWithBudget(startOfMonth, campId);

            var model = new CampaignPacingVM
            {
                CampaignBudgetStats = budgetStats,
                ShowPerfStats = showPerfStats
            };
            return View(model);
        }

        public ActionResult PacingGrid()
        {
            return View();
        }

        public ActionResult PerformanceGrid()
        {
            return View();
        }

        //[HttpPost]
        public JsonResult PacingData(KendoGridMvcRequest request)
        {
            // The "agg" aggregates will get computed outside of KendoGridEx
            if (request.AggregateObjects != null)
                request.AggregateObjects = request.AggregateObjects.Where(ao => ao.Aggregate != "agg");

            int? campId = null;
            DateTime? date = null;
            if (!date.HasValue)
                date = DateTime.Today;
            var startOfMonth = new DateTime(date.Value.Year, date.Value.Month, 1);

            var budgetStats = GetCampaignStatsWithBudget(startOfMonth, campId);
            var dtos = budgetStats.Select(bs => new CampaignPacingDTO
            {
                NumExtAccts = bs.Campaign.ExtAccounts.Count,
                Advertiser = bs.Campaign.Advertiser.Name,
                CampaignId = bs.Campaign.Id,
                Campaign = bs.Campaign.Name,
                Budget = bs.Budget.MediaSpend(),
                Cost = bs.Cost,
                MediaSpend = bs.MediaSpend(),
                TotalRev = bs.TotalRevenue(),
                Margin = bs.Margin(),
                MarginPct = bs.Budget.MarginPct / 100,
                PlatformNames = string.Join(",", bs.Campaign.ExtAccounts.Select(a => a.Platform).Distinct().Select(p => p.Name)),
                PctOfGoal = bs.FractionReached(),
                SalesRep = bs.Campaign.Advertiser.SalesRepName(),
                AM = bs.Campaign.Advertiser.AMName()
            }).ToList();
            var kgrid = new KendoGridEx<CampaignPacingDTO>(request, dtos);
            //return CreateJsonResult(kgrid);
            return CreateJsonResult(kgrid, allowGet: true);
        }

        //[HttpPost]
        public JsonResult PerformanceData(KendoGridMvcRequest request)
        {
            // The "agg" aggregates will get computed outside of KendoGridEx
            if (request.AggregateObjects != null)
                request.AggregateObjects = request.AggregateObjects.Where(ao => ao.Aggregate != "agg");

            int? campId = null;
            DateTime? date = null;
            if (!date.HasValue)
                date = DateTime.Today;
            var startOfMonth = new DateTime(date.Value.Year, date.Value.Month, 1);

            var budgetStats = GetCampaignStatsWithBudget(startOfMonth, campId);
            var dtos = budgetStats.Select(bs => new PerformanceDTO
            {
                CampaignId = bs.Campaign.Id,
                Campaign = bs.Campaign.Name,
                Budget = bs.Budget.MediaSpend(),
                Cost = bs.Cost,
                MediaSpend = bs.MediaSpend(),
                TotalRev = bs.TotalRevenue(),
                Margin = bs.Margin(),
                MarginPct = bs.Budget.MarginPct / 100,
                PlatformNames = string.Join(",", bs.Campaign.ExtAccounts.Select(a => a.Platform).Distinct().Select(p => p.Name)),
                PctOfGoal = bs.FractionReached(),
                Impressions = bs.Impressions,
                Clicks = bs.Clicks,
                TotalConv = bs.TotalConv,
                PostClickConv = bs.PostClickConv,
                PostViewConv = bs.PostViewConv,
                CTR = bs.CTR,
                CPA = bs.CPA
            }).ToList();
            var kgrid = new KendoGridEx<PerformanceDTO>(request, dtos);
            //return CreateJsonResult(kgrid);
            return CreateJsonResult(kgrid, allowGet: true);
        }

        // ---

        // Fills in external account stats if campId is specified
        private List<TDStatWithBudget> GetCampaignStatsWithBudget(DateTime startOfMonth, int? campId)
        {
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

                // If we're viewing one particular campaign, get its external accounts stats
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
            return budgetStats;
        }

        //       T could be CampaignPacingDTO, PerformanceDTO...
        private JsonResult CreateJsonResult<T>(KendoGridEx<T> kgrid, bool allowGet = false)
        {
            var kg = new KG<T>();
            kg.data = kgrid.Data;
            kg.total = kgrid.Total;
            kg.aggregates = Aggregates(kgrid);
            //kg.aggregates = kgrid.Aggregates;

            var json = Json(kg, allowGet ? JsonRequestBehavior.AllowGet : JsonRequestBehavior.DenyGet);
            return json;
        }

        private object Aggregates<T>(KendoGridEx<T> kgrid)
        {
            if (kgrid.Total == 0 || kgrid.Aggregates == null) return null;

            decimal budget = ((dynamic)kgrid.Aggregates)["Budget"]["sum"];
            decimal cost = ((dynamic)kgrid.Aggregates)["Cost"]["sum"];
            decimal mediaSpend = ((dynamic)kgrid.Aggregates)["MediaSpend"]["sum"];
            decimal totalRev = ((dynamic)kgrid.Aggregates)["TotalRev"]["sum"];
            decimal margin = ((dynamic)kgrid.Aggregates)["Margin"]["sum"];

            decimal? marginPct = null;
            if (totalRev != 0)
                marginPct = 1 - cost / totalRev;

            decimal? pctOfGoal = null;
            if (budget != 0)
                pctOfGoal = mediaSpend / budget;

            if (typeof(T) == typeof(CampaignPacingDTO))
            {
                int numExtAccts = ((dynamic)kgrid.Aggregates)["NumExtAccts"]["sum"];

                var aggs = new
                {
                    NumExtAccts = new { sum = numExtAccts },
                    Budget = new { sum = budget },
                    Cost = new { sum = cost },
                    MediaSpend = new { sum = mediaSpend },
                    TotalRev = new { sum = totalRev },
                    Margin = new { sum = margin },
                    MarginPct = new { agg = marginPct },
                    PctOfGoal = new { agg = pctOfGoal }
                };
                return aggs;
            }
            else if (typeof(T) == typeof(PerformanceDTO))
            {
                int impressions = ((dynamic)kgrid.Aggregates)["Impressions"]["sum"];
                int clicks = ((dynamic)kgrid.Aggregates)["Clicks"]["sum"];
                int totalConv = ((dynamic)kgrid.Aggregates)["TotalConv"]["sum"];
                int postClickConv = ((dynamic)kgrid.Aggregates)["PostClickConv"]["sum"];
                int postViewConv = ((dynamic)kgrid.Aggregates)["PostViewConv"]["sum"];

                double? ctr = null;
                if (impressions != 0)
                    ctr = (double)clicks / impressions;

                decimal? cpa = null;
                if (totalConv != 0)
                    cpa = mediaSpend / totalConv;

                var aggs = new
                {
                    Budget = new { sum = budget },
                    Cost = new { sum = cost },
                    MediaSpend = new { sum = mediaSpend },
                    TotalRev = new { sum = totalRev },
                    Margin = new { sum = margin },
                    MarginPct = new { agg = marginPct },
                    PctOfGoal = new { agg = pctOfGoal },
                    Impressions = new { sum = impressions },
                    Clicks = new { sum = clicks },
                    TotalConv = new { sum = totalConv },
                    PostClickConv = new { sum = postClickConv },
                    PostViewConv = new { sum = postViewConv },
                    CTR = new { agg = ctr },
                    CPA = new { agg = cpa }
                };
                return aggs;
            }
            else
                return null;
        }
    }
}