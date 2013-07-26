using ClientPortal.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeExtracter.Etl.SearchMarketing.Loaders
{
    public class BingLoader : Loader<Dictionary<string, string>>
    {
        private const string bingChannel = "bing";
        private readonly int advertiserId;

        public BingLoader(int advertiserId)
        {
            this.advertiserId = advertiserId;
        }

        protected override int Load(List<Dictionary<string, string>> items)
        {
            Logger.Info("Loading {0} SearchDailySummaries..", items.Count);
            AddDependentSearchCampaigns(items);
            var count = UpsertSearchDailySummaries(items);
            return count;
        }

        private static int UpsertSearchDailySummaries(List<Dictionary<string, string>> items)
        {
            var addedCount = 0;
            var updatedCount = 0;
            var itemCount = 0;
            using (var db = new ClientPortalContext())
            {
                foreach (var item in items)
                {
                    var campaignName = item["CampaignName"];
                    var pk1 = db.SearchCampaigns.Single(c => c.Channel == bingChannel && c.SearchCampaignName == campaignName).SearchCampaignId;
                    var pk2 = DateTime.Parse(item["GregorianDate"]);
                    var source = new SearchDailySummary2
                    {
                        SearchCampaignId = pk1,
                        Date = pk2,
                        Network = ".",
                        Device = ".",
                        ClickType = ".",
                        Revenue = decimal.Parse(item["Revenue"]),
                        Cost = decimal.Parse(item["Spend"]),
                        Orders = int.Parse(item["Conversions"]),
                        Clicks = int.Parse(item["Clicks"]),
                        Impressions = int.Parse(item["Impressions"]),
                        CurrencyId = 1 // item["CurrencyCode"] == "USD" ? 1 : -1 // NOTE: non USD (if exists) -1 for now
                    };
                    var target = db.Set<SearchDailySummary2>().Find(pk1, pk2, ".", ".", ".");
                    if (target == null)
                    {
                        db.SearchDailySummary2.Add(source);
                        addedCount++;
                    }
                    else
                    {
                        AutoMapper.Mapper.Map(source, target);
                        db.Entry(target).State = EntityState.Modified;
                        updatedCount++;
                    }
                    itemCount++;
                }
                Logger.Info("Saving {0} SearchDailySummaries ({1} updates, {2} additions)", itemCount, updatedCount, addedCount);
                db.SaveChanges();
            }
            return itemCount;
        }

        private void AddDependentSearchCampaigns(List<Dictionary<string, string>> items)
        {
            using (var db = new ClientPortalContext())
            {
                foreach (var campaignName in items.Select(i => i["CampaignName"]).Distinct())
                {
                    if (!db.SearchCampaigns.Any(c => c.Channel == bingChannel && c.SearchCampaignName == campaignName))
                    {
                        db.SearchCampaigns.Add(new SearchCampaign
                        {
                            AdvertiserId = advertiserId,
                            SearchCampaignName = campaignName,
                            Channel = bingChannel
                        });
                        Logger.Info("Saving new SearchCampaign: {0}", campaignName);
                        db.SaveChanges();
                    }
                }
            }
        }
    }
}
