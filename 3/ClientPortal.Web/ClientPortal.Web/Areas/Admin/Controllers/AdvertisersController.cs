using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClientPortal.Data.Contexts;

namespace ClientPortal.Web.Areas.Admin.Controllers
{
    public class AdvertisersController : Controller
    {
        private ClientPortalContext db = new ClientPortalContext();

        //
        // GET: /Admin/Advertisers/

        public ActionResult Index()
        {
            return View(db.Advertisers.ToList());
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
                db.Entry(advertiser).State = EntityState.Modified;
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