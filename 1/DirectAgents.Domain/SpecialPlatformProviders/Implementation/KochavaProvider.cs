using System.Linq;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.DTO;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.SpecialPlatformProviders.Contracts;

namespace DirectAgents.Domain.SpecialPlatformProviders.Implementation
{
    public class KochavaProvider : SpecialPlatformProvider
    {
        public KochavaProvider() : base(Platform.Code_Kochava)
        {
        }

        public override IQueryable<SpecialPlatformSummary> GetDatesRangeByAccounts(ClientPortalProgContext context)
        {
            var kochavaAccounts = GetAccountsByPlatform(context);
            var kochavaSummaries = kochavaAccounts.Select(account => new SpecialPlatformSummary
            {
                Account = account,
                EarliestDate = context.KochavaItems
                    .Where(x => x.AccountId == account.Id)
                    .Select(x => x.Date)
                    .Min(),
                LatestDate = context.KochavaItems
                    .Where(x => x.AccountId == account.Id)
                    .Select(x => x.Date)
                    .Max()
            });
            return kochavaSummaries;
        }
    }
}
