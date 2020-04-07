using System.Linq;
using System.Collections.Generic;
using DirectAgents.Domain.SpecialPlatformsDataProviders.Adform.Models;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.SpecialPlatformsDataProviders.Models;

namespace DirectAgents.Domain.SpecialPlatformsDataProviders.Adform
{
    /// <summary>
    /// Adform web portal data provider service.
    /// </summary>
    /// <seealso cref="IAdformWebPortalDataService" />
    public class AdformWebPortalDataService : IAdformWebPortalDataService
    {
        private ClientPortalProgContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdformWebPortalDataService"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public AdformWebPortalDataService(ClientPortalProgContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Gets the accounts latests information.
        /// </summary>
        /// <returns>Latests dates for accounts.</returns>
        public IEnumerable<AdformLatestsInfo> GetAccountsLatestsInfo()
        {
            var accounts = GetAccountsForStatsDisplaying();
            var latestsInfo = accounts.Select(account => new AdformLatestsInfo { Account = account }).ToList();
            FillWithDailyLatestsInfo(latestsInfo);
            FillWithLineItemLatestsInfo(latestsInfo);
            FillWithCampaignLatestsInfo(latestsInfo);
            FillWithBannerLatestsInfo(latestsInfo);
            return latestsInfo;
        }

        private List<ExtAccount> GetAccountsForStatsDisplaying()
        {
            return context.ExtAccounts
                .Where(x => x.Platform.Code == Platform.Code_Adform && x.ExternalId != null)
                .OrderBy(a => a.Disabled)
                .ThenBy(a => a.Name)
                .ToList();
        }

        private void FillWithLineItemLatestsInfo(List<AdformLatestsInfo> allAccountsLatestsInfo)
        {
            var latestsData = context.AdfLineItemSummaries
                .GroupBy(x => x.LineItem.Id)
                .Select(x => new SpecialPlatformLatestsSummary
                {
                    AccountId = x.Key,
                    EarliestDate = x.Min(z => z.Date),
                    LatestDate = x.Max(z => z.Date),
                }).ToList();
            foreach (var accountLatestInfo in allAccountsLatestsInfo)
            {
                accountLatestInfo.LineItemLatestsInfo =
                    latestsData.FirstOrDefault(latest => latest.AccountId == accountLatestInfo.Account.Id);
            }
        }

        private void FillWithBannerLatestsInfo(List<AdformLatestsInfo> allAccountsLatestsInfo)
        {
            var latestsData = context.AdfBannerSummaries
                .GroupBy(x => x.Banner.AccountId)
                .Select(x => new SpecialPlatformLatestsSummary
                {
                    AccountId = x.Key,
                    EarliestDate = x.Min(z => z.Date),
                    LatestDate = x.Max(z => z.Date),
                }).ToList();
            foreach (var accountLatestInfo in allAccountsLatestsInfo)
            {
                accountLatestInfo.BannerLatestsInfo =
                    latestsData.FirstOrDefault(latest => latest.AccountId == accountLatestInfo.Account.Id);
            }
        }

        private void FillWithDailyLatestsInfo(List<AdformLatestsInfo> allAccountsLatestsInfo)
        {
            var latestsData = context.AdfDailySummaries
                .GroupBy(x => x.Account.Id)
                .Select(x => new SpecialPlatformLatestsSummary
                {
                    AccountId = x.Key,
                    EarliestDate = x.Min(z => z.Date),
                    LatestDate = x.Max(z => z.Date),
                }).ToList();
            foreach (var accountLatestInfo in allAccountsLatestsInfo)
            {
                accountLatestInfo.DailyLatestsInfo =
                    latestsData.FirstOrDefault(latest => latest.AccountId == accountLatestInfo.Account.Id);
            }
        }

        private void FillWithCampaignLatestsInfo(List<AdformLatestsInfo> allAccountsLatestsInfo)
        {
            var latestsData = context.AdfCampaignSummaries
                .GroupBy(x => x.Campaign.AccountId)
                .Select(x => new SpecialPlatformLatestsSummary
                {
                    AccountId = x.Key,
                    EarliestDate = x.Min(z => z.Date),
                    LatestDate = x.Max(z => z.Date),
                }).ToList();
            foreach (var accountLatestInfo in allAccountsLatestsInfo)
            {
                accountLatestInfo.CampaignLatestsInfo =
                    latestsData.FirstOrDefault(latest => latest.AccountId == accountLatestInfo.Account.Id);
            }
        }
    }
}
