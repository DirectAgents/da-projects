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

            return CreateJsonResult(kgrid, CampaignsController.Aggregates(kgrid), allowGet: true);
        }
        //[HttpPost]
        public JsonResult PacingDetail(KendoGridMvcRequest request, int campId) //, DateTime? month)
        {
            DateTime currMonth = CurrentMonthTD;
            var stat = tdRepo.GetCampStats(currMonth, campId);
            var dtos = stat.PlatformStats.Select(s => new CampaignPacingDTO(s));
            var kgrid = new KendoGridEx<CampaignPacingDTO>(request, dtos);

            return CreateJsonResult(kgrid, CampaignsController.Aggregates(kgrid), allowGet: true);
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

            return CreateJsonResult(kgrid, CampaignsController.Aggregates(kgrid), allowGet: true);
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
	}
}