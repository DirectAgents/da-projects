using System;
using System.Linq;
using EomTool.Domain.Abstract;
using EomTool.Domain.Entities;

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
    }
}
