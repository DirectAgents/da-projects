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

            Session["advId"] = advId.ToString();
            return View(campaigns);
        }

        public ActionResult CreateNew(int advId)
        {
            var campaign = new Campaign
            {
                AdvertiserId = advId,
                Name = "New",
                DefaultBudget = new BudgetVals()
            };
            if (tdRepo.AddCampaign(campaign))
                return RedirectToAction("Index", new { advId = Session["advId"] });
            else
                return Content("Error creating Campaign");
        }
        public ActionResult Delete(int id)
        {
            tdRepo.DeleteCampaign(id);
            return RedirectToAction("Index", new { advId = Session["advId"] });
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
                    return RedirectToAction("Index", new { advId = Session["advId"] });
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

        // --- Stats ---

        // Non-Kendo version
        public ActionResult Pacing(int? campId, bool showPerfStats = false)
        {
            DateTime currMonth = SetChooseMonthViewData();
            var budgetStats = GetCampaignStatsWithBudget(currMonth, campId);

            var model = new CampaignPacingVM
            {
                CampaignBudgetStats = budgetStats,
                ShowPerfStats = showPerfStats
            };
            return View(model);
        }
        public ActionResult Pacing2(int campId)
        {
            DateTime currMonth = SetChooseMonthViewData();
            var campStats = tdRepo.GetCampStats(currMonth, campId);
            var model = new List<TDCampStats>() { campStats };
            return View(model);
        }

        public ActionResult PacingGrid()
        {
            SetChooseMonthViewData();
            return View();
        }

        public ActionResult PerformanceGrid()
        {
            return View();
        }

        //[HttpPost]
        public JsonResult PacingData(KendoGridMvcRequest request) //, DateTime? month)
        {
            // The "agg" aggregates will get computed outside of KendoGridEx
            if (request.AggregateObjects != null)
                request.AggregateObjects = request.AggregateObjects.Where(ao => ao.Aggregate != "agg");

            var startOfMonth = CurrentMonthTD;
            var budgetStats = GetCampaignStatsWithBudget(startOfMonth);
            var dtos = budgetStats.Select(bs => new CampaignPacingDTO(bs)).ToList();
            var kgrid = new KendoGridEx<CampaignPacingDTO>(request, dtos);
            //return CreateJsonResult(kgrid);
            return CreateJsonResult(kgrid, Aggregates(kgrid), allowGet: true);
        }

        //[HttpPost]
        public JsonResult PerformanceData(KendoGridMvcRequest request)
        {
            // The "agg" aggregates will get computed outside of KendoGridEx
            if (request.AggregateObjects != null)
                request.AggregateObjects = request.AggregateObjects.Where(ao => ao.Aggregate != "agg");

            var startOfMonth = CurrentMonthTD;
            var budgetStats = GetCampaignStatsWithBudget(startOfMonth);
            var dtos = budgetStats.Select(bs => new PerformanceDTO(bs)).ToList();
            var kgrid = new KendoGridEx<PerformanceDTO>(request, dtos);
            //return CreateJsonResult(kgrid);
            return CreateJsonResult(kgrid, Aggregates(kgrid), allowGet: true);
        }

        // ---

        // Fills in external account stats if campId is specified
        private List<TDStatWithBudget> GetCampaignStatsWithBudget(DateTime startOfMonth, int? campId = null)
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

        // T could be CampaignPacingDTO, PerformanceDTO...
        public static object Aggregates<T>(KendoGridEx<T> kgrid)
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
                var aggs = new
                {
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