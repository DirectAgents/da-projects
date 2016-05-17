using System;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using ClientPortal.Web.Areas.Prog.Models;
using ClientPortal.Web.Controllers;
using DirectAgents.Domain.Abstract;

namespace ClientPortal.Web.Areas.Prog.Controllers
{
    [Authorize]
    public class HomeController : CPController
    {
        public HomeController(ITDRepository progRepository, ClientPortal.Data.Contracts.IClientPortalRepository cpRepository)
        {
            this.progRepo = progRepository;
            this.cpRepo = cpRepository;
        }

        public FileResult Logo()
        {
            var userInfo = GetUserInfo();
            if (userInfo.ProgAdvertiser == null || userInfo.ProgAdvertiser.Logo == null)
                return null;
            WebImage logo = new WebImage(userInfo.ProgAdvertiser.Logo);
            return File(logo.GetBytes(), "image/" + logo.ImageFormat, logo.FileName);
        }

        // "Executive Summary"
        public ActionResult Index()
        {
            var userInfo = GetUserInfo();

            var today = DateTime.Today;
            var yesterday = today.AddDays(-1);

            int advId = userInfo.ProgAdvertiser.Id;
            DateTime earliestStatDate = progRepo.EarliestStatDate(advId: advId) ?? yesterday;

            var model = new ExecSumVM
            {
                UserInfo = userInfo,
                StartDate = earliestStatDate,
                MTDStat = progRepo.MTDBasicStat(advId, yesterday),
                CTDStat = progRepo.DateRangeBasicStat(advId, earliestStatDate, yesterday)
            };
            return View(model);
        }

        public ActionResult Strategy()
        {
            var userInfo = GetUserInfo();
            int advId = userInfo.ProgAdvertiser.Id;

            var yesterday = DateTime.Today.AddDays(-1);
            var model = new ReportVM
            {
                UserInfo = userInfo,
                Stats = progRepo.MTDStrategyBasicStats(advId, endDate: yesterday)
            };
            return View(model);
        }

        public ActionResult Weekly()
        {
            var userInfo = GetUserInfo();

            var model = new ReportVM
            {
                UserInfo = userInfo
            };
            return View(model);
        }

        public ActionResult Creative()
        {
            var userInfo = GetUserInfo();
            int advId = userInfo.ProgAdvertiser.Id;
            DateTime yesterday = DateTime.Today.AddDays(-1);
            DateTime campaignStart = progRepo.EarliestStatDate(advId, checkAll: true) ?? yesterday;

            var model = new ReportVM
            {
                UserInfo = userInfo,
                StartDate = campaignStart,
                EndDate = yesterday
            };
            return View(model);
        }

        public ActionResult Site()
        {
            var userInfo = GetUserInfo();
            int advId = userInfo.ProgAdvertiser.Id;

            var yesterday = DateTime.Today.AddDays(-1);
            var monthStart = new DateTime(yesterday.Year, yesterday.Month, 1);

            var stats = progRepo.MTDSiteBasicStats(advId, endDate: yesterday)
                .OrderByDescending(s => s.Impressions).ThenBy(s => s.SiteName);

            var model = new ReportVM
            {
                UserInfo = userInfo,
                StartDate = monthStart,
                EndDate = yesterday,
                Stats = stats
            };
            return View(model);
        }

        public ActionResult Lead()
        {
            var userInfo = GetUserInfo();
            int advId = userInfo.ProgAdvertiser.Id;

            var yesterday = DateTime.Today.AddDays(-1);
            var monthStart = new DateTime(yesterday.Year, yesterday.Month, 1);

            var leadInfos = progRepo.MTDLeadInfos(advId, endDate: yesterday)
                .OrderByDescending(i => i.Time).ThenBy(i => i.Country).ThenBy(i => i.City);

            var model = new ReportVM
            {
                UserInfo = userInfo,
                StartDate = monthStart,
                EndDate = yesterday,
                LeadInfos = leadInfos
            };
            return View(model);
        }
    }
}