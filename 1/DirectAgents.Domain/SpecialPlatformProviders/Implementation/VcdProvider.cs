using System.Linq;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.DTO;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.SpecialPlatformProviders.Contracts;

namespace DirectAgents.Domain.SpecialPlatformProviders.Implementation
{
    public class VcdProvider : SpecialPlatformProvider
    {
        public VcdProvider() : base(Platform.Code_AraAmazon)
        {
        }

        public override IQueryable<SpecialPlatformSummary> GetDatesRangeByAccounts(ClientPortalProgContext context)
        {
            var vcdAccounts = GetAccountsByPlatform(context);
            var vcdSummaries = vcdAccounts.Select(account => new SpecialPlatformSummary
            {
                Account = account,
                EarliestDate = context.VcdAnalytic
                    .Where(x => x.AccountId == account.Id)
                    .Select(x => x.Date)
                    .Min(),
                LatestDate = context.VcdAnalytic
                    .Where(x => x.AccountId == account.Id)
                    .Select(x => x.Date)
                    .Max()
            });
            return vcdSummaries;
        }
    }
}
