using CakeExtracter.Commands;
using ClientPortal.Data.Contexts;
using ClientPortal.Data.Contracts;
using ClientPortal.Web.Controllers;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI;

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

        public ActionResult Index(int? am, int? advertiserid, int? mincampaigns, bool cpmonly = true)
        {
            var offers = cpRepo.Offers(am, advertiserid, cpmonly, mincampaigns);

            offers = offers
                .OrderBy(o => o.Advertiser.AdvertiserName)
                .ThenBy(o => o.OfferName);

            ViewBag.FilterAM = am;
            ViewBag.FilterAdv = advertiserid;
            ViewBag.MinCampaigns = mincampaigns;
            ViewBag.CPMonly = cpmonly;
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
            if (offer == null || offer.Logo == null)
                return null;

            WebImage logo = new WebImage(offer.Logo);
            return File(logo.GetBytes(), "image/" + logo.ImageFormat, logo.FileName);
        }

        public ActionResult EditLogo(int id, bool inner = false)
        {
            var offer = cpRepo.GetOffer(id);
            if (offer == null)
                return HttpNotFound();

            if (inner)
                return View("EditLogoInner", offer);
            else
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

        [HttpPost]
        public ActionResult DeleteLogo(int id)
        {
            var offer = cpRepo.GetOffer(id);
            if (offer != null)
            {
                offer.Logo = null;
                cpRepo.SaveChanges();
            }
            return RedirectToAction("Show", new { id = id });
        }

        public ActionResult SynchCampaigns(int offerid)
        {
            SynchCampaignsCommand.RunStatic(0, offerid);

            if (Request.IsAjaxRequest())
                return RedirectToAction("Show", new { id = offerid });
            else
                return Content("Synch complete. Click 'back' and refresh.");
        }

        public ActionResult SynchStatsForDrops(int offerid)
        {
            CampaignDropsController.SynchStatsForAllDrops(cpRepo, offerid);

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
