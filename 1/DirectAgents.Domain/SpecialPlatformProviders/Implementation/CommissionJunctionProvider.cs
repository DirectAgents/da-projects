using System.Collections.Generic;
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

        public override IEnumerable<SpecialPlatformSummary> GetDatesRangeByAccounts(ClientPortalProgContext context)
        {
            var cjSummaries = GetSummariesGroupedByAccountId(context).ToList();
            AssignExtAccountForSummaries(cjSummaries, context);
            return cjSummaries;
        }

        private static IEnumerable<SpecialPlatformSummary> GetSummariesGroupedByAccountId(
            ClientPortalProgContext context)
        {
            return context.CjAdvertiserCommissions
                .GroupBy(x => x.AccountId)
                .Select(x => new SpecialPlatformSummary
                {
                    AccountId = x.Key,
                    EarliestDate = x.Min(z => z.EventDate),
                    LatestDate = x.Max(z => z.EventDate)
                });
        }
    }
}
