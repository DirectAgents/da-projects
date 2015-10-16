using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.DTO;
using DirectAgents.Domain.Entities.TD;
using DirectAgents.Web.Areas.TD.Models;

namespace DirectAgents.Web.Areas.TD.Controllers
{
    public class CampStatsController : DirectAgents.Web.Controllers.ControllerBase
    {
        public CampStatsController(ITDRepository tdRepository)
        {
            this.tdRepo = tdRepository;
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