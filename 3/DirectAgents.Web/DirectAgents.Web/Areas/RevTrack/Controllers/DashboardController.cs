using System;
using System.Collections.Generic;
using System.Web.Mvc;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.DTO;
using DirectAgents.Web.Areas.RevTrack.Models;

namespace DirectAgents.Web.Areas.RevTrack.Controllers
{
    public class DashboardController : DirectAgents.Web.Controllers.ControllerBase
    {
        public DashboardController(IMainRepository daRepository, IRevTrackRepository revTrackRepository, ISuperRepository superRepository)
        {
            this.daRepo = daRepository;
            this.rtRepo = revTrackRepository;
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

            //TEMP!
            int id = 0;
            foreach (var stat in clientStats)
            {
                stat.Id = id++;
                stat.Budget = 10000;
            }

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

	}
}