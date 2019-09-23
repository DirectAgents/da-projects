using System.Collections.Generic;
using System.Linq;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.SpecialPlatformProviders.Contracts;
using DirectAgents.Domain.SpecialPlatformsDataProviders.Models;

namespace DirectAgents.Domain.SpecialPlatformProviders.Implementation
{
    public class CommissionJunctionProvider : SpecialPlatformProvider
    {
        public CommissionJunctionProvider() : base(Platform.Code_CJ)
        {
        }

        public override IEnumerable<SpecialPlatformLatestsSummary> GetDatesRangeByAccounts(ClientPortalProgContext context)
        {
            var cjSummaries = GetSummariesGroupedByAccountId(context).ToList();
            AssignExtAccountForSummaries(cjSummaries, context);
            return cjSummaries;
        }

        private IEnumerable<SpecialPlatformLatestsSummary> GetSummariesGroupedByAccountId(
            ClientPortalProgContext context)
        {
            return context.CjAdvertiserCommissions
                .GroupBy(x => x.AccountId)
                .Select(x => new SpecialPlatformLatestsSummary
                {
                    AccountId = x.Key,
                    EarliestDate = x.Min(z => z.EventDate),
                    LatestDate = x.Max(z => z.EventDate)
                });
        }
    }
}
