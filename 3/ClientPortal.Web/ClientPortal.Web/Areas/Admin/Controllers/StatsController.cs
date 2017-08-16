using System;
using System.Web.Mvc;
using ClientPortal.Data.Contracts;
using ClientPortal.Web.Controllers;

namespace ClientPortal.Web.Areas.Admin.Controllers
{
    [Authorize(Users = "admin")]
    public class StatsController : CPController
    {
        public StatsController(IClientPortalRepository cpRepository)
        {
            cpRepo = cpRepository;
        }

        public ActionResult SearchAccounts(int searchProfileId)
        {
            var searchAccounts = cpRepo.SearchAccounts(searchProfileId);
            return View(searchAccounts);
        }

        public ActionResult Week(DateTime weekStart, int searchAccountId)
        {
            var weekEnd = weekStart.AddDays(6);
            var searchAccount = cpRepo.GetSearchAccount(searchAccountId);
            if (searchAccount == null)
                return HttpNotFound();

            bool cassConvs = false; // could get from SearchProfile
            var stats = cpRepo.GetCampaignStats(searchAccount.SearchProfile, searchAccountId, weekStart, weekEnd, false, cassConvs);
            ViewBag.SearchAccount = searchAccount;
            ViewBag.WeekStart = weekStart;
            return View(stats);
        }
        public ActionResult DecreaseWeekOrders(DateTime weekStart, int searchCampaignId, int by = 1)
        {
            var weekEnd = weekStart.AddDays(6);
            var searchCampaign = cpRepo.GetSearchCampaign(searchCampaignId);
            if (searchCampaign == null)
                return HttpNotFound();

            cpRepo.DecreaseCampaignOrders(searchCampaignId, weekStart, weekEnd, by);

            return RedirectToAction("Week", new { weekStart = weekStart.ToShortDateString(), searchAccountId = searchCampaign.SearchAccountId });
        }
    }
}