using ClientPortal.Data.Contexts;
using ClientPortal.Data.Contracts;
using ClientPortal.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace ClientPortal.Web.Controllers
{
    public class CPController : Controller
    {
        protected IClientPortalRepository cpRepo;

        protected ActionResult CheckLogout(UserInfo userInfo)
        {
            ActionResult result = null;
            if (!userInfo.HasUserProfile)
            {
                try
                {
                    LogoutAndRecordEvent();
                }
                catch
                {
                }
                result = RedirectToAction("Login", "Account");
            }
            return result;
        }

        protected void LogoutAndRecordEvent() // ? have an "int? userId" parameter ?
        {
            cpRepo.AddUserEvent(WebSecurity.CurrentUserId, "logout", true);
            WebSecurity.Logout();
        }

        protected UserInfo GetUserInfo()
        {
            var userProfile = GetUserProfile();

            Advertiser advertiser = null;
            if (userProfile != null)
                advertiser = GetAdvertiser(userProfile.CakeAdvertiserId);

            var userInfo = new UserInfo(userProfile, advertiser);
            return userInfo;
        }

        public static ClientPortal.Web.Models.UserProfile GetUserProfile()
        {
            ClientPortal.Web.Models.UserProfile userProfile = null;

            if (WebSecurity.Initialized)
            {
                var userID = WebSecurity.CurrentUserId;
                using (var usersContext = new UsersContext())
                {
                    userProfile = usersContext.UserProfiles.FirstOrDefault(c => c.UserId == userID);
                }
            }
            else 
                throw new Exception("web security not initialized");

            return userProfile;
        }

        public static int? GetAdvertiserId()
        {
            int? advertiserId = null;

            var userProfile = GetUserProfile();
            if (userProfile != null)
                advertiserId = userProfile.CakeAdvertiserId;

            return advertiserId;
        }

        public Advertiser GetAdvertiser()
        {
            int? advId = GetAdvertiserId();
            return GetAdvertiser(advId);
        }
        private Advertiser GetAdvertiser(int? advId)
        {
            if (advId.HasValue)
                return cpRepo.GetAdvertiser(advId.Value);
            else
                return null;
        }

        protected ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Go", "Home");
            }
        }

    }
}