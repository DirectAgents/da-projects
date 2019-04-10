using System.Linq;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.DTO;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.SpecialPlatformProviders.Contracts;

namespace DirectAgents.Domain.SpecialPlatformProviders.Implementation
{
    public class CommissionJunctionProvider : SpecialPlatformProvider
    {
        public CommissionJunctionProvider() : base(Platform.Code_CJ)
        {
        }

        public override IQueryable<SpecialPlatformSummary> GetDatesRangeByAccounts(ClientPortalProgContext context)
        {
            var cjAccounts = GetAccountsByPlatform(context);
            var cjSummaries = cjAccounts.Select(account => new SpecialPlatformSummary
            {
                Account = account,
                EarliestDate = context.CjAdvertiserCommissions
                    .Where(x => x.AccountId == account.Id)
                    .Select(x => x.EventDate)
                    .Min(),
                LatestDate = context.CjAdvertiserCommissions
                    .Where(x => x.AccountId == account.Id)
                    .Select(x => x.EventDate)
                    .Max()
            });
            return cjSummaries;
        }
    }
}
