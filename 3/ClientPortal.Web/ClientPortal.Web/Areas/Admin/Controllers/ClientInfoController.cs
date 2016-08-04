using System;
using System.Web.Helpers;
using System.Web.Mvc;
using ClientPortal.Data.Contexts;
using ClientPortal.Data.Contracts;
using ClientPortal.Web.Controllers;

namespace ClientPortal.Web.Areas.Admin.Controllers
{
    [Authorize(Users = "admin")]
    public class ClientInfoController : CPController
    {
        public ClientInfoController(IClientPortalRepository cpRepository, DirectAgents.Domain.Abstract.ITDRepository progRepository)
        {
            cpRepo = cpRepository;
            progRepo = progRepository;
        }

        public FileResult Logo(int id)
        {
            var clientInfo = cpRepo.GetClientInfo(id);
            if (clientInfo == null)
                return null;

            WebImage logo = new WebImage(clientInfo.Logo);
            return File(logo.GetBytes(), "image/" + logo.ImageFormat, logo.FileName);
        }
        public ActionResult EditLogo(int id = 0)
        {
            var clientInfo = cpRepo.GetClientInfo(id);
            if (clientInfo == null)
            {
                return HttpNotFound();
            }
            return View(clientInfo);
        }
        [HttpPost]
        public ActionResult UploadLogo(int id)
        {
            WebImage logo = WebImage.GetImageFromRequest();
            byte[] imageBytes = logo.GetBytes();

            var clientInfo = cpRepo.GetClientInfo(id);
            clientInfo.Logo = imageBytes;
            cpRepo.SaveChanges();

            return null;
        }

        public ActionResult Create(int userId)
        {
            var userProfile = cpRepo.GetUserProfile(userId);
            if (userProfile == null)
                return HttpNotFound();

            var clientInfo = new ClientInfo();

            // Set the client's name - from the user's SearchAdvertiser, ProgAdvertiser, CakeAdvertiser... or the username
            if (userProfile.SearchProfile != null)
                clientInfo.Name = userProfile.SearchProfile.SearchProfileName;
            if (String.IsNullOrWhiteSpace(clientInfo.Name) && userProfile.TDAdvertiserId != null)
            {
                var progAdv = progRepo.Advertiser(userProfile.TDAdvertiserId.Value);
                if (progAdv != null)
                    clientInfo.Name = progAdv.Name;
            }
            if (String.IsNullOrWhiteSpace(clientInfo.Name) && userProfile.CakeAdvertiserId != null)
            {
                var cakeAdv = cpRepo.GetAdvertiser(userProfile.CakeAdvertiserId.Value);
                if (cakeAdv != null)
                    clientInfo.Name = cakeAdv.AdvertiserName;
            }
            if (String.IsNullOrWhiteSpace(clientInfo.Name))
            {
                clientInfo.Name = userProfile.UserName;
            }

            userProfile.ClientInfo = clientInfo;
            cpRepo.SaveChanges();

            return RedirectToAction("Setup", "Users", new { id = userId });
        }
    }
}