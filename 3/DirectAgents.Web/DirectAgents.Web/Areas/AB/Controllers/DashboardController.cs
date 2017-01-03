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

        public ActionResult Index(int? x)
        {
            var monthStart = new DateTime(2016, 12, 1); //TESTING

            var model = new DashboardVM
            {
                ABStats = GetStatsByClient(monthStart, x)
            };
            return View(model);
        }
        private IEnumerable<ABStat> GetStatsByClient(DateTime monthStart, int? maxClients = null)
        {
            // If maxClients not specified, will use cached version if available
            // If specified, clear out cached version and re-retrieve
            if (maxClients.HasValue)
                Session["StatsByClient"] = null;

            var sessionStats = Session["StatsByClient"];
            if (sessionStats != null)
                return (IEnumerable<ABStat>)sessionStats;

            var clientStats = superRepo.StatsByClient(monthStart, maxClients: maxClients);

            Session["StatsByClient"] = clientStats;

            return clientStats;
        }

        public ActionResult EditClientStat(int id, decimal? sb, decimal? ec, decimal? ic)
        {
            var clientStats = (IEnumerable<ABStat>)Session["StatsByClient"];
            foreach (var clientStat in clientStats)
            {
                if (clientStat.Id == id) // the one to edit
                {
                    if (sb.HasValue)
                        clientStat.StartBal = sb.Value;
                    if (ec.HasValue)
                        clientStat.ExtCred = ec.Value;
                    if (ic.HasValue)
                        clientStat.IntCred = ic.Value;
                    break;
                }
            }
            return RedirectToAction("Index");
        }

        //Breakdown by department...
        public ActionResult Client(int id)
        {
            var client = abRepo.Client(id);
            if (client == null)
                return HttpNotFound();

            var monthStart = new DateTime(2016, 12, 1); //TESTING

            var model = new DashboardVM
            {
                ABClient = client,
                ABStats = superRepo.StatsForClient(id, monthStart)
            };
            return View(model);
        }
    }
}