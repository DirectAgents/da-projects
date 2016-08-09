using System.Linq;
using System.Web.Mvc;
using ClientPortal.Data.Contracts;
using ClientPortal.Web.Areas.Admin.Models;
using ClientPortal.Web.Controllers;

namespace ClientPortal.Web.Areas.Admin.Controllers
{
    [Authorize(Users = "admin")]
    public class UsersController : CPController
    {
        public UsersController(IClientPortalRepository cpRepository, DirectAgents.Domain.Abstract.ITDRepository progRepository)
        {
            cpRepo = cpRepository;
            progRepo = progRepository;
        }

        public ActionResult Index()
        {
            var cakeAdvertisers = cpRepo.Advertisers.ToList();
            var progAdvertisers = progRepo.Advertisers().ToList(); //TODO? don't load images? (also check DAWeb)
            var userProfiles = cpRepo.UserProfiles().OrderBy(up => up.UserName).ToList();
            var userVMs = userProfiles.Select(up => new UserVM
            {
                UserProfile = up,
                CakeAdvertiser = (up.CakeAdvertiserId.HasValue ? cakeAdvertisers.FirstOrDefault(a => a.AdvertiserId == up.CakeAdvertiserId.Value) : null),
                ProgAdvertiser = (up.TDAdvertiserId.HasValue ? progAdvertisers.FirstOrDefault(a => a.Id == up.TDAdvertiserId.Value) : null)
            });
            return View(userVMs);
        }

        public ActionResult Setup(int id)
        {
            var userProfile = cpRepo.GetUserProfile(id);
            var userVM = new UserVM
            {
                UserProfile = userProfile,
                CakeAdvertiser = userProfile.CakeAdvertiserId == null ? null : cpRepo.GetAdvertiser((int)userProfile.CakeAdvertiserId),
                ProgAdvertiser = userProfile.TDAdvertiserId == null ? null : progRepo.Advertiser((int)userProfile.TDAdvertiserId)
            };

            if (userVM.UserProfile == null)
                return HttpNotFound();

            return View(userVM);
        }

        public ActionResult AssignClientInfo(int id)
        {
            var userProfile = cpRepo.GetUserProfile(id);
            if (userProfile == null)
                return HttpNotFound();

            ViewBag.ClientInfos = cpRepo.ClientInfos().OrderBy(c => c.Name).ThenBy(c => c.Id);
            return View(userProfile);
        }

        [HttpPost]
        public ActionResult AssignClientInfo(int id, int clientinfoid)
        {
            var userProfile = cpRepo.GetUserProfile(id);
            var clientInfo = cpRepo.GetClientInfo(clientinfoid);
            if (userProfile != null && clientInfo != null)
            {
                userProfile.ClientInfo = clientInfo;
                cpRepo.SaveChanges();
            }
            return RedirectToAction("Setup", new { id = id });
        }

        public ActionResult UnassignClientInfo(int id)
        {
            var userProfile = cpRepo.GetUserProfile(id);
            if (userProfile != null && userProfile.ClientInfoId.HasValue)
            {
                userProfile.ClientInfoId = null;
                cpRepo.SaveChanges();
            }
            return RedirectToAction("Setup", new { id = id });
        }
    }
}