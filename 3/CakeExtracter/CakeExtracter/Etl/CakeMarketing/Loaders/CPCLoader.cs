using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CakeExtracter.CakeMarketingApi.Entities;
using EomTool.Domain.Abstract;
using EomTool.Domain.Entities;

namespace CakeExtracter.Etl.CakeMarketing.Loaders
{
    class CPCLoader : Loader<CampaignSummary>
    {
        private readonly DateTime date;
        private readonly IMainRepository mainRepo;

        public CPCLoader(DateTime date,IMainRepository mainRepository)
        {
            this.date = date;
            this.mainRepo = mainRepository;
        }

        //TODO: move code to Upsert...()
        // Check if campaign/affiliate exist in db; need to be added?

        protected override int Load(List<CampaignSummary> items)
        {
            foreach (var campaignItem in items)
            {
                if (campaignItem.Affiliate.AffiliateId > 0 && campaignItem.Sellable > 0)
                {
                    Item item = new Item
                    {
                        affid = campaignItem.Affiliate.AffiliateId,
                        pid = campaignItem.Offer.OfferId,
                        num_units = campaignItem.Sellable,
                        revenue_per_unit = campaignItem.Revenue / campaignItem.Sellable,
                        cost_per_unit = campaignItem.Cost / campaignItem.Sellable,
                        notes = "From Cake",
                        accounting_notes = "From Cake",
                        name = "Cake/" + date.ToString("yyyy-MM-dd") + "/aff:" + campaignItem.Affiliate.AffiliateId + "/offer:" + campaignItem.Offer.OfferId + "/type:Click - CPC"
                    };
                    item.SetDefaultStatuses();
                    item.SetDefaultTypes();
                    item.source_id = Source.Cake;
                    item.unit_type_id = UnitType.CPC;
                    mainRepo.AddItem(item);
                }
            }
            mainRepo.SaveChanges();
            return items.Count;
        }
    }
}
