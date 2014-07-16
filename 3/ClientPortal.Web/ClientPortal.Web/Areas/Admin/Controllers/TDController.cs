using ClientPortal.Data.Contracts;
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
            return View();
        }

        public ActionResult InsertionOrders()
        {
            var insertionOrders = tdRepo.InsertionOrders().ToList();
            var userProfiles = cpRepo.UserProfiles().Where(up => up.InsertionOrderId.HasValue).ToList();
            foreach (var io in insertionOrders)
            {
                io.UserProfiles = userProfiles.Where(up => up.InsertionOrderId == io.InsertionOrderID);
            }

            return View(insertionOrders);
        }

        public ActionResult CreateUserProfile(int ioID)
        {
            var insertionOrder = tdRepo.GetInsertionOrder(ioID);
            if (insertionOrder == null)
                return HttpNotFound();

            return View(insertionOrder);
        }

        [HttpPost]
        public ActionResult CreateUserProfile(int ioID, string username, string password) //, bool sendemail, string email)
        {
            var insertionOrder = tdRepo.GetInsertionOrder(ioID);
            if (insertionOrder != null)
            {
                WebSecurity.CreateUserAndAccount(
                    username, password,
                    new { InsertionOrderId = insertionOrder.InsertionOrderID });

                //if (sendemail && !String.IsNullOrWhiteSpace(email))
                //    Helpers.SendUserProfileEmail(username, password, email);
            }
            return RedirectToAction("InsertionOrders");
        }

    }
}
