using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClientPortal.Data.Contexts;
using System.Web.Helpers;
using WebMatrix.WebData;

namespace ClientPortal.Web.Areas.Admin.Controllers
{
    [Authorize(Users = "admin")]
    public class AdvertisersController : Controller
    {
        private ClientPortalContext db = new ClientPortalContext();

        //
        // GET: /Admin/Advertisers/

        public ActionResult Index(string sort)
        {
            var advertisers = db.Advertisers.AsQueryable();
            switch (sort)
            {
                case "AdvertiserName":
                    advertisers = advertisers.OrderBy(a => a.AdvertiserName);
                    break;
                default:
                    advertisers = advertisers.OrderBy(a => a.AdvertiserId);
                    break;
            }
            var advList = advertisers.ToList();

            // fill in the user profiles for the advertisers
            // TODO: do a join on the db
            var userProfiles = db.UserProfiles.ToList();
            foreach (var advertiser in advList)
            {
                advertiser.UserProfiles = userProfiles.Where(u => u.CakeAdvertiserId == advertiser.AdvertiserId);
            }

            return View(advList);
        }

        //
        // GET: /Admin/Advertisers/Details/5

        public ActionResult Details(int id = 0)
        {
            Advertiser advertiser = db.Advertisers.Find(id);
            if (advertiser == null)
            {
                return HttpNotFound();
            }
            return View(advertiser);
        }

        //
        // GET: /Admin/Advertisers/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Admin/Advertisers/Create

        [HttpPost]
        public ActionResult Create(Advertiser advertiser)
        {
            if (ModelState.IsValid)
            {
                db.Advertisers.Add(advertiser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(advertiser);
        }

        //
        // GET: /Admin/Advertisers/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Advertiser advertiser = db.Advertisers.Find(id);
            if (advertiser == null)
            {
                return HttpNotFound();
            }
            return View(advertiser);
        }

        //
        // POST: /Admin/Advertisers/Edit/5

        [HttpPost]
        public ActionResult Edit(Advertiser advertiser)
        {
            if (ModelState.IsValid)
            {
                var entry = db.Entry(advertiser);
                entry.State = EntityState.Modified;
                entry.Property(x => x.Logo).IsModified = false;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(advertiser);
        }

        public ActionResult EditContacts(int id = 0)
        {
            Advertiser advertiser = db.Advertisers.Find(id);
            if (advertiser == null)
            {
                return HttpNotFound();
            }
            return View(advertiser);
        }
        [HttpPost]
        public ActionResult EditContacts(int id, string contactids)
        {
            var contactIds = contactids.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(c => Convert.ToInt32(c));

            Advertiser advertiser = db.Advertisers.Find(id);
            advertiser.AdvertiserContacts.Clear();
            db.SaveChanges();

            int order = 1;
            foreach (var contactId in contactIds)
            {
                var contact = db.Contacts.Find(contactId);
                if (contact != null)
                {
                    AdvertiserContact ac = new AdvertiserContact() { Contact = contact, Order = order++ };
                    advertiser.AdvertiserContacts.Add(ac);
                }
            }
            db.SaveChanges();

            return View(advertiser);
        }

        public FileResult Logo(int id)
        {
            Advertiser advertiser = db.Advertisers.Find(id);
            if (advertiser == null)
                return null;

            WebImage logo = new WebImage(advertiser.Logo);
            return File(logo.GetBytes(), "image/" + logo.ImageFormat, logo.FileName);
        }
        public ActionResult EditLogo(int id = 0)
        {
            Advertiser advertiser = db.Advertisers.Find(id);
            if (advertiser == null)
            {
                return HttpNotFound();
            }
            return View(advertiser);
        }
        [HttpPost]
        public ActionResult UploadLogo(int id)
        {
            WebImage logo = WebImage.GetImageFromRequest();
            byte[] imageBytes = logo.GetBytes();

            Advertiser advertiser = db.Advertisers.Find(id);
            advertiser.Logo = imageBytes;
            db.SaveChanges();

            return null;
        }

        public ActionResult CreateUserProfile(int id = 0)
        {
            Advertiser advertiser = db.Advertisers.Find(id);
            if (advertiser == null)
            {
                return HttpNotFound();
            }
            return View(advertiser);
        }

        [HttpPost]
        public ActionResult CreateUserProfile(int id, string username, string password)
        {
            Advertiser advertiser = db.Advertisers.Find(id);
            if (advertiser != null)
            {
                WebSecurity.CreateUserAndAccount(
                    username,
                    password,
                    new
                    {
                        CakeAdvertiserId = advertiser.AdvertiserId,
                    });
            }
            return RedirectToAction("Index");
        }

        //
        // GET: /Admin/Advertisers/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Advertiser advertiser = db.Advertisers.Find(id);
            if (advertiser == null)
            {
                return HttpNotFound();
            }
            return View(advertiser);
        }

        //
        // POST: /Admin/Advertisers/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Advertiser advertiser = db.Advertisers.Find(id);
            db.Advertisers.Remove(advertiser);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}