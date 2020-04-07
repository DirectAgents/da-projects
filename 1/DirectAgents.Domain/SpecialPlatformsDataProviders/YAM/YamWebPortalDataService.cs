using System.Linq;
using System.Collections.Generic;
using DirectAgents.Domain.SpecialPlatformsDataProviders.YAM.Models;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.SpecialPlatformsDataProviders.Models;

namespace DirectAgents.Domain.SpecialPlatformsDataProviders.YAM
{
    /// <summary>
    /// YAM web portal data provider service.
    /// </summary>
    /// <seealso cref="IYamWebPortalDataService" />
    public class YamWebPortalDataService : IYamWebPortalDataService
    {
        private ClientPortalProgContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="YamWebPortalDataService"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public YamWebPortalDataService(ClientPortalProgContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Gets the accounts latests information.
        /// </summary>
        /// <returns>Latests dates for accounts.</returns>
        public IEnumerable<YamLatestsInfo> GetAccountsLatestsInfo()
        {
            var accounts = GetAccountsForStatsDisplaying();
            var latestsInfo = accounts.Select(account => new YamLatestsInfo { Account = account }).ToList();
            FillWithDailyLatestsInfo(latestsInfo);
            FillWithCampaignLatestsInfo(latestsInfo);
            FillWithAdLatestsInfo(latestsInfo);
            FillWithCreativeLatestsInfo(latestsInfo);
            FillWithLineLatestsInfo(latestsInfo);
            FillWithPixelLatestsInfo(latestsInfo);
            return latestsInfo;
        }

        private List<ExtAccount> GetAccountsForStatsDisplaying()
        {
            return context.ExtAccounts
                .Where(x => x.Platform.Code == Platform.Code_YAM && x.ExternalId != null)
                .OrderBy(a => a.Disabled)
                .ThenBy(a => a.Name)
                .ToList();
        }

        private void FillWithDailyLatestsInfo(List<YamLatestsInfo> allAccountsLatestsInfo)
        {
            var latestsData = context.YamDailySummaries
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

        private void FillWithCampaignLatestsInfo(List<YamLatestsInfo> allAccountsLatestsInfo)
        {
            var latestsData = context.YamCampaignSummaries
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

        private void FillWithAdLatestsInfo(List<YamLatestsInfo> allAccountsLatestsInfo)
        {
            var latestsData = context.YamAdSummaries
                .GroupBy(x => x.Ad.Id)
                .Select(x => new SpecialPlatformLatestsSummary
                {
                    AccountId = x.Key,
                    EarliestDate = x.Min(z => z.Date),
                    LatestDate = x.Max(z => z.Date),
                }).ToList();
            foreach (var accountLatestInfo in allAccountsLatestsInfo)
            {
                accountLatestInfo.AdLatestsInfo =
                    latestsData.FirstOrDefault(latest => latest.AccountId == accountLatestInfo.Account.Id);
            }
        }

        private void FillWithLineLatestsInfo(List<YamLatestsInfo> allAccountsLatestsInfo)
        {
            var latestsData = context.YamLineSummaries
                .GroupBy(x => x.Line.Id)
                .Select(x => new SpecialPlatformLatestsSummary
                {
                    AccountId = x.Key,
                    EarliestDate = x.Min(z => z.Date),
                    LatestDate = x.Max(z => z.Date),
                }).ToList();
            foreach (var accountLatestInfo in allAccountsLatestsInfo)
            {
                accountLatestInfo.LineLatestsInfo =
                    latestsData.FirstOrDefault(latest => latest.AccountId == accountLatestInfo.Account.Id);
            }
        }

        private void FillWithCreativeLatestsInfo(List<YamLatestsInfo> allAccountsLatestsInfo)
        {
            var latestsData = context.YamCreativeSummaries
                .GroupBy(x => x.Creative.AccountId)
                .Select(x => new SpecialPlatformLatestsSummary
                {
                    AccountId = x.Key,
                    EarliestDate = x.Min(z => z.Date),
                    LatestDate = x.Max(z => z.Date),
                }).ToList();
            foreach (var accountLatestInfo in allAccountsLatestsInfo)
            {
                accountLatestInfo.CreativeLatestsInfo =
                    latestsData.FirstOrDefault(latest => latest.AccountId == accountLatestInfo.Account.Id);
            }
        }

        private void FillWithPixelLatestsInfo(List<YamLatestsInfo> allAccountsLatestsInfo)
        {
            var latestsData = context.YamPixelSummaries
                .GroupBy(x => x.Pixel.AccountId)
                .Select(x => new SpecialPlatformLatestsSummary
                {
                    AccountId = x.Key,
                    EarliestDate = x.Min(z => z.Date),
                    LatestDate = x.Max(z => z.Date),
                }).ToList();
            foreach (var accountLatestInfo in allAccountsLatestsInfo)
            {
                accountLatestInfo.PixelLatestsInfo =
                    latestsData.FirstOrDefault(latest => latest.AccountId == accountLatestInfo.Account.Id);
            }
        }
    }
}
