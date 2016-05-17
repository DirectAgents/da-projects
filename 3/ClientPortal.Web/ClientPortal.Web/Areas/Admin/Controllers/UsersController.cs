﻿using System.Linq;
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
    }
}