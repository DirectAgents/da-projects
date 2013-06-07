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

        public UserInfo GetUserInfo()
        {
            var userProfile = GetUserProfile();

            Advertiser advertiser = null;
            if (userProfile != null)
                advertiser = GetAdvertiser(userProfile.CakeAdvertiserId);

            var userInfo = new UserInfo(userProfile, advertiser);
            return userInfo;
        }

        public static UserProfile GetUserProfile()
        {
            UserProfile userProfile = null;

            if (WebSecurity.Initialized)
            {
                var userID = WebSecurity.CurrentUserId;
                using (var usersContext = new UsersContext())
                {
                    userProfile = usersContext.UserProfiles.FirstOrDefault(c => c.UserId == userID);
                }
            }
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
    }
}