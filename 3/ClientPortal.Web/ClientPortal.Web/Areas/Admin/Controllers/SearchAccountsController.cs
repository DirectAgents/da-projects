using System;
using System.Linq;
using System.Web.Mvc;
using CakeExtracter.Commands;
using ClientPortal.Data.Contexts;
using ClientPortal.Data.Contracts;
using ClientPortal.Web.Controllers;

namespace ClientPortal.Web.Areas.Admin.Controllers
{
    [Authorize(Users = "admin")]
    public class SearchAccountsController : CPController
    {
        public SearchAccountsController(IClientPortalRepository cpRepository)
        {
            cpRepo = cpRepository;
        }

        public ActionResult Create(int spId, string channel)
        {
            var searchAccount = new SearchAccount
            {
                SearchProfileId = spId,
                Channel = channel,
                Name = "New"
            };
            cpRepo.CreateSearchAccount(searchAccount);
            return RedirectToAction("EditProfile", "SearchAdmin", new { spId = spId });
        }

        public ActionResult Edit(int id)
        {
            var searchAccount = cpRepo.GetSearchAccount(id);
            if (searchAccount == null)
                return HttpNotFound();

            FillStatsRange(searchAccount);
            return View(searchAccount);
        }
        [HttpPost]
        public ActionResult Edit(SearchAccount searchAccount)
        {
            if (ModelState.IsValid)
            {
                if (cpRepo.SaveSearchAccount(searchAccount))
                    return RedirectToAction("EditProfile", "SearchAdmin", new { spId = searchAccount.SearchProfileId });
                ModelState.AddModelError("", "SearchAccount could not be saved.");
            }
            return View(searchAccount);
        }
        
        private void FillStatsRange(SearchAccount searchAccount)
        {
            var sds = cpRepo.GetSearchDailySummaries(searchAccountId: searchAccount.SearchAccountId);
            if (sds.Any())
            {
                var selDate = sds.Select(x => x.Date);
                searchAccount.EarliestStat = selDate.Min();
                searchAccount.LatestStat = selDate.Max();
            }
        }

        public ActionResult Synch(int id)
        {
            var searchAccount = cpRepo.GetSearchAccount(id);
            if (searchAccount == null)
                return HttpNotFound();

            FillStatsRange(searchAccount);
            return View(searchAccount);
        }
        [HttpPost]
        public ActionResult Synch(int id, DateTime? from, DateTime? to)
        {
            var searchAccount = cpRepo.GetSearchAccount(id);
            if (searchAccount == null)
                return HttpNotFound();

            SynchSearchDailySummariesAdWordsCommand.RunStatic(clientId: searchAccount.AccountCode, start: from, end: to, getAllStats: true);
            return RedirectToAction("Synch", new { id = id });
        }

    }
}