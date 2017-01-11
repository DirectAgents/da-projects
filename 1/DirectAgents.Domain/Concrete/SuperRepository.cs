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

        // maxClients = max # of cients per department; total could be more
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

        // ByDepartment...
        public IEnumerable<ABStat> StatsForClient(int abClientId, DateTime monthStart)
        {
            var lineItemList = new List<IRTLineItem>();
            foreach (var deptRepo in departmentRepos)
            {
                var rtLineItem = deptRepo.StatSummaryForClient(abClientId, monthStart);
                lineItemList.Add(rtLineItem);
            }
            var abStatList = new List<ABStat>();
            foreach (var lineItem in lineItemList)
            {
                var abStat = new ABStat(lineItem);
                abStatList.Add(abStat);
            }
            return abStatList;
        }

        //TODO: (starting with programmatic) Do for other depts... [assume each dept has distinct vendors?]
        public IEnumerable<ABStat> StatsByVendor(int abClientId, DateTime monthStart)
        {
            return null;
        }

        //TODO: (starting with programmatic) Do for other depts...
        public IEnumerable<ABStat> StatsByLineItem(int abClientId, DateTime monthStart)
        {
            var lineItemList = new List<IRTLineItem>();
            foreach (var deptRepo in departmentRepos)
            {
                var rtLineItem = deptRepo.StatBreakdownByLineItem(abClientId, monthStart);
                lineItemList.AddRange(rtLineItem);
            }
            var abStatList = new List<ABStat>();
            foreach (var lineItem in lineItemList)
            {
                var abStat = new ABStat(lineItem);
                abStatList.Add(abStat);
            }
            return abStatList;
        }

    }
}
