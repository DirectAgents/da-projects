using CakeExtracter.Common;
using CakeExtracter.Etl.SocialMarketing.EntitiesLoaders;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg.Facebook.Campaign;
using System.Collections.Generic;
using System.Linq;

namespace CakeExtracter.Etl.SocialMarketing.LoadersDA
{
    public class FacebookCampaignSummaryLoader : Loader<FbCampaignSummary>
    {
        private readonly FacebookCampaignsLoader fbCampaignsLoader;
        private readonly DateRange dateRange;

        private List<FbCampaignSummary> latestSummaries = new List<FbCampaignSummary>();

        public FacebookCampaignSummaryLoader(int accountId, DateRange dateRange)
            : base(accountId)
        {
            fbCampaignsLoader = new FacebookCampaignsLoader();
            this.dateRange = dateRange;
        }

        protected override int Load(List<FbCampaignSummary> items)
        {
            EnsureCampaignsEntitiesData(items);
            latestSummaries.AddRange(items);
            return items.Count;
        }

        protected override void AfterLoadAction()
        {
            DeleteOldSummariesFromDb();
            LoadLatestSummariesToDb(latestSummaries);
        }

        private void LoadSummaries(List<FbCampaignSummary> summaries)
        {
            latestSummaries.AddRange(summaries);
        }

        private void EnsureCampaignsEntitiesData(List<FbCampaignSummary> items)
        {
            var fbCampaigns = items.Select(item => item.Campaign).ToList();
            fbCampaignsLoader.AddUpdateDependentEntities(fbCampaigns);
            items.ForEach(item =>
            {
                item.CampaignId = item.Campaign.Id;
            });
        }

        private void DeleteOldSummariesFromDb()
        {
            SafeContextWrapper.TryMakeTransaction((ClientPortalProgContext db) =>
            {
                db.FbCampaignSummaries.Where(x => (x.Date >= dateRange.FromDate && x.Date <= dateRange.ToDate)
                    && x.Campaign.AccountId == accountId).DeleteFromQuery();
            }, "BulkDeleteByQuery");
        }

        private void LoadLatestSummariesToDb(List<FbCampaignSummary> summaries)
        {
            SafeContextWrapper.TryMakeTransaction((ClientPortalProgContext db) =>
            {
                db.BulkInsert(summaries);
            }, "BulkInsert");
        }
    }
}
