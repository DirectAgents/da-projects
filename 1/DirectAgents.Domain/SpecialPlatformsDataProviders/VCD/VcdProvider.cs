using System.Collections.Generic;
using System.Linq;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.SpecialPlatformProviders.Contracts;
using DirectAgents.Domain.SpecialPlatformsDataProviders.Models;

namespace DirectAgents.Domain.SpecialPlatformProviders.Implementation
{
    public class VcdProvider : SpecialPlatformProvider
    {
        public VcdProvider() : base(Platform.Code_AraAmazon)
        {
        }

        public override IEnumerable<SpecialPlatformLatestsSummary> GetDatesRangeByAccounts(ClientPortalProgContext context)
        {
            var vcdSummaries = GetSummariesGroupedByAccountId(context).ToList();
            AssignExtAccountForSummaries(vcdSummaries, context);
            return vcdSummaries;
        }

        private IEnumerable<SpecialPlatformLatestsSummary> GetSummariesGroupedByAccountId(
            ClientPortalProgContext context)
        {
            return context.VcdAnalytic
                .GroupBy(x => x.AccountId)
                .Select(x => new SpecialPlatformLatestsSummary
                {
                    AccountId = x.Key,
                    EarliestDate = x.Min(z => z.Date),
                    LatestDate = x.Max(z => z.Date)
                });
        }
    }
}
