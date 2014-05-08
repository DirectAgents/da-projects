using EomTool.Domain.Abstract;
using EomTool.Domain.DTOs;
using EomTool.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

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
            var campaigns = Campaigns(accountManagerId, advertiserId);
            return CampaignAmounts(campaigns, byAffiliate);
        }
        private IQueryable<CampaignAmount> CampaignAmounts(IQueryable<Campaign> campaigns, bool byAffiliate)
        {
            var items = Items(true);

            IQueryable<CampaignAmount> amounts;
            if (!byAffiliate) // by campaign
            {
                var itemGroups = items.GroupBy(i => new { i.pid, i.revenue_currency_id });
                var rawAmounts = from ig in itemGroups
                                 select new
                                 {
                                     ig.Key.pid,
                                     ig.Key.revenue_currency_id,
                                     revenue = ig.Sum(g => g.total_revenue.HasValue ? g.total_revenue.Value : 0),
                                     numUnits = (int)ig.Sum(g => g.num_units),
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
                              NumUnits = ra.numUnits,
                              NumAffs = ra.numAffs
                          };
            }
            else // by affiliate
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
                              NumUnits = (int)itemGroup.Sum(g => g.num_units),
                              NumAffs = 1
                          };
            }
            return amounts;
        }

        public IEnumerable<CampaignAmount> CampaignAmounts(IEnumerable<CampAffId> campAffIds)
        {
            var pids = campAffIds.Select(ca => ca.pid).Distinct();
            var campaigns = context.Campaigns.Where(c => pids.Contains(c.pid));
            var campaignAmountsAll = CampaignAmounts(campaigns, true);

            //now, narrow down to just the specified campaign/affiliate combos
            List<CampaignAmount> campaignAmounts = new List<CampaignAmount>();
            var caGroups = campaignAmountsAll.GroupBy(ca => ca.Pid);
            foreach (var caGroup in caGroups)
            {   // doing one pid at a time...
                var affids = campAffIds.Where(ca => ca.pid == caGroup.Key).Select(ca => ca.affid);
                var amounts = caGroup.Where(ca => affids.Contains(ca.AffId.Value));
                campaignAmounts.AddRange(amounts);
            }
            return campaignAmounts;
        }

        // Given the specified pid/affid pairs, generate an Invoice with InvoiceItems
        public Invoice GenerateInvoice(IEnumerable<CampAffId> campAffIds)
        {
            var invoice = new Invoice();

            //var itemGroups = Items(true).GroupBy(i => new { i.pid, i.affid, i.revenue_currency_id });
            // could do a where contains using a list of distinct pids... then do aggregates

            var items = Items(true);

            foreach (var campAffId in campAffIds)
            {
                var itemGroups = items.Where(i => i.pid == campAffId.pid && i.affid == campAffId.affid)
                                      .GroupBy(i => new { i.revenue_currency_id, i.revenue_per_unit });
                foreach (var itemGroup in itemGroups) // usually just one (currency/revenue_per_unit)
                {
                    var invoiceItem = new InvoiceItem()
                    {
                        pid = campAffId.pid,
                        affid = campAffId.affid,
                        currency_id = itemGroup.Key.revenue_currency_id,
                        CurrencyName = CurrencyName(itemGroup.Key.revenue_currency_id),
                        amount_per_unit = itemGroup.Key.revenue_per_unit,
                        num_units = (int)itemGroup.Sum(i => i.num_units)
                    };
                    invoiceItem.total_amount = invoiceItem.amount_per_unit * invoiceItem.num_units;
                    invoice.InvoiceItems.Add(invoiceItem);
                }
            }
            GenerateInvoiceLineItems(invoice, true);

            return invoice;
        }

        private void GenerateInvoiceLineItems(Invoice invoice, bool setAdvertiser)
        {
            // group the items by pid/currency/amount_per_unit and generate a LineItem for each (with subitems for the various affiliates)
            var itemGroups = invoice.InvoiceItems.GroupBy(i => new { i.pid, i.currency_id, i.amount_per_unit });
            foreach (var itemGroup in itemGroups)
            {
                // if pid == null, skip ?

                var lineItem = new InvoiceLineItem()
                {
                    Campaign = GetCampaign(itemGroup.Key.pid.Value),
                    Currency = GetCurrency(itemGroup.Key.currency_id),
                    SubItems = itemGroup.ToList()
                };
                context.Campaigns.Detach(lineItem.Campaign);

                invoice.LineItems.Add(lineItem);
            }

            if (setAdvertiser && invoice.LineItems.Count > 0)
            { // Assume all campaigns are for the same advertiser. Use the first one.
                invoice.Advertiser = GetAdvertiser(invoice.LineItems[0].Campaign.advertiser_id);
                context.Advertisers.Detach(invoice.Advertiser);
            }
        }

        public void SaveInvoice(Invoice invoice, string note = null, bool markSentToAccounting = false)
        {
            if (note != null)
            {
                var invoiceNote = new InvoiceNote()
                {
                    note = note,
                    added_by = "who",
                    created = DateTime.Now
                };
                invoice.InvoiceNotes.Add(invoiceNote);
            }
            if (markSentToAccounting)
                invoice.invoice_status_id = InvoiceStatus.AccountingReview;

            context.Invoices.AddObject(invoice);
            SaveChanges();
        }

        public Invoice GetInvoice(int id, bool fill)
        {
            var invoice = context.Invoices.FirstOrDefault(i => i.id == id);
            if (fill && invoice != null)
            {
                GenerateInvoiceLineItems(invoice, true);
            }
            return invoice;
        }

        public IEnumerable<Invoice> Invoices(bool fill)
        {
            var invoices = context.Invoices.AsEnumerable();
            if (fill)
            {
                foreach (var invoice in invoices)
                {
                    GenerateInvoiceLineItems(invoice, true);
                }
            }
            return invoices;
        }

        //---

        private IQueryable<Item> Items(bool invoiceableOnly)
        {
            var items = context.Items.AsQueryable();
            if (invoiceableOnly)
                items = items.Where(i => i.campaign_status_id == CampaignStatus.Default || i.campaign_status_id == CampaignStatus.Finalized);

            return items;
        }

        private List<Currency> _currencyList;
        private List<Currency> CurrencyList
        {
            get
            {
                if (_currencyList == null)
                {
                    _currencyList = context.Currencies.ToList();
                }
                return _currencyList;
            }
        }

        private Currency GetCurrency(int currencyId)
        {
            var currency = CurrencyList.FirstOrDefault(c => c.id == currencyId);
            return currency;
        }

        private string CurrencyName(int currencyId)
        {
            string currencyName = null;
            var currency = CurrencyList.FirstOrDefault(c => c.id == currencyId);
            if (currency != null)
                currencyName = currency.name;
            return currencyName;
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
