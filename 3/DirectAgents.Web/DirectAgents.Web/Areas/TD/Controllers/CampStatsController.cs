using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.DTO;
using DirectAgents.Web.Areas.TD.Models;
using KendoGridBinderEx;
using KendoGridBinderEx.ModelBinder.Mvc;

namespace DirectAgents.Web.Areas.TD.Controllers
{
    public class CampStatsController : DirectAgents.Web.Controllers.ControllerBase
    {
        public CampStatsController(ITDRepository tdRepository)
        {
            this.tdRepo = tdRepository;
        }

        public ActionResult Pacing(int? campId, bool showPerfStats = false)
        {
            DateTime currMonth = SetChooseMonthViewData();
            var campStats = GetCampStats(currMonth, campId);
            var model = new CampaignPacingVM
            {
                CampStats = campStats,
                ShowPerfStats = showPerfStats
            };
            return View(model);
        }

        public ActionResult PacingGrid()
        {
            SetChooseMonthViewData();
            return View();
        }
        //[HttpPost]
        public JsonResult PacingData(KendoGridMvcRequest request) //, DateTime? month)
        {
            // The "agg" aggregates will get computed outside of KendoGridEx
            if (request.AggregateObjects != null)
                request.AggregateObjects = request.AggregateObjects.Where(ao => ao.Aggregate != "agg");

            var startOfMonth = CurrentMonthTD;
            var campStats = GetCampStats(startOfMonth);
            var dtos = campStats.Select(s => new CampaignPacingDTO(s));
            var kgrid = new KendoGridEx<CampaignPacingDTO>(request, dtos);

            return CreateJsonResult(kgrid, Aggregates(kgrid), allowGet: true);
        }
        //[HttpPost]
        public JsonResult PacingDetail(KendoGridMvcRequest request, int campId) //, DateTime? month)
        {
            DateTime currMonth = CurrentMonthTD;
            var stat = tdRepo.GetCampStats(currMonth, campId);
            var dtos = stat.PlatformStats.Select(s => new CampaignPacingDTO(s));
            var kgrid = new KendoGridEx<CampaignPacingDTO>(request, dtos);

            return CreateJsonResult(kgrid, Aggregates(kgrid), allowGet: true);
        }

        public ActionResult PerformanceGrid()
        {
            SetChooseMonthViewData();
            return View();
        }
        //[HttpPost]
        public JsonResult PerformanceData(KendoGridMvcRequest request) //, DateTime? month)
        {
            // The "agg" aggregates will get computed outside of KendoGridEx
            if (request.AggregateObjects != null)
                request.AggregateObjects = request.AggregateObjects.Where(ao => ao.Aggregate != "agg");

            var startOfMonth = CurrentMonthTD;
            var campStats = GetCampStats(startOfMonth);
            var dtos = campStats.Select(s => new PerformanceDTO(s));
            var kgrid = new KendoGridEx<PerformanceDTO>(request, dtos);

            return CreateJsonResult(kgrid, Aggregates(kgrid), allowGet: true);
        }

        private IEnumerable<TDCampStats> GetCampStats(DateTime startOfMonth, int? campId = null)
        {
            DateTime currMonth = CurrentMonthTD;

            var campaigns = tdRepo.Campaigns();
            if (campId.HasValue)
                campaigns = campaigns.Where(c => c.Id == campId.Value);

            var campStatsList = new List<TDCampStats>();
            foreach (var camp in campaigns.OrderBy(c => c.Advertiser.Name).ThenBy(c => c.Name))
            {
                var stat = tdRepo.GetCampStats(currMonth, camp.Id);
                if (!stat.AllZeros())
                    campStatsList.Add(stat);
                // TODO: include campStats for campaigns with BudgetInfos (or PlatformBudgetInfos?), even if no stats ?
            }
            return campStatsList;
        }

        public ActionResult Spreadsheet(int campId)
        {
            return View(campId);
        }
        public JsonResult SpreadsheetData(int campId)
        {
            DateTime currMonth = CurrentMonthTD;
            var campStats = tdRepo.GetCampStats(currMonth, campId);

            var dtos = new List<CampaignPacingDTO> { new CampaignPacingDTO(campStats) };
            foreach (var platStat in campStats.PlatformStats)
            {
                dtos.Add(new CampaignPacingDTO(platStat));
            }
            var json = Json(dtos, JsonRequestBehavior.AllowGet);
            //var json = Json(dtos);
            return json;
        }


        // T could be CampaignPacingDTO, PerformanceDTO...
        public static object Aggregates<T>(KendoGridEx<T> kgrid)
        {
            if (kgrid.Total == 0 || kgrid.Aggregates == null) return null;

            decimal budget = ((dynamic)kgrid.Aggregates)["Budget"]["sum"];
            decimal daCost = ((dynamic)kgrid.Aggregates)["DACost"]["sum"];
            decimal mediaSpend = ((dynamic)kgrid.Aggregates)["MediaSpend"]["sum"];
            decimal totalRev = ((dynamic)kgrid.Aggregates)["TotalRev"]["sum"];
            decimal margin = ((dynamic)kgrid.Aggregates)["Margin"]["sum"];

            decimal? marginPct = null;
            if (totalRev != 0)
                marginPct = 1 - daCost / totalRev;

            decimal? pctOfGoal = null;
            if (budget != 0)
                pctOfGoal = mediaSpend / budget;

            if (typeof(T) == typeof(CampaignPacingDTO))
            {
                var aggs = new
                {
                    Budget = new { sum = budget },
                    DACost = new { sum = daCost },
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
                    DACost = new { sum = daCost },
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