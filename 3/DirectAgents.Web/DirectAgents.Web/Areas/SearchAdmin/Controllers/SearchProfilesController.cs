using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Entities.CPSearch;

namespace DirectAgents.Web.Areas.SearchAdmin.Controllers
{
    public class SearchProfilesController : DirectAgents.Web.Controllers.ControllerBase
    {
        public SearchProfilesController(ICPSearchRepository cpSearchRepository)
        {
            this.cpSearchRepo = cpSearchRepository;
        }

        public ActionResult Index(string orderBy, int? activeSinceMonths = 1)
        {
            // use -1 for activeSinceMonths to show all profiles
            DateTime? activeSince = null;
            if (activeSinceMonths.HasValue && activeSinceMonths >= 0) // if 0, use yesterday. otherwise, X months prior to today
                activeSince = (activeSinceMonths.Value == 0) ? DateTime.Today.AddDays(-1) : DateTime.Today.AddMonths(-activeSinceMonths.Value);
            var searchProfiles = cpSearchRepo.SearchProfiles(activeSince: activeSince);

            if (orderBy != null && orderBy.ToLower() == "id")
                searchProfiles = searchProfiles.OrderBy(x => x.SearchProfileId);
            else
                searchProfiles = searchProfiles.OrderBy(x => x.SearchProfileName).ThenBy(x => x.SearchProfileId);

            ViewBag.OrderBy = orderBy;
            ViewBag.ShowAll = (activeSince == null);
            return View(searchProfiles);
        }

        public ActionResult IndexGauge()
        {
            var searchProfiles = cpSearchRepo.SearchProfiles(includeGauges: true);
            return View(searchProfiles.OrderBy(x => x.SearchProfileName));
        }

        public ActionResult CreateNew(string name)
        {
            int maxId = 0;
            var searchProfiles = cpSearchRepo.SearchProfiles();
            if (searchProfiles.Any())
            {
                maxId = searchProfiles.Max(x => x.SearchProfileId);
            }
            var searchProfile = new SearchProfile
            {
                SearchProfileId = maxId + 1,
                SearchProfileName = String.IsNullOrEmpty(name) ? "zNew" : name,
                StartDayOfWeek = (int)DayOfWeek.Monday,
                ShowRevenue = true
            };
            cpSearchRepo.SaveSearchProfile(searchProfile, createIfDoesntExist: true, saveIfExists: false);
            return RedirectToAction("Index", new { activeSinceMonths = -1 });
        }

        public ActionResult Show(int id)
        {
            var searchProfile = cpSearchRepo.GetSearchProfile(id);
            if (searchProfile == null)
                return HttpNotFound();
            return View(searchProfile);
        }

        public ActionResult ConvTypes(int id)
        {
            var searchProfile = cpSearchRepo.GetSearchProfile(id);
            if (searchProfile == null)
                return HttpNotFound();
            return View(searchProfile);
        }
        //[HttpPost]
        public JsonResult ConvTypesData(int id)
        {
            var searchConvTypes = cpSearchRepo.GetConvTypes(id).OrderBy(ct => ct.Alias);

            var kg = new KG<SearchConvType>();
            kg.data = searchConvTypes;
            kg.total = searchConvTypes.Count();

            var json = Json(kg, JsonRequestBehavior.AllowGet);
            //var json = Json(kg);
            return json;
        }
        [HttpPost]
        public ActionResult ConvTypesUpdate(SearchConvType[] models)
        {
            //TODO: try/catch/return errors
            foreach (var row in models)
            {
                var searchConvType = cpSearchRepo.GetConvType(row.SearchConvTypeId);
                searchConvType.Alias = row.Alias;
            }
            cpSearchRepo.SaveChanges();

            return Json(models);
        }
    }

    class KG<T>
    {
        public IEnumerable<T> data { get; set; }
        public int total { get; set; }
        public object aggregates { get; set; }
    }
}