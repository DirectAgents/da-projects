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

        // The department repositories
        private IEnumerable<IDepartmentRepository> departmentRepos;

        //NOTE: In DAWeb, the underlying repositories are instantiated via ninject and disposed via ControllerBase.Dispose().
        //      We don't have the IRepositories in a constructor here so the controllers can access the child repositories directly.

        public void SetRepositories(IMainRepository mainRepo, IRevTrackRepository rtRepo)
        {
            this.mainRepo = mainRepo;
            this.rtRepo = rtRepo;

            //NOTE: We _do_ set up the department repos here
            departmentRepos = new List<IDepartmentRepository>
            {
                new Cake_DeptRepository(mainRepo)
            };
        }

        public IEnumerable<ABStat> StatsByClient(DateTime monthStart, int? maxClients = null)
        {
            //List<IRTLineItem> rtLineItemList = new List<IRTLineItem>();

            var overallStats = new List<ABStat>();

            foreach (var deptRepo in departmentRepos)
            {
                var rtLineItems = deptRepo.StatsByClient(monthStart, maxClients: maxClients);
                //rtLineItemList.AddRange(rtLineItems);

                foreach (var li in rtLineItems)
                {
                    var abStat = new ABStat
                    {
                        Id = li.RTId, //TODO: switch to ABId
                        Client = li.Name,
                        Rev = li.Revenue,
                        Cost = li.Cost
                    };
                    //TODO: set StartBal, CurrBal, CredLim, etc

                    overallStats.Add(abStat);
                    //TODO: Instead of adding to overallStats, create a collection for each department.
                    //      Then, find a way to combine them together, merging the stats for omnichannel clients
                }
            }
            //var rtGroups = rtLineItemList.GroupBy(g => g.ABId);
            //foreach (var grp in rtGroups)
            //{
            //    var abStat = new ABStat
            //    {
            //        Id = grp.Key.HasValue ? grp.Key.Value : -1,
            //        //Client = ?
            //        Rev = grp.Sum(li => li.Revenue),
            //        Cost = grp.Sum(li => li.Cost)
            //    };
            //    overallStats.Add(abStat);
            //}

            return overallStats;
        }

        //public IEnumerable<ABStat> StatsByClient(DateTime monthStart, int? maxClients = null)
        //{
        //    DateTime monthEnd = monthStart.AddMonths(1).AddDays(-1);
        //    var offerDailySummaries = mainRepo.GetOfferDailySummaries(null, monthStart, monthEnd);
        //    var statList = new List<ABStat>();

        //    var advertisers = mainRepo.GetAdvertisers().OrderBy(a => a.AdvertiserName).ToList(); //TODO: include? (e.g. AcctMgr)
        //    foreach (var adv in advertisers)
        //    {
        //        if (maxClients.HasValue && statList.Count >= maxClients.Value)
        //            break;

        //        adv.OfferIds = mainRepo.OfferIds(advId: adv.AdvertiserId);
        //        var ods = offerDailySummaries.Where(o => adv.OfferIds.Contains(o.OfferId));
        //        if (ods.Any())
        //        {
        //            var abStat = new ABStat
        //            {
        //                Id = adv.AdvertiserId,
        //                Client = adv.AdvertiserName,
        //                Rev = ods.Sum(o => o.Revenue),
        //                Cost = ods.Sum(o => o.Cost)
        //            };
        //            // if maxClients is specified, get at most that number of non-zero abStats
        //            if (!maxClients.HasValue || abStat.Rev > 0 || abStat.Cost > 0)
        //                statList.Add(abStat);
        //        }
        //    }
        //    return statList;
        //}
        // attempting to make more efficient...
        //public IEnumerable<ABStat> StatsByClient(DateTime monthStart)
        //{
        //    DateTime monthEnd = monthStart.AddMonths(1).AddDays(-1);
        //    var offerDailySummaries = mainRepo.GetOfferDailySummaries(null, monthStart, monthEnd);
        //    var offerStats = offerDailySummaries.GroupBy(o => o.OfferId).Select(g => new
        //    {
        //        OfferId = g.Key,
        //        Revenue = g.Sum(o => o.Revenue),
        //        Cost = g.Sum(o => o.Cost)
        //    }
        //    ).ToList().AsQueryable();

        //    var statList = new List<ABStat>();
        //    var advertisers = mainRepo.GetAdvertisers().OrderBy(a => a.AdvertiserName).ToList(); //TODO: include? (e.g. AcctMgr)
        //    foreach (var adv in advertisers)
        //    {
        //        adv.OfferIds = mainRepo.OfferIds(advId: adv.AdvertiserId); // ?Could we avoid one db call per advertiser?
        //        var advOfferStats = offerStats.Where(o => adv.OfferIds.Contains(o.OfferId));
        //        if (advOfferStats.Any())
        //        {
        //            var abStat = new ABStat
        //            {
        //                Client = adv.AdvertiserName,
        //                Rev = advOfferStats.Sum(o => o.Revenue),
        //                Cost = advOfferStats.Sum(o => o.Cost)
        //            };
        //            statList.Add(abStat);
        //        }
        //    }
        //    return statList;
        //}
    }
}
