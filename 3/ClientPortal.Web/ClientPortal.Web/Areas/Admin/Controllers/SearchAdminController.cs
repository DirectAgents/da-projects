using ClientPortal.Data.Contracts;
using ClientPortal.Web.Controllers;
using System.Linq;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace ClientPortal.Web.Areas.Admin.Controllers
{
    [Authorize(Users = "admin")]
    public class SearchAdminController : CPController
    {
        public SearchAdminController(IClientPortalRepository cpRepository)
        {
            cpRepo = cpRepository;
        }

        public ActionResult SearchProfiles()
        {
            var searchProfiles = cpRepo.SearchProfiles();
            return View(searchProfiles.ToList());
        }

        public ActionResult CreateUserProfile(int spId)
        {
            var searchProfile = cpRepo.GetSearchProfile(spId);
            if (searchProfile == null)
                return HttpNotFound();
            return View(searchProfile);
        }

        [HttpPost]
        public ActionResult CreateUserProfile(int spId, string username, string password)
        {
            // TODO: validation
            var searchProfile = cpRepo.GetSearchProfile(spId);
            if (searchProfile != null)
            {
                WebSecurity.CreateUserAndAccount(
                    username, password,
                    new { SearchProfileId = searchProfile.SearchProfileId });
            }
            return RedirectToAction("SearchProfiles");
        }

    }
}
