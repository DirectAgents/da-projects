using System;
using System.Collections.Generic;
using System.Linq;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.DTO;

namespace DirectAgents.Domain.Concrete
{
    public class SuperRepository : ISuperRepository
    {
        // The underlying repositories:
        private IMainRepository mainRepo;
        private IRevTrackRepository rtRepo;

        //NOTE: In DAWeb, the underlying repositories are instantiated via ninject and disposed via ControllerBase.Dispose().
        //      We don't have the IRepositories in a constructor here so the controllers can access the child repositories directly.

        public void SetRepositories(IMainRepository mainRepo, IRevTrackRepository rtRepo)
        {
            this.mainRepo = mainRepo;
            this.rtRepo = rtRepo;
        }

        public IEnumerable<ABStat> StatsByClient(DateTime monthStart)
        {
            DateTime monthEnd = monthStart.AddMonths(1).AddDays(-1);
            var offerDailySummaries = mainRepo.GetOfferDailySummaries(null, monthStart, monthEnd);
            var statList = new List<ABStat>();

            var advertisers = mainRepo.GetAdvertisers().OrderBy(a => a.AdvertiserName).ToList(); //TODO: include? (e.g. AcctMgr)
            foreach (var adv in advertisers)
            {
                adv.OfferIds = mainRepo.OfferIds(advId: adv.AdvertiserId);
                var ods = offerDailySummaries.Where(o => adv.OfferIds.Contains(o.OfferId));
                if (ods.Any())
                {
                    var abStat = new ABStat
                    {
                        Client = adv.AdvertiserName,
                        Rev = ods.Sum(o => o.Revenue),
                        Cost = ods.Sum(o => o.Cost)
                    };
                    statList.Add(abStat);
                }
            }
            return statList;
        }

    }
}
