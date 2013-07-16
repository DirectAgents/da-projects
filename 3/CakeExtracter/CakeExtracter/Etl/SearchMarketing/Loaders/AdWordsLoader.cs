using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ClientPortal.Data.Contexts;

namespace CakeExtracter.Etl.SearchMarketing.Loaders
{
    public class AdWordsLoader : Loader<Dictionary<string, string>>
    {
        protected override int Load(List<Dictionary<string, string>> items)
        {
            Logger.Info("Loading {0} SearchDailySummaries..", items.Count);
            AddDependentAdvertisers(items);
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
                    var campaignName = item["campaign"];
                    var pk1 = db.SearchCampaigns.Single(c => c.SearchCampaignName == campaignName).SearchCampaignId;
                    var pk2 = DateTime.Parse(item["day"].Replace('-', '/'));
                    var source = new SearchDailySummary
                        {
                            SearchCampaignId = pk1,
                            Date = pk2,
                            Revenue = decimal.Parse(item["totalConvValue"]),
                            Cost = decimal.Parse(item["cost"]),
                            Orders = int.Parse(item["conv1PerClick"]),
                            Clicks = int.Parse(item["clicks"]),
                            Impressions = int.Parse(item["impressions"]),
                            CurrencyId = item["currency"] == "USD" ? 1 : -1 // NOTE: non USD (if exists) -1 for now
                        };
                    var target = db.Set<SearchDailySummary>().Find(pk1, pk2);
                    if (target == null)
                    {
                        db.SearchDailySummaries.Add(source);
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

        private void AddDependentAdvertisers(List<Dictionary<string, string>> items)
        {
            using (var db = new ClientPortalContext())
            {
                foreach (var advertiserName in items.Select(c => c["account"]).Distinct())
                {
                    if (!db.Advertisers.Any(c => c.AdvertiserName == advertiserName))
                    {
                        // Get next advertiser in range of low and high number
                        const int lowAdvertiserId = 90000;
                        const int highAdvertiserId = 100000;
                        int advertiserId;
                        var advertiserIdQuery = db.Advertisers.Select(c => c.AdvertiserId)
                                                  .Where(c => c >= lowAdvertiserId && c <= highAdvertiserId);
                        if (advertiserIdQuery.Any())
                        {
                            advertiserId = advertiserIdQuery.Max() + 1;
                            if (advertiserId > highAdvertiserId)
                                throw new Exception("too many advertisers");
                        }
                        else
                        {
                            advertiserId = lowAdvertiserId;
                        }
                        db.Advertisers.Add(new Advertiser
                            {
                                AdvertiserId = advertiserId,
                                AdvertiserName = advertiserName,
                                Culture = "en-US",
                                HasSearch = true
                            });
                        Logger.Info("Saving new Advertiser: {0} ({1})", advertiserName, advertiserId);
                        db.SaveChanges();
                    }
                }
            }
        }

        private void AddDependentSearchCampaigns(List<Dictionary<string, string>> items)
        {
            using (var db = new ClientPortalContext())
            {
                foreach (var tuple in items.Select(c => Tuple.Create(c["account"], c["campaign"])).Distinct())
                {
                    var advertiserName = tuple.Item1;
                    var campaignName = tuple.Item2;
                    if (!db.SearchCampaigns.Any(c => c.SearchCampaignName == campaignName))
                    {
                        db.SearchCampaigns.Add(new SearchCampaign
                        {
                            Advertiser = db.Advertisers.Single(c => c.AdvertiserName == advertiserName),
                            SearchCampaignName = campaignName
                        });
                        Logger.Info("Saving new SearchCampaign: {0} ({1})", campaignName, advertiserName);
                        db.SaveChanges();
                    }
                }
            }
        }
    }
}
