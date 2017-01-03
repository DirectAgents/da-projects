using System;
using System.Collections.Generic;
using System.Linq;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.DTO;

namespace DirectAgents.Domain.Concrete
{
    public class Prog_DeptRepository : IDepartmentRepository
    {
        // Assume this repo is disposed of elsewhere
        public IRevTrackRepository rtRepo { get; set; }

        public Prog_DeptRepository(IRevTrackRepository rtRepo)
        {
            this.rtRepo = rtRepo;
        }

        public IEnumerable<IRTLineItem> StatsByClient(DateTime monthStart, bool includeZeros = false, int? maxClients = null)
        {
            var clients = rtRepo.ProgClients();
            var lineItems = new List<IRTLineItem>();
            foreach (var client in clients.OrderBy(c => c.Name))
            {
                if (maxClients.HasValue && lineItems.Count >= maxClients.Value)
                    break;

                var clientStats = rtRepo.GetProgClientStats(monthStart, client.Id);
                if (includeZeros || clientStats.DACost > 0 || clientStats.TotalRevenue > 0)
                {
                    var lineItem = new RTLineItem(clientStats);
                    lineItems.Add(lineItem);
                }
            }
            return lineItems;
        }

        public IRTLineItem StatsForClient(int id, DateTime monthStart)
        {
            var lineItemList = new List<IRTLineItem>();
            var progClients = rtRepo.ProgClients(ABClientId: id);

            // Usually there's just one...
            foreach (var progClient in progClients)
            {
                var progClientStats = rtRepo.GetProgClientStats(monthStart, progClient);
                var lineItem = new RTLineItem(progClientStats);
                lineItemList.Add(lineItem);
            }
            var summaryLineItem = new RTLineItem(lineItemList)
            {
                Name = "Programmatic"
            };
            return summaryLineItem;
        }
    }

    public class Cake_DeptRepository : IDepartmentRepository
    {
        // Assume this repo is disposed of elsewhere
        public IMainRepository mainRepo { get; set; }

        public Cake_DeptRepository(IMainRepository mainRepo)
        {
            this.mainRepo = mainRepo;
        }

        public IEnumerable<IRTLineItem> StatsByClient(DateTime monthStart, bool includeZeros = false, int? maxClients = null)
        {
            var advertisers = mainRepo.GetAdvertisers().OrderBy(a => a.AdvertiserName).ToList(); //TODO: include? (e.g. AcctMgr)
            var lineItems = StatsForAdvertisers(advertisers, monthStart, includeZeros, maxClients);
            return lineItems;
        }

        private IEnumerable<IRTLineItem> StatsForAdvertisers(IEnumerable<DirectAgents.Domain.Entities.Cake.Advertiser> advertisers, DateTime monthStart, bool includeZeros = false, int? maxClients = null)
        {
            DateTime monthEnd = monthStart.AddMonths(1).AddDays(-1);
            var offerDailySummaries = mainRepo.GetOfferDailySummaries(null, monthStart, monthEnd);
            var lineItems = new List<IRTLineItem>();

            foreach (var adv in advertisers)
            {
                if (maxClients.HasValue && lineItems.Count >= maxClients.Value)
                    break;

                adv.OfferIds = mainRepo.OfferIds(advId: adv.AdvertiserId);
                var ods = offerDailySummaries.Where(o => adv.OfferIds.Contains(o.OfferId));
                if (ods.Any())
                {
                    var lineItem = new RTLineItem
                    {
                        ABId = adv.ABClientId,
                        RTId = adv.AdvertiserId,
                        Name = adv.AdvertiserName,
                        Revenue = ods.Sum(o => o.Revenue),
                        Cost = ods.Sum(o => o.Cost)
                    };
                    // if maxClients is specified, get at most that number of non-zero abStats
                    if (includeZeros || lineItem.Revenue > 0 || lineItem.Cost > 0)
                        lineItems.Add(lineItem);
                }
            }
            return lineItems;
        }

        public IRTLineItem StatsForClient(int id, DateTime monthStart)
        {
            // Usually there's just one...
            var advertisers = mainRepo.GetAdvertisers(ABClientId: id);
            var advLineItems = StatsForAdvertisers(advertisers, monthStart);

            var summaryLineItem = new RTLineItem(advLineItems)
            {
                Name = "Cake"
            };
            return summaryLineItem;
        }
    }
}
