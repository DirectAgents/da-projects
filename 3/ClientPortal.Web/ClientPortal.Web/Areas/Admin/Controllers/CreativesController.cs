using CakeExtracter.Commands;
using ClientPortal.Data.Contexts;
using ClientPortal.Data.Contracts;
using ClientPortal.Web.Controllers;
using System;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;

namespace ClientPortal.Web.Areas.Admin.Controllers
{
    [Authorize(Users = "admin")]
    public class CreativesController : CPController
    {
        public CreativesController(IClientPortalRepository cpRepository)
        {
            this.cpRepo = cpRepository;
        }

        public ActionResult Start(int offerid)
        {
            var offer = cpRepo.GetOffer(offerid);
            return View(offer);
        }

        public ActionResult Index(int? offerid)
        {
            if (offerid.HasValue)
                ViewData["offer"] = cpRepo.GetOffer(offerid.Value);

            var creatives = cpRepo.Creatives(offerid).OrderByDescending(c => c.DateCreated);
            return View(creatives);
        }

        public ActionResult Edit(int id)
        {
            var creative = cpRepo.GetCreative(id);
            if (creative == null)
                return HttpNotFound();

            return View(creative);
        }

        [HttpPost]
        public ActionResult Edit(Creative creative)
        {
            if (ModelState.IsValid)
            {
                bool success = cpRepo.SaveCreative(creative, true);
                if (success)
                    return RedirectToAction("Index", "Creatives", new { offerid = creative.OfferId });

                ModelState.AddModelError("", "Creative could not be saved");
            }
            cpRepo.FillExtended_Creative(creative);
            return View(creative);
        }

        [AllowAnonymous]
        public FileResult Thumbnail(int id)
        {
            var creative = cpRepo.GetCreative(id);
            if (creative == null)
                return null;

            WebImage thumbnail = new WebImage(creative.Thumbnail);
            return File(thumbnail.GetBytes(), "image/" + thumbnail.ImageFormat, thumbnail.FileName);
        }

        public ActionResult EditThumbnail(int id)
        {
            var creative = cpRepo.GetCreative(id);
            if (creative == null)
            {
                return HttpNotFound();
            }
            return View(creative);
        }
        [HttpPost]
        public ActionResult UploadThumbnail(int id)
        {
            WebImage image = WebImage.GetImageFromRequest();
            byte[] imageBytes = image.GetBytes();

            var creative = cpRepo.GetCreative(id);
            if (creative != null)
            {
                creative.Thumbnail = imageBytes;
                cpRepo.SaveChanges();
            }
            return null;
        }

        public ActionResult DeleteThumbnail(int id)
        {
            var creative = cpRepo.GetCreative(id);
            if (creative == null)
            {
                return HttpNotFound();
            }
            creative.Thumbnail = null;
            cpRepo.SaveChanges();

            return RedirectToAction("Index", new { offerid = creative.OfferId });
        }

        public ActionResult Synch(int offerid, bool overwrite = false)
        {
            SynchCreativesCommand.RunStatic(0, offerid, overwrite);
            return RedirectToAction("Index", new { offerid = offerid });
        }
    }
}
