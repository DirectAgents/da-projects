using ClientPortal.Data.Contracts;
using ClientPortal.Web.Controllers;
using System.Web.Mvc;

namespace ClientPortal.Web.Areas.Admin.Controllers
{
    [Authorize(Users = "admin")]
    public class TDController : CPController
    {
        public TDController(ITDRepository tdRepository)
        {
            tdRepo = tdRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult InsertionOrders()
        {
            var insertionOrders = tdRepo.InsertionOrders();
            return View(insertionOrders);
        }
    }
}
