using System.Linq;
using System.Collections.Generic;
using DirectAgents.Domain.SpecialPlatformsDataProviders.DBM.Models;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.SpecialPlatformsDataProviders.Models;

namespace DirectAgents.Domain.SpecialPlatformsDataProviders.DBM
{
    /// <summary>
    /// DBM web portal data provider service.
    /// </summary>
    /// <seealso cref="IDbmWebPortalDataService" />
    public class DbmWebPortalDataService : IDbmWebPortalDataService
    {
        private ClientPortalProgContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbmWebPortalDataService"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public DbmWebPortalDataService(ClientPortalProgContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Gets the accounts latests information.
        /// </summary>
        /// <returns>Latests dates for accounts.</returns>
        public IEnumerable<DbmLatestsInfo> GetAccountsLatestsInfo()
        {
            var accounts = GetAccountsForStatsDisplaying();
            var latestsInfo = accounts.Select(account => new DbmLatestsInfo { Account = account }).ToList();
            FillWithCreativeLatestsInfo(latestsInfo);
            FillWithLineItemLatestsInfo(latestsInfo);
            return latestsInfo;
        }

        private List<ExtAccount> GetAccountsForStatsDisplaying()
        {
            return context.ExtAccounts
                .Where(x => x.Platform.Code == Platform.Code_DBM && x.ExternalId != null)
                .OrderBy(a => a.Disabled)
                .ThenBy(a => a.Name)
                .ToList();
        }

        private void FillWithLineItemLatestsInfo(List<DbmLatestsInfo> allAccountsLatestsInfo)
        {
            var latestsData = context.DbmLineItemSummaries
                .GroupBy(x => x.LineItem.AccountId)
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

        private void FillWithCreativeLatestsInfo(List<DbmLatestsInfo> allAccountsLatestsInfo)
        {
            var latestsData = context.DbmCreativeSummaries
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
    }
}
