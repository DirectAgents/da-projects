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
        private IABRepository abRepo;

        // The department repositories
        private IEnumerable<IDepartmentRepository> departmentRepos;

        //NOTE: In DAWeb, the underlying repositories are instantiated via ninject and disposed via ControllerBase.Dispose().
        //      We don't have the IRepositories in a constructor here so the controllers can access the child repositories directly.

        public void SetRepositories(IMainRepository mainRepo, IRevTrackRepository rtRepo, IABRepository abRepo)
        {
            this.mainRepo = mainRepo;
            this.rtRepo = rtRepo;
            this.abRepo = abRepo;

            //NOTE: We _do_ set up the department repos here
            departmentRepos = new List<IDepartmentRepository>
            {
                new Cake_DeptRepository(mainRepo),
                new Prog_DeptRepository(rtRepo)
            };
        }

        public IEnumerable<ABStat> StatsByClient(DateTime monthStart, int? maxClients = null)
        {
            List<IRTLineItem> rtLineItemList = new List<IRTLineItem>();

            foreach (var deptRepo in departmentRepos)
            {
                var rtLineItems = deptRepo.StatsByClient(monthStart, maxClients: maxClients);
                rtLineItemList.AddRange(rtLineItems);
            }

            var orphanLineItems = rtLineItemList.Where(li => !li.ABId.HasValue).ToList();
            var rtGroups = rtLineItemList.Where(li => li.ABId.HasValue).GroupBy(g => g.ABId.Value);
            var abClients = abRepo.Clients();
            var abClientIds = abClients.Select(c => c.Id).ToArray();

            var orphanGroups = rtGroups.Where(g => !abClientIds.Contains(g.Key));
            var orphanGroupLIs = orphanGroups.Select(g => new RTLineItem(g));
            orphanLineItems.AddRange(orphanGroupLIs);

            var overallABStatsList =
                (from c in abClients.ToList()
                 from g in rtGroups
                 where c.Id == g.Key
                 select new ABStat(g)
                 {
                     Id = c.Id,
                     Client = c.Name
                 }).ToList();

            if (orphanLineItems.Any())
            {
                var orphanABStat = new ABStat(orphanLineItems)
                {
                    Id = -1,
                    Client = "zUnassigned"
                };
                overallABStatsList.Add(orphanABStat);
            }
            //TODO: set StartBal, CurrBal, CredLim, etc

            return overallABStatsList;
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
