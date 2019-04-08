using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CakeExtracter.Commands;
using CakeExtracter.Commands.Search;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Entities.CPSearch;

namespace DirectAgents.Web.Areas.SearchAdmin.Controllers
{
    public class SearchAccountsController : Web.Controllers.ControllerBase
    {
        public SearchAccountsController(ICPSearchRepository cpSearchRepository)
        {
            this.cpSearchRepo = cpSearchRepository;
        }

        public ActionResult Index(int? spId, string channel)
        {
            var searchAccounts = GetAllOrderedSearchAccounts(spId: spId, channel: channel);
            return View(searchAccounts);
        }

        public ActionResult IndexGauge(int? spId, string channel)
        {
            var searchAccounts = GetAllOrderedSearchAccounts(spId: spId, channel: channel, includeGauges: true);
            return View(searchAccounts);
        }

        public ActionResult Create(int spId, string channel)
        {
            var searchAccount = new SearchAccount
            {
                SearchProfileId = spId,
                Channel = channel,
                Name = "New"
            };
            cpSearchRepo.SaveSearchAccount(searchAccount, createIfDoesntExist: true);
            return RedirectToAction("Index", "SearchAccounts", new { spId = spId });
        }

        public ActionResult Edit(int id)
        {
            var searchAccount = cpSearchRepo.GetSearchAccount(id);
            if (searchAccount == null)
                return HttpNotFound();
            return View(searchAccount);
        }
        [HttpPost]
        public ActionResult Edit(SearchAccount searchAccount)
        {
            if (ModelState.IsValid)
            {
                if (cpSearchRepo.SaveSearchAccount(searchAccount))
                    return RedirectToAction("Index", new { spId = searchAccount.SearchProfileId });
                ModelState.AddModelError("", "SearchAccount could not be saved.");
            }
            return View(searchAccount);
        }

        [HttpPost]
        public ActionResult Sync(int id, DateTime? from, DateTime? to)
        {
            var searchAccount = cpSearchRepo.GetSearchAccount(id);
            if (searchAccount == null)
                return HttpNotFound();

            switch (searchAccount.Channel)
            {
                case SearchAccount.GoogleChannel:
                    SynchSearchDailySummariesAdWordsCommand.RunStatic(clientId: searchAccount.AccountCode, start: from, end: to, getAllStats: true);
                    break;
                case SearchAccount.BingChannel:
                    int accountId;
                    if (int.TryParse(searchAccount.AccountCode, out accountId))
                        SynchSearchDailySummariesBingCommand.RunStatic(accountId: accountId, start: from, end: to, getConversionTypeStats: true);
                    break;
                case SearchAccount.AppleChannel:
                    SynchSearchDailySummariesAppleCommand.RunStatic(clientId: searchAccount.AccountCode, start: from, end: to);
                    break;
            }

            if (searchAccount.SearchProfileId == null)
                return Content("SearchProfileId not set.");
            else
                return RedirectToAction("IndexGauge", new { spId = searchAccount.SearchProfileId.Value });
        }

        /// <summary>
        /// The method returns all search accounts by the required parameters.
        /// Order: accounts that have not null/empty value of account code, ordered by search profile, then by channel, then by name.
        /// Further accounts that have null/empty value of account code, ordered by search profile, then by channel, then by name.
        /// </summary>
        /// <param name="spId">Search profile Id</param>
        /// <param name="channel">Channel</param>
        /// <param name="includeGauges">Show stats gauge for account or not</param>
        /// <returns>Sorted list of all search accounts</returns>
        private List<SearchAccount> GetAllOrderedSearchAccounts(int? spId = null, string channel = null, bool includeGauges = false)
        {
            var allSearchAccounts = cpSearchRepo.SearchAccounts(spId: spId, channel: channel, includeGauges: includeGauges).ToList();

            var enabledOrderedSearchAccounts = GetEnabledSearchAccounts(allSearchAccounts);
            var disabledSearchAccounts = GetDisabledSearchAccounts(allSearchAccounts);

            return enabledOrderedSearchAccounts
                .Concat(disabledSearchAccounts)
                .ToList();
        }

        /// <summary>
        /// The method returns enabled search accounts (have not null/empty value of account code).
        /// Ordered by search profile, then by channel, then by name
        /// </summary>
        /// <param name="allSearchAccounts">Not sorted list of all search accounts</param>
        /// <returns>Sorted list of enabled search accounts</returns>
        private static IEnumerable<SearchAccount> GetEnabledSearchAccounts(IEnumerable<SearchAccount> allSearchAccounts)
        {
            var enabledOrderedSearchAccounts = allSearchAccounts
                .Where(account => !string.IsNullOrEmpty(account.AccountCode))
                .OrderBy(account => account.SearchProfileId)
                .ThenBy(account => account.Channel)
                .ThenBy(account => account.Name)
                .ToList();
            return enabledOrderedSearchAccounts;
        }

        /// <summary>
        /// The method returns disabled search accounts (have null/empty value of account code).
        /// Ordered by search profile, then by channel, then by name
        /// </summary>
        /// <param name="allSearchAccounts">Not sorted list of all search accounts</param>
        /// <returns>Sorted list of disabled search accounts</returns>
        private static IEnumerable<SearchAccount> GetDisabledSearchAccounts(
            IEnumerable<SearchAccount> allSearchAccounts)
        {
            var disabledOrderedSearchAccounts = allSearchAccounts
                .Where(account => string.IsNullOrEmpty(account.AccountCode))
                .OrderBy(account => account.SearchProfileId)
                .ThenBy(account => account.Channel)
                .ThenBy(account => account.Name)
                .ToList();
            return disabledOrderedSearchAccounts;
        }
    }
}