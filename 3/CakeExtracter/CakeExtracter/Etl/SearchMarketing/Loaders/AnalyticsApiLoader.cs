using CakeExtracter.Etl.SearchMarketing.Extracters;
using ClientPortal.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeExtracter.Etl.SearchMarketing.Loaders
{
    public class AnalyticsApiLoader : Loader<AnalyticsRow>
    {
        private readonly int advertiserId;

        public AnalyticsApiLoader(int advertiserId)
        {
            this.advertiserId = advertiserId;
        }

        protected override int Load(List<AnalyticsRow> rows)
        {
            Logger.Info("Loading {0} Analytics rows..", rows.Count);
            AddDependentSearchCampaigns(rows);
            var count = UpsertAnalyticsRows(rows);
            return count;
        }

        private int UpsertAnalyticsRows(List<AnalyticsRow> rows)
        {
            var addedCount = 0;
            var updatedCount = 0;
            var itemCount = 0;
            using (var db = new ClientPortalContext())
            {
                foreach (var row in rows)
                {
                    var pk1 = db.SearchCampaigns.Single(c => c.ExternalId == row.CampaignId && c.AdvertiserId == this.advertiserId && c.Channel == "google").SearchCampaignId;
                    var pk2 = row.Date;
                    var source = new GoogleAnalyticsSummary
                    {
                        SearchCampaignId = pk1,
                        Date = pk2,
                        Transactions = row.Transactions,
                        Revenue = row.Revenue
                    };

                    var target = db.Set<GoogleAnalyticsSummary>().Find(pk1, pk2);
                    if (target == null)
                    {
                        db.GoogleAnalyticsSummaries.Add(source);
                        addedCount++;
                    }
                    else
                    {
                        db.Entry(target).State = EntityState.Detached;
                        target = AutoMapper.Mapper.Map(source, target);
                        db.Entry(target).State = EntityState.Modified;
                        updatedCount++;
                    }
                    itemCount++;
                }
                Logger.Info("Saving {0} GoogleAnalyticsSummaries ({1} updates, {2} additions)", itemCount, updatedCount, addedCount);
                db.SaveChanges();
            }
            return itemCount;
        }

        private void AddDependentSearchCampaigns(List<AnalyticsRow> rows)
        {
            using (var db = new ClientPortalContext())
            {
                foreach (var campaignId in rows.Select(r => r.CampaignId).Distinct())
                {
                    var existing = db.SearchCampaigns.SingleOrDefault(c => c.ExternalId == campaignId && c.AdvertiserId == this.advertiserId && c.Channel == "google");

                    if (existing == null)
                    {
                        db.SearchCampaigns.Add(new SearchCampaign
                        {
                            AdvertiserId = this.advertiserId,
                            SearchCampaignName = campaignId.ToString(), // TODO: get the campaign's name
                            Channel = "google",
                            ExternalId = campaignId
                        });
                        Logger.Info("Saving new SearchCampaign: {0} (advertiser {1})", campaignId, this.advertiserId);
                        db.SaveChanges();
                    }
                    //else if (existing.SearchCampaignName != campaignName)
                    //{
                    //    existing.SearchCampaignName = campaignName;
                    //    Logger.Info("Saving updated SearchCampaign name: {0} (advertiser {1})", campaignName, this.advertiserId);
                    //    db.SaveChanges();
                    //}
                }
            }
        }

    }
}
