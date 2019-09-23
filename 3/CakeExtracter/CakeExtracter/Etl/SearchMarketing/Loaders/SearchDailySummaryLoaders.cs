using System.Collections.Generic;
using System.Linq;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPSearch;

namespace CakeExtracter.Etl.SearchMarketing.Loaders
{
    public class SearchDailySummaryFromOldDbLoader : Loader<ClientPortal.Data.Contexts.SearchCampaign>
    {
        private Dictionary<string, int> searchCampaignIdLookupByExternalId = new Dictionary<string, int>();

        public SearchDailySummaryFromOldDbLoader(int searchAccountId)
        {
            CreateSearchCampaignIdLookup(searchAccountId);
        }

        private void CreateSearchCampaignIdLookup(int searchAccountId)
        {
            using (var db = new ClientPortalSearchContext())
            {
                var searchCamps = db.SearchCampaigns.Where(sc => sc.SearchAccountId == searchAccountId && !string.IsNullOrEmpty(sc.ExternalId));
                searchCampaignIdLookupByExternalId = searchCamps.ToDictionary(sc => sc.ExternalId, sc => sc.SearchCampaignId);
            }
        }

        protected override int Load(List<ClientPortal.Data.Contexts.SearchCampaign> items)
        {
            Logger.Info("Loading SearchDailySummaries from {0} SearchCampaigns..", items.Count);
            int count = UpsertItems(items);
            return count;
        }

        private int UpsertItems(List<ClientPortal.Data.Contexts.SearchCampaign> items)
        {
            var itemCount = 0; //SearchCampaigns
            var addedCount = 0; //SearchDailySummaries
            var updatedCount = 0; //SearchDailySummaries

            using (var db = new ClientPortalSearchContext())
            {
                foreach (var searchCampaign in items)
                {
                    if (string.IsNullOrEmpty(searchCampaign.ExternalId))
                    {
                        Logger.Info("SearchCampaign ({0}) has no ExternalId. Skipping copy of its SearchDailySummaries.", searchCampaign.SearchCampaignId);
                        break;
                    }
                    int searchCampaignId = searchCampaignIdLookupByExternalId[searchCampaign.ExternalId];
                    foreach (var sds in searchCampaign.SearchDailySummaries)
                    {
                        var newSDS = CreateSDS(searchCampaignId, sds);

                        //TODO: check if it exists - for the campId-Date-Network-Device (PK)

                        db.SearchDailySummaries.Add(newSDS);
                        addedCount++;
                    }
                    itemCount++;
                }
                Logger.Info("Saving SearchDailySummaries ({0} updates, {1} additions)", updatedCount, addedCount);
                db.SaveChanges();
            }
            return itemCount;
        }

        // The passed-in SDS is from the old database
        private SearchDailySummary CreateSDS(int searchCampaignId, ClientPortal.Data.Contexts.SearchDailySummary sds)
        {
            var newSDS = new SearchDailySummary
            {
                SearchCampaignId = searchCampaignId,
                Date = sds.Date,
                Network = sds.Network,
                Device = sds.Device,
                Revenue = sds.Revenue,
                Cost = sds.Cost,
                Orders = sds.Orders,
                Clicks = sds.Clicks,
                Impressions = sds.Impressions,
                CurrencyId = sds.CurrencyId,
                ViewThrus = sds.ViewThrus,
                CassConvs = sds.CassConvs,
                CassConVal = sds.CassConVal
            };
            return newSDS;
        }
    }
}
