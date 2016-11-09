using System.Linq;
using System.Web.Mvc;
using ClientPortal.Data.Contracts;
using ClientPortal.Web.Areas.Admin.Models;
using ClientPortal.Web.Controllers;
using DirectAgents.Domain.Abstract;

namespace ClientPortal.Web.Areas.Admin.Controllers
{
    [Authorize(Users = "admin")]
    public class UsersController : CPController
    {
        public UsersController(IClientPortalRepository cpRepository, ICPProgRepository progRepository)
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

        public ActionResult AssignProgAdvertiser(int id)
        {
            var userProfile = cpRepo.GetUserProfile(id);
            var userVM = new UserVM
            {
                UserProfile = userProfile,
                CakeAdvertiser = userProfile.CakeAdvertiserId == null ? null : cpRepo.GetAdvertiser((int)userProfile.CakeAdvertiserId),
                ProgAdvertiser = userProfile.TDAdvertiserId == null ? null : progRepo.Advertiser((int)userProfile.TDAdvertiserId)
            };
            if (userProfile == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProgAdvertisers = progRepo.Advertisers().OrderBy(p => p.Name).ThenBy(p => p.Id);
            return View(userVM);
        }

        [HttpPost]
        public ActionResult AssignProgAdvertiser(int id, int progadvertiserid)
        {
            var userProfile = cpRepo.GetUserProfile(id);
            var userVM = new UserVM
            {
                UserProfile = userProfile,
                CakeAdvertiser = userProfile.CakeAdvertiserId == null ? null : cpRepo.GetAdvertiser((int)userProfile.CakeAdvertiserId),
                ProgAdvertiser = userProfile.TDAdvertiserId == null ? null : progRepo.Advertiser((int)userProfile.TDAdvertiserId)
            };
            var progAdvertiser = progRepo.Advertiser(progadvertiserid);

            if (userProfile != null && progAdvertiser != null)
            {
                userVM.UserProfile.TDAdvertiserId = progadvertiserid;
                userVM.ProgAdvertiser = progAdvertiser;
                cpRepo.SaveChanges();
                progRepo.SaveChanges();
            }
            return RedirectToAction("Setup", new { id = id });
        }

        public ActionResult UnassignProgAdvertiser(int id)
        {
            var userProfile = cpRepo.GetUserProfile(id);
            var userVM = new UserVM
            {
                UserProfile = userProfile,
                CakeAdvertiser = userProfile.CakeAdvertiserId == null ? null : cpRepo.GetAdvertiser((int)userProfile.CakeAdvertiserId),
                ProgAdvertiser = userProfile.TDAdvertiserId == null ? null : progRepo.Advertiser((int)userProfile.TDAdvertiserId)
            };
            if (userProfile != null && userVM.UserProfile.TDAdvertiserId.HasValue)
            {
                userVM.UserProfile.TDAdvertiserId = null;
                userVM.ProgAdvertiser = null;
                cpRepo.SaveChanges();
                progRepo.SaveChanges();
            }
            return RedirectToAction("Setup", new { id = id });
        }

        public ActionResult AssignCakeAdvertiser(int id)
        {
            var userProfile = cpRepo.GetUserProfile(id);
            var userVM = new UserVM
            {
                UserProfile = userProfile,
                CakeAdvertiser = userProfile.CakeAdvertiserId == null ? null : cpRepo.GetAdvertiser((int)userProfile.CakeAdvertiserId),
                ProgAdvertiser = userProfile.TDAdvertiserId == null ? null : progRepo.Advertiser((int)userProfile.TDAdvertiserId)
            };
            if (userProfile == null)
            {
                return HttpNotFound();
            }
            ViewBag.CakeAdvertisers = cpRepo.Advertisers.OrderBy(c => c.AdvertiserName).ThenBy(c => c.AdvertiserId);
            return View(userVM);
        }

        [HttpPost]
        public ActionResult AssignCakeAdvertiser(int id, int cakeadvertiserid)
        {
            var userProfile = cpRepo.GetUserProfile(id);
            var userVM = new UserVM
            {
                UserProfile = userProfile,
                CakeAdvertiser = userProfile.CakeAdvertiserId == null ? null : cpRepo.GetAdvertiser((int)userProfile.CakeAdvertiserId),
                ProgAdvertiser = userProfile.TDAdvertiserId == null ? null : progRepo.Advertiser((int)userProfile.TDAdvertiserId)
            };
            var cakeAdvertiser = cpRepo.GetAdvertiser(cakeadvertiserid);

            if (userProfile != null && cakeAdvertiser != null)
            {
                userVM.UserProfile.CakeAdvertiserId = cakeadvertiserid;
                userVM.CakeAdvertiser = cakeAdvertiser;
                cpRepo.SaveChanges();
            }
            return RedirectToAction("Setup", new { id = id });
        }

        public ActionResult UnassignCakeAdvertiser(int id)
        {
            var userProfile = cpRepo.GetUserProfile(id);
            var userVM = new UserVM
            {
                UserProfile = userProfile,
                CakeAdvertiser = userProfile.CakeAdvertiserId == null ? null : cpRepo.GetAdvertiser((int)userProfile.CakeAdvertiserId),
                ProgAdvertiser = userProfile.TDAdvertiserId == null ? null : progRepo.Advertiser((int)userProfile.TDAdvertiserId)
            };
            if (userProfile != null && userVM.UserProfile.CakeAdvertiserId.HasValue)
            {
                userVM.UserProfile.CakeAdvertiserId = null;
                userVM.CakeAdvertiser = null;
                cpRepo.SaveChanges();
            }
            return RedirectToAction("Setup", new { id = id });
        }

        public ActionResult AssignSearchProfile(int id)
        {
            var userProfile = cpRepo.GetUserProfile(id);
            if (userProfile == null)
                return HttpNotFound();

            ViewBag.SearchProfiles = cpRepo.SearchProfiles().OrderBy(s => s.SearchProfileName).ThenBy(s => s.SearchProfileId);
            return View(userProfile);
        }

        [HttpPost]
        public ActionResult AssignSearchProfile(int id, int searchprofileid)
        {
            var userProfile = cpRepo.GetUserProfile(id);
            var searchProfile = cpRepo.GetSearchProfile(searchprofileid);
            if (userProfile != null && searchProfile != null)
            {
                userProfile.SearchProfile = searchProfile;
                userProfile.SearchProfileId = searchProfile.SearchProfileId;
                
                cpRepo.SaveChanges();
            }
            return RedirectToAction("Setup", new { id = id });
        }

        public ActionResult UnassignSearchProfile(int id)
        {
            var userProfile = cpRepo.GetUserProfile(id);
            if (userProfile != null && userProfile.SearchProfileId.HasValue)
            {
                userProfile.SearchProfileId = null;
                userProfile.SearchProfile = null;
                cpRepo.SaveChanges();
            }
            return RedirectToAction("Setup", new { id = id });
        }
    }
}