using System.Collections.Generic;
using System.Linq;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.SpecialPlatformProviders.Contracts;
using DirectAgents.Domain.SpecialPlatformsDataProviders.Models;

namespace DirectAgents.Domain.SpecialPlatformProviders.Implementation
{
    public class DspProvider : SpecialPlatformProvider
    {
        public DspProvider() : base(Platform.Code_DspAmazon)
        {
        }

        public override IEnumerable<SpecialPlatformLatestsSummary> GetDatesRangeByAccounts(ClientPortalProgContext context)
        {
            var dspSummaries = GetSummariesGroupedByAccountId(context).ToList();
            AssignExtAccountForSummaries(dspSummaries, context);
            return dspSummaries;
        }

        private IEnumerable<SpecialPlatformLatestsSummary> GetSummariesGroupedByAccountId(
            ClientPortalProgContext context)
        {
            return context.DspCreativesMetricValues
                .GroupBy(x => x.Creative.AccountId)
                .Select(x => new SpecialPlatformLatestsSummary
                {
                    AccountId = x.Key,
                    EarliestDate = x.Min(z => z.Date),
                    LatestDate = x.Max(z => z.Date)
                });
        }
    }
}
