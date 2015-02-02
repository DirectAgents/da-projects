using System.Web.Mvc;
using ClientPortal.Data.Contracts;
using ClientPortal.Web.Areas.TD.Models;
using ClientPortal.Web.Controllers;

namespace ClientPortal.Web.Areas.TD.Controllers
{
    [Authorize]
    public class ReportsController : CPController
    {
        public ReportsController(ITDRepository tdRepository, IClientPortalRepository cpRepository)
        {
            tdRepo = tdRepository;
            cpRepo = cpRepository;
        }

        public ActionResult Summary(string metric1, string metric2)
        {
            var userInfo = GetUserInfo();
            var model = new TDReportModel(userInfo, metric1, metric2);
            return PartialView(model);
        }

        public ActionResult Sample()
        {
            return PartialView();
        }

        public ActionResult Creative()
        {
            var userInfo = GetUserInfo();
            var model = new TDReportModel(userInfo);
            return PartialView(model);
        }

        public ActionResult Weekly()
        {
            var userInfo = GetUserInfo();
            var model = new TDReportModel(userInfo);
            return PartialView(model);
        }

        public ActionResult Monthly()
        {
            var userInfo = GetUserInfo();
            var model = new TDReportModel(userInfo);
            return PartialView(model);
        }

    }
}
