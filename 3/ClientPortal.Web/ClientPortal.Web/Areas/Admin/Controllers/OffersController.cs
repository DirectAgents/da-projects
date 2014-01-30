﻿using CakeExtracter.Commands;
using ClientPortal.Data.Contexts;
using ClientPortal.Data.Contracts;
using ClientPortal.Web.Controllers;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;

namespace ClientPortal.Web.Areas.Admin.Controllers
{
    [Authorize(Users = "admin")]
    public class OffersController : CPController
    {
        public OffersController(IClientPortalRepository cpRepository)
        {
            this.cpRepo = cpRepository;
        }

        public ActionResult Filter()
        {
            var offers = cpRepo.Offers(null, null, true);
            var accountManagers = offers.Select(o => o.Advertiser).Distinct().Where(a => a.AccountManagerId != null).Select(a => a.AccountManager).Distinct().OrderBy(am => am.FullName);

            return View(accountManagers);
        }

        public ActionResult Index(int? am, int? advertiserid, int? mincampaigns)
        {
            var offers = cpRepo.Offers(am, advertiserid, true, mincampaigns);

            offers = offers
                .OrderBy(o => o.Advertiser.AdvertiserName)
                .ThenBy(o => o.OfferName);

            return View(offers);
        }

        public ActionResult Show(int id)
        {
            var offer = cpRepo.GetOffer(id);

            return View(offer);
        }

        [AllowAnonymous]
        public FileResult Logo(int id)
        {
            var offer = cpRepo.GetOffer(id);
            if (offer == null)
                return null;

            WebImage logo = new WebImage(offer.Logo);
            return File(logo.GetBytes(), "image/" + logo.ImageFormat, logo.FileName);
        }

        public ActionResult EditLogo(int id)
        {
            var offer = cpRepo.GetOffer(id);
            if (offer == null)
            {
                return HttpNotFound();
            }
            return View(offer);
        }
        [HttpPost]
        public ActionResult UploadLogo(int id)
        {
            WebImage image = WebImage.GetImageFromRequest();
            byte[] imageBytes = image.GetBytes();

            var offer = cpRepo.GetOffer(id);
            if (offer != null)
            {
                offer.Logo = imageBytes;
                cpRepo.SaveChanges();
            }
            return null;
        }

        public ActionResult SynchCampaigns(int offerid)
        {
            var cmd = new SynchCampaignsCommand
            {
                OfferId = offerid
            };
            cmd.Run(null);

            if (Request.IsAjaxRequest())
                return RedirectToAction("Show", new { id = offerid });
            else
                return Content("Synch complete. Click 'back' and refresh.");
        }

        // for testing...

        public ActionResult DropDebug(int id)
        {
            var offer = cpRepo.GetOffer(id);
            return View(offer);
        }

        public ActionResult ReportDebug(int id)
        {
            var offer = cpRepo.GetOffer(id);
            return View(offer);
        }
    }
}
