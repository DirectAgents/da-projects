using System;
using System.Collections.Generic;
using System.Web.Mvc;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.DTO;
using DirectAgents.Web.Areas.AB.Models;

namespace DirectAgents.Web.Areas.AB.Controllers
{
    public class DashboardController : DirectAgents.Web.Controllers.ControllerBase
    {
        public DashboardController(IMainRepository daRepository, IRevTrackRepository rtRepository, IABRepository abRepository, ISuperRepository superRepository)
        {
            this.daRepo = daRepository;
            this.rtRepo = rtRepository;
            this.abRepo = abRepository;
            this.superRepo = superRepository;
        }

        public ActionResult ProgTest(int id)
        {
            var monthStart = new DateTime(2016, 12, 1); //testing
            var pStats = rtRepo.GetProgClientStats(monthStart, id);

            return null;
        }

        public ActionResult ChooseMonth(DateTime month)
        {
            SetCurrentMonth("AB", month);
            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult Index(int x = 10)
        {
            DateTime currMonth = SetChooseMonthViewData("AB");

            var model = new DashboardVM
            {
                ABStats = GetStatsByClient(currMonth, x)
            };
            return View(model);
        }
        private IEnumerable<ABStat> GetStatsByClient(DateTime monthStart, int? maxClients = null)
        {
            //// If maxClients not specified, will use cached version if available
            //// If specified, clear out cached version and re-retrieve
            //if (maxClients.HasValue)
            //    Session["StatsByClient"] = null;

            //var sessionStats = Session["StatsByClient"];
            //if (sessionStats != null)
            //    return (IEnumerable<ABStat>)sessionStats;

            var clientStats = superRepo.StatsByClient(monthStart, maxClients: maxClients);

            //Session["StatsByClient"] = clientStats;

            return clientStats;
        }

        //public ActionResult EditClientStat(int id, decimal? sb, decimal? ec, decimal? ic)
        //{
        //    var clientStats = (IEnumerable<ABStat>)Session["StatsByClient"];
        //    foreach (var clientStat in clientStats)
        //    {
        //        if (clientStat.Id == id) // the one to edit
        //        {
        //            if (sb.HasValue)
        //                clientStat.StartBal = sb.Value;
        //            if (ec.HasValue)
        //                clientStat.ExtCred = ec.Value;
        //            if (ic.HasValue)
        //                clientStat.IntCred = ic.Value;
        //            break;
        //        }
        //    }
        //    return RedirectToAction("Index");
        //}

        //Breakdown by department/source...
        public ActionResult BySource(int clientId)
        {
            var client = abRepo.Client(clientId);
            if (client == null)
                return HttpNotFound();

            DateTime currMonth = SetChooseMonthViewData("AB");

            var model = new DashboardVM
            {
                ABClient = client,
                ABStats = superRepo.StatsForClient(clientId, currMonth)
            };
            return View(model);
        }

        public ActionResult LineItems(int clientId)
        {
            var client = abRepo.Client(clientId);
            if (client == null)
                return HttpNotFound();

            DateTime currMonth = SetChooseMonthViewData("AB");

            var lineItems = superRepo.StatsByLineItem(clientId, currMonth);

            var model = new DashboardVM
            {
                ABClient = client,
                LineItems = lineItems
            };
            return View(model);
        }

        // Client Summary - multiple months
        public ActionResult Client(int id)
        {
            var client = abRepo.Client(id);
            if (client == null)
                return HttpNotFound();

            var monthGroups = new List<LineItemGroup>();

            var month = DateTime.Today.FirstDayOfMonth(addMonths: -2);
            for (int i = 0; i < 3; i++) // get lineitems for last three months
            {
                var monthStats = superRepo.StatsByLineItem(id, month, separateFees: true);
                var liGroup = new LineItemGroup
                {
                    Month = month,
                    LineItems = monthStats
                };
                monthGroups.Add(liGroup);
                month = month.AddMonths(1);
            }

            var model = new DetailVM
            {
                ABClient = client,
                MonthGroups = monthGroups
            };

            //TODO: get summary info for the client
            // ??? by lineitem(<-try this) or by vendor ?
            // (for cake, need offaff dailysums)
            // superRepo.StatsForClient?
            // payments for the current month
            // somehow get a starting balance; go back to beginning of last month... for now, start at zero
            // ...later, when we have a StartingBalance entity: if there's one for the beginning of this month, use it. otherwise try going back
            // ...to the beginning of last month and see if there's one there.  (?check if any mid-month?)  if none found, use zero.
            // [Check AccountingBackup sheets for what's in included on the page.]
            //
            // # units?

            return View(model);
        }
    }
}