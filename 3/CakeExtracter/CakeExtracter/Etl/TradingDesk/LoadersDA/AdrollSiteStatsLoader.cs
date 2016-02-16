using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Etl.TradingDesk.Extracters;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.TD;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA
{
    public class AdrollSiteStatsLoader : Loader<AdrollSiteStatsRow>
    {
        private TDSiteSummaryLoader siteSummaryLoader;
        private Dictionary<string, int> siteIdLookupByName = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        private int AccountId;
        private DateTime date;

        public AdrollSiteStatsLoader(int accountId, DateTime date)
        {
            this.siteSummaryLoader = new TDSiteSummaryLoader();
            this.AccountId = accountId;
            this.date = date;
        }

        protected override int Load(List<AdrollSiteStatsRow> items)
        {
            siteIdLookupByName.Clear(); // because this could get pretty big (lots of sites)

            Logger.Info("Loading {0} SiteDailySummaries..", items.Count);
            AddUpdateDependentSites(items);
            var ssItems = items.Select(i => CreateSiteSummary((AdrollSiteStatsRow)i, siteIdLookupByName[((AdrollSiteStatsRow)i).website])).ToList();
            var count = siteSummaryLoader.UpsertDailySummaries(ssItems);
            return count;
        }

        public SiteSummary CreateSiteSummary(AdrollSiteStatsRow item, int siteId)
        {
            var sSum = new SiteSummary //fill with new columns
            {
                SiteId = siteId,
                AccountId = this.AccountId,
                Date = this.date,
                Impressions = item.impression,
                Clicks = item.click,
                Cost = item.cost
            };
            return sSum;
        }

        private void AddUpdateDependentSites(List<AdrollSiteStatsRow> items)
        {
            var siteNames = items.Select(i => i.website).Distinct();
            TDSiteSummaryLoader.AddUpdateDependentSites(siteNames, siteIdLookupByName);
        }
    }
}