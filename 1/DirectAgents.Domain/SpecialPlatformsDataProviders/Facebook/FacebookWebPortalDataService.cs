using System;
using System.Collections.Generic;
using System.Linq;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.Facebook;
using DirectAgents.Domain.SpecialPlatformsDataProviders.Facebook.Models;
using DirectAgents.Domain.SpecialPlatformsDataProviders.Models;

namespace DirectAgents.Domain.SpecialPlatformsDataProviders.Facebook
{
    /// <summary>
    /// Facebook web portal data provider service.
    /// </summary>
    /// <seealso cref="DirectAgents.Domain.SpecialPlatformsDataProviders.Facebook.IFacebookWebPortalDataService" />
    public class FacebookWebPortalDataService : IFacebookWebPortalDataService
    {
        private ClientPortalProgContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookWebPortalDataService"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public FacebookWebPortalDataService(ClientPortalProgContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Gets the accounts latests information.
        /// </summary>
        /// <returns>
        /// Latests dates for accounts
        /// </returns>
        public IEnumerable<FacebookLatestsInfo> GetAccountsLatestsInfo()
        {
            var accounts = GetAccountsForStatsDisplaying();
            var latestsInfo = accounts.Select(account => new FacebookLatestsInfo { Account = account }).ToList();
            FillWithDailyLatestsInfo(latestsInfo);
            FillWithCampaignsLatestsInfo(latestsInfo);
            FillWithAdsetsLatestsInfo(latestsInfo);
            FillWithAdsetActionsLatestsInfo(latestsInfo);
            FillWithAdsLatestsInfo(latestsInfo);
            FillWithAdsActionsLatestsInfo(latestsInfo);
            return latestsInfo;
        }

        /// <summary>
        /// Gets the accounts totals information on Campaigns Level.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <returns>
        /// Latests Campaigns Totals for accounts.
        /// </returns>
        public IEnumerable<FacebookTotalsInfo> GetCampaignTotalsInfo(DateTime fromDate, DateTime toDate)
        {
            var accountsTotals = GetAccountsForStatsDisplaying().Select(account => new FacebookTotalsInfo { Account = account }).ToList();
            context.FbCampaignSummaries.Where(sum => sum.Date >= fromDate && sum.Date <= toDate).ToList()
                .GroupBy(x => x.Campaign.AccountId).ToList().ForEach(gr => SetTotalsInfoForAccount(gr, accountsTotals));
            return accountsTotals;
        }

        /// <summary>
        /// Gets the accounts totals information on Daily Level.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <returns>
        /// Latests Daily Totals for accounts.
        /// </returns>
        public IEnumerable<FacebookTotalsInfo> GetDailyTotalsInfo(DateTime fromDate, DateTime toDate)
        {
            var accountsTotals = GetAccountsForStatsDisplaying().Select(account => new FacebookTotalsInfo { Account = account }).ToList();
            context.FbDailySummaries.Where(sum => sum.Date >= fromDate && sum.Date <= toDate).ToList()
                .GroupBy(x => x.AccountId).ToList().ForEach(gr => SetTotalsInfoForAccount(gr, accountsTotals));
            return accountsTotals;
        }

        /// <summary>
        /// Gets the accounts totals information on Adsets Level.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <returns>
        /// Latests Adsets Totals for accounts.
        /// </returns>
        public IEnumerable<FacebookTotalsInfo> GetAdsetsTotalsInfo(DateTime fromDate, DateTime toDate)
        {
            var accountsTotals = GetAccountsForStatsDisplaying().Select(account => new FacebookTotalsInfo { Account = account }).ToList();
            context.FbAdSetSummaries.Where(sum => sum.Date >= fromDate && sum.Date <= toDate).ToList()
                .GroupBy(x => x.AdSet.AccountId).ToList().ForEach(gr => SetTotalsInfoForAccount(gr, accountsTotals));
            return accountsTotals;
        }

        /// <summary>
        /// Gets the accounts totals information on Ads Level.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <returns>
        /// Latests Ads Totals for accounts.
        /// </returns>
        public IEnumerable<FacebookTotalsInfo> GetAdsTotalsInfo(DateTime fromDate, DateTime toDate)
        {
            var accountsTotals = GetAccountsForStatsDisplaying().Select(account => new FacebookTotalsInfo { Account = account }).ToList();
            context.FbAdSummaries.Where(sum => sum.Date >= fromDate && sum.Date <= toDate).ToList()
                .GroupBy(x => x.Ad.AccountId).ToList().ForEach(gr => SetTotalsInfoForAccount(gr, accountsTotals));
            return accountsTotals;
        }

        /// <summary>
        /// Gets the ad set actions totals information.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <returns>Adset Actions Totals Info.</returns>
        public IEnumerable<FacebookAccountActionsTotals> GetAdSetActionsTotalsInfo(DateTime fromDate, DateTime toDate)
        {
            return context.FbAdSetActions.Where(sum => sum.Date >= fromDate && sum.Date <= toDate)
                 .GroupBy(x => new { x.AdSet.AccountId, x.ActionTypeId }).Select(gr => new
                 {
                     gr.FirstOrDefault().ActionType,
                     gr.FirstOrDefault().AdSet.ExtAccount,
                     gr.FirstOrDefault().ClickAttrWindow,
                     gr.FirstOrDefault().ViewAttrWindow,
                     PostClick = gr.Sum(z => z.PostClick),
                     PostView = gr.Sum(z => z.PostView),
                     PostClickVal = gr.Sum(z => z.PostClickVal),
                     PostViewVal = gr.Sum(z => z.PostViewVal),
                 }).ToList().GroupBy(item => item.ExtAccount.Id).Select(gr =>
                     new FacebookAccountActionsTotals
                     {
                         Account = gr.First().ExtAccount,
                         ActionTotals = gr.Select(act => new FacebookActionsTotals
                         {
                             Id = act.ActionType.Id,
                             Code = act.ActionType.Code,
                             ClickAttrWindow = act.ClickAttrWindow,
                             ViewAttrWindow = act.ViewAttrWindow,
                             PostClick = act.PostClick,
                             PostView = act.PostView,
                             PostClickVal = act.PostClickVal,
                             PostViewVal = act.PostViewVal,
                         }).ToList(),
                     });
        }

        /// <summary>
        /// Gets the ads actions totals information.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <returns>Ads Actions Totals Info.</returns>
        public IEnumerable<FacebookAccountActionsTotals> GetAdsActionsTotalsInfo(DateTime fromDate, DateTime toDate)
        {
            return context.FbAdActions.Where(sum => sum.Date >= fromDate && sum.Date <= toDate)
                 .GroupBy(x => new { x.Ad.AccountId, x.ActionTypeId }).Select(gr => new
                 {
                     gr.FirstOrDefault().ActionType,
                     gr.FirstOrDefault().Ad.ExtAccount,
                     gr.FirstOrDefault().ClickAttrWindow,
                     gr.FirstOrDefault().ViewAttrWindow,
                     PostClick = gr.Sum(z => z.PostClick),
                     PostView = gr.Sum(z => z.PostView),
                     PostClickVal = gr.Sum(z => z.PostClickVal),
                     PostViewVal = gr.Sum(z => z.PostViewVal),
                 }).ToList().GroupBy(item => item.ExtAccount.Id).Select(gr =>
                     new FacebookAccountActionsTotals
                     {
                         Account = gr.First().ExtAccount,
                         ActionTotals = gr.Select(act => new FacebookActionsTotals
                         {
                             Id = act.ActionType.Id,
                             Code = act.ActionType.Code,
                             ClickAttrWindow = act.ClickAttrWindow,
                             ViewAttrWindow = act.ViewAttrWindow,
                             PostClick = act.PostClick,
                             PostView = act.PostView,
                             PostClickVal = act.PostClickVal,
                             PostViewVal = act.PostViewVal,
                         }).ToList(),
                     });
        }

        private void SetTotalsInfoForAccount(IGrouping<int, FbBaseSummary> gr, List<FacebookTotalsInfo> allAccountsTotalInfo)
        {
            var totalsInfo = allAccountsTotalInfo.Find(ti => ti.Account.Id == gr.Key);
            totalsInfo.ImpressionsTotal = gr.Sum(z => z.Impressions);
            totalsInfo.ClicksTotal = gr.Sum(z => z.Clicks);
            totalsInfo.AllClicksTotal = gr.Sum(z => z.AllClicks);
            totalsInfo.CostTotal = gr.Sum(z => z.Cost);
            totalsInfo.PostClickConvTotal = gr.Sum(z => z.PostClickConv);
            totalsInfo.PostViewConvTotal = gr.Sum(z => z.PostViewConv);
            totalsInfo.PostClickRevTotal = gr.Sum(z => z.PostClickRev);
            totalsInfo.PostViewRevTotal = gr.Sum(z => z.PostViewRev);
        }

        private List<ExtAccount> GetAccountsForStatsDisplaying()
        {
            return context.ExtAccounts
                .Where(x => x.Platform.Code == Platform.Code_FB && x.ExternalId != null)
                .OrderBy(a => a.Disabled)
                .ThenBy(a => a.Name)
                .ToList();
        }

        private void FillDailySummaryTotalsInfo(List<FacebookLatestsInfo> allAccountsLatestsInfo)
        {
            var latestsData = context.FbCampaignSummaries
                .GroupBy(x => x.Campaign.AccountId)
                .Select(x => new SpecialPlatformLatestsSummary
                {
                    AccountId = x.Key,
                    EarliestDate = x.Min(z => z.Date),
                    LatestDate = x.Max(z => z.Date),
                }).ToList();
            latestsData.ForEach(latests =>
                allAccountsLatestsInfo.First(accountLatests => accountLatests.Account.Id == latests.AccountId).DailyLatestsInfo = latests);
        }

        private void FillWithDailyLatestsInfo(List<FacebookLatestsInfo> allAccountsLatestsInfo)
        {
            var latestsData = context.FbDailySummaries
                .GroupBy(x => x.AccountId)
                .Select(x => new SpecialPlatformLatestsSummary
                {
                    AccountId = x.Key,
                    EarliestDate = x.Min(z => z.Date),
                    LatestDate = x.Max(z => z.Date),
                }).ToList();
            latestsData.ForEach(latests =>
                allAccountsLatestsInfo.First(accountLatests => accountLatests.Account.Id == latests.AccountId).DailyLatestsInfo = latests);
        }

        private void FillWithCampaignsLatestsInfo(List<FacebookLatestsInfo> allAccountsLatestsInfo)
        {
            var latestsData = context.FbCampaignSummaries
                .GroupBy(x => x.Campaign.AccountId)
                .Select(x => new SpecialPlatformLatestsSummary
                {
                    AccountId = x.Key,
                    EarliestDate = x.Min(z => z.Date),
                    LatestDate = x.Max(z => z.Date),
                }).ToList();
            latestsData.ForEach(latests =>
                allAccountsLatestsInfo.First(accountLatests => accountLatests.Account.Id == latests.AccountId).CampaignLatestsInfo = latests);
        }

        private void FillWithAdsetsLatestsInfo(List<FacebookLatestsInfo> allAccountsLatestsInfo)
        {
            var latestsData = context.FbAdSetSummaries
                .GroupBy(x => x.AdSet.AccountId)
                .Select(x => new SpecialPlatformLatestsSummary
                {
                    AccountId = x.Key,
                    EarliestDate = x.Min(z => z.Date),
                    LatestDate = x.Max(z => z.Date),
                }).ToList();
            latestsData.ForEach(latests =>
                allAccountsLatestsInfo.First(accountLatests => accountLatests.Account.Id == latests.AccountId).AdsetsLatestsInfo = latests);
        }

        private void FillWithAdsetActionsLatestsInfo(List<FacebookLatestsInfo> allAccountsLatestsInfo)
        {
            var latestsData = context.FbAdSetActions
                .GroupBy(x => x.AdSet.AccountId)
                .Select(x => new SpecialPlatformLatestsSummary
                {
                    AccountId = x.Key,
                    EarliestDate = x.Min(z => z.Date),
                    LatestDate = x.Max(z => z.Date),
                }).ToList();
            latestsData.ForEach(latests =>
                allAccountsLatestsInfo.First(accountLatests => accountLatests.Account.Id == latests.AccountId).AdsetsActionsLatestsInfo = latests);
        }

        private void FillWithAdsLatestsInfo(List<FacebookLatestsInfo> allAccountsLatestsInfo)
        {
            var latestsData = context.FbAdSummaries
                  .GroupBy(x => x.Ad.AccountId)
                  .Select(x => new SpecialPlatformLatestsSummary
                  {
                      AccountId = x.Key,
                      EarliestDate = x.Min(z => z.Date),
                      LatestDate = x.Max(z => z.Date),
                  }).ToList();
            latestsData.ForEach(latests =>
                allAccountsLatestsInfo.First(accountLatests => accountLatests.Account.Id == latests.AccountId).AdsLatestsInfo = latests);
        }

        private void FillWithAdsActionsLatestsInfo(List<FacebookLatestsInfo> allAccountsLatestsInfo)
        {
            var latestsData = context.FbAdActions
                .GroupBy(x => x.Ad.AccountId)
                .Select(x => new SpecialPlatformLatestsSummary
                {
                    AccountId = x.Key,
                    EarliestDate = x.Min(z => z.Date),
                    LatestDate = x.Max(z => z.Date),
                }).ToList();
            latestsData.ForEach(latests =>
                allAccountsLatestsInfo.First(accountLatests => accountLatests.Account.Id == latests.AccountId).AdsActionsLatestsInfo = latests);
        }
    }
}
