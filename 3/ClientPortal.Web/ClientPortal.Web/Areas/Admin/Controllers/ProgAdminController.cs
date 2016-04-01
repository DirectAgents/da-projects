using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ClientPortal.Data.Contracts;
using ClientPortal.Web.Areas.Admin.Models;
using ClientPortal.Web.Controllers;
using WebMatrix.WebData;

namespace ClientPortal.Web.Areas.Admin.Controllers
{
    [Authorize(Users = "admin")]
    public class ProgAdminController : CPController
    {
        public ProgAdminController(DirectAgents.Domain.Abstract.ITDRepository datdRepository, IClientPortalRepository cpRepository)
        {
            datdRepo = datdRepository;
            cpRepo = cpRepository;
        }

        public ActionResult Advertisers()
        {
            var tdAdvertisers = datdRepo.Advertisers().OrderBy(a => a.Name).ToList();
            var userProfiles = cpRepo.UserProfiles().Where(up => up.TDAdvertiserId.HasValue).ToList();
            var advertiserVMs = new List<AdvertiserVM>();
            foreach (var tdAdv in tdAdvertisers)
            {
                var advVM = new AdvertiserVM
                {
                    TDAdvertiser = tdAdv,
                    UserProfiles = userProfiles.Where(up => up.TDAdvertiserId.Value == tdAdv.Id)
                };
                advertiserVMs.Add(advVM);
            }
            return View(advertiserVMs);
        }

        public ActionResult CreateUserProfile(int advId) // (for a TDAdvertiser)
        {
            var tdAdvertiser = datdRepo.Advertiser(advId);
            if (tdAdvertiser == null)
                return HttpNotFound();
            return View(tdAdvertiser);
        }

        [HttpPost]
        public ActionResult CreateUserProfile(int advId, string username, string password)
        {
            var tdAdvertiser = datdRepo.Advertiser(advId);
            if (tdAdvertiser != null)
            {
                if (String.IsNullOrWhiteSpace(username))
                    ModelState.AddModelError("", "Username must be supplied.");
                else if (WebSecurity.UserExists(username))
                    ModelState.AddModelError("", string.Format("The username '{0}' already exists.", username));
                if (String.IsNullOrWhiteSpace(password))
                    ModelState.AddModelError("", "Password must be supplied.");

                if (!ModelState.IsValid)
                    return View(tdAdvertiser);

                WebSecurity.CreateUserAndAccount(
                    username, password,
                    new { TDAdvertiserId = tdAdvertiser.Id });
            }
            return RedirectToAction("Advertisers");
        }

        public ActionResult AssignUserProfile(int advId)
        {
            var tdAdvertiser = datdRepo.Advertiser(advId);
            if (tdAdvertiser == null)
                return HttpNotFound();

            ViewBag.UserProfiles = cpRepo.UserProfiles().OrderBy(up => up.UserName);
            return View(tdAdvertiser);
        }
        [HttpPost]
        public ActionResult AssignUserProfile(int advId, int userId)
        {
            var tdAdvertiser = datdRepo.Advertiser(advId);
            var userProfile = cpRepo.GetUserProfile(userId);
            if (tdAdvertiser != null && userProfile != null)
            {
                userProfile.TDAdvertiserId = tdAdvertiser.Id;
                cpRepo.SaveChanges();
            }
            return RedirectToAction("Advertisers");
        }

        public ActionResult Test()
        {
            var advId = 2;
            var startDate = new DateTime(2015, 11, 1);
            var endDate = new DateTime(2016, 3, 31);

            //var stats = datdRepo.DailySummaryBasicStats(advId, startDate, endDate);
            //var stats = datdRepo.DayOfWeekBasicStats(advId, startDate, endDate);
            var stat = datdRepo.MTDBasicStat(advId, endDate);
            //var stat = datdRepo.DateRangeBasicStat(advId, startDate, endDate);

            var json = Json(stat, JsonRequestBehavior.AllowGet);
            //var json = Json(stats);
            return json;        }
    }
}