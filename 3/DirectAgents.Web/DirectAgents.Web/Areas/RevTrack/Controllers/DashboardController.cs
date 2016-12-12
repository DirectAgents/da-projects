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

        public ActionResult Index(int? x)
        {
            var monthStart = new DateTime(2015, 5, 1); //TESTING

            var model = new DashboardVM
            {
                ABStats = GetStatsByClient(monthStart, x)
            };
            return View(model);
        }

        IEnumerable<ABStat> GetStatsByClient(DateTime monthStart, int? maxClients = null)
        {
            if (maxClients.HasValue && maxClients == -1)
                Session["StatsByClient"] = null;

            var sessionStats = Session["StatsByClient"];
            if (sessionStats != null)
                return (IEnumerable<ABStat>)sessionStats;

            IEnumerable<ABStat> clientStats;
            if (maxClients.HasValue)
                clientStats = superRepo.StatsByClientX(monthStart, maxClients: maxClients);
            else
                clientStats = superRepo.StatsByClient(monthStart);

            if (maxClients.HasValue && maxClients > -1)
                Session["StatsByClient"] = clientStats;

            return clientStats;
        }

        public ActionResult EditClientStat(int id, decimal? sb, decimal? cl)
        {
            var clientStats = (IEnumerable<ABStat>)Session["StatsByClient"];
            foreach (var clientStat in clientStats)
            {
                if (clientStat.Id == id) // the one to edit
                {
                    if (sb.HasValue)
                        clientStat.StartBal = sb.Value;
                    if (cl.HasValue)
                        clientStat.CredLim = cl.Value;
                    break;
                }
            }
            return RedirectToAction("Index");
        }

	}
}