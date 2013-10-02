using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ClientPortal.Data.Contexts;

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
                    var campaignId = int.Parse(item["CampaignId"]);
                    var pk1 = db.SearchCampaigns.Single(c => c.Channel == bingChannel && c.SearchCampaignName == campaignName).SearchCampaignId;
                    var pk2 = DateTime.Parse(item["GregorianDate"]);
                    var pk3 = ".";
                    var pk4 = ".";
                    var pk5 = ".";
                    var source = new SearchDailySummary2
                    {
                        SearchCampaignId = pk1,
                        Date = pk2,
                        Network = pk3,
                        Device = pk4,
                        ClickType = pk5,
                        Revenue = decimal.Parse(item["Revenue"]),
                        Cost = decimal.Parse(item["Spend"]),
                        Orders = int.Parse(item["Conversions"]),
                        Clicks = int.Parse(item["Clicks"]),
                        Impressions = int.Parse(item["Impressions"]),
                        CurrencyId = 1 // item["CurrencyCode"] == "USD" ? 1 : -1 // NOTE: non USD (if exists) -1 for now
                    };
                    var target = db.Set<SearchDailySummary2>().Find(pk1, pk2, pk3, pk4, pk5);
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
                foreach (var tuple in items.Select(c => Tuple.Create(c["CampaignName"], c["CampaignId"])).Distinct())
                {
                    var campaignName = tuple.Item1;
                    var campaignId = int.Parse(tuple.Item2);

                    var existing = db.SearchCampaigns.SingleOrDefault(c => c.ExternalId == campaignId && c.AdvertiserId == advertiserId && c.Channel == bingChannel);

                    if (existing == null)
                    {
                        db.SearchCampaigns.Add(new SearchCampaign
                        {
                            AdvertiserId = advertiserId,
                            SearchCampaignName = campaignName,
                            Channel = bingChannel,
                            ExternalId = campaignId
                        });
                        Logger.Info("Saving new SearchCampaign: {0}", campaignName);
                        db.SaveChanges();
                    }
                    else if (existing.SearchCampaignName != campaignName)
                    {
                        existing.SearchCampaignName = campaignName;
                        Logger.Info("Saving updated SearchCampaign name: {0}", campaignName);
                        db.SaveChanges();
                    }
                }
            }
        }
    }
}
