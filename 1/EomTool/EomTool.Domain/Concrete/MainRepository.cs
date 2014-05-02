using System;
using System.Linq;
using EomTool.Domain.Abstract;
using EomTool.Domain.Entities;
using EomTool.Domain.DTOs;

namespace EomTool.Domain.Concrete
{
    public partial class MainRepository : IMainRepository
    {
        EomEntities context;

        public MainRepository(EomEntities context)
        {
            this.context = context;
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
        //---

        public Advertiser GetAdvertiser(int advId)
        {
            return context.Advertisers.FirstOrDefault(a => a.id == advId);
        }

        public IQueryable<AccountManager> AccountManagers(bool withActivityOnly = false)
        {
            if (withActivityOnly)
                return Campaigns(null, null, true).Select(c => c.AccountManager).Distinct();
            else
                return context.AccountManagers;
        }

        public IQueryable<Campaign> Campaigns(int? accountManagerId, int? advertiserId, bool activeOnly = false)
        {
            var campaigns = context.Campaigns.AsQueryable();
            if (accountManagerId.HasValue)
                campaigns = campaigns.Where(c => c.account_manager_id == accountManagerId.Value);
            if (advertiserId.HasValue)
                campaigns = campaigns.Where(c => c.advertiser_id == advertiserId.Value);

            if (activeOnly)
            {
                return (from c in campaigns
                        join i in context.Items on c.pid equals i.pid
                        select c).Distinct();
            }
            else
                return campaigns;
        }

        public IQueryable<CampaignAmount> CampaignAmounts(int? accountManagerId, int? advertiserId, bool byAffiliate = false)
        {
            var items = context.Items.AsQueryable();
            var campaigns = Campaigns(accountManagerId, advertiserId);

            IQueryable<CampaignAmount> amounts;
            if (byAffiliate)
            {
                var itemGroups = items.GroupBy(i => new { i.pid, i.affid, i.revenue_currency_id });

                amounts = from itemGroup in itemGroups
                          join c in campaigns on itemGroup.Key.pid equals c.pid
                          join a in context.Affiliates on itemGroup.Key.affid equals a.affid
                          join rc in context.Currencies on itemGroup.Key.revenue_currency_id equals rc.id
                          select new CampaignAmount()
                          {
                              AdvId = c.advertiser_id,
                              AdvertiserName = c.Advertiser.name,
                              Pid = itemGroup.Key.pid,
                              CampaignName = c.campaign_name,
                              AffId = itemGroup.Key.affid,
                              AffiliateName = a.name2,
                              RevenueCurrency = rc.name,
                              Revenue = itemGroup.Sum(g => g.total_revenue.HasValue ? g.total_revenue.Value : 0),
                              NumAffs = 1
                          };
            }
            else // by campaign
            {
                var itemGroups = items.GroupBy(i => new { i.pid, i.revenue_currency_id });
                var rawAmounts = from ig in itemGroups
                                 select new {
                                     ig.Key.pid,
                                     ig.Key.revenue_currency_id,
                                     revenue = ig.Sum(g => g.total_revenue.HasValue ? g.total_revenue.Value : 0),
                                     numAffs = ig.Select(g => g.affid).Distinct().Count()
                                 };

                amounts = from ra in rawAmounts
                          join c in campaigns on ra.pid equals c.pid
                          join rc in context.Currencies on ra.revenue_currency_id equals rc.id
                          select new CampaignAmount()
                          {
                              AdvId = c.advertiser_id,
                              AdvertiserName = c.Advertiser.name,
                              Pid = ra.pid,
                              CampaignName = c.campaign_name,
                              RevenueCurrency = rc.name,
                              Revenue = ra.revenue,
                              NumAffs = ra.numAffs
                          };
            }
            return amounts;
        }

        //---

        public bool CampaignExists(int pid)
        {
            return context.Campaigns.Any(c => c.pid == pid);
        }
        public Campaign GetCampaign(int pid)
        {
            return context.Campaigns.FirstOrDefault(c => c.pid == pid);
        }

        public bool AffiliateExists(int affId)
        {
            return context.Affiliates.Any(a => a.affid == affId);
        }
        public Affiliate GetAffiliate(int affId)
        {
            return context.Affiliates.FirstOrDefault(a => a.affid == affId);
        }

        public Source GetSource(int sourceId)
        {
            return context.Sources.FirstOrDefault(s => s.id == sourceId);
        }
        public Source GetSource(string sourceName)
        {
            return context.Sources.FirstOrDefault(s => s.name == sourceName);
        }

        public bool UnitTypeExists(int unitTypeId)
        {
            return context.UnitTypes.Any(u => u.id == unitTypeId);
        }
        public UnitType GetUnitType(int unitTypeId)
        {
            return context.UnitTypes.FirstOrDefault(u => u.id == unitTypeId);
        }
        public UnitType GetUnitType(string unitTypeName)
        {
            return context.UnitTypes.FirstOrDefault(ut => ut.name == unitTypeName);
        }

        public bool CurrencyExists(int currency)
        {
            return context.Currencies.Any(c => c.id == currency);
        }
        public Currency GetCurrency(string currencyName)
        {
            return context.Currencies.FirstOrDefault(c => c.name == currencyName);
        }

        public void AddItem(Item item)
        {
            context.Items.AddObject(item);
        }
        public bool ItemExists(Item item)
        {
            return context.Items.Any(i =>
                i.pid == item.pid &&
                i.affid == item.affid &&
                i.unit_type_id == item.unit_type_id &&
                i.num_units == item.num_units &&
                i.revenue_currency_id == item.revenue_currency_id &&
                i.revenue_per_unit == item.revenue_per_unit &&
                i.cost_currency_id == item.cost_currency_id &&
                i.cost_per_unit == item.cost_per_unit &&
                i.notes == item.notes &&
                i.source_id == item.source_id);
        }
    }
}
