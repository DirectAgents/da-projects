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
                    var campaign = db.SearchCampaigns.SingleOrDefault(c => c.ExternalId == row.CampaignId && c.AdvertiserId == this.advertiserId && c.Channel == "google");
                    if (campaign == null)
                    {   // try matching to a bing campaign
                        var campaigns = db.SearchCampaigns.Where(c => c.AdvertiserId == this.advertiserId && c.Channel == "bing").ToList();
                        campaigns = campaigns.Where(c => c.SearchCampaignName.Replace(" ", "").Contains(row.CampaignName)).ToList();
                        if (campaigns.Count > 0)
                            campaign = campaigns[0]; // TODO: if more than one, throw exception?
                        else
                            Logger.Info("AnalyticsApiLoader - could not find a matching campaign for: {0}", row.CampaignName);
                    }
                    if (campaign != null)
                    {
                        var pk1 = campaign.SearchCampaignId;
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
                foreach (var tuple in rows.Select(r => Tuple.Create(r.CampaignId, r.CampaignName)).Distinct())
                {
                    var campaignId = tuple.Item1;
                    var campaignName = tuple.Item2;
                    var channel = "google";

                    SearchCampaign existing = null;

                    if (campaignId.HasValue)
                    {
                        existing = db.SearchCampaigns.SingleOrDefault(c => c.ExternalId == campaignId && c.AdvertiserId == this.advertiserId && c.Channel == "google");
                    }
                    else // no campaignId; must be bing... try to match it
                    {
                        channel = "bing";
                        var campaigns = db.SearchCampaigns.Where(c => c.AdvertiserId == this.advertiserId && c.Channel == "bing").ToList();
                        campaigns = campaigns.Where(c => c.SearchCampaignName.Replace(" ", "").Contains(campaignName)).ToList();
                        if (campaigns.Count > 0) // TODO: what to do if more than one?
                        {
                            existing = campaigns[0];
                            campaignName = existing.SearchCampaignName;
                        }
                    }

                    if (existing == null)
                    {
                        db.SearchCampaigns.Add(new SearchCampaign
                        {
                            AdvertiserId = this.advertiserId,
                            SearchCampaignName = campaignName,
                            Channel = channel,
                            ExternalId = campaignId
                        });
                        Logger.Info("Saving new SearchCampaign: {0} [{1}] (advertiser {2})", campaignName, campaignId, this.advertiserId);
                        db.SaveChanges();
                    }
                    else if (existing.SearchCampaignName != campaignName)
                    {
                        existing.SearchCampaignName = campaignName;
                        Logger.Info("Saving updated SearchCampaign name: {0} (advertiser {1})", campaignName, this.advertiserId);
                        db.SaveChanges();
                    }
                }
            }
        }

    }
}
