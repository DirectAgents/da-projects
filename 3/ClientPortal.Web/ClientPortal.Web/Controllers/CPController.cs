using System;
using System.IO;
using System.Web.Mvc;
using ClientPortal.Data.Contexts;
using ClientPortal.Data.Contracts;
using ClientPortal.Data.Entities.TD;
using ClientPortal.Web.Models;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Concrete;
using DirectAgents.Domain.Entities.TD;
using WebMatrix.WebData;

namespace ClientPortal.Web.Controllers
{
    public class CPController : Controller
    {
        protected IClientPortalRepository cpRepo;
        protected ClientPortal.Data.Contracts.ITDRepository cptdRepo;
        protected DirectAgents.Domain.Abstract.ITDRepository datdRepo;

        // return a redirect if needed to logout; otherwise return null
        protected ActionResult CheckLogout(UserInfo userInfo)
        {
            if (CheckLogoutBool(userInfo))
                return RedirectToAction("Login", "Account", new { area = "" });
            else
                return null;
        }
        protected ActionResult CheckLogoutTD(UserInfo userInfo)
        {
            if (CheckLogoutBool(userInfo))
                return RedirectToAction("Login", "Account", new { area = "", ReturnUrl = "/td" });
            else
                return null;
        }

        // return true if needed to logout
        private bool CheckLogoutBool(UserInfo userInfo)
        {
            if (userInfo.HasUserProfile)
                return false;

            try
            {
                LogoutAndRecordEvent();
            }
            catch
            {
            }
            return true;
        }

        protected void LogoutAndRecordEvent() // ? have an "int? userId" parameter ?
        {
            cpRepo.AddUserEvent(WebSecurity.CurrentUserId, "logout", true);
            WebSecurity.Logout();
        }

        protected UserInfo GetUserInfo()
        {
            var userProfile = GetUserProfile();

            ClientPortal.Data.Contexts.Advertiser cpAdvertiser = null;
            TradingDeskAccount tradingDeskAccount = null;
            DirectAgents.Domain.Entities.TD.Advertiser datdAdvertiser = null;
            if (userProfile != null)
            {
                cpAdvertiser = GetAdvertiser(userProfile.CakeAdvertiserId);
                if (userProfile.TradingDeskAccountId.HasValue && cptdRepo != null)
                    tradingDeskAccount = cptdRepo.GetTradingDeskAccount(userProfile.TradingDeskAccountId.Value);
                if (userProfile.TDAdvertiserId.HasValue && datdRepo != null)
                    datdAdvertiser = datdRepo.Advertiser(userProfile.TDAdvertiserId.Value);
            }
            var userInfo = new UserInfo(userProfile, cpAdvertiser, tradingDeskAccount: tradingDeskAccount, datdAdvertiser: datdAdvertiser);
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

        public ClientPortal.Data.Contexts.Advertiser GetAdvertiser()
        {
            int? advId = GetAdvertiserId();
            return GetAdvertiser(advId);
        }
        private ClientPortal.Data.Contexts.Advertiser GetAdvertiser(int? advId)
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
            if (cptdRepo != null) cptdRepo.Dispose();
            if (datdRepo != null) datdRepo.Dispose();
            base.Dispose(disposing);
        }
    }
}