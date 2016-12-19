using System;
using System.Collections.Generic;
using System.Linq;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.DTO;

namespace DirectAgents.Domain.Concrete
{
    public interface IDepartmentRepository
    {
        IEnumerable<IRTLineItem> StatsByClient(DateTime monthStart, int? maxClients = null);
    }

    public class Cake_DeptRepository : IDepartmentRepository
    {
        // Assume this mainRepo is disposed of elsewhere
        public IMainRepository mainRepo { get; set; }

        public Cake_DeptRepository(IMainRepository mainRepo)
        {
            this.mainRepo = mainRepo;
        }

        public IEnumerable<IRTLineItem> StatsByClient(DateTime monthStart, int? maxClients = null)
        {
            DateTime monthEnd = monthStart.AddMonths(1).AddDays(-1);
            var offerDailySummaries = mainRepo.GetOfferDailySummaries(null, monthStart, monthEnd);
            var lineItems = new List<IRTLineItem>();

            var advertisers = mainRepo.GetAdvertisers().OrderBy(a => a.AdvertiserName).ToList(); //TODO: include? (e.g. AcctMgr)
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
                        ClientName = adv.AdvertiserName,
                        Revenue = ods.Sum(o => o.Revenue),
                        Cost = ods.Sum(o => o.Cost)
                    };
                    // if maxClients is specified, get at most that number of non-zero abStats
                    if (!maxClients.HasValue || lineItem.Revenue > 0 || lineItem.Cost > 0)
                        lineItems.Add(lineItem);
                }
            }
            return lineItems;
        }
    }
}
