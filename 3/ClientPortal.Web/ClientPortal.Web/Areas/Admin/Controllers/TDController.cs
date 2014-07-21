using ClientPortal.Data.Contracts;
using ClientPortal.Data.Entities.TD;
using ClientPortal.Web.Areas.TD.Models;
using ClientPortal.Web.Controllers;
using System.Linq;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace ClientPortal.Web.Areas.Admin.Controllers
{
    [Authorize(Users = "admin")]
    public class TDController : CPController
    {
        public TDController(ITDRepository tdRepository, IClientPortalRepository cpRepository)
        {
            tdRepo = tdRepository;
            cpRepo = cpRepository;
        }

        public ActionResult Index()
        {
            var userInfo = GetUserInfo();
            var model = new TDHomeModel(userInfo);
            return View(model);
        }

        public ActionResult InsertionOrders()
        {
            var insertionOrders = tdRepo.InsertionOrders();
            //var insertionOrders = tdRepo.InsertionOrders().ToList();
            //var userProfiles = cpRepo.UserProfiles().Where(up => up.InsertionOrderId.HasValue).ToList();
            //foreach (var io in insertionOrders)
            //{
            //    io.UserProfiles = userProfiles.Where(up => up.InsertionOrderId == io.InsertionOrderID);
            //}

            return View(insertionOrders);
        }

        public ActionResult CreateAccount(int ioID)
        {
            tdRepo.CreateAccountForInsertionOrder(ioID);
            return RedirectToAction("InsertionOrders");
        }

        public ActionResult TradingDeskAccounts()
        {
            var tdAccounts = tdRepo.TradingDeskAccounts().ToList();
            var userProfiles = cpRepo.UserProfiles().Where(up => up.TradingDeskAccountId.HasValue).ToList();
            foreach (var tdAccount in tdAccounts)
            {
                tdAccount.UserProfiles = userProfiles.Where(up => up.TradingDeskAccountId == tdAccount.TradingDeskAccountId);
                var advIds = tdAccount.UserProfiles.Where(up => up.CakeAdvertiserId.HasValue).Select(up => up.CakeAdvertiserId.Value);
                tdAccount.Advertisers = cpRepo.Advertisers.Where(a => advIds.Contains(a.AdvertiserId)).ToList();
            }
            return View(tdAccounts);
        }

        public ActionResult CreateUserProfile(int tdaId)
        {
            var tdAccount = tdRepo.GetTradingDeskAccount(tdaId);
            if (tdAccount == null)
                return HttpNotFound();

            return View(tdAccount);
        }

        [HttpPost]
        public ActionResult CreateUserProfile(int tdaId, string username, string password) //, bool sendemail, string email)
        {
            var tdAccount = tdRepo.GetTradingDeskAccount(tdaId);
            if (tdAccount != null)
            {
                WebSecurity.CreateUserAndAccount(
                    username, password,
                    new { TradingDeskAccountId = tdAccount.TradingDeskAccountId });

                //if (sendemail && !String.IsNullOrWhiteSpace(email))
                //    Helpers.SendUserProfileEmail(username, password, email);
            }
            return RedirectToAction("TradingDeskAccounts");
        }

    }
}
