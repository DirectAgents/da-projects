﻿using System;
using System.Linq;
using System.Web.Mvc;
using CakeExtracter.Commands;
using DirectAgents.Domain.Abstract;

namespace DirectAgents.Web.Controllers
{
    public class TestController : ControllerBase
    {
        public TestController(IMainRepository mainRepository)
        {
            this.daRepo = mainRepository;
        }

        public ActionResult Cake()
        {
            var adv = daRepo.GetAdvertisers();
            var names = adv.Select(x => x.AdvertiserName).ToArray();
            var text = String.Join("\n", names);
            return Content(text);
        }

        public ActionResult SynchSearch()
        {
            int searchProfileId = 40;
            DateTime? start = new DateTime(2015, 11, 1);
            DateTime? end = null;
            int? daysAgoToStart = null;
            string getClickAssistConvStats = "both";
            //SynchSearchDailySummariesAdWordsCommand.RunStatic(searchProfileId, start, end, daysAgoToStart, getClickAssistConvStats);
            //SynchSearchDailySummariesBingCommand.RunStatic(searchProfileId, start, end, daysAgoToStart);

            return Content("okay");
        }
	}
}