﻿using System;
using System.Web.Mvc;
using ClientPortal.Web.Controllers;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Entities.TD;

namespace ClientPortal.Web.Areas.Prog.Controllers
{
    public class StatsController : CPController
    {
        public StatsController(ITDRepository progRepository, ClientPortal.Data.Contracts.IClientPortalRepository cpRepository)
        {
            this.progRepo = progRepository;
            this.cpRepo = cpRepository;
        }

        public JsonResult ExecMTD()
        {
            var yesterday = DateTime.Today.AddDays(-1);
            return GetMonthStatsJson(yesterday);
        }
        public JsonResult ExecLastMonth()
        {
            DateTime endDate;
            var today = DateTime.Today; //Note: if it's the 1st, last month is still considered the current month
            if (today.Day == 1) // ...always go to last day of previous month, even if it has more days than the "current" month
                endDate = today.AddMonths(-1).AddDays(-1);
            else
                endDate = today.AddDays(-1).AddMonths(-1);

            return GetMonthStatsJson(endDate);
        }
        private JsonResult GetMonthStatsJson(DateTime endDate)
        {
            var userInfo = GetUserInfo();
            if (userInfo.ProgAdvertiser == null)
                return null;

            var basicStat = progRepo.MTDBasicStat(userInfo.ProgAdvertiser.Id, endDate);
            var array = new BasicStat[] { basicStat };

            var json = Json(array, JsonRequestBehavior.AllowGet);
            return json;
        }
	}
}