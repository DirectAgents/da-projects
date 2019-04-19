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

        private static object lockObj = new object();

        private List<FbCampaignSummary> latestSummaries = new List<FbCampaignSummary>();

        public FacebookCampaignSummaryLoader(int accountId, DateRange dateRange)
            : base(accountId)
        {
            fbCampaignsLoader = new FacebookCampaignsLoader();
            this.dateRange = dateRange;
        }

        protected override int Load(List<FbCampaignSummary> summaries)
        {
            EnsureCampaignsEntitiesData(summaries);
            // sometimes from api can be pulled duplicate summaries
            var uniqueSummaries = summaries.GroupBy(s => new { s.CampaignId, s.Date }).Select(gr => gr.First()).ToList();
            var notProcessedSummaries = uniqueSummaries.
                 Where(s => !latestSummaries.Any(ls => s.CampaignId == ls.CampaignId && s.Date == ls.Date)).ToList();
            latestSummaries.AddRange(notProcessedSummaries);
            return summaries.Count;
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
            SafeContextWrapper.TryMakeTransactionWithLock((ClientPortalProgContext db) =>
            {
                db.FbCampaignSummaries.Where(x => (x.Date >= dateRange.FromDate && x.Date <= dateRange.ToDate)
                    && x.Campaign.AccountId == accountId).DeleteFromQuery();
            }, lockObj, "BulkDeleteByQuery");
        }

        private void LoadLatestSummariesToDb(List<FbCampaignSummary> summaries)
        {
            SafeContextWrapper.TryMakeTransactionWithLock((ClientPortalProgContext db) =>
            {
                db.BulkInsert(summaries);
            }, lockObj, "BulkInsert");
        }
    }
}
