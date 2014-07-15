using ClientPortal.Data.Contexts;
using ClientPortal.Data.Contracts;
using ClientPortal.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
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

        public UserProfile GetUserProfile()
        {
            UserProfile userProfile = null;

            if (WebSecurity.Initialized)
                userProfile = cpRepo.GetUserProfile(WebSecurity.CurrentUserId);
            else 
                throw new Exception("web security not initialized");

            return userProfile;
        }

        public int? GetAdvertiserId()
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

        protected string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

        // ---

        protected override void Dispose(bool disposing)
        {
            if (cpRepo != null) cpRepo.Dispose();
            base.Dispose(disposing);
        }
    }
}