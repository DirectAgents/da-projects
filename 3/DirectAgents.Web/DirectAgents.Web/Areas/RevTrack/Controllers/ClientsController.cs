﻿using System.Web.Mvc;
using DirectAgents.Domain.Abstract;

namespace DirectAgents.Web.Areas.RevTrack.Controllers
{
    public class ClientsController : DirectAgents.Web.Controllers.ControllerBase
    {
        public ClientsController(IRevTrackRepository revTrackRepository)
        {
            this.rtRepo = revTrackRepository;
        }

        public ActionResult Index()
        {
            return Content("test");
        }
	}
}