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

        public AccountManagerTeam GetAccountManagerTeam(int id)
        {
            return context.AccountManagerTeams.FirstOrDefault(am => am.id == id);
        }

        public Advertiser GetAdvertiser(int id)
        {
            return context.Advertisers.FirstOrDefault(a => a.id == id);
        }

        public IQueryable<AccountManagerTeam> AccountManagerTeams(bool withActivityOnly = false)
        {
            if (withActivityOnly)
                return Campaigns(null, null, true).Select(c => c.AccountManagerTeam).Distinct();
            else
                return context.AccountManagerTeams;
        }

        // ---

        public IQueryable<Campaign> Campaigns(int? amId, int? advertiserId, bool activeOnly = false)
        {
            var campaigns = context.Campaigns.AsQueryable();
            if (amId.HasValue)
                campaigns = campaigns.Where(c => c.account_manager_id == amId.Value);
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

        public IEnumerable<CampaignAmount> CampaignAmounts(int? amId, int? advertiserId, bool byAffiliate, int? campaignStatus)
        {
            var campaigns = Campaigns(amId, advertiserId);
            return CampaignAmounts(campaigns, byAffiliate, campaignStatus);
        }
        private IEnumerable<CampaignAmount> CampaignAmounts(IQueryable<Campaign> campaigns, bool byAffiliate, int? campaignStatus)
        {
            var items = Items(campaignStatus);

            IEnumerable<CampaignAmount> amounts;
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

                //var invItems = InvoiceItems(pid, affId)
                var invItemGroups = context.InvoiceItems.Where(i => i.pid.HasValue)
                                                        .GroupBy(i => new { pid = i.pid.Value, i.currency_id });
                var invoicedAmounts = from ig in invItemGroups
                                      select new
                                      {
                                          ig.Key.pid,
                                          ig.Key.currency_id,
                                          amount = ig.Sum(g => g.total_amount.HasValue ? g.total_amount.Value : 0)
                                      };

                var query = from ra in rawAmounts
                            join c in campaigns on ra.pid equals c.pid
                            join rc in context.Currencies on ra.revenue_currency_id equals rc.id
                            join ia in invoicedAmounts on new { ra.pid, currency_id = ra.revenue_currency_id } equals
                                                          new { ia.pid, ia.currency_id } into gj
                            from invoicedAmount in gj.DefaultIfEmpty()
                            select new { ra, c, rc, invoicedAmount };

                amounts = query.ToList().Select(q => new CampaignAmount()
                          {
                              AdvId = q.c.advertiser_id,
                              AdvertiserName = q.c.Advertiser.name,
                              Pid = q.ra.pid,
                              CampaignName = q.c.campaign_name,
                              CampaignDisplayName = q.c.DisplayName,
                              RevenueCurrency = q.rc.name,
                              Revenue = q.ra.revenue,
                              InvoicedAmount = (q.invoicedAmount == null ? 0 : q.invoicedAmount.amount),
                              NumUnits = q.ra.numUnits,
                              NumAffs = q.ra.numAffs
                          });
            }
            else // by affiliate
            {
                var itemGroups = items.GroupBy(i => new { i.pid, i.affid, i.revenue_currency_id });
                var rawAmounts = from ig in itemGroups
                                 select new
                                 {
                                     ig.Key.pid,
                                     ig.Key.affid,
                                     ig.Key.revenue_currency_id,
                                     revenue = ig.Sum(g => g.total_revenue.HasValue ? g.total_revenue.Value : 0),
                                     numUnits = (int)ig.Sum(g => g.num_units)
                                 };

                var invItemGroups = context.InvoiceItems.Where(i => i.pid.HasValue && i.affid.HasValue)
                                                        .GroupBy(i => new { pid = i.pid.Value, affid = i.affid.Value, i.currency_id });
                var invoicedAmounts = from ig in invItemGroups
                                      select new
                                      {
                                          ig.Key.pid,
                                          ig.Key.affid,
                                          ig.Key.currency_id,
                                          amount = ig.Sum(g => g.total_amount.HasValue ? g.total_amount.Value : 0)
                                      };

                var query = from ra in rawAmounts
                            join c in campaigns on ra.pid equals c.pid
                            join a in context.Affiliates on ra.affid equals a.affid
                            join rc in context.Currencies on ra.revenue_currency_id equals rc.id
                            join ia in invoicedAmounts on new { ra.pid, ra.affid, currency_id = ra.revenue_currency_id } equals
                                                          new { ia.pid, ia.affid, ia.currency_id } into gj
                            from invoicedAmount in gj.DefaultIfEmpty()
                            select new { ra, c, a, rc, invoicedAmount };

                amounts = query.AsEnumerable().Select(q => new CampaignAmount()
                          {
                              AdvId = q.c.advertiser_id,
                              AdvertiserName = q.c.Advertiser.name,
                              Pid = q.ra.pid,
                              CampaignName = q.c.campaign_name,
                              CampaignDisplayName = q.c.DisplayName,
                              AffId = q.ra.affid,
                              AffiliateName = q.a.name2,
                              RevenueCurrency = q.rc.name,
                              Revenue = q.ra.revenue,
                              InvoicedAmount = (q.invoicedAmount == null ? 0 : q.invoicedAmount.amount),
                              NumUnits = q.ra.numUnits,
                              NumAffs = 1
                          });
            }
            return amounts;
        }

        // unused...
        public IEnumerable<CampaignAmount> CampaignAmounts(IEnumerable<CampAffId> campAffIds)
        {
            var pids = campAffIds.Select(ca => ca.pid).Distinct();
            var campaigns = context.Campaigns.Where(c => pids.Contains(c.pid));
            var campaignAmountsAll = CampaignAmounts(campaigns, true, null);

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

        public IQueryable<InvoiceItem> InvoiceItems(int? pid, int? affId)
        {
            var invItems = context.InvoiceItems.AsQueryable();
            if (pid.HasValue)
                invItems = invItems.Where(i => i.pid == pid.Value);
            if (affId.HasValue)
                invItems = invItems.Where(i => i.affid == affId.Value);
            return invItems;
        }

        // Given the specified pid/affid pairs, generate an Invoice with InvoiceItems
        public Invoice GenerateInvoice(IEnumerable<CampAffId> campAffIds)
        {
            var invoice = new Invoice();

            //var itemGroups = Items(true).GroupBy(i => new { i.pid, i.affid, i.revenue_currency_id });
            // could do a where contains using a list of distinct pids... then do aggregates

            //var items = Items(CampaignStatus.Default); // only unfinalized amounts will be included
            var items = Items(null);

            foreach (var campAffId in campAffIds)
            {
                var itemGroups = items.Where(i => i.pid == campAffId.pid && i.affid == campAffId.affid && i.total_revenue.HasValue && i.total_revenue > 0)
                                      .GroupBy(i => new { i.revenue_currency_id, i.revenue_per_unit, i.unit_type_id });
                foreach (var itemGroup in itemGroups) // usually just one (currency/revenue_per_unit/unit_type)
                {
                    var invoiceItem = new InvoiceItem()
                    {
                        pid = campAffId.pid,
                        affid = campAffId.affid,
                        currency_id = itemGroup.Key.revenue_currency_id,
                        CurrencyName = CurrencyName(itemGroup.Key.revenue_currency_id),
                        amount_per_unit = itemGroup.Key.revenue_per_unit,
                        unit_type_id = itemGroup.Key.unit_type_id,
                        UnitTypeName = UnitTypeName(itemGroup.Key.unit_type_id),
                        num_units = (int)itemGroup.Sum(i => i.num_units)
                    };
                    invoiceItem.total_amount = invoiceItem.amount_per_unit * invoiceItem.num_units;
                    invoice.InvoiceItems.Add(invoiceItem);
                }
            }
            //TODO: check for invoiceitem amounts that have already been invoiced. Remove or reduce the amounts accordingly.

            GenerateInvoiceLineItems(invoice, true);

            return invoice;
        }

        private void GenerateInvoiceLineItems(Invoice invoice, bool setExtended)
        {
            bool firstGroup = true;

            // group the items by pid/currency/amount_per_unit/unit_type and generate a LineItem for each (with subitems for the various affiliates)
            var itemGroups = invoice.InvoiceItems.GroupBy(i => new { i.pid, i.currency_id, i.amount_per_unit, i.unit_type_id });
            foreach (var itemGroup in itemGroups)
            {
                // if pid == null, skip ?

                var lineItem = new InvoiceLineItem()
                {
                    Campaign = GetCampaign(itemGroup.Key.pid.Value),
                    Currency = GetCurrency(itemGroup.Key.currency_id),
                    ItemCode = itemGroup.Key.unit_type_id.HasValue ? ItemCode(itemGroup.Key.unit_type_id.Value) : null,
                    SubItems = itemGroup.ToList()
                };
                if (firstGroup && setExtended)
                    SetInvoiceExtended(invoice, lineItem.Campaign);

                context.Campaigns.Detach(lineItem.Campaign);

                invoice.LineItems.Add(lineItem);
                firstGroup = false;
            }
        }

        // Set the extended properties of an invoice. Pass in an example campaign, if known.
        private void SetInvoiceExtended(Invoice invoice, Campaign campaign = null)
        {
            if (campaign == null)
            {   // Assume all items' campaigns are for the same advertiser (& account manager). Use the first one.
                var itemWithPid = invoice.InvoiceItems.FirstOrDefault(i => i.pid.HasValue);
                if (itemWithPid != null)
                    campaign = GetCampaign(itemWithPid.pid.Value);
            }
            if (campaign != null)
            {
                invoice.Advertiser = campaign.Advertiser;
                context.Advertisers.Detach(invoice.Advertiser);

                invoice.Advertiser.AccountManagerTeam = campaign.AccountManagerTeam;
                context.AccountManagerTeams.Detach(invoice.Advertiser.AccountManagerTeam);
            }
        }

        public void SaveInvoice(Invoice invoice, bool markSentToAccounting = false)
        {
            if (markSentToAccounting)
                invoice.invoice_status_id = InvoiceStatus.AccountingReview;

            context.Invoices.AddObject(invoice);
            SaveChanges();
        }

        public IQueryable<Invoice> Invoices(bool fillExtended)
        {
            var invoices = context.Invoices.AsQueryable();
            if (fillExtended)
            {
                foreach (var invoice in invoices)
                {
                    SetInvoiceExtended(invoice);
                }
            }
            return invoices;
        }

        public Invoice GetInvoice(int id, bool fillLineItems = false)
        {
            var invoice = context.Invoices.FirstOrDefault(i => i.id == id);
            if (fillLineItems && invoice != null)
            {
                GenerateInvoiceLineItems(invoice, true);
            }
            return invoice;
        }

        public bool SetInvoiceStatus(int id, int statusId)
        {
            bool success = false;
            var invoice = GetInvoice(id);
            if (invoice != null)
            {
                invoice.invoice_status_id = statusId;
                SaveChanges();
                success = true;
            }
            return success;
        }

        //---

        private IQueryable<Item> Items(int? campaignStatus)
        {
            var items = context.Items.AsQueryable();
            if (campaignStatus.HasValue)
                items = items.Where(i => i.campaign_status_id == campaignStatus);
            return items;
        }
        //private IQueryable<Item> Items(int[] campaignStatuses)
        //{
        //    var items = context.Items.Where(i => campaignStatuses.Contains(i.campaign_status_id));
        //    return items;
        //}

        //---

        private List<UnitType> _unitTypeList;
        private List<UnitType> UnitTypeList
        {
            get
            {
                if (_unitTypeList == null)
                {
                    _unitTypeList = context.UnitTypes.ToList();
                }
                return _unitTypeList;
            }
        }

        public string UnitTypeName(int unitTypeId)
        {
            var unitType = UnitTypeList.FirstOrDefault(ut => ut.id == unitTypeId);
            if (unitType != null)
                return unitType.name;
            else
                return null;
        }
        public string ItemCode(int unitTypeId)
        {
            string unitTypeName = UnitTypeName(unitTypeId);
            return UnitType.ToItemCode(unitTypeName);
        }

        public bool UnitTypeExists(int unitTypeId)
        {
            return UnitTypeList.Any(u => u.id == unitTypeId);
        }
        public UnitType GetUnitType(int unitTypeId)
        {
            return UnitTypeList.FirstOrDefault(u => u.id == unitTypeId);
        }
        public UnitType GetUnitType(string unitTypeName)
        {
            return UnitTypeList.FirstOrDefault(ut => ut.name == unitTypeName);
        }

        //---

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

        public string CurrencyName(int currencyId)
        {
            string currencyName = null;
            var currency = CurrencyList.FirstOrDefault(c => c.id == currencyId);
            if (currency != null)
                currencyName = currency.name;
            return currencyName;
        }

        public bool CurrencyExists(int currency)
        {
            return CurrencyList.Any(c => c.id == currency);
        }
        public Currency GetCurrency(int currencyId)
        {
            return CurrencyList.FirstOrDefault(c => c.id == currencyId);
        }
        public Currency GetCurrency(string currencyName)
        {
            return CurrencyList.FirstOrDefault(c => c.name == currencyName);
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
        public bool SaveCampaign(Campaign inCampaign)
        {
            var campaign = GetCampaign(inCampaign.pid);
            if (campaign == null)
                return false;

            campaign.display_name = inCampaign.display_name;
            SaveChanges();
            return true;
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
