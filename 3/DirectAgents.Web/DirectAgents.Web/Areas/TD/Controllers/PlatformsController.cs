using System;
using System.Linq;
using System.Web.Mvc;
using CakeExtracter.Commands;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Entities.TD;

namespace DirectAgents.Web.Areas.TD.Controllers
{
    public class PlatformsController : DirectAgents.Web.Controllers.ControllerBase
    {
        public PlatformsController(ITDRepository tdRepository)
        {
            this.tdRepo = tdRepository;
        }

        public ActionResult Index()
        {
            var platforms = tdRepo.Platforms()
                .OrderBy(p => p.Name);
            return View(platforms);
        }

        public ActionResult CreateNew()
        {
            var platform = new Platform
            {
                Code = "z",
                Name = "zNew"
            };
            if (tdRepo.AddPlatform(platform))
                return RedirectToAction("Index");
            else
                return Content("Error creating Partner");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var platform = tdRepo.Platform(id);
            if (platform == null)
                return HttpNotFound();
            return View(platform);
        }
        [HttpPost]
        public ActionResult Edit(Platform plat)
        {
            if (ModelState.IsValid)
            {
                if (tdRepo.SavePlatform(plat))
                    return RedirectToAction("Index");
                ModelState.AddModelError("", "Platform could not be saved.");
            }
            // ?fillextended / setupforedit?
            return View(plat);
        }

        public ActionResult Maintenance(int id)
        {
            var platform = tdRepo.Platform(id);
            if (platform == null)
                return HttpNotFound();

            return View(platform);
        }

        public ActionResult SyncAccounts(int id)
        {
            var platform = tdRepo.Platform(id);
            if (platform == null)
                return HttpNotFound();
            if (platform.Code == Platform.Code_AdRoll)
            {
                DASynchAdrollAccounts.RunStatic();
            }
            return RedirectToAction("Maintenance", new { id = id });
        }

        public ActionResult SyncStats(int id, DateTime? start)
        {
            var platform = tdRepo.Platform(id);
            if (platform == null)
                return HttpNotFound();

            if (platform.Code == Platform.Code_AdRoll)
                DASynchAdrollStats.RunStatic(startDate: start, oneStatPer: "all");
            else if (platform.Code == Platform.Code_DBM)
                DASynchDBMStats.RunStatic();
            else if (platform.Code == Platform.Code_FB)
                DASynchFacebookStats.RunStatic(startDate: start);

            return RedirectToAction("Maintenance", new { id = id });
        }
	}
}