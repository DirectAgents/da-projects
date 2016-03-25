using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ClientPortal.Data.Contracts;
using ClientPortal.Data.Entities.TD.DBM;
using ClientPortal.Web.Areas.TD.Models;
using ClientPortal.Web.Controllers;
using ClientPortal.Web.Models;

namespace ClientPortal.Web.Areas.TD.Controllers
{
    [Authorize]
    public class ReportsController : CPController
    {
        public ReportsController(ITDRepository cptdRepository, IClientPortalRepository cpRepository)
        {
            cptdRepo = cptdRepository;
            cpRepo = cpRepository;
        }

        public ActionResult Summary(string metric1, string metric2)
        {
            var userInfo = GetUserInfo();
            var model = new TDSummaryReportModel(userInfo, metric1, metric2);
            return PartialView(model);
        }

        public ActionResult Creative()
        {
            var userInfo = GetUserInfo();
            var model = new TDReportModelWithDates(userInfo);
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

        public ActionResult Analysis()
        {
            var userInfo = GetUserInfo();
            var model = new TDAnalysisModel(userInfo);
            model.UserListStats = GetTopUserListStats(userInfo);

            return PartialView(model);
        }
        private IEnumerable<UserListStat> GetTopUserListStats(UserInfo userInfo)
        {
            var insertionOrderID = userInfo.InsertionOrderID;
            if (insertionOrderID.HasValue)
            {
                var userListRuns = cptdRepo.UserListRuns(insertionOrderID.Value);
                if (userListRuns.Any())
                {
                    var ulr = userListRuns.OrderByDescending(u => u.Date).First();
                    return ulr.Lists.OrderByDescending(uls => uls.MatchRatio);
                }
            }
            return null;
        }
    }
}
