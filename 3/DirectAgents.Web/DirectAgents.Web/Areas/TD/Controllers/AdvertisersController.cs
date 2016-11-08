using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Entities.CPProg;

namespace DirectAgents.Web.Areas.TD.Controllers
{
    public class AdvertisersController : DirectAgents.Web.Controllers.ControllerBase
    {
        public AdvertisersController(ICPProgRepository cpProgRepository)
        {
            this.cpProgRepo = cpProgRepository;
        }

        public ActionResult Index()
        {
            var advertisers = cpProgRepo.Advertisers() //TODO? don't load images? (also check ClientPortal.Web)
                .OrderBy(a => a.Name);

            return View(advertisers);
        }

        public ActionResult CreateNew()
        {
            var advertiser = new Advertiser
            {
                Name = "zNew"
            };
            if (cpProgRepo.AddAdvertiser(advertiser))
                return RedirectToAction("Index");
            else
                return Content("Error creating Advertiser");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var advertiser = cpProgRepo.Advertiser(id);
            if (advertiser == null)
                return HttpNotFound();
            SetupForEdit();
            return View(advertiser);
        }
        [HttpPost]
        public ActionResult Edit(Advertiser adv)
        {
            if (ModelState.IsValid)
            {
                if (cpProgRepo.SaveAdvertiser(adv, includeLogo: false))
                    return RedirectToAction("Index");
                ModelState.AddModelError("", "Campaign could not be saved.");
            }
            //fillextended?
            SetupForEdit();
            return View(adv);
        }
        private void SetupForEdit()
        {
            ViewBag.Employees = cpProgRepo.Employees().OrderBy(e => e.FirstName).ThenBy(e => e.LastName).ToList();
        }

        public FileResult Logo(int id)
        {
            var advertiser = cpProgRepo.Advertiser(id);
            if (advertiser == null)
                return null;
            WebImage logo = new WebImage(advertiser.Logo);
            return File(logo.GetBytes(), "image/" + logo.ImageFormat, logo.FileName);
        }
        public ActionResult EditLogo(int id)
        {
            var advertiser = cpProgRepo.Advertiser(id);
            if (advertiser == null)
                return HttpNotFound();
            return View(advertiser);
        }
        [HttpPost]
        public ActionResult UploadLogo(int id)
        {
            var advertiser = cpProgRepo.Advertiser(id);
            if (advertiser == null)
                return null;

            WebImage logo = WebImage.GetImageFromRequest();
            byte[] imageBytes = logo.GetBytes();

            advertiser.Logo = imageBytes;
            cpProgRepo.SaveChanges();

            return null;
        }
    }
}