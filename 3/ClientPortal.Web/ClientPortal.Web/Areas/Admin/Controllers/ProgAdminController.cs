using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CakeExtracter.Commands;
using ClientPortal.Data.Contracts;
using ClientPortal.Web.Areas.Admin.Models;
using ClientPortal.Web.Controllers;
using WebMatrix.WebData;

namespace ClientPortal.Web.Areas.Admin.Controllers
{
    [Authorize(Users = "admin")]
    public class ProgAdminController : CPController
    {
        public ProgAdminController(DirectAgents.Domain.Abstract.ITDRepository progRepository, IClientPortalRepository cpRepository)
        {
            progRepo = progRepository;
            cpRepo = cpRepository;
        }

        public ActionResult Advertisers()
        {
            var progAdvertisers = progRepo.Advertisers().OrderBy(a => a.Name).ToList(); //TODO? Don't load images? (also check DAWeb)
            var userProfiles = cpRepo.UserProfiles().Where(up => up.TDAdvertiserId.HasValue).ToList();
            var advertiserVMs = new List<AdvertiserVM>();
            foreach (var progAdv in progAdvertisers)
            {
                var advVM = new AdvertiserVM
                {
                    ProgAdvertiser = progAdv,
                    UserProfiles = userProfiles.Where(up => up.TDAdvertiserId.Value == progAdv.Id)
                };
                advertiserVMs.Add(advVM);
            }
            return View(advertiserVMs);
        }

        public ActionResult CreateUserProfile(int advId) // (for a TDAdvertiser)
        {
            var progAdvertiser = progRepo.Advertiser(advId);
            if (progAdvertiser == null)
                return HttpNotFound();
            return View(progAdvertiser);
        }

        [HttpPost]
        public ActionResult CreateUserProfile(int advId, string username, string password)
        {
            var progAdvertiser = progRepo.Advertiser(advId);
            if (progAdvertiser != null)
            {
                if (String.IsNullOrWhiteSpace(username))
                    ModelState.AddModelError("", "Username must be supplied.");
                else if (WebSecurity.UserExists(username))
                    ModelState.AddModelError("", string.Format("The username '{0}' already exists.", username));
                if (String.IsNullOrWhiteSpace(password))
                    ModelState.AddModelError("", "Password must be supplied.");

                if (!ModelState.IsValid)
                    return View(progAdvertiser);

                WebSecurity.CreateUserAndAccount(
                    username, password,
                    new { TDAdvertiserId = progAdvertiser.Id });
            }
            return RedirectToAction("Advertisers");
        }

        public ActionResult AssignUserProfile(int advId)
        {
            var progAdvertiser = progRepo.Advertiser(advId);
            if (progAdvertiser == null)
                return HttpNotFound();

            ViewBag.UserProfiles = cpRepo.UserProfiles().OrderBy(up => up.UserName);
            return View(progAdvertiser);
        }
        [HttpPost]
        public ActionResult AssignUserProfile(int advId, int userId)
        {
            var progAdvertiser = progRepo.Advertiser(advId);
            var userProfile = cpRepo.GetUserProfile(userId);
            if (progAdvertiser != null && userProfile != null)
            {
                userProfile.TDAdvertiserId = progAdvertiser.Id;
                cpRepo.SaveChanges();
            }
            return RedirectToAction("Advertisers");
        }

        public ActionResult UploadStats()
        {
            var extAccounts = progRepo.ExtAccounts()
                .OrderBy(a => a.Platform.Name).ThenBy(a => a.Name).ToList();
            return View(extAccounts);
        }
        public ActionResult UploadFile(HttpPostedFileBase file, string acctId, string statsType, string statsDate)
        {
            int accountId;
            if (!Int32.TryParse(acctId, out accountId))
                return null;
            if (progRepo.ExtAccount(accountId) == null)
                return null;

            DateTime? statsDateNullable = null;
            DateTime parseDate;
            if (DateTime.TryParse(statsDate, out parseDate))
                statsDateNullable = parseDate;
            else
                statsDateNullable = null;

            using (var reader = new StreamReader(file.InputStream))
            {
                if (statsType != null && statsType.ToUpper().StartsWith("CONV"))
                    DASynchAdrollConvCsv.RunStatic(accountId, reader); // TODO: generic Conv syncher?
                else
                    DASynchTDDailySummaries.RunStatic(accountId, reader, statsType, statsDate: statsDateNullable);
            }
            return null;
        }
    }
}